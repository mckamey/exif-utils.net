using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using XmpUtils.Xmp;

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
						IEnumerable<XmpProperty> properties = new XmpExtractor().Extract(filename);

						if (properties.Any())
						{
							using (TextWriter writer = File.CreateText(filename + ".xmp"))
							{
								new RdfUtility().ToXml(properties).Save(writer);
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
