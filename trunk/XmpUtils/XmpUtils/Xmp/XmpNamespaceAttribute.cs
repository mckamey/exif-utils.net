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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace XmpUtils.Xmp
{
	/// <summary>
	/// Attribute which provides mapping to XMP property names
	/// </summary>
	[AttributeUsage(AttributeTargets.Class|AttributeTargets.Struct|AttributeTargets.Enum|AttributeTargets.Field, AllowMultiple=false)]
	public sealed class XmpNamespaceAttribute : Attribute
	{
		#region Constants

		public static readonly XmpNamespaceAttribute Empty = new XmpNamespaceAttribute();
		private static readonly char[] PrefixDelims = { ',' };

		#endregion Constants

		#region Fields

		private IEnumerable<string> prefixes;
		private string ns;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		public XmpNamespaceAttribute()
			: this(null, null)
		{
		}

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="ns">fully qualified XMP namespace URI</param>
		public XmpNamespaceAttribute(string ns)
			: this(ns, null)
		{
		}

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="ns">fully qualified XML namespace URI</param>
		/// <param name="prefix">XML namespace prefix</param>
		public XmpNamespaceAttribute(string ns, string prefix)
		{
			this.ns = ns;
			this.prefixes = String.IsNullOrEmpty(prefix) ?
				Enumerable.Empty<string>() :
				prefix.Split(PrefixDelims, StringSplitOptions.RemoveEmptyEntries);
		}

		#endregion Init

		#region Properties

		/// <summary>
		/// Gets a prioritized sequence of preferred prefixes for this namespace
		/// </summary>
		public string PreferredPrefix
		{
			get { return this.prefixes.FirstOrDefault(); }
		}

		/// <summary>
		/// Gets a prioritized sequence of preferred prefixes for this namespace
		/// </summary>
		public IEnumerable<string> Prefixes
		{
			get { return this.prefixes; }
		}

		/// <summary>
		/// Gets the official namespace URI
		/// </summary>
		public string Namespace
		{
			get { return this.ns; }
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// Gets the namespace and prefix which corresponds to the specified value
		/// </summary>
		/// <param name="value"></param>
		/// <param name="ns"></param>
		/// <param name="prefix"></param>
		public static void GetNamespace(Enum value, out string ns, out string prefix)
		{
			if (value == null)
			{
				prefix = ns = null;
				return;
			}

			string name;
			FieldInfo fieldInfo;
			Type type = AttributeUtility.GetEnumInfo(value, out name, out fieldInfo);

			// check for namespace on enum then on type
			var xns = AttributeUtility
				.FindAttributes<XmpNamespaceAttribute>(fieldInfo, type)
				.FirstOrDefault() ?? XmpNamespaceAttribute.Empty;

			ns = xns.Namespace;
			prefix = xns.PreferredPrefix;
		}

		#endregion Methods
	}
}
