//#define DIAGNOSTICS

using System;
using System.IO;
using System.Linq;

using XmpUtils.Xmp;
using XmpUtils.Xmp.Schemas;

namespace XmpDemo
{
	public class Program
	{
		public static void Main()
		{
			// this will rip through all JPEGs in the current directory
			foreach (string filename in Directory.GetFiles(".", "*.jpg", SearchOption.TopDirectoryOnly))
			{
				TextWriter console = Console.Out;
#if DIAGNOSTICS
				using (TextWriter output = File.CreateText(filename + ".txt"))
#endif
				{
					console.WriteLine("Processing "+filename);

#if DIAGNOSTICS
					Console.SetOut(output);
					try
#endif
					{
						// extract properties out of JPEG
						XmpPropertyCollection properties = XmpPropertyCollection.LoadFromImage(filename);

						// serialize properties to XML
						using (TextWriter writer = File.CreateText(filename + ".xmp"))
						{
							properties.SaveAsXml(writer);
						}
					}
#if DIAGNOSTICS
					finally
					{
						Console.SetOut(console);
					}
#endif
				}
			}

			// this will rip through all XMPs in the current directory
			foreach (string filename in Directory.GetFiles(".", "*.xmp", SearchOption.TopDirectoryOnly))
			{
				Console.Out.WriteLine("Processing "+filename);

				// deserialize properties from XML
				XmpPropertyCollection properties = XmpPropertyCollection.LoadFromXml(filename);

				// custom value extractions
				XmpMetadata meta = XmpMetadata.Create(properties);
				meta.Creator = "Changed the creator via XmpProperty";
				meta.Copyright = "Copyright changed as well.";
				meta.Tags = new string[]
				{
					"Keyword-1",
					"Tag-2",
					"Subject-3"
				};
				meta.Apply(properties);

				// re-serialize properties to new XML
				using (TextWriter writer = File.CreateText(Path.GetFileNameWithoutExtension(filename) + ".xml"))
				{
					properties.SaveAsXml(writer);
				}
			}
		}
	}
}
