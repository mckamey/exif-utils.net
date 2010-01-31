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
			Console.Write("Enter filename: ");

			string filename = Console.ReadLine();

			IEnumerable<XmpProperty> properties = new XmpExtractor().Extract(filename);

			if (properties.Any())
			{
				using (TextWriter writer = File.CreateText(filename + ".xmp"))
				{
					new RdfUtility().ToXml(properties).Save(writer);
				}
			}
		}
	}
}
