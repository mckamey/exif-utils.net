using System;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;

namespace ExifUtils.Exif.IO
{
	/// <summary>
	/// Utility class for writing EXIF data
	/// </summary>
	public static class ExifWriter
	{
		#region Write Methods

		/// <summary>
		/// Adds a collection of EXIF properties to an image.
		/// </summary>
		/// <param name="inputPath">file path of original image</param>
		/// <param name="outputPath">file path of modified image</param>
		/// <param name="properties"></param>
		public static void AddExifData(string inputPath, string outputPath, ExifPropertyCollection properties)
		{
			using (Image image = Image.FromFile(inputPath))
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
			using (Image image = Image.FromFile(inputPath))
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

			if (image.PropertyItems == null || image.PropertyItems.Length < 1)
			{
				// Must use Reflection to get access to PropertyItem constructor
				ConstructorInfo ctor = typeof(PropertyItem).GetConstructor(Type.EmptyTypes);
				if (ctor == null)
				{
					throw new NotSupportedException("Unable to instantiate a System.Drawing.Imaging.PropertyItem");
				}

				propertyItem = ctor.Invoke(null) as PropertyItem;
			}
			else
			{
				// The .NET interface for GDI+ does not allow instantiation of the
				// PropertyItem class. Therefore one must be stolen off the Image
				// and repurposed.  GDI+ uses PropertyItem by value so there is no
				// side effect when changing the values and reassigning to the image.
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
	}
}
