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

using XmpUtils.Xmp.ValueTypes;

namespace XmpUtils.Xmp.Schemas
{
	[XmpNamespace("http://ns.adobe.com/xap/1.0/mm/", "xmpMM")]
	public enum XmpMediaManagementSchema
	{
		[XmpMediaManagementProperty(XmpMediaManagementType.ResourceRef, Category=XmpCategory.Internal)]
		DerivedFrom,

		[XmpBasicProperty(XmpBasicType.URI, Category=XmpCategory.Internal)]
		DocumentID,

		[XmpMediaManagementProperty(XmpMediaManagementType.ResourceEvent, XmpQuantity.Seq, Category=XmpCategory.Internal)]
		History,

		[XmpMediaManagementProperty(XmpMediaManagementType.ResourceRef, XmpQuantity.Bag, Category=XmpCategory.Internal)]
		Ingredients,

		[XmpBasicProperty(XmpBasicType.URI, Category=XmpCategory.Internal)]
		InstanceID,

		[XmpMediaManagementProperty(XmpMediaManagementType.ResourceRef, Category=XmpCategory.Internal)]
		ManagedFrom,

		[XmpMediaManagementProperty(XmpMediaManagementType.AgentName, Category=XmpCategory.Internal)]
		Manager,

		[XmpBasicProperty(XmpBasicType.URI, Category=XmpCategory.Internal)]
		ManageTo,

		[XmpBasicProperty(XmpBasicType.URI, Category=XmpCategory.Internal)]
		ManageUI,

		[XmpBasicProperty(XmpBasicType.Text, Category=XmpCategory.Internal)]
		ManagerVariant,

		[XmpBasicProperty(XmpBasicType.URI, Category=XmpCategory.Internal)]
		OriginalDocumentID,

		// TODO: bag struct
		[XmpBasicProperty(XmpBasicType.Unknown, Quantity=XmpQuantity.Bag, Category=XmpCategory.Internal)]
		Pantry,

		[XmpMediaManagementProperty(XmpMediaManagementType.RenditionClass, Category=XmpCategory.Internal)]
		RenditionClass,

		[XmpBasicProperty(XmpBasicType.Text, Category=XmpCategory.Internal)]
		RenditionParams,

		[XmpBasicProperty(XmpBasicType.Text, Category=XmpCategory.Internal)]
		VersionID,

		[XmpMediaManagementProperty(XmpMediaManagementType.Version, XmpQuantity.Seq, Category=XmpCategory.Internal)]
		Versions,

		[Obsolete("Deprecated for privacy protection.")]
		[XmpBasicProperty(XmpBasicType.URL, Category=XmpCategory.Internal)]
		LastURL,

		[Obsolete("Deprecated in favor of xmpMM:DerivedFrom. A reference to the document of which this is a rendition.")]
		[XmpMediaManagementProperty(XmpMediaManagementType.ResourceRef, Category=XmpCategory.Internal)]
		RenditionOf,

		[Obsolete("Deprecated. Previously used only to support the xmpMM:LastURL property.")]
		[XmpBasicProperty(XmpBasicType.Integer, Category=XmpCategory.Internal)]
		SaveID
	}
}
