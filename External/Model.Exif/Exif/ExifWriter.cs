using System;
using System.Text;
using System.Drawing;

namespace PhotoLib.Model.Exif
{
	/// <summary>
	/// Utility class for writing EXIF data
	/// </summary>
	public static class ExifWriter
	{
		#region Methods

		/// <summary>
		/// Adds an EXIF property to an image
		/// </summary>
		/// <param name="image"></param>
		/// <param name="exif"></param>
		public static void AddToImage(Image image, ExifProperty exif)
		{
			#region Create a PropertyItem

			if (exif.PropertyItem == null)
			{
				if (image.PropertyItems != null && image.PropertyItems.Length > 0)
				{
					exif.PropertyItem = image.PropertyItems[0];
				}
				else
				{
					throw new NotImplementedException("Can only add EXIF properties to images which already have properties.");
				}
			}

			#endregion Create a PropertyItem

			exif.PropertyItem.Id = (int)exif.Tag;
			exif.PropertyItem.Type = (short)exif.Type;

			Type dataType = ExifDataTypeAttribute.GetDataType(exif.Tag);

			switch (exif.Type)
			{
				case ExifType.Ascii:
				{
					exif.PropertyItem.Value = Encoding.ASCII.GetBytes(Convert.ToString(exif.Value)+'\0');
					break;
				}
				case ExifType.Byte:
				{
					if (dataType == typeof(UnicodeEncoding))
					{
						exif.PropertyItem.Value = Encoding.Unicode.GetBytes(Convert.ToString(exif.Value)+'\0');
					}
					else
					{
						goto default;
					}
					break;
				}
				default:
				{
					throw new NotImplementedException(String.Format("Encoding for EXIF property \"{0}\" has not yet been implemented.", exif.DisplayName));
				}
			}
			exif.PropertyItem.Len = exif.PropertyItem.Value.Length;

			//foreach (int id in image.PropertyIdList)
			//{
			//    if (id == exif.PropertyItem.Id)
			//    {
			//        image.RemovePropertyItem(id);
			//        break;
			//    }
			//}
			image.SetPropertyItem(exif.PropertyItem);
		}

		#endregion Methods
	}
}
