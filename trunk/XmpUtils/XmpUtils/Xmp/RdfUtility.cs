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
using System.Xml.Linq;

namespace XmpUtils.Xmp
{
	public class RdfUtility
	{
		#region Constants

		private const string XmpMetaPrefix = "x";
		private const string XmpMetaNamespace = "adobe:ns:meta/";

		private const string RdfPrefix = "rdf";
		private const string RdfNamespace = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";

		private const string RdfAboutValue = "";

		#endregion Constants

		#region Methods

		public XDocument ToXml(params XmpProperty[] properties)
		{
			return this.ToXml((IEnumerable<XmpProperty>)properties);
		}

		public XDocument ToXml(IEnumerable<XmpProperty> properties)
		{
			XElement rdf = new XElement(
				XName.Get("RDF", RdfNamespace),
				new XAttribute(XNamespace.Xmlns + RdfPrefix, RdfNamespace));

			var groups =
				from xmp in properties
				group xmp by xmp.Namespace;

			foreach (var group in groups)
			{
				XElement description = new XElement(
					XName.Get("Description", RdfNamespace),
					new XAttribute(XName.Get("about", RdfNamespace), RdfAboutValue));

				bool needsPrefix = true;

				foreach (XmpProperty property in group)
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
				case XmpQuantity.Alt:
				{
					// TODO: serialize array
					break;
				}
				case XmpQuantity.Structure:
				{
					// TODO: serialize structure
					break;
				}
				default:
				case XmpQuantity.Simple:
				{
					elem.Add(property.Value);
					break;
				}
			}

			return elem;
		}

		#endregion Methods
	}
}
