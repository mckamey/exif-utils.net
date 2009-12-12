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
using System.Reflection;
using System.Text;

namespace ExifUtils.Exif.IO
{
	/// <summary>
	/// Utility class for writing EXIF data
	/// </summary>
	public static class ExifWriter
	{
		#region Fields

		private static ConstructorInfo PropertyItem_Ctor = null;

		#endregion Fields

		#region Write Methods

		/// <summary>
		/// Adds a collection of EXIF properties to an image.
		/// </summary>
		/// <param name="inputPath">file path of original image</param>
		/// <param name="outputPath">file path of modified image</param>
		/// <param name="properties"></param>
		public static void AddExifData(string inputPath, string outputPath, ExifPropertyCollection properties)
		{
			// minimally load image
			Image image;
			using (ExifReader.LoadImage(inputPath, out image))
			{
				using (image)
				{
					ExifWriter.AddExifData(image, properties);
					image.Save(outputPath);
				}
			}
		}

		/// <summary>
		/// Adds an EXIF property to an image.
		/// </summary>
		/// <param name="inputPath">file path of original image</param>
		/// <param name="outputPath">file path of modified image</param>
		/// <param name="property"></param>
		public static void AddExifData(string inputPath, string outputPath, ExifProperty property)
		{
			// minimally load image
			Image image;
			using (ExifReader.LoadImage(inputPath, out image))
			{
				using (image)
				{
					ExifWriter.AddExifData(image, property);
					image.Save(outputPath);
				}
			}
		}

		/// <summary>
		/// Adds a collection of EXIF properties to an image.
		/// </summary>
		/// <param name="image"></param>
		/// <param name="properties"></param>
		public static void AddExifData(Image image, ExifPropertyCollection properties)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}

			if (properties == null || properties.Count < 1)
			{
				return;
			}

			foreach (ExifProperty property in properties)
			{
				ExifWriter.AddExifData(image, property);
			}
		}

		/// <summary>
		/// Adds an EXIF property to an image.
		/// </summary>
		/// <param name="image"></param>
		/// <param name="property"></param>
		public static void AddExifData(Image image, ExifProperty property)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			if (property == null)
			{
				return;
			}

			PropertyItem propertyItem;

			// The .NET interface for GDI+ does not allow instantiation of the
			// PropertyItem class. Therefore one must be stolen off the Image
			// and repurposed.  GDI+ uses PropertyItem by value so there is no
			// side effect when changing the values and reassigning to the image.
			if (image.PropertyItems == null || image.PropertyItems.Length < 1)
			{
				propertyItem = ExifWriter.CreatePropertyItem();
			}
			else
			{
				propertyItem = image.PropertyItems[0];
			}

			propertyItem.Id = (int)property.Tag;
			propertyItem.Type = (short)property.Type;

			Type dataType = ExifDataTypeAttribute.GetDataType(property.Tag);

			propertyItem.Value = ExifEncoder.ConvertData(dataType, property.Type, property.Value);
			propertyItem.Len = propertyItem.Value.Length;

			// This appears to not be necessary
			ExifWriter.RemoveExifData(image, property.Tag);
			image.SetPropertyItem(propertyItem);
		}

		/// <summary>
		/// Remvoes EXIF properties from an image.
		/// </summary>
		/// <param name="inputPath">file path of original image</param>
		/// <param name="outputPath">file path of modified image</param>
		/// <param name="exifTags">tags to remove</param>
		public static void RemoveExifData(string inputPath, string outputPath, params ExifTag[] exifTags)
		{
			ExifWriter.RemoveExifData(inputPath, outputPath, (IEnumerable<ExifTag>)exifTags);
		}

		/// <summary>
		/// Remvoes EXIF properties from an image.
		/// </summary>
		/// <param name="inputPath">file path of original image</param>
		/// <param name="outputPath">file path of modified image</param>
		/// <param name="exifTags">tags to remove</param>
		public static void RemoveExifData(string inputPath, string outputPath, IEnumerable<ExifTag> exifTags)
		{
			// minimally load image
			Image image;
			using (ExifReader.LoadImage(inputPath, out image))
			{
				using (image)
				{
					ExifWriter.RemoveExifData(image, exifTags);
					image.Save(outputPath);
				}
			}
		}

		/// <summary>
		/// Remvoes EXIF properties from an image.
		/// </summary>
		/// <param name="image"></param>
		/// <param name="exifTags">tags to remove</param>
		public static void RemoveExifData(Image image, params ExifTag[] exifTags)
		{
			ExifWriter.RemoveExifData(image, (IEnumerable<ExifTag>)exifTags);
		}

		/// <summary>
		/// Remvoes EXIF properties from an image.
		/// </summary>
		/// <param name="image"></param>
		/// <param name="exifTags">tags to remove</param>
		public static void RemoveExifData(Image image, IEnumerable<ExifTag> exifTags)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			if (exifTags == null)
			{
				return;
			}

			foreach (ExifTag tag in exifTags)
			{
				int propertyID = (int)tag;
				foreach (int id in image.PropertyIdList)
				{
					if (id == propertyID)
					{
						image.RemovePropertyItem(propertyID);
						break;
					}
				}
			}
		}

		#endregion Write Methods

		#region Copy Methods

		/// <summary>
		/// Copies EXIF data from one image to another
		/// </summary>
		/// <param name="source"></param>
		/// <param name="dest"></param>
		public static void CloneExifData(Image source, Image dest)
		{
			ExifWriter.CloneExifData(source, dest, -1);
		}

		/// <summary>
		/// Copies EXIF data from one image to another
		/// </summary>
		/// <param name="source"></param>
		/// <param name="dest"></param>
		/// <param name="maxPropertyBytes">setting to filter properties</param>
		public static void CloneExifData(Image source, Image dest, int maxPropertyBytes)
		{
			bool filter = (maxPropertyBytes > 0);

			// preserve EXIF
			foreach (PropertyItem prop in source.PropertyItems)
			{
				if (filter && prop.Len > maxPropertyBytes)
				{
					// skip large sections
					continue;
				}

				dest.SetPropertyItem(prop);
			}
		}

		#endregion Copy Methods

		#region Utility Methods

		/// <summary>
		/// Uses Reflection to instantiate a PropertyItem
		/// </summary>
		/// <returns></returns>
		internal static PropertyItem CreatePropertyItem()
		{
			if (ExifWriter.PropertyItem_Ctor == null)
			{
				// Must use Reflection to get access to PropertyItem constructor
				ExifWriter.PropertyItem_Ctor = typeof(PropertyItem).GetConstructor(BindingFlags.NonPublic|BindingFlags.Instance, null, Type.EmptyTypes, null);
				if (ExifWriter.PropertyItem_Ctor == null)
				{
					throw new NotSupportedException("Unable to instantiate a "+typeof(PropertyItem).FullName);
				}
			}

			return (PropertyItem)ExifWriter.PropertyItem_Ctor.Invoke(null);
		}

		#endregion Utility Methods
	}
}
