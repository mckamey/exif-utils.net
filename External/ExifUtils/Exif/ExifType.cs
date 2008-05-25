using System;

namespace PhotoLib.Model.Exif
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
