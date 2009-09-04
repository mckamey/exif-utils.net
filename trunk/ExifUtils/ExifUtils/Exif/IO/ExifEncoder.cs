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

namespace ExifUtils.Exif.IO
{
	/// <summary>
	/// Encodes the GDI+ representation of EXIF properties.
	/// </summary>
	internal static class ExifEncoder
	{
		#region Byte Encoding

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private static byte[] WriteBytes(object value)
		{
			if (value == null)
			{
				return new byte[0];
			}

			if (value is Array)
			{
				Array array = value as Array;
				int count = array.Length;
				byte[] data = new byte[count];

				for (int i=0; i<count; i++)
				{
					data[i] = Convert.ToByte(array.GetValue(i));
				}

				return data;
			}

			if (value.GetType().IsValueType || value is IConvertible)
			{
				return new byte[]
				{
					Convert.ToByte(value)
				};
			}

			throw new ArgumentException(String.Format("Error converting {0} to byte[].", value.GetType().Name));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private static byte[] WriteUInt16(object value)
		{
			if (value == null)
			{
				return new byte[0];
			}

			if (value is Array)
			{
				Array array = value as Array;
				int count = array.Length;
				byte[] data = new byte[count*ExifDecoder.UInt16Size];

				for (int i=0; i<count; i++)
				{
					byte[] item = BitConverter.GetBytes(Convert.ToUInt16(array.GetValue(i)));
					item.CopyTo(data, i*ExifDecoder.UInt16Size);
				}

				return data;
			}

			if (value.GetType().IsValueType || value is IConvertible)
			{
				return BitConverter.GetBytes(Convert.ToUInt16(value));
			}

			throw new ArgumentException(String.Format("Error converting {0} to UInt16[].", value.GetType().Name));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private static byte[] WriteInt32(object value)
		{
			if (value == null)
			{
				return new byte[0];
			}

			if (value is Array)
			{
				Array array = value as Array;
				int count = array.Length;
				byte[] data = new byte[count*ExifDecoder.Int32Size];

				for (int i=0; i<count; i++)
				{
					byte[] item = BitConverter.GetBytes(Convert.ToInt32(array.GetValue(i)));
					item.CopyTo(data, i*ExifDecoder.Int32Size);
				}

				return data;
			}

			if (value.GetType().IsValueType || value is IConvertible)
			{
				return BitConverter.GetBytes(Convert.ToInt32(value));
			}

			throw new ArgumentException(String.Format("Error converting {0} to Int32[].", value.GetType().Name));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private static byte[] WriteUInt32(object value)
		{
			if (value == null)
			{
				return new byte[0];
			}

			if (value is Array)
			{
				Array array = value as Array;
				int count = array.Length;
				byte[] data = new byte[count*ExifDecoder.UInt32Size];

				for (int i=0; i<count; i++)
				{
					byte[] item = BitConverter.GetBytes(Convert.ToUInt32(array.GetValue(i)));
					item.CopyTo(data, i*ExifDecoder.UInt32Size);
				}

				return data;
			}

			if (value.GetType().IsValueType || value is IConvertible)
			{
				return BitConverter.GetBytes(Convert.ToUInt32(value));
			}

			throw new ArgumentException(String.Format("Error converting {0} to UInt32[].", value.GetType().Name));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private static byte[] WriteRational(object value)
		{
			if (value == null)
			{
				return new byte[0];
			}

			if (value is Array)
			{
				Array array = value as Array;
				int count = array.Length;
				byte[] data = new byte[count*ExifDecoder.RationalSize];

				for (int i=0; i<count; i++)
				{
					Rational<int> item = (Rational<int>)Convert.ChangeType(array.GetValue(i), typeof(Rational<int>));
					BitConverter.GetBytes(item.Numerator).CopyTo(data, i*ExifDecoder.RationalSize);
					BitConverter.GetBytes(item.Numerator).CopyTo(data, i*ExifDecoder.RationalSize+ExifDecoder.Int32Size);
				}

				return data;
			}

			if (value.GetType().IsValueType || value is IConvertible)
			{
				byte[] data = new byte[ExifDecoder.RationalSize];

				Rational<int> item = (Rational<int>)Convert.ChangeType(value, typeof(Rational<int>));
				BitConverter.GetBytes(item.Numerator).CopyTo(data, 0);
				BitConverter.GetBytes(item.Numerator).CopyTo(data, ExifDecoder.UInt32Size);

				return data;
			}

			throw new ArgumentException(String.Format("Error converting {0} to Rational<int>[].", value.GetType().Name));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private static byte[] WriteURational(object value)
		{
			if (value == null)
			{
				return new byte[0];
			}

			if (value is Array)
			{
				Array array = value as Array;
				int count = array.Length;
				byte[] data = new byte[count*ExifDecoder.URationalSize];

				for (int i=0; i<count; i++)
				{
					Rational<uint> item = (Rational<uint>)Convert.ChangeType(array.GetValue(i), typeof(Rational<uint>));
					BitConverter.GetBytes(item.Numerator).CopyTo(data, i*ExifDecoder.URationalSize);
					BitConverter.GetBytes(item.Numerator).CopyTo(data, i*ExifDecoder.URationalSize+ExifDecoder.UInt32Size);
				}

				return data;
			}

			if (value.GetType().IsValueType || value is IConvertible)
			{
				byte[] data = new byte[ExifDecoder.RationalSize];

				Rational<uint> item = (Rational<uint>)Convert.ChangeType(value, typeof(Rational<uint>));
				BitConverter.GetBytes(item.Numerator).CopyTo(data, 0);
				BitConverter.GetBytes(item.Numerator).CopyTo(data, ExifDecoder.UInt32Size);

				return data;
			}

			throw new ArgumentException(String.Format("Error converting {0} to Rational<uint>[].", value.GetType().Name));
		}

		#endregion Byte Encoding

		#region Data Conversion

		public static byte[] ConvertData(Type dataType, ExifType targetType, object value)
		{
			switch (targetType)
			{
				case ExifType.Ascii:
				{
					return Encoding.ASCII.GetBytes(Convert.ToString(value) + '\0');
				}
				case ExifType.Byte:
				case ExifType.Raw:
				{
					if (dataType == typeof(UnicodeEncoding))
					{
						return Encoding.Unicode.GetBytes(Convert.ToString(value) + '\0');
					}

					return ExifEncoder.WriteBytes(value);
				}
				case ExifType.Int32:
				{
					return ExifEncoder.WriteInt32(value);
				}
				case ExifType.Rational:
				{
					return ExifEncoder.WriteRational(value);
				}
				case ExifType.UInt16:
				{
					return ExifEncoder.WriteUInt16(value);
				}
				case ExifType.UInt32:
				{
					return ExifEncoder.WriteUInt32(value);
				}
				case ExifType.URational:
				{
					return ExifEncoder.WriteURational(value);
				}
				default:
				{
					throw new NotImplementedException(String.Format("Encoding for EXIF type \"{0}\" has not yet been implemented.", targetType));
				}
			}
		}

		#endregion Data Conversion
	}
}
