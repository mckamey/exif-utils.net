using System;

namespace ExifUtils.Exif
{
	#region Example Usage

	//class Program
	//{
	//    static void Main(string[] args)
	//    {
	//        const string FILENAME = @"..\..\..\Tims_10D.JPG";
	//        System.Drawing.Imaging.PropertyItem2 prop;
	//        using (System.Drawing.Bitmap photo = new System.Drawing.Bitmap(FILENAME))
	//        {
	//            prop = photo.GetPropertyItem(/*MakerNote*/0x927c);
	//        }
	//        using (System.IO.FileStream file = new System.IO.FileStream(FILENAME, System.IO.FileMode.Open))
	//        {
	//            PropertyItem2[] makerNotes = IfdReader.DecodeIFD(prop.Value, file);
	//        }
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
		/// http://www.ee.cooper.edu/courses/course_pages/past_courses/EE458/TIFF/
		/// http://search.cpan.org/src/EXIFTOOL/Image-ExifTool-6.36/html/TagNames/Canon.html
		/// http://www.burren.cx/david/canon.html
		/// http://cpan.uwinnipeg.ca/htdocs/Image-ExifTool/Image/ExifTool/Canon.pm.html
		/// </remarks>
		public static PropertyItem2[] DecodeIFD(byte[] bytes, System.IO.FileStream fullFile)
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
					items[i].Value = new byte[length];//CopyBytes(bytes, offset, length);
					fullFile.Position = offset;
					fullFile.Read(items[i].Value, 0, length);
				}
				else
				{
					// read in the Data as byte[]
					items[i].Value = CopyBytes(bytes, index, length);
				}
				index += UInt32_Size;
			}

			return items;
		}

		#endregion Methods

		#region Helper Methods

		private static byte[] CopyBytes(byte[] bytes, int index, int length)
		{
#if DEBUG
			try
			{
#endif
			byte[] data = new byte[length];
			Array.Copy(bytes, index, data, 0, length);
			return data;
#if DEBUG
			}
			catch
			{
				return null;
			}
#endif
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
