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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ExifUtils.Exif.IO
{
	/// <summary>
	/// Utility class for reading EXIF data 
	/// </summary>
	public static class ExifReader
	{
		#region Methods

		/// <summary>
		/// Creates a ExifPropertyCollection from an image file path.
		/// Minimally loads image only enough to get PropertyItems.
		/// </summary>
		/// <param name="imagePath"></param>
		/// <param name="exifTags">filter of EXIF tags to include</param>
		/// <returns>Collection of ExifProperty items</returns>
		public static ExifPropertyCollection GetExifData(string imagePath, params ExifTag[] exifTags)
		{
			return ExifReader.GetExifData(imagePath, (ICollection<ExifTag>)exifTags);
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

			// minimally load image
			Image image;
			using (ExifReader.LoadImage(imagePath, out image))
			{
				using (image)
				{
					propertyItems = image.PropertyItems;
				}
			}

			return new ExifPropertyCollection(propertyItems, exifTags);
		}

		/// <summary>
		/// Creates a ExifPropertyCollection from the PropertyItems of a Bitmap.
		/// </summary>
		/// <param name="image"></param>
		/// <param name="exifTags">filter of EXIF tags to include</param>
		/// <returns></returns>
		public static ExifPropertyCollection GetExifData(Image image, params ExifTag[] exifTags)
		{
			return ExifReader.GetExifData(image, (ICollection<ExifTag>)exifTags);
		}

		/// <summary>
		/// Creates a ExifPropertyCollection from the PropertyItems of a Bitmap.
		/// </summary>
		/// <param name="image"></param>
		/// <param name="exifTags">filter of EXIF tags to include</param>
		/// <returns></returns>
		public static ExifPropertyCollection GetExifData(Image image, ICollection<ExifTag> exifTags)
		{
			if (image == null)
			{
				throw new NullReferenceException("image");
			}

			return new ExifPropertyCollection(image.PropertyItems, exifTags);
		}

		#endregion Methods

		#region Utility Methods

		/// <summary>
		/// Minimally load image without verifying image data.
		/// </summary>
		/// <param name="imagePath"></param>
		/// <param name="image">the loaded image object</param>
		/// <returns>the stream object to dispose of when finished</returns>
		internal static IDisposable LoadImage(string imagePath, out Image image)
		{
			FileStream stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
			image = Image.FromStream(stream, false, false);
			return stream;
		}

		#endregion Utility Methods
	}
}
