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
