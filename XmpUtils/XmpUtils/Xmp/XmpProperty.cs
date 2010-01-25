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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

using XmpUtils.Xmp.ValueTypes;

namespace XmpUtils.Xmp
{
	/// <summary>
	/// Represents any XMP property 
	/// </summary>
	public class XmpProperty
	{
		#region Fields

		private Enum schema;
		private IEnumerable<XmpProperty> qualifiers = Enumerable.Empty<XmpProperty>();

		#endregion Fields

		#region Mutable Properties

		/// <summary>
		/// Gets and sets the XMP Schema this property conforms to
		/// </summary>
		public Enum Schema
		{
			get { return this.schema; }
			set { this.Populate(value); }
		}

		/// <summary>
		/// Gets and sets the value of the property
		/// </summary>
		public object Value
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets any qualifiers for this property
		/// </summary>
		public IEnumerable<XmpProperty> Qualifiers
		{
			get { return this.qualifiers; }
			set
			{
				if (value == null)
				{
					this.qualifiers = Enumerable.Empty<XmpProperty>();
				}
				else
				{
					this.qualifiers = value;
				}
			}
		}

		/// <summary>
		/// Gets and sets an internal measure of the priority of the source
		/// </summary>
		/// <remarks>
		/// XMP is highest priority with EXIF/TIFF next, etc.
		/// </remarks>
		public decimal Priority
		{
			get;
			set;
		}

		#endregion Mutable Properties

		#region Reflective Properties

		/// <summary>
		/// Gets the XMP category of this property
		/// </summary>
		public XmpCategory Category
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the corresponding .NET Type for a single value of this property
		/// </summary>
		public Type DataType
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the description of this property
		/// </summary>
		public string Description
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the offical name of this property
		/// </summary>
		public string Name
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the official namespace of this property
		/// </summary>
		public string Namespace
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the recommended prefix of this property
		/// </summary>
		public string Prefix
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the structure of this property
		/// </summary>
		public XmpQuantity Quantity
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the XMP Value Type for a single value of this property
		/// </summary>
		public Enum ValueType
		{
			get;
			private set;
		}

		#endregion Reflective Properties

		#region Utility Methods

		protected virtual void Populate(Enum schema)
		{
			this.schema = schema;

			if (schema == null)
			{
				// reset reflective properties to defaults
				this.Category = XmpCategory.External;
				this.Description = null;
				this.Name = null;
				this.Namespace = null;
				this.Quantity = XmpQuantity.Single;
				this.ValueType = XmpBasicType.Unknown;
				return;
			}

			string name;
			FieldInfo fieldInfo;
			Type type = AttributeUtility.GetEnumInfo(schema, out name, out fieldInfo);

			// check for property info on property enum only
			XmpPropertyAttribute xp = AttributeUtility
				.FindAttributes<XmpPropertyAttribute>(fieldInfo)
				.FirstOrDefault();

			if (xp == null)
			{
				this.Category = XmpCategory.External;
				this.Name = name;
				this.Quantity = XmpQuantity.Single;
				this.ValueType = XmpBasicType.Unknown;
			}
			else
			{
				this.Category = xp.Category;
				this.Name = String.IsNullOrEmpty(xp.Name) ? name : xp.Name;
				this.Quantity = xp.Quantity;
				this.ValueType = xp.ValueType;
			}

			// check for namespace on property enum, then on type
			XmpNamespaceAttribute xns = AttributeUtility
				.FindAttributes<XmpNamespaceAttribute>(fieldInfo, type)
				.FirstOrDefault() ?? XmpNamespaceAttribute.Empty;

			this.Namespace = xns.Namespace;
			this.Prefix = xns.PreferredPrefix;

			// check for description on property enum only
			this.Description = AttributeUtility
				.FindAttributes<DescriptionAttribute>(fieldInfo)
				.Select(d => d.Description)
				.FirstOrDefault();

			string valueTypeName;
			FieldInfo valueTypeInfo;
			AttributeUtility.GetEnumInfo(this.ValueType, out valueTypeName, out valueTypeInfo);

			// check for description on value type enum, then on property enum, then on type
			this.DataType = AttributeUtility
				.FindAttributes<XmpValueTypeAttribute>(fieldInfo, valueTypeInfo, type)
				.Select(xta => xta.DataType)
				.FirstOrDefault() ?? typeof(string);
		}

		#endregion Utility Methods
	}
}
