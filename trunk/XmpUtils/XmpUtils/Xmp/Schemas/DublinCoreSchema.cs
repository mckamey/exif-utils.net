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

using XmpUtils.Xmp.ValueTypes;

namespace XmpUtils.Xmp.Schemas
{
	[XmpNamespace("http://purl.org/dc/elements/1.1/", "dc")]
	public enum DublinCoreSchema
	{
		[XmpBasicProperty(XmpBasicType.ProperName, Quantity=XmpQuantity.Bag, Name="contributor")]
		Contributor,

		[XmpBasicProperty(XmpBasicType.Text, Name="coverage")]
		Coverage,

		[XmpBasicProperty(XmpBasicType.ProperName, XmpQuantity.Seq, Name="creator")]
		Creator,

		[XmpBasicProperty(XmpBasicType.Date, XmpQuantity.Seq, Name="date")]
		Date,

		[XmpBasicProperty(XmpBasicType.LangAlt, XmpQuantity.Alt, Name="description")]
		Description,

		[XmpBasicProperty(XmpBasicType.MIMEType, Category=XmpCategory.Internal, Name="format")]
		Format,

		[XmpBasicProperty(XmpBasicType.Text, Name="identifier")]
		Identifier,

		[XmpBasicProperty(XmpBasicType.Locale, XmpQuantity.Bag, Category=XmpCategory.Internal, Name="language")]
		Language,

		[XmpBasicProperty(XmpBasicType.ProperName, XmpQuantity.Bag, Name="publisher")]
		Publisher,

		[XmpBasicProperty(XmpBasicType.Text, XmpQuantity.Bag, Name="relation")]
		Relation,

		[XmpBasicProperty(XmpBasicType.LangAlt, XmpQuantity.Alt, Name="rights")]
		Rights,

		[XmpBasicProperty(XmpBasicType.Text, Name="source")]
		Source,

		[XmpBasicProperty(XmpBasicType.Text, XmpQuantity.Bag, Name="subject")]
		Subject,

		[XmpBasicProperty(XmpBasicType.LangAlt, XmpQuantity.Alt, Name="title")]
		Title,

		[XmpBasicProperty(XmpBasicType.Choice, XmpQuantity.Bag, Name="type")]
		Type
	}
}
