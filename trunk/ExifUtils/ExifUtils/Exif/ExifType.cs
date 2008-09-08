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

namespace ExifUtils.Exif
{
	/// <summary>
	/// Defines types for Exif Properties.
	/// </summary>
	/// <remarks>
	/// http://msdn.microsoft.com/library/en-us/gdicpp/GDIPlus/GDIPlusreference/constants/imagepropertytagtypeconstants.asp
	/// </remarks>
	public enum ExifType
	{
		/// <summary>
		/// Specifies that the type is either unknown or not defined.
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// Specifies that the value data member is an array of bytes.
		/// </summary>
		Byte = 1,

		/// <summary>
		/// Specifies that the value data member is a null-terminated ASCII string.
		/// </summary>
		/// <remarks>If you set <see cref="ExifProperty.Type">ExifProperty.Type</see> to <see cref="ExifProperty.Type"/>, you should set the length data member to the length of the string including the NULL terminator. For example, the string HELLO would have a length of 6.</remarks>
		Ascii = 2,

		/// <summary>
		/// Specifies that the value data member is an array of signed short (16-bit) integers.
		/// </summary>
		UInt16 = 3,

		/// <summary>
		/// Specifies that the value data member is an array of unsigned long (32-bit) integers.
		/// </summary>
		UInt32 = 4,

		/// <summary>
		/// Specifies that the value data member is an array of pairs of unsigned long integers. Each pair represents a fraction; the first integer is the numerator and the second integer is the denominator.
		/// </summary>
		URational = 5,

		/// <summary>
		/// Specifies that the value data member is an array of bytes that can hold values of any data type.
		/// </summary>
		Raw = 7,

		/// <summary>
		/// Specifies that the value data member is an array of signed long (32-bit) integers.
		/// </summary>
		Int32 = 9,

		/// <summary>
		/// Specifies that the value data member is an array of pairs of signed long integers. Each pair represents a fraction; the first integer is the numerator and the second integer is the denominator.
		/// </summary>
		Rational = 10
	}
}
