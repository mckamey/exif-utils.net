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
	/// Attribute which provides hints about EXIF data types
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple=false)]
	internal sealed class ExifDataTypeAttribute : Attribute
	{
		#region Fields

		private ExifType exifType = ExifType.Unknown;
		private Type dataType = null;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		public ExifDataTypeAttribute()
		{
		}

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="dataType"></param>
		public ExifDataTypeAttribute(Type dataType)
		{
			this.dataType = dataType;
		}

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="exifType"></param>
		public ExifDataTypeAttribute(ExifType exifType)
		{
			this.exifType = exifType;
		}

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="type"></param>
		/// <param name="exifType"></param>
		public ExifDataTypeAttribute(Type type, ExifType exifType)
		{
			this.dataType = type;
			this.exifType = exifType;
		}

		#endregion Init

		#region Properties

		/// <summary>
		/// 
		/// </summary>
		public Type DataType
		{
			get { return this.dataType; }
		}

		/// <summary>
		/// 
		/// </summary>
		public ExifType ExifType
		{
			get { return this.exifType; }
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// Gets the data type which corresponds to the specified Type.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Type GetDataType(object value)
		{
			if (value == null)
				return null;

			Type type = value.GetType();
			if (!type.IsEnum)
				throw new NotImplementedException();

			System.Reflection.FieldInfo fieldInfo = type.GetField(Enum.GetName(type, value));

			if (!ExifDataTypeAttribute.IsDefined(fieldInfo, typeof(ExifDataTypeAttribute)))
				return null;

			ExifDataTypeAttribute attribute = (ExifDataTypeAttribute)ExifDataTypeAttribute.GetCustomAttribute(fieldInfo, typeof(ExifDataTypeAttribute));

			return attribute.DataType;
		}

		/// <summary>
		/// Gets the exif type which corresponds to the specified Type.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static ExifType GetExifType(object value)
		{
			if (value == null)
				return ExifType.Unknown;

			Type type = value.GetType();
			if (!type.IsEnum)
				throw new NotImplementedException();

			System.Reflection.FieldInfo fieldInfo = type.GetField(Enum.GetName(type, value));

			if (!ExifDataTypeAttribute.IsDefined(fieldInfo, typeof(ExifDataTypeAttribute)))
				return ExifType.Unknown;

			ExifDataTypeAttribute attribute = (ExifDataTypeAttribute)ExifDataTypeAttribute.GetCustomAttribute(fieldInfo, typeof(ExifDataTypeAttribute));

			return attribute.ExifType;
		}

		#endregion Methods
	}
}
