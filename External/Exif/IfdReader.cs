using System;

namespace PhotoLib.Model.Exif
{
	#region Example Usage

	//class Program
	//{
	//    static void Main(string[] args)
	//    {
	//        System.Drawing.Bitmap photo = new System.Drawing.Bitmap(@"..\..\..\IMG_0048.JPG");

	//        System.Drawing.Imaging.PropertyItem2 prop = photo.GetPropertyItem(/*MakerNote*/0x927c);
	//        PropertyItem2[] makerNotes = IfdReader.DecodeIFD(prop.Value);
	//    }
	//}

	#endregion Example Usage

	#region PropertyItem2

	internal class PropertyItem2
	{
		#region Fields

		private int id;
		private int len;
		private short type;
		private byte[] value;

		#endregion Fields

		#region Init

		public PropertyItem2()
		{
		}

		#endregion Init

		#region Properties

		public int Id
		{
			get { return this.id; }
			set { this.id = value; }
		}

		public int Len
		{
			get { return this.len; }
			set { this.len = value; }
		}

		public short Type
		{
			get { return this.type; }
			set { this.type = value; }
		}

		public byte[] Value
		{
			get { return this.value; }
			set { this.value = value; }
		}

		#endregion Properties
	}

	#endregion PropertyItem2

	#region IfdReader

	internal static class IfdReader
	{
		#region Constants

		private static readonly int UInt16_Size = sizeof(UInt16);
		private static readonly int UInt32_Size = sizeof(UInt32);

		#endregion Constants

		#region Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		/// <remarks>
		/// References:
		/// http://search.cpan.org/src/EXIFTOOL/Image-ExifTool-6.36/html/TagNames/Canon.html
		/// http://www.burren.cx/david/canon.html
		/// http://cpan.uwinnipeg.ca/htdocs/Image-ExifTool/Image/ExifTool/Canon.pm.html
		/// </remarks>
		public static PropertyItem2[] DecodeIFD(byte[] bytes)
		{
			int index = 0;
			int count = (int)BitConverter.ToUInt16(bytes, index);
			index += UInt16_Size;
			PropertyItem2[] items = new PropertyItem2[count];

			for (int i=0; i<count; i++)
			{
				items[i] = new PropertyItem2();

				// read in the ID (2 bytes)
				items[i].Id = (int)BitConverter.ToUInt16(bytes, index);
				index += UInt16_Size;

				// read in the Type (2 bytes)
				items[i].Type = (short)BitConverter.ToUInt16(bytes, index);
				index += UInt16_Size;

				// read in the Length (4 bytes)
				items[i].Len = (int)BitConverter.ToUInt32(bytes, index);
				index += UInt32_Size;

				int length = GetSizeOf(items[i].Type) * items[i].Len;
				if (length > 4)
				{
					// read in the Data as offset (4 bytes)
					int offset = (int)BitConverter.ToUInt32(bytes, index);
#if DEBUG
					try {
#endif
					items[i].Value = CopyBytes(bytes, offset, length);
#if DEBUG
					} catch {}
#endif
					index += UInt32_Size;
				}
				else
				{
					// read in the Data as byte[]
					items[i].Value = CopyBytes(bytes, index, length);
					index += length;
				}
			}

			return items;
		}

		#endregion Methods

		#region Helper Methods

		private static byte[] CopyBytes(byte[] bytes, int index, int length)
		{
			byte[] data = new byte[length];
			Array.Copy(bytes, index, data, 0, length);
			return data;
		}

		private static int GetSizeOf(short type)
		{
			switch (type)
			{
				case 1:/* unsigned byte */
					return 1;
				case 2:/* ascii strings */
					return 1;
				case 3:/* unsigned short */
					return 2;
				case 4:/* unsigned long */
					return 4;
				case 5:/* unsigned rational */
					return 8;
				case 6:/* signed byte */
					return 1;
				case 7:/* undefined */
					return 1;
				case 8:/* signed short */
					return 2;
				case 9:/* signed long */
					return 4;
				case 10:/* signed rational */
					return 8;
				case 11:/* single float */
					return 4;
				case 12:/* double float */
					return 8;

				default:
					return 0;
			}
		}

		#endregion Helper Methods
	}

	#endregion IfdReader
}
