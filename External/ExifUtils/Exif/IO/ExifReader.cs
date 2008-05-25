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
