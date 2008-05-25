using System;
using System.Drawing;

using PhotoLib.Model.Exif;

namespace ExifDemoDriver
{
	class Program
	{
		static void Main(string[] args)
		{
			// choose an image
			Console.Write("Enter image load path: ");
			string imagePath = Console.ReadLine();
			Console.WriteLine();

			//----------------------------------------------

			// minimally loads image and closes it
			ExifPropertyCollection properties = ExifReader.GetExifData(imagePath);

			// dump properties to console
			foreach (ExifProperty property in properties)
			{
				Console.WriteLine("{0}: {1}", property.DisplayName, property.DisplayValue);
			}
			Console.WriteLine();

			//----------------------------------------------

			int lastDot = imagePath.LastIndexOf('.');
			string outputPath = imagePath.Substring(0, lastDot)+"_COPYRIGHT_LOREM_IPSUM"+imagePath.Substring(lastDot);
			Console.WriteLine("Adding dummy copyright to image and saving to:\r\n\t"+outputPath);

			// add copyright tag
			ExifProperty copyright = new ExifProperty();
			copyright.Tag = ExifTag.Copyright;
			copyright.Value = String.Format(
				"Copyright (c){0} Lorem ipsum dolor sit amet. All rights reserved.",
				DateTime.Now.Year);

			ExifWriter.AddExifData(imagePath, outputPath, copyright);
		}
	}
}
