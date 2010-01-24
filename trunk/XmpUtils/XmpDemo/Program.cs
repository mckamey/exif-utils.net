using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;

using XmpUtils.Xmp;

namespace XmpDemo
{
	public class Program
	{
		private static List<XmpProperty> properties = new List<XmpProperty>();

		public static void Main()
		{
			Console.Write("Enter filename: ");

			string filename = Console.ReadLine();

			using (Stream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				BitmapMetadata metadata = LoadMetadata(stream);
				Parse(metadata, "/", 0);
			}

			if (properties.Count > 0)
			{
				using (TextWriter writer = File.CreateText(filename + ".xmp"))
				{
					new RdfUtility().ToXml(properties).Save(writer);
				}
			}
		}

		private static void Parse(BitmapMetadata metadata, string objName, int depth)
		{
			Console.Write(new String('\t', depth));
			Console.WriteLine("{0} => {1}", objName, metadata.Format);
			depth++;

			foreach (string name in metadata)
			{
				object value = metadata.GetQuery(name);
				if (value == null)
				{
					continue;
				}

				if (value is BitmapMetadata)
				{
					Parse((BitmapMetadata)value, name, depth);
					continue;
				}

				if (value is BitmapMetadataBlob)
				{
					value = ((BitmapMetadataBlob)value).GetBlobValue();
				}

				Console.Write(new String('\t', depth));
				Console.WriteLine("{0} => {1}: {2}", name, value.GetType(), Convert.ToString(value));

				Enum property = (Enum)XmpNamespaceUtility.Instance.Parse(name.Substring(1));
				if (property != null)
				{
					Program.properties.Add(new XmpProperty
					{
						Schema = property,
						Value = value
					});
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
