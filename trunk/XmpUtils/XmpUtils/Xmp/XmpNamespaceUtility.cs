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
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace XmpUtils.Xmp
{
	internal sealed class XmpNamespaceUtility
	{
		#region Constants

		public static readonly XmpNamespaceUtility Instance = new XmpNamespaceUtility();

		private readonly IDictionary<string, Type> PrefixLookup = new Dictionary<string, Type>();
		private readonly IDictionary<string, Type> NamespaceLookup = new Dictionary<string, Type>();

		#endregion Constants

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		private XmpNamespaceUtility()
		{
			this.AddNamespaces(typeof(XmpNamespaceAttribute).Assembly.GetTypes());
		}

		#endregion Init

		#region Methods

		public void AddNamespaces(IEnumerable<MemberInfo> members)
		{
			var types =
				from xns in AttributeUtility.FindWithAttributes<XmpNamespaceAttribute>(members)
				from prefix in xns.Attribute.Prefixes
				select new
				{
					Namespace = xns.Attribute.Namespace,
					Prefix = prefix,
					Type = (Type)xns.Member
				};

			foreach (var type in types)
			{
				if (!type.Type.IsEnum)
				{
					throw new ArgumentException("XMP schema definition types must be enum with attributes.");
				}

				this.NamespaceLookup[type.Namespace] = type.Type;
				if (!String.IsNullOrEmpty(type.Prefix))
				{
					this.PrefixLookup[type.Prefix] = type.Type;
				}
			}
		}

		public object ParsePrefix(string qualifiedName)
		{
			if (String.IsNullOrEmpty(qualifiedName))
			{
				return null;
			}

			int index = qualifiedName.LastIndexOf(':');
			if (index < 0)
			{
				return null;
			}

			return this.ParsePrefix(qualifiedName.Substring(0, index), qualifiedName.Substring(index+1));
		}

		public object ParsePrefix(string prefix, string localName)
		{
			return this.Parse(prefix, localName, this.PrefixLookup);
		}

		public object ParseNamespace(XName name)
		{
			return this.ParseNamespace(name.NamespaceName, name.LocalName);
		}

		public object ParseNamespace(string ns, string localName)
		{
			return this.Parse(ns, localName, this.NamespaceLookup);
		}

		private object Parse(string scope, string localName, IDictionary<string, Type> lookup)
		{
			if (String.IsNullOrEmpty(scope))
			{
				return null;
			}

			scope = scope.Replace("\\", "");

			Type enumType;
			if (!lookup.TryGetValue(scope, out enumType))
			{
				return null;
			}

			try
			{
				return Enum.Parse(enumType, localName, true);
			}
			catch
			{
				foreach (object value in Enum.GetValues(enumType))
				{
					string name = Enum.GetName(enumType, value);
					FieldInfo fieldInfo = enumType.GetField(name);

					// check for property info on property enum only
					XmpPropertyAttribute xp = AttributeUtility
						.FindAttributes<XmpPropertyAttribute>(fieldInfo)
						.FirstOrDefault();

					if (xp != null && StringComparer.OrdinalIgnoreCase.Equals(xp.Name, localName))
					{
						return fieldInfo.GetValue(enumType);
					}
				}
				return null;
			}
		}

		#endregion Methods
	}
}
