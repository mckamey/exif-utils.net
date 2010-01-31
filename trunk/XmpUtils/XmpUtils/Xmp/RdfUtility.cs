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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using XmpUtils.Xmp.ValueTypes;

namespace XmpUtils.Xmp
{
	public class RdfUtility
	{
		#region EqualityComparer<TObj, TVal>

		/// <summary>
		/// Wrapper which projects an object before comparison
		/// </summary>
		/// <typeparam name="TObj">type of objects to be compared</typeparam>
		/// <typeparam name="TVal">type of values in the comparison</typeparam>
		private class EqualityComparer<TObj, TVal> :
			IEqualityComparer<TObj>
		{
			#region Fields

			private readonly Func<TObj, TVal> Projection;

			#endregion Fields

			#region Init

			public EqualityComparer(Func<TObj, TVal> projection)
			{
				this.Projection = projection;
			}

			#endregion Init

			#region IEqualityComparer<TObj> Members

			public bool Equals(TObj x, TObj y)
			{
				return EqualityComparer<TVal>.Default.Equals(this.Projection(x), this.Projection(y));
			}

			public int GetHashCode(TObj obj)
			{
				return EqualityComparer<TVal>.Default.GetHashCode(this.Projection(obj));
			}

			#endregion IEqualityComparer<TObj> Members
		}

		#endregion EqualityComparer<TObj, TVal>

		#region Constants

		private const string XmpMetaPrefix = "x";
		private const string XmpMetaNamespace = "adobe:ns:meta/";

		private const string RdfPrefix = "rdf";
		private const string RdfNamespace = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";

		private const string RdfAboutValue = "";

		private static readonly EqualityComparer<XmpProperty, Enum> DistinctFilter = new EqualityComparer<XmpProperty, Enum>(x => x.Schema);

		#endregion Constants

		#region Methods

		public XDocument ToXml(IEnumerable<XmpProperty> properties)
		{
			XElement rdf = new XElement(
				XName.Get("RDF", RdfNamespace),
				new XAttribute(XNamespace.Xmlns + RdfPrefix, RdfNamespace));

			// group into each schema namespace (as per XMP recommendation)
			var groups =
				from xmp in properties
				group xmp by xmp.Namespace;

			foreach (var g in groups)
			{
				XElement description = new XElement(
					XName.Get("Description", RdfNamespace),
					new XAttribute(XName.Get("about", RdfNamespace), RdfAboutValue));

				// sort and de-dup by priority
				var props =
					(from p in g
					 orderby p.Schema, p.Priority descending
					 select p).Distinct(RdfUtility.DistinctFilter);

				bool needsPrefix = true;

				foreach (XmpProperty property in props)
				{
					if (needsPrefix)
					{
						if (!String.IsNullOrEmpty(property.Prefix) || !String.IsNullOrEmpty(property.Namespace))
						{
							description.Add(new XAttribute(XNamespace.Xmlns + property.Prefix, property.Namespace));
						}
						needsPrefix = false;
					}

					XElement elem = this.ToXml(property);
					if (elem != null)
					{
						description.Add(elem);
					}
				}

				rdf.Add(description);
			}

			return new XDocument(
				new XElement(
					XName.Get("xmpmeta", XmpMetaNamespace),
					new XAttribute(XNamespace.Xmlns + XmpMetaPrefix, XmpMetaNamespace),
					rdf));
		}

		public XElement ToXml(XmpProperty property)
		{
			if (property.Value == null)
			{
				return null;
			}

			XElement elem = new XElement(
				XName.Get(property.Name, property.Namespace));

			switch (property.Quantity)
			{
				case XmpQuantity.Bag:
				case XmpQuantity.Seq:
				{
					XElement list = new XElement(XName.Get(property.Quantity.ToString(), RdfNamespace));
					elem.Add(list);

					IEnumerable array = property.Value as IEnumerable;
					if (array == null)
					{
						list.Add(new XComment("Unexpected value: "+Convert.ToString(property.Value)));
						break;
					}
					if (array is IDictionary<string, object>)
					{
						array = ((IDictionary<string, object>)array).Values;
					}

					foreach (object item in array)
					{
						object child = item is XmpProperty ?
							this.ToXml((XmpProperty)item) :
							item;

						list.Add(new XElement(XName.Get("li", RdfNamespace), child));
					}
					break;
				}
				case XmpQuantity.Alt:
				{
					if (property.ValueType is XmpBasicType &&
						((XmpBasicType)property.ValueType) == XmpBasicType.LangAlt)
					{
						XElement list = new XElement(XName.Get(property.Quantity.ToString(), RdfNamespace));
						elem.Add(list);

						IDictionary<string, object> dictionary = property.Value as IDictionary<string, object>;
						if (dictionary == null)
						{
							list.Add(new XComment("Unexpected value: "+Convert.ToString(property.Value)));
							break;
						}

						foreach (KeyValuePair<string, object> item in dictionary)
						{
							object child = item.Value is XmpProperty ?
								this.ToXml((XmpProperty)item.Value) :
								item.Value;

							list.Add(new XElement(
								XName.Get("li", RdfNamespace),
								new XAttribute(XNamespace.Xml+"lang", item.Key),
								child));
						}
					}
					else
					{
						// TODO: find how to process non-lang alts
						elem.Add(new XComment("Alt value: "+Convert.ToString(property.Value)));
					}
					break;
				}
				default:
				case XmpQuantity.Single:
				{
					if (property.Value is XmpProperty)
					{
						elem.Add(this.ToXml((XmpProperty)property.Value));
					}
					else if (property.DataType == typeof(string) ||
						property.DataType == typeof(DateTime))
					{
						elem.Add(property.Value);
					}
					else if (property.Value is IDictionary<string, object>)
					{
						foreach (KeyValuePair<string, object> item in (IDictionary<string, object>)property.Value)
						{
							object child = item.Value is XmpProperty ?
								this.ToXml((XmpProperty)item.Value) :
								item.Value;

							elem.Add(new XElement(XName.Get(item.Key, property.Namespace), child));
						}
					}
					else
					{
						elem.Add(new XComment("Unexpected value: "+Convert.ToString(property.Value)));
					}
					break;
				}
			}

			return elem;
		}

		#endregion Methods
	}
}
