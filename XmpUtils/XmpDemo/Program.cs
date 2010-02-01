using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using XmpUtils.Xmp;
using System.Xml.Linq;

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
						IEnumerable<XmpProperty> properties = new XmpExtractor().Extract(filename);

						if (properties.Any())
						{
							// serialize properties to XML
							using (TextWriter writer = File.CreateText(filename + ".xmp"))
							{
								new RdfUtility(properties).Document.Save(writer);
							}

							// deserialize properties from XML
							using (TextReader reader = File.OpenText(filename + ".xmp"))
							{
								properties = new RdfUtility(XDocument.Load(reader)).GetProperties(properties.Select(p => p.Schema)).ToList();
							}

							// re-serialize properties to new XML
							using (TextWriter writer = File.CreateText(filename + ".xmp2"))
							{
								new RdfUtility(properties).Document.Save(writer);
							}
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
