#region License
/*---------------------------------------------------------------------------------*\

	Distributed under the terms of an MIT-style license:

	The MIT License

	Copyright (c) 2005-2010 Stephen M. McKamey

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

namespace XmpUtils.Xmp.TypeConverters
{
	internal class XmpCollectionConverter : CollectionConverter
	{
		#region Methods

		public override PropertyDescriptorCollection GetProperties(
			ITypeDescriptorContext context,
			object value,
			Attribute[] attributes)
		{
			PropertyDescriptor[] descriptors = null;
			XmpPropertyCollection properties = value as XmpPropertyCollection;
			if (properties != null)
			{
				descriptors = new PropertyDescriptor[properties.Count];
				int i = 0;
				foreach (XmpProperty property in (((XmpPropertyCollection)value)))
				{
					descriptors[i++] = new XmpCollectionConverter.XmpPropertyDescriptor(property.Schema, property.Name);
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
			if (value is XmpPropertyCollection && destinationType == typeof(string))
			{
				return ((XmpPropertyCollection)value).Count+" XMP Properties";
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}

		#endregion Methods

		#region Nested Types

		// Nested Types
		private class XmpPropertyDescriptor : System.ComponentModel.TypeConverter.SimplePropertyDescriptor
		{
			#region Fields

			private Enum schema;

			#endregion Fields

			#region Methods

			public XmpPropertyDescriptor(Enum schema, string label) :
				base(typeof(XmpPropertyCollection), label, typeof(XmpProperty))
			{
				this.schema = schema;
			}

			public override object GetValue(object instance)
			{
				if (instance is XmpPropertyCollection)
				{
					XmpPropertyCollection properties = (XmpPropertyCollection)instance;
					return properties[this.schema];
				}
				return null;
			}

			public override void SetValue(object instance, object value)
			{
				if (instance is XmpPropertyCollection &&
					value is XmpProperty)
				{
					XmpPropertyCollection properties = (XmpPropertyCollection)instance;
					properties[this.schema] = (XmpProperty)value;
					this.OnValueChanged(instance, EventArgs.Empty);
				}
			}

			#endregion Methods
		}

		#endregion Nested Types
	}
}
