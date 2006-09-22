using System;
using System.ComponentModel;

namespace PhotoLib.Model.Exif.TypeConverters
{
	public class ExifConverter : System.ComponentModel.ExpandableObjectConverter
	{
		#region Init

		/// <summary>
		/// Ctor.
		/// </summary>
		public ExifConverter()
		{
		}

		#endregion Init

		#region Methods

		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			if (value is ExifProperty && destinationType == typeof(string))
				return ((ExifProperty)value).DisplayValue;

			return base.ConvertTo(context, culture, value, destinationType);
		}

		#endregion Methods
	}
}
