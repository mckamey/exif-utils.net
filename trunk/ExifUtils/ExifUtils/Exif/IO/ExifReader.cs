#region License
/*---------------------------------------------------------------------------------*\

	Distributed under the terms of an MIT-style license:

	The MIT License

	Copyright (c) 2005-2006 Stephen M. McKamey

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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace ExifUtils.Exif.IO
{
	/// <summary>
	/// Utility class for reading EXIF data 
	/// </summary>
	public static class ExifReader
	{
		#region Methods

		/// <summary>
		/// Creates a ExifPropertyCollection from the PropertyItems of a Bitmap.
		/// </summary>
		/// <param name="image"></param>
		/// <param name="exifTags">additional EXIF tags to include</param>
		/// <returns></returns>
		public static ExifPropertyCollection GetExifData(Image image, ICollection<ExifTag> exifTags)
		{
			if (exifTags == null)
			{
				return new ExifPropertyCollection(image.PropertyItems);
			}

			return new ExifPropertyCollection(image.PropertyItems, exifTags);
		}

		/// <summary>
		/// Creates a ExifPropertyCollection from the PropertyItems of a Bitmap.
		/// </summary>
		/// <param name="image"></param>
		/// <returns></returns>
		public static ExifPropertyCollection GetExifData(Image image)
		{
			return ExifReader.GetExifData(image, null);
		}

		/// <summary>
		/// Creates a ExifPropertyCollection from an image file path.
		/// Minimally loads image only enough to get PropertyItems.
		/// </summary>
		/// <param name="imagePath"></param>
		/// <param name="exifTags">collection of EXIF tags to include</param>
		/// <returns>Collection of ExifProperty items</returns>
		public static ExifPropertyCollection GetExifData(string imagePath, ICollection<ExifTag> exifTags)
		{
			PropertyItem[] propertyItems;

			using (FileStream stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
			{
				// minimally load image
				using (Image image = Image.FromStream(stream, true, false))
				{
					propertyItems = image.PropertyItems;
				}
			}

			if (exifTags == null)
			{
				return new ExifPropertyCollection(propertyItems);
			}

			return new ExifPropertyCollection(propertyItems, exifTags);
		}

		/// <summary>
		/// Creates a ExifPropertyCollection from an image file path.
		/// Minimally loads image only enough to get PropertyItems.
		/// </summary>
		/// <param name="imagePath"></param>
		/// <returns>Collection of ExifProperty items</returns>
		public static ExifPropertyCollection GetExifData(string imagePath)
		{
			return ExifReader.GetExifData(imagePath, null);
		}

		#endregion Methods
	}
}
