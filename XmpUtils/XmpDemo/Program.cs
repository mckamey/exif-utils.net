using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

using XmpUtils.Xmp;
using XmpUtils.Xmp.Schemas;
using XmpUtils.Xmp.ValueTypes;

namespace XmpDemo
{
	public class Program
	{
		public static void Main()
		{
			Console.Write("Enter filename: ");

			string filename = Console.ReadLine();

			object value;
			using (Stream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				BitmapMetadata metadata = LoadMetadata(stream);
				value = ProcessMetadata(metadata, "/", 0);
			}

			List<XmpProperty> allProperties = new List<XmpProperty>();

			FindProperties(value, allProperties);

			if (allProperties.Count > 0)
			{
				using (TextWriter writer = File.CreateText(filename + ".xmp"))
				{
					new RdfUtility().ToXml(allProperties).Save(writer);
				}
			}
		}

		private static void FindProperties(object value, List<XmpProperty> allProperties)
		{
			IEnumerable list = value as IEnumerable;
			if (list != null)
			{
				foreach (object item in list)
				{
					IEnumerable<XmpProperty> properties = item as IEnumerable<XmpProperty>;
					if (properties == null)
					{
						FindProperties(item, allProperties);
					}
					else
					{
						allProperties.AddRange(properties);
					}
				}
			}
		}

		private static object ProcessMetadata(BitmapMetadata metadata, string objName, int depth)
		{
			Console.Write(new String('\t', depth));
			Console.WriteLine("{0} => {1}", objName, metadata.Format);

			switch (metadata.Format)
			{
				case "xmpbag":
				case "xmpseq":
				{
					return ProcessArray(metadata, depth+1);
				}
				case "xmpalt":
				case "xmpstruct":
				{
					return ProcessXmpStruct(metadata, depth+1);
				}
				case "xmp":
				{
					return ProcessXmp(metadata, depth+1);
				}
				case "exif":
				{
					return ProcessBlock(metadata, typeof(ExifSchema), depth+1);
				}
				case "ifd":
				{
					return ProcessBlock(metadata, typeof(ExifTiffSchema), depth+1);
				}
				default:
				{
					return ProcessArray(metadata, depth+1);
				}
			}
		}

		private static IEnumerable<XmpProperty> ProcessBlock(BitmapMetadata metadata, Type enumType, int depth)
		{
			List<XmpProperty> properties = new List<XmpProperty>();

			foreach (string name in metadata)
			{
				object value = metadata.GetQuery(name);
				if (value == null)
				{
					continue;
				}

				Console.Write(new String('\t', depth));
				Console.WriteLine("{0} => {1}: {2}", name, value.GetType(), Convert.ToString(value));

				if (value is BitmapMetadata)
				{
					value = ProcessMetadata((BitmapMetadata)value, name, depth);
					IDictionary<string, object> dictionary = value as IDictionary<string, object>;
					if (dictionary != null)
					{
						value =
							(from key in dictionary.Keys
							 let val = dictionary[key]
							 select new
							 {
								 Key = key.Substring(key.LastIndexOf(':')+1),
								 Value = val
							 }).ToDictionary(item => item.Key, item => item.Value);
					}
				}

				if (value is BitmapMetadataBlob)
				{
					value = ((BitmapMetadataBlob)value).GetBlobValue();
				}

				if (value == null)
				{
					continue;
				}

				IEnumerable<XmpProperty> valueProps = value as IEnumerable<XmpProperty>;
				if (valueProps != null)
				{
					properties.AddRange(valueProps);
					continue;
				}

				Enum schema = ParseSchema(enumType, name);
				if (schema == null)
				{
					continue;
				}

				XmpProperty property = new XmpProperty
				{
					Schema = schema,
					Priority = 0.25m
				};

				property.Value = ProcessValue(property, value);

				properties.Add(property);
			}

			return properties;
		}

		private static object ProcessValue(XmpProperty property, object value)
		{
			switch (Type.GetTypeCode(property.DataType))
			{
				case TypeCode.String:
				{
					if (property.Quantity != XmpQuantity.Single)
					{
						value = new object[] { value };
					}
					break;
				}
				case TypeCode.DateTime:
				{
					DateTime date;
					if (DateTime.TryParseExact(
							Convert.ToString(value),
							"yyyy':'MM':'dd HH':'mm':'ss",
							DateTimeFormatInfo.InvariantInfo,
							DateTimeStyles.RoundtripKind,
							out date))
					{
						// clean up to ISO-8601
						value = date.ToString("yyyy'-'MM'-'ddTHH':'mm':'ss");
					}
					break;
				}
				case TypeCode.Object:
				{
					if (property.ValueType is XmpBasicType &&
						((XmpBasicType)property.ValueType) == XmpBasicType.LangAlt)
					{
						value = new Dictionary<string, object>
						{
							{ "x-default", Convert.ToString(value) }
						};
					}
					else
					{
						// TODO
					}
					break;
				}
				default:
				{
					break;
				}
			}

			return value;
		}

		private static IEnumerable<XmpProperty> ProcessXmp(BitmapMetadata metadata, int depth)
		{
			List<XmpProperty> properties = new List<XmpProperty>();

			foreach (string name in metadata)
			{
				// http://msdn.microsoft.com/en-us/library/ee719796(VS.85).aspx
				// http://search.cpan.org/
				object value = metadata.GetQuery(name);
				if (value == null)
				{
					continue;
				}

				Console.Write(new String('\t', depth));
				Console.WriteLine("{0} => {1}: {2}", name, value.GetType(), Convert.ToString(value));

				Enum schema = (Enum)XmpNamespaceUtility.Instance.Parse(name.TrimStart('/'));
				if (schema == null)
				{
					continue;
				}

				XmpProperty property = new XmpProperty
				{
					Schema = schema,
					Priority = 0.75m
				};

				if (value is BitmapMetadata)
				{
					value = ProcessMetadata((BitmapMetadata)value, name, depth);
					IDictionary<string, object> dictionary = value as IDictionary<string, object>;
					if (dictionary != null)
					{
						value =
							(from key in dictionary.Keys
							 let val = dictionary[key]
							 select new
							 {
								 Key = key.Substring(key.LastIndexOf(':')+1),
								 Value = val
							 }).ToDictionary(item => item.Key, item => item.Value);
					}
				}

				if (value is BitmapMetadataBlob)
				{
					value = ((BitmapMetadataBlob)value).GetBlobValue();
				}

				if (value == null)
				{
					continue;
				}

				property.Value = value;
				properties.Add(property);
			}

			return properties;
		}

		private static Dictionary<string, object> ProcessXmpStruct(BitmapMetadata metadata, int depth)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();

			foreach (string name in metadata)
			{
				object value = metadata.GetQuery(name);
				if (value == null)
				{
					continue;
				}
				if (value is BitmapMetadata)
				{
					value = ProcessMetadata((BitmapMetadata)value, name, depth);
					continue;
				}

				if (value is BitmapMetadataBlob)
				{
					value = ((BitmapMetadataBlob)value).GetBlobValue();
				}

				dictionary[name.TrimStart('/')] = value;

				Console.Write(new String('\t', depth));
				Console.WriteLine("{0} => {1}: {2}", name, value.GetType(), Convert.ToString(value));
			}

			return dictionary;
		}

		private static Array ProcessArray(BitmapMetadata metadata, int depth)
		{
			ArrayList array = new ArrayList();

			foreach (string name in metadata)
			{
				object value = metadata.GetQuery(name);
				if (value == null)
				{
					continue;
				}
				if (value is BitmapMetadata)
				{
					value = ProcessMetadata((BitmapMetadata)value, name, depth);
				}

				if (value is BitmapMetadataBlob)
				{
					value = ((BitmapMetadataBlob)value).GetBlobValue();
				}

				array.Add(value);

				Console.Write(new String('\t', depth));
				Console.WriteLine("{0} => {1}: {2}", name, value.GetType(), Convert.ToString(value));
			}

			return array.ToArray();
		}

		private static Enum ParseSchema(Type enumType, string name)
		{
			name = name.TrimStart('/', '{').TrimEnd('}');
			string[] parts = name.Split(new char[]{'='}, 2, StringSplitOptions.RemoveEmptyEntries);

			if (parts.Length < 2)
			{
				return null;
			}

			switch (parts[0])
			{
				case "ushort":
				{
					ushort value;
					
					if (!UInt16.TryParse(parts[1], out value) ||
						!Enum.IsDefined(enumType, value))
					{
						return null;
					}

					return (Enum)Enum.ToObject(enumType, value);
				}
				default:
				case "long":
				{
					long value;

					if (!Int64.TryParse(parts[1], out value) ||
						!Enum.IsDefined(enumType, value))
					{
						return null;
					}

					return (Enum)Enum.ToObject(enumType, value);
				}
			}
		}

		private static BitmapMetadata LoadMetadata(Stream stream)
		{
			BitmapDecoder decoder = BitmapDecoder.Create(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnDemand);

			if (decoder.Frames[0] != null)
			{
				return decoder.Frames[0].Metadata as BitmapMetadata;
			}

			return null;
		}
	}
}
