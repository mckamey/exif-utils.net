using System;
using System.ComponentModel;
using System.Globalization;

namespace ExifUtils.Exif.TypeConverters
{
	internal class ExifConverter : ExpandableObjectConverter
	{
		#region Methods

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (value is ExifProperty && destinationType == typeof(string))
			{
				return ((ExifProperty)value).DisplayValue;
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}

		#endregion Methods
	}
}
