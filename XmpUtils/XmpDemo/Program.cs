using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

using XmpUtils;
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
					return ProcessXmp(metadata, 0.8m, depth+1);
				}
				case "exif":
				case "gps":
				{
					return ProcessBlock(metadata, typeof(ExifSchema), 0.2m, depth+1);
				}
				case "ifd":
				{
					return ProcessBlock(metadata, typeof(ExifTiffSchema), 0.4m, depth+1);
				}
				//case "iptc":
				//{
				//    return ProcessBlock(metadata, typeof(IptcSchema), 0.6m, depth+1);
				//}
				case "thumb":
				case "chrominance":
				case "luminance":
				{
					// these are suppressed in XMP
					return null;
				}
				default:
				{
					return ProcessArray(metadata, depth+1);
				}
			}
		}

		private static IEnumerable<XmpProperty> ProcessBlock(BitmapMetadata metadata, Type enumType, decimal priority, int depth)
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
					Priority = priority
				};

				property.Value = ProcessValue(property, value);

				if (property.Value != null)
				{
					properties.Add(property);
				}
			}

			return properties;
		}

		private static object ProcessValue(XmpProperty property, object value)
		{
			switch (Type.GetTypeCode(property.DataType))
			{
				case TypeCode.String:
				{
					if (property.Quantity != XmpQuantity.Single &&
						!(value is Array))
					{
						value = new object[] { value };
					}

					if (property.Quantity == XmpQuantity.Single &&
						value is byte[] &&
						property.ValueType is XmpBasicType &&
						((XmpBasicType)property.ValueType) == XmpBasicType.Text)
					{
						value = new String(Encoding.UTF8.GetChars((byte[])value));
					}

					if (property.ValueType is ExifType)
					{
						switch ((ExifType)property.ValueType)
						{
							case ExifType.GPSCoordinate:
							{
								Array array = value as Array;
								if (array != null && array.Length == 3)
								{
									try
									{
										GpsCoordinate gps = new GpsCoordinate();
										gps.Degrees = (Rational<uint>)ProcessRational(property, array.GetValue(0));
										gps.Minutes = (Rational<uint>)ProcessRational(property, array.GetValue(1));
										gps.Seconds = (Rational<uint>)ProcessRational(property, array.GetValue(2));
										value = gps.ToString("X");
									}
									catch { }
								}
								break;
							}
							case ExifType.Rational:
							{
								Array array = value as Array;
								if (array == null)
								{
									value = Convert.ToString(ProcessRational(property, value));
								}
								else
								{
									for (int i=0; i<array.Length; i++)
									{
										array.SetValue(Convert.ToString(ProcessRational(property, array.GetValue(i))), i);
									}
									value = array;
								}
								break;
							}
						}
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
							DateTimeStyles.AssumeLocal,
							out date))
					{
						// clean up to ISO-8601
						value = date.ToString("yyyy'-'MM'-'ddTHH':'mm':'ss.ffK");
					}
					break;
				}
				case TypeCode.Object:
				{
					if (property.ValueType is XmpBasicType &&
						((XmpBasicType)property.ValueType) == XmpBasicType.LangAlt)
					{
						string str;
						if (value is byte[])
						{
							str = new String(Encoding.UTF8.GetChars((byte[])value));
							int end = str.IndexOf('\0');
							if (end >= 0)
							{
								str = str.Substring(0, end);
							}
							value = str;
						}
						else
						{
							str = Convert.ToString(value);
						}

						if (String.IsNullOrEmpty(str))
						{
							value = null;
						}
						else
						{
							value = new Dictionary<string, object>
							{
								{ "x-default", str }
							};
						}
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

		private static object ProcessRational(XmpProperty property, object value)
		{
			if (value == null)
			{
				return null;
			}

			switch (Type.GetTypeCode(value.GetType()))
			{
				case TypeCode.UInt64:
				{
					ulong rational = (ulong)value;
					uint numerator = (uint)(rational & 0xFFFFFFFFL);
					uint denominator = (uint)(rational >> 32);
					value = new Rational<uint>(numerator, denominator, false);
					break;
				}
				case TypeCode.Int64:
				{
					long rational = (long)value;
					int numerator = (int)(rational & 0xFFFFFFFFL);
					int denominator = (int)(rational >> 32);
					value = new Rational<int>(numerator, denominator, false);
					break;
				}
				case TypeCode.UInt32:
				{
					uint rational = (uint)value;
					ushort numerator = (ushort)(rational & 0xFFFF);
					ushort denominator = (ushort)(rational >> 16);
					value = new Rational<ushort>(numerator, denominator, false);
					break;
				}
				case TypeCode.Int32:
				{
					int rational = (int)value;
					short numerator = (short)(rational & 0xFFFF);
					short denominator = (short)(rational >> 16);
					value = new Rational<short>(numerator, denominator, false);
					break;
				}
				case TypeCode.UInt16:
				{
					ushort rational = (ushort)value;
					byte numerator = (byte)(rational & 0xFF);
					byte denominator = (byte)(rational >> 8);
					value = new Rational<byte>(numerator, denominator, false);
					break;
				}
				case TypeCode.Int16:
				{
					short rational = (short)value;
					sbyte numerator = (sbyte)(rational & 0xFF);
					sbyte denominator = (sbyte)(rational >> 8);
					value = new Rational<sbyte>(numerator, denominator, false);
					break;
				}
				default:
				{
					break;
				}
			}

			return value;
		}

		private static IEnumerable<XmpProperty> ProcessXmp(BitmapMetadata metadata, decimal priority, int depth)
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
					Priority = priority
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
				Console.WriteLine("{0} => {1}: {2}", name, value != null ? value.GetType().Name : "null", Convert.ToString(value));
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
