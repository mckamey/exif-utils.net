#region License
/*---------------------------------------------------------------------------------*\

	Distributed under the terms of an MIT-style license:

	The MIT License

	Copyright (c) 2005-2009 Stephen M. McKamey

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

using ExifUtils.Exif;
using ExifUtils.Exif.IO;

namespace ExifDemo
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
