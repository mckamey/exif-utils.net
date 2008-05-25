using System;
using System.ComponentModel;
using System.Globalization;

namespace PhotoLib.Model.Exif.TypeConverters
{
	public class ExifCollectionConverter : CollectionConverter
	{
		#region Init

		/// <summary>
		/// Ctor.
		/// </summary>
		public ExifCollectionConverter()
		{
		}

		#endregion Init

		#region Methods

		public override PropertyDescriptorCollection GetProperties(
			ITypeDescriptorContext context,
			object value,
			Attribute[] attributes)
		{
			PropertyDescriptor[] descriptors = null;
			ExifPropertyCollection exifs = value as ExifPropertyCollection;
			if (exifs != null)
			{
				descriptors = new PropertyDescriptor[exifs.Count];
				int i = 0;
				foreach (ExifProperty exif in (((ExifPropertyCollection)value)))
				{
					descriptors[i++] = new ExifCollectionConverter.ExifPropertyDescriptor(exif.Tag, exif.DisplayName);
				}
			}
			return new PropertyDescriptorCollection(descriptors);
		}

		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override object ConvertTo(
			ITypeDescriptorContext context,
			CultureInfo culture,
			object value,
			Type destinationType)
		{
			if (value is ExifPropertyCollection && destinationType == typeof(string))
			{
				return ((ExifPropertyCollection)value).Count+" EXIF Properties";
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}

		#endregion Methods

		#region Nested Types

		// Nested Types
		private class ExifPropertyDescriptor : System.ComponentModel.TypeConverter.SimplePropertyDescriptor
		{
			#region Fields

			private ExifTag id;

			#endregion Fields

			#region Methods

			public ExifPropertyDescriptor(ExifTag id, string label) :
				base(typeof(ExifPropertyCollection), label, typeof(ExifProperty))
			{
				this.id = id;
			}

			public override object GetValue(object instance)
			{
				if (instance is ExifPropertyCollection)
				{
					ExifPropertyCollection exifs = (ExifPropertyCollection)instance;
					return exifs[this.id];
				}
				return null;
			}

			public override void SetValue(object instance, object value)
			{
				if (instance is ExifPropertyCollection &&
					value is ExifProperty)
				{
					ExifPropertyCollection exifs = (ExifPropertyCollection)instance;
					exifs[this.id] = (ExifProperty)value;
					this.OnValueChanged(instance, EventArgs.Empty);
				}
			}

			#endregion Methods
		}

		#endregion Nested Types
	}
}
