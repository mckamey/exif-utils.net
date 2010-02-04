using System;
using System.Collections;
using System.Collections.Generic;
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
			// this will rip through all JPEGs in the current direction
			foreach (string filename in Directory.GetFiles(".", "*.jpg", SearchOption.TopDirectoryOnly))
			{
				TextWriter console = Console.Out;
				using (TextWriter output = File.CreateText(filename + ".txt"))
				{
					console.WriteLine("Processing "+filename);

					Console.SetOut(output);
					try
					{
						// extract properties out of JPEG
						XmpPropertyCollection properties = XmpPropertyCollection.LoadFromImage(filename);

						if (properties.Any())
						{
							// serialize properties to XML
							using (TextWriter writer = File.CreateText(filename + ".xmp"))
							{
								properties.SaveAsXml(writer);
							}

							List<XmpProperty> propertyList;

							// deserialize properties from XML
							using (TextReader reader = File.OpenText(filename + ".xmp"))
							{
								IEnumerable<Enum> schemas = properties.Select(p => p.Schema);
								propertyList = XmpPropertyCollection.LoadFromXml(reader).GetProperties(schemas).ToList();
							}

							// re-serialize properties to new XML
							using (TextWriter writer = File.CreateText(filename + ".xmp2"))
							{
								new XmpPropertyCollection(propertyList).SaveAsXml(writer);
							}

							// test single value extraction
							IEnumerable<string> tags = properties.GetValue(DublinCoreSchema.Subject, (IEnumerable<string>)null);
						}
					}
					finally
					{
						Console.SetOut(console);
					}
				}
			}
		}
	}
}
