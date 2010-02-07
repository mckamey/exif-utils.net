//#define DIAGNOSTICS

#region License
/*---------------------------------------------------------------------------------*\

	Distributed under the terms of an MIT-style license:

	The MIT License

	Copyright (c) 2005-2010 Stephen M. McKamey

	Permission is hereby granted, free of charge, to any person obtaining a copy
	of this software and associated documentation files (the "Software"), to deal
	in the Software without restriction, including without limitation the rights
	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
	copies of the Software, and to permit persons to whom the Software is
	furnished to do so, subject to the following conditions:

	The above copyright notice and this permission notice shall be included in
	all copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
	THE SOFTWARE.

\*---------------------------------------------------------------------------------*/
#endregion License

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
				ImageXmp meta = ImageXmp.Create(properties);
				meta.Creator = "Changed the creator via XmpProperty";
				meta.Copyright = "Copyright changed as well.";
				meta.Tags = new string[]
				{
					"Keyword-1",
					"Tag-2",
					"Subject-3"
				};

				// apply values back into properties
				properties[DublinCoreSchema.Creator] = meta.Creator;
				properties[DublinCoreSchema.Rights] = meta.Copyright;
				properties[DublinCoreSchema.Subject] = meta.Tags;

				// re-serialize properties to new XML
				using (TextWriter writer = File.CreateText(Path.GetFileNameWithoutExtension(filename) + ".xml"))
				{
					properties.SaveAsXml(writer);
				}
			}
		}
	}
}
