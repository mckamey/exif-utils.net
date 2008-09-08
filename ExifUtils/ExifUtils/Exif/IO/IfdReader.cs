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
using System.Drawing.Imaging;

namespace ExifUtils.Exif.IO
{
	#region Example Usage

	//class Program
	//{
	//    static void Main(string[] args)
	//    {
	//        const string FileName = @"..\..\..\Example.jpg";
	//        System.Drawing.Imaging.PropertyItem prop;
	//        using (System.Drawing.Bitmap photo = new System.Drawing.Bitmap(FileName))
	//        {
	//            prop = photo.GetPropertyItem(/*MakerNote*/0x927c);
	//        }
	//        using (System.IO.FileStream file = new System.IO.FileStream(FileName, System.IO.FileMode.Open))
	//        {
	//            PropertyItem[] makerNotes = IfdReader.DecodeIFD(prop.Value, file);
	//        }
	//    }
	//}

	#endregion Example Usage

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
		/// <param name="fullFile"></param>
		/// <returns></returns>
		/// <remarks>
		/// References:
		/// http://www.ee.cooper.edu/courses/course_pages/past_courses/EE458/TIFF/
		/// http://search.cpan.org/src/EXIFTOOL/Image-ExifTool-6.36/html/TagNames/Canon.html
		/// http://www.burren.cx/david/canon.html
		/// http://cpan.uwinnipeg.ca/htdocs/Image-ExifTool/Image/ExifTool/Canon.pm.html
		/// </remarks>
		public static PropertyItem[] DecodeIFD(byte[] bytes, FileStream fullFile)
		{
			int index = 0;
			int count = (int)BitConverter.ToUInt16(bytes, index);
			index += UInt16_Size;
			PropertyItem[] items = new PropertyItem[count];

			for (int i=0; i<count; i++)
			{
				items[i] = ExifWriter.CreatePropertyItem();

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

		#region Utility Methods

		private static byte[] CopyBytes(byte[] bytes, int index, int length)
		{
#if DEBUG
			try
#endif
			{
				byte[] data = new byte[length];
				Array.Copy(bytes, index, data, 0, length);
				return data;
			}
#if DEBUG
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
				{
					return 1;
				}
				case 2:/* ascii strings */
				{
					return 1;
				}
				case 3:/* unsigned short */
				{
					return 2;
				}
				case 4:/* unsigned long */
				{
					return 4;
				}
				case 5:/* unsigned rational */
				{
					return 8;
				}
				case 6:/* signed byte */
				{
					return 1;
				}
				case 7:/* undefined */
				{
					return 1;
				}
				case 8:/* signed short */
				{
					return 2;
				}
				case 9:/* signed long */
				{
					return 4;
				}
				case 10:/* signed rational */
				{
					return 8;
				}
				case 11:/* single float */
				{
					return 4;
				}
				case 12:/* double float */
				{
					return 8;
				}
				default:
				{
					return 0;
				}
			}
		}

		#endregion Utility Methods
	}

	#endregion IfdReader
}
