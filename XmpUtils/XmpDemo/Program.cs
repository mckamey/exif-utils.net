using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

using XmpUtils.Xmp;

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
				value = ProcessMetadata(metadata.GetQuery("/xmp") as BitmapMetadata, "/xmp", 0);
			}

			IEnumerable<XmpProperty> properties = value as IEnumerable<XmpProperty>;
			if (properties != null && properties.Count() > 0)
			{
				using (TextWriter writer = File.CreateText(filename + ".xmp"))
				{
					new RdfUtility().ToXml(properties).Save(writer);
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
				case "xmpalt":
				{
					return ProcessXmpArray(metadata, depth+1);
				}
				case "xmpstruct":
				{
					return ProcessXmpStruct(metadata, depth+1);
				}
				case "xmp":
				{
					return ProcessXmp(metadata, depth+1);
				}
				default:
				{
					return ProcessXmpStruct(metadata, depth+1);
				}
			}
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

		private static Array ProcessXmpArray(BitmapMetadata metadata, int depth)
		{
			ArrayList array = new ArrayList();

			foreach (string name in metadata)
			{
				// TODO: use the name as the xml:lang in xmpalt

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
					Schema = schema
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
