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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

using XmpUtils.Xmp.TypeConverters;
using XmpUtils.Xmp.ValueTypes;

namespace XmpUtils.Xmp
{
	/// <summary>
	/// Collection of XMP properties which can serialize as XMP RDF
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(XmpCollectionConverter))]
	public class XmpPropertyCollection :
		ICollection<XmpProperty>,
		ICollection
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

		private static readonly XmlWriterSettings XmlSettings = new XmlWriterSettings
		{
			ConformanceLevel = ConformanceLevel.Auto,
			Indent = true,
			IndentChars = "\t",
			NewLineChars = Environment.NewLine,
			CloseOutput = false,
			OmitXmlDeclaration = true
		};

		private static readonly EqualityComparer<XmpProperty, Enum> DistinctFilter = new EqualityComparer<XmpProperty, Enum>(x => x.Schema);

		#endregion Constants

		#region Fields

		private XDocument document;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		public XmpPropertyCollection()
		{
		}

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="doc"></param>
		public XmpPropertyCollection(XDocument doc)
		{
			this.XmpDocument = doc;
		}

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="properties"></param>
		public XmpPropertyCollection(IEnumerable<XmpProperty> properties)
		{
			this.SetProperties(properties);
		}

		#endregion Init

		#region Properties

		public XDocument XmpDocument
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
			set
			{
				// TODO: validate this is correct document structure
				this.document = value;
			}
		}

		/// <summary>
		/// Gets and sets the value for a single XMP property
		/// </summary>
		/// <param name="schema"></param>
		/// <returns></returns>
		public object this[Enum schema]
		{
			get
			{
				XmpProperty property = this.GetProperty(schema);
				if (property == null)
				{
					return null;
				}
				return property.Value;
			}
			set
			{
				this.Add(new XmpProperty
				{
					Schema=schema,
					Value=value
				});
			}
		}

		#endregion Properties

		#region XmpProperty Read Methods

		public T GetValue<T>(Enum schema)
		{
			return this.GetValue(schema, default(T));
		}

		public T GetValue<T>(Enum schema, T defaultValue)
		{
			T value;
			if (!this.TryGetValue(schema, out value))
			{
				return defaultValue;
			}
			return value;
		}

		public bool TryGetValue<T>(Enum schema, out T value)
		{
			XmpProperty property = this.GetProperty(schema);
			if (property == null ||
				property.Value == null)
			{
				value = default(T);
				return false;
			}

			Type type = typeof(T);

			if (property.Value is T)
			{
				value = (T)property.Value;
			}
			else if (type.IsEnum)
			{
				try
				{
					value = (T)Enum.Parse(type, Convert.ToString(property.Value), true);
				}
				catch
				{
					value = default(T);
					return false;
				}
			}
			else
			{
				value = (T)Convert.ChangeType(property.Value, type);
			}

			return true;
		}

		public IEnumerable<XmpProperty> GetProperties(IEnumerable schemas)
		{
			foreach (Enum schema in schemas)
			{
				XmpProperty property = this.GetProperty(schema);
				if (property == null)
				{
					continue;
				}
				yield return property;
			}
		}

		public XmpProperty GetProperty(Enum schema)
		{
			XmpProperty property = new XmpProperty
			{
				Schema = schema
			};

			XElement elem = this.XmpDocument.Descendants(XName.Get(property.Name, property.Namespace)).FirstOrDefault();
			if (elem == null)
			{
				return null;
			}

			return this.GetProperty(property, elem);
		}

		private XmpProperty GetProperty(XmpProperty property, XElement elem)
		{
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

			return XmpPropertyCollection.ProcessValue(property);
		}

		internal static XmpProperty ProcessValue(XmpProperty property)
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
						XmpPropertyCollection.ProcessRational<long>(property);
						break;
					}
				}
			}

			return property;
		}

		private static void ProcessRational<T>(XmpProperty property)
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
			XElement rdf = this.GetRdfRoot();

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
					 select p).Distinct(XmpPropertyCollection.DistinctFilter);

				XElement description = this.GetRdfSection(rdf, g.Key.Prefix, g.Key.Namespace);

				foreach (XmpProperty property in props)
				{
					this.Add(property, description);
				}
			}
		}

		public void Add(XmpProperty property)
		{
			XElement description = this.GetRdfSection(this.GetRdfRoot(), property.Prefix, property.Namespace);

			this.Add(property, description);
		}

		private void Add(XmpProperty property, XElement description)
		{
			// clear all existing properties with same name
			description.Elements(XName.Get(property.Name, property.Namespace)).Remove();

			// convert XmpProperty to XML element
			XElement elem = this.CreateElement(property);
			if (elem != null)
			{
				description.Add(elem);
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

			bool isLangAlt = false;
			switch (property.Quantity)
			{
				case XmpQuantity.Alt:
				{
					if (property.ValueType is XmpBasicType &&
						((XmpBasicType)property.ValueType) == XmpBasicType.LangAlt)
					{
						isLangAlt = true;

						IDictionary<string, object> dictionary = property.Value as IDictionary<string, object>;
						if (dictionary == null)
						{
							// emit as list without xml:lang
							goto case XmpQuantity.Bag;
						}

						XElement list = new XElement(XName.Get(property.Quantity.ToString(), RdfNamespace));

						foreach (KeyValuePair<string, object> item in dictionary)
						{
							if (item.Value == null || item.Value.ToString() == String.Empty)
							{
								continue;
							}

							XElement li = new XElement(XName.Get("li", RdfNamespace));
							if (!String.IsNullOrEmpty(item.Key))
							{
								li.Add(new XAttribute(XNamespace.Xml+"lang", item.Key));
							}
							else if (!list.HasElements)
							{
								li.Add(new XAttribute(XNamespace.Xml+"lang", "x-default"));
							}

							if (item.Value is XmpProperty)
							{
								// TODO: evaluate this against RDF spec
								// http://www.w3.org/TR/REC-rdf-syntax/#section-Syntax-parsetype-resource
								li.Add(new XAttribute(XName.Get("parseType", RdfNamespace), "Resource"));
								li.Add(this.CreateElement((XmpProperty)item.Value));
							}
							else
							{
								li.Add(item.Value);
							}

							list.Add(li);
						}

						if (list.HasElements)
						{
							elem.Add(list);
						}
					}
					else
					{
						// TODO: find how to process non-lang alts
						// emit as list
						goto case XmpQuantity.Bag;
					}
					break;
				}
				case XmpQuantity.Bag:
				case XmpQuantity.Seq:
				{
					IEnumerable array = property.Value as IEnumerable;
					if (array == null || array is string)
					{
						array = new object[] { property.Value };
					}
					else if (array is IDictionary<string, object>)
					{
						array = ((IDictionary<string, object>)array).Values;
					}

					XElement list = new XElement(
						XName.Get(property.Quantity.ToString(), RdfNamespace));

					foreach (object item in array)
					{
						if (item == null || item.ToString() == String.Empty)
						{
							continue;
						}

						XElement li = new XElement(XName.Get("li", RdfNamespace));
						if (isLangAlt && !list.HasElements)
						{
							li.Add(new XAttribute(XNamespace.Xml+"lang", "x-default"));
						}

						if (item is XmpProperty)
						{
							// TODO: evaluate this against RDF spec
							// http://www.w3.org/TR/REC-rdf-syntax/#section-Syntax-parsetype-resource
							li.Add(new XAttribute(XName.Get("parseType", RdfNamespace), "Resource"));
							li.Add(this.CreateElement((XmpProperty)item));
						}
						else
						{
							li.Add(item);
						}

						list.Add(li);
					}

					if (list.HasElements)
					{
						elem.Add(list);
					}
					break;
				}
				default:
				case XmpQuantity.Single:
				{
					if (property.Value is XmpProperty)
					{
						elem.Add(new XAttribute(XName.Get("parseType", RdfNamespace), "Resource"));
						elem.Add(this.CreateElement((XmpProperty)property.Value));
					}
					else if (property.Value is IDictionary<string, object>)
					{
						elem.Add(new XAttribute(XName.Get("parseType", RdfNamespace), "Resource"));
						foreach (KeyValuePair<string, object> item in (IDictionary<string, object>)property.Value)
						{
							XElement child = new XElement(XName.Get(item.Key, property.Namespace));

							if (item.Value is XmpProperty)
							{
								// TODO: evaluate this against RDF spec
								// http://www.w3.org/TR/REC-rdf-syntax/#section-Syntax-parsetype-resource
								child.Add(new XAttribute(XName.Get("parseType", RdfNamespace), "Resource"));
								child.Add(this.CreateElement((XmpProperty)item.Value));
							}
							else
							{
								child.Add(item.Value);
							}

							elem.Add(child);
						}
					}
					else
					{
						elem.Add(property.Value);
					}
					break;
				}
			}

			return elem;
		}

		private XElement GetRdfSection(XElement rdf, string prefix, string ns)
		{
			XName attrName = XNamespace.Xmlns + prefix;

			// find the first rdf:Description for the corresponding namespace
			XElement description = rdf.Elements(XName.Get("Description", RdfNamespace))
				.FirstOrDefault(d => d.Attributes(attrName).Any(a => a.Value == ns));

			if (description == null)
			{
				// create a section for this namespace
				description = new XElement(XName.Get("Description", RdfNamespace),
					new XAttribute(XName.Get("about", RdfNamespace), RdfAboutValue),
					new XAttribute(attrName, ns));

				rdf.Add(description);
			}

			return description;
		}

		private XElement GetRdfRoot()
		{
			return this.XmpDocument.Descendants(XName.Get("RDF", RdfNamespace)).First();
		}

		#endregion XmpProperty Write Methods

		#region Serialization Methods

		/// <summary>
		/// Loads XmpProperties from XML
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static XmpPropertyCollection LoadFromXml(string filename)
		{
			using (TextReader reader = File.OpenText(filename))
			{
				return XmpPropertyCollection.LoadFromXml(reader);
			}
		}

		/// <summary>
		/// Loads XmpProperties from XML
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		public static XmpPropertyCollection LoadFromXml(TextReader reader)
		{
			return new XmpPropertyCollection(XDocument.Load(reader));
		}

		public static XmpPropertyCollection LoadFromImage(Stream stream)
		{
			IEnumerable<XmpProperty> properties = new XmpExtractor().Extract(stream);

			return new XmpPropertyCollection(properties);
		}

		public static XmpPropertyCollection LoadFromImage(string filename)
		{
			IEnumerable<XmpProperty> properties = new XmpExtractor().Extract(filename);

			return new XmpPropertyCollection(properties);
		}

		/// <summary>
		/// Saves XmpProperties to XML
		/// </summary>
		/// <param name="filename"></param>
		public void SaveAsXml(string filename)
		{
			using (TextWriter writer = File.CreateText(filename))
			{
				this.SaveAsXml(writer);
			}
		}

		/// <summary>
		/// Saves XmpProperties to XML
		/// </summary>
		/// <param name="writer"></param>
		public void SaveAsXml(TextWriter writer)
		{
			using (XmlWriter xmlWriter = XmlWriter.Create(writer, XmpPropertyCollection.XmlSettings))
			{
				this.XmpDocument.Save(xmlWriter);
			}
		}

		/// <summary>
		/// Saves XmpProperties to XML
		/// </summary>
		/// <param name="writer"></param>
		public void SaveAsXml(XmlWriter writer)
		{
			this.XmpDocument.Save(writer);
		}

		#endregion Serialization Methods

		#region Schema Registration Methods

		/// <summary>
		/// Allows registration of custom XMP schema enumerations
		/// </summary>
		/// <param name="assembly"></param>
		public static void RegisterSchemas(Assembly assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}

			XmpNamespaceUtility.Instance.AddNamespaces(assembly.GetTypes());
		}

		/// <summary>
		/// Allows registration of custom XMP schema enumerations
		/// </summary>
		/// <param name="schemas"></param>
		public static void RegisterSchemas(params Type[] schemas)
		{
			XmpNamespaceUtility.Instance.AddNamespaces(schemas);
		}

		#endregion Schema Registration Methods

		#region ICollection<XmpProperty> Members

		public void Clear()
		{
			this.GetRdfRoot().Elements().Remove();
		}

		public bool Contains(XmpProperty property)
		{
			return this.XmpDocument.Descendants(XName.Get(property.Name, property.Namespace)).Any();
		}

		public void CopyTo(XmpProperty[] array, int index)
		{
			foreach(XmpProperty property in this)
			{
				array[index] = property;
				index++;
			}
		}

		public int Count
		{
			get { return this.Count(); }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(XmpProperty property)
		{
			IEnumerable<XElement> elems = this.XmpDocument.Descendants(XName.Get(property.Name, property.Namespace));
			if (!elems.Any())
			{
				return false;
			}
			elems.Remove();
			return true;
		}

		#endregion ICollection<XmpProperty> Members

		#region IEnumerable<XmpProperty> Members

		public IEnumerator<XmpProperty> GetEnumerator()
		{
			var elems = this.GetRdfRoot().Elements(XName.Get("Description", RdfNamespace)).Elements();
			foreach (XElement elem in elems)
			{
				Enum schema = XmpNamespaceUtility.Instance.ParseNamespace(elem.Name) as Enum;
				if (schema == null)
				{
					continue;
				}

				XmpProperty property = new XmpProperty
				{
					Schema = schema
				};

				yield return this.GetProperty(property, elem);
			}
		}

		#endregion IEnumerable<XmpProperty> Members

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion IEnumerable Members

		#region ICollection Members

		void ICollection.CopyTo(Array array, int index)
		{
			foreach (XmpProperty property in this)
			{
				array.SetValue(property, index);
				index++;
			}
		}

		bool ICollection.IsSynchronized
		{
			get { return ((ICollection)this.XmpDocument).IsSynchronized; }
		}

		object ICollection.SyncRoot
		{
			get { return ((ICollection)this.XmpDocument).SyncRoot; }
		}

		#endregion ICollection Members
	}
}
