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
			ExifPropertyCollection properties = ExifReader.LoadFromFile(imagePath);

			// dump properties to console
			foreach (ExifProperty property in properties)
			{
				Console.WriteLine("{0}: {1}", property.DisplayName, property.DisplayValue);
			}
			Console.WriteLine();

			//----------------------------------------------

			using (Image image = Image.FromFile(imagePath))
			{
				imagePath = imagePath.Substring(0, imagePath.LastIndexOf('.'))+"_COPYRIGHT_LOREM_IPSUM"+imagePath.Substring(imagePath.LastIndexOf('.'));
				Console.WriteLine("Adding dummy copyright to image and saving to: "+imagePath);

				// add copyright tag
				ExifProperty copyright = new ExifProperty();
				copyright.Tag = ExifTag.Copyright;
				copyright.Value = "Copyright (c)2006 Lorem ipsum dolor sit amet.";
				copyright.AddExifToImage(image);

				// save a copy
				image.Save(imagePath);
			}
		}
	}
}
