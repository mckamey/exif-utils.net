using System;

namespace PhotoLib.Model.Exif
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple=false)]
	public sealed class ExifDataTypeAttribute : Attribute
	{
		#region Fields

		private ExifType exifType = ExifType.Raw;
		private Type dataType = null;

		#endregion Fields

		#region Init

		public ExifDataTypeAttribute()
		{
		}

		public ExifDataTypeAttribute(Type dataType)
		{
			this.dataType = dataType;
		}

		public ExifDataTypeAttribute(ExifType exifType)
		{
			this.exifType = exifType;
		}

		public ExifDataTypeAttribute(Type type, ExifType exifType)
		{
			this.dataType = type;
			this.exifType = exifType;
		}

		#endregion Init

		#region Properties

		public Type DataType
		{
			get { return this.dataType; }
		}

		public ExifType ExifType
		{
			get { return this.exifType; }
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// Gets the data type which corresponds to the specified Type.
		/// </summary>
		/// <param name="type"></param>
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
		/// <param name="type"></param>
		/// <returns></returns>
		public static ExifType GetExifType(object value)
		{
			if (value == null)
				return ExifType.Raw;

			Type type = value.GetType();
			if (!type.IsEnum)
				throw new NotImplementedException();

			System.Reflection.FieldInfo fieldInfo = type.GetField(Enum.GetName(type, value));

			if (!ExifDataTypeAttribute.IsDefined(fieldInfo, typeof(ExifDataTypeAttribute)))
				return ExifType.Raw;

			ExifDataTypeAttribute attribute = (ExifDataTypeAttribute)ExifDataTypeAttribute.GetCustomAttribute(fieldInfo, typeof(ExifDataTypeAttribute));

			return attribute.ExifType;
		}

		#endregion Methods
	}
}
