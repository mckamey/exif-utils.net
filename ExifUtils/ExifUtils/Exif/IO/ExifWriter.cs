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
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.IO;

namespace ExifUtils.Exif.IO
{
	/// <summary>
	/// Utility class for writing EXIF data
	/// </summary>
	public static class ExifWriter
	{
		#region Fields

		private static ConstructorInfo ctorPropertyItem = null;

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
			using (Image image = ExifReader.LoadImage(inputPath))
			{
				ExifWriter.AddExifData(image, properties);
				image.Save(outputPath);
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
			using (Image image = ExifReader.LoadImage(inputPath))
			{
				ExifWriter.AddExifData(image, property);
				image.Save(outputPath);
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
				throw new NullReferenceException("image was null");
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
				throw new NullReferenceException("image was null");
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

			switch (property.Type)
			{
				case ExifType.Ascii:
				{
					propertyItem.Value = Encoding.ASCII.GetBytes(Convert.ToString(property.Value)+'\0');
					break;
				}
				case ExifType.Byte:
				{
					if (dataType == typeof(UnicodeEncoding))
					{
						propertyItem.Value = Encoding.Unicode.GetBytes(Convert.ToString(property.Value)+'\0');
					}
					else
					{
						goto default;
					}
					break;
				}
				default:
				{
					throw new NotImplementedException(String.Format("Encoding for EXIF property \"{0}\" has not yet been implemented.", property.DisplayName));
				}
			}
			propertyItem.Len = propertyItem.Value.Length;

			// This appears to not be necessary
			//foreach (int id in image.PropertyIdList)
			//{
			//    if (id == exif.PropertyItem.Id)
			//    {
			//        image.RemovePropertyItem(id);
			//        break;
			//    }
			//}
			image.SetPropertyItem(propertyItem);
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
			if (ExifWriter.ctorPropertyItem == null)
			{
				// Must use Reflection to get access to PropertyItem constructor
				ExifWriter.ctorPropertyItem = typeof(PropertyItem).GetConstructor(Type.EmptyTypes);
				if (ExifWriter.ctorPropertyItem == null)
				{
					throw new NotSupportedException("Unable to instantiate a System.Drawing.Imaging.PropertyItem");
				}
			}

			return (PropertyItem)ExifWriter.ctorPropertyItem.Invoke(null);
		}

		#endregion Utility Methods
	}
}
