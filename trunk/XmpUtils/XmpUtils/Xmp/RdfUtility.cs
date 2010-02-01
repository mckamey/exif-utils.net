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

		#region Fields

		private XDocument document;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		public RdfUtility()
		{
		}

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="doc"></param>
		public RdfUtility(XDocument doc)
		{
			this.Document = doc;
		}

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="properties"></param>
		public RdfUtility(IEnumerable<XmpProperty> properties)
		{
			this.SetProperties(properties);
		}

		#endregion Init

		#region Document Methods

		public XDocument Document
		{
			get
			{
				if (this.document == null)
				{
					this.document = new XDocument(
						new XElement(XName.Get("xmpmeta", XmpMetaNamespace),
							new XAttribute(XNamespace.Xmlns + XmpMetaPrefix, XmpMetaNamespace),
							new XElement(XName.Get("RDF", RdfNamespace),
								new XAttribute(XNamespace.Xmlns + RdfPrefix, RdfNamespace))));
				}
				return this.document;
			}
			set { this.document = value; }
		}

		#endregion Document Methods

		#region XmpProperty Read Methods

		public IEnumerable<XmpProperty> GetProperties(IEnumerable schemas)
		{
			foreach (object schema in schemas)
			{
				XmpProperty property = this.GetProperty(schema);
				if (property == null)
				{
					continue;
				}
				yield return property;
			}
		}

		public XmpProperty GetProperty(object schema)
		{
			XmpProperty property = new XmpProperty
			{
				Schema = (Enum)schema
			};

			XElement elem = this.Document.Descendants(XName.Get(property.Name, property.Namespace)).FirstOrDefault();
			if (elem == null)
			{
				return null;
			}

			switch (property.Quantity)
			{
				case XmpQuantity.Alt:
				{
					elem = elem.Element(XName.Get(property.Quantity.ToString(), RdfNamespace));
					if (elem != null)
					{
						if (elem.Elements().Count() == 0)
						{
							property.Value = elem.Value;
						}
						else
						{
							if (property.ValueType is XmpBasicType &&
							((XmpBasicType)property.ValueType) == XmpBasicType.LangAlt)
							{
								// convert to dictionary
								property.Value = elem.Elements().ToDictionary(
									n => n.Attribute(XNamespace.Xml+"lang").Value,
									n => (object)n.Value);
							}
							else
							{
								// TODO: find how best to process non-lang alts
								// convert to array
								property.Value = elem.Elements(XName.Get("li", RdfNamespace)).Select(n => n.Value).ToList();
							}
						}
					}
					break;
				}
				case XmpQuantity.Bag:
				case XmpQuantity.Seq:
				{
					elem = elem.Element(XName.Get(property.Quantity.ToString(), RdfNamespace));
					if (elem != null)
					{
						if (elem.Elements().Count() == 0)
						{
							property.Value = elem.Value;
						}
						else
						{
							// convert to array
							property.Value = elem.Elements(XName.Get("li", RdfNamespace)).Select(n => n.Value).ToList();
						}
					}
					break;
				}
				default:
				{
					if (elem.Elements().Count() == 0)
					{
						property.Value = elem.Value;
					}
					else
					{
						// convert to dictionary
						property.Value = elem.Elements().ToDictionary(
							n => n.Name.LocalName,
							n => (object)n.Value);
					}
					break;
				}
			}

			return this.ProcessValue(property);
		}

		private XmpProperty ProcessValue(XmpProperty property)
		{
			if (property.ValueType is XmpBasicType)
			{
				switch ((XmpBasicType)property.ValueType)
				{
					case XmpBasicType.Boolean:
					{
						bool value;
						if (Boolean.TryParse(Convert.ToString(property.Value), out value))
						{
							property.Value = value;
						}
						break;
					}
					case XmpBasicType.Date:
					{
						DateTime value;
						if (DateTime.TryParse(Convert.ToString(property.Value), out value))
						{
							property.Value = value;
						}
						break;
					}
					case XmpBasicType.Integer:
					{
						int value;
						if (Int32.TryParse(Convert.ToString(property.Value), out value))
						{
							property.Value = value;
						}
						break;
					}
					case XmpBasicType.Real:
					{
						decimal value;
						if (Decimal.TryParse(Convert.ToString(property.Value), out value))
						{
							property.Value = value;
						}
						break;
					}
				}
			}
			else if (property.ValueType is ExifType)
			{
				switch ((ExifType)property.ValueType)
				{
					case ExifType.GpsCoordinate:
					{
						GpsCoordinate gps;
						if (GpsCoordinate.TryParse(Convert.ToString(property.Value), out gps))
						{
							property.Value = gps;
						}
						break;
					}
					case ExifType.Rational:
					{
						// TODO: how best to determine type of Rational<T>
						this.ProcessRational<long>(property);
						break;
					}
				}
			}

			return property;
		}

		private void ProcessRational<T>(XmpProperty property)
			where T : IConvertible
		{
			Rational<T> rational;
			if (Rational<T>.TryParse(Convert.ToString(property.Value), out rational))
			{
				property.Value = rational;
			}
		}

		#endregion XmpProperty Read Methods

		#region XmpProperty Write Methods

		public void SetProperties(IEnumerable<XmpProperty> properties)
		{
			XElement rdf = this.Document.Descendants(XName.Get("RDF", RdfNamespace)).First();

			// group into each schema namespace (as per XMP recommendation)
			var groups =
				from xmp in properties
				group xmp by new
				{
					Prefix = xmp.Prefix,
					Namespace = xmp.Namespace
				};

			foreach (var g in groups)
			{
				// sort and de-dup by priority
				var props =
					(from p in g
					 orderby p.Schema, p.Priority descending
					 select p).Distinct(RdfUtility.DistinctFilter);

				XName prefix = XNamespace.Xmlns + g.Key.Prefix;
				string ns = g.Key.Namespace;

				XElement description =
					rdf.Elements(XName.Get("Description", RdfNamespace))
					.FirstOrDefault(d => d.Attributes(prefix).Any(a => a.Value == ns));

				if (description == null)
				{
					description = new XElement(XName.Get("Description", RdfNamespace),
						new XAttribute(XName.Get("about", RdfNamespace), RdfAboutValue),
						new XAttribute(prefix, ns));

					rdf.Add(description);
				}

				foreach (XmpProperty property in props)
				{
					description.Elements(XName.Get(property.Name, property.Namespace)).Remove();

					XElement elem = this.CreateElement(property);
					if (elem != null)
					{
						description.Add(elem);
					}
				}
			}
		}

		private XElement CreateElement(XmpProperty property)
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
							this.CreateElement((XmpProperty)item) :
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
								this.CreateElement((XmpProperty)item.Value) :
								item.Value;

							list.Add(new XElement(XName.Get("li", RdfNamespace),
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
						elem.Add(this.CreateElement((XmpProperty)property.Value));
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
								this.CreateElement((XmpProperty)item.Value) :
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

		#endregion XmpProperty Write Methods
	}
}
