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
using System.ComponentModel;
using System.Globalization;

namespace ExifUtils.Exif.TypeConverters
{
	internal class ExifCollectionConverter : CollectionConverter
	{
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
