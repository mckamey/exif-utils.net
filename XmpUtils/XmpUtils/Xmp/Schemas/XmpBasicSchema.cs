using System;

using XmpUtils.Xmp;
using XmpUtils.Xmp.ValueTypes;

namespace XmpUtils.Xmp.Schemas
{
	[XmpNamespace("http://ns.adobe.com/xap/1.0/", "xap,xmp")]
	public enum XmpBasicSchema
	{
		[XmpBasicProperty(XmpBasicType.XPath, XmpQuantity.Bag)]
		Advisory,

		[XmpBasicProperty(XmpBasicType.URL, Category=XmpCategory.Internal)]
		BaseURL,

		[XmpBasicProperty(XmpBasicType.Date, Category=XmpCategory.Internal)]
		CreateDate,

		[XmpMediaManagementProperty(XmpMediaManagementType.AgentName, Category=XmpCategory.Internal)]
		CreatorTool,

		[XmpBasicProperty(XmpBasicType.Text, XmpQuantity.Bag)]
		Identifier,

		[XmpBasicProperty(XmpBasicType.Text)]
		Label,

		[XmpBasicProperty(XmpBasicType.Date, Category=XmpCategory.Internal)]
		MetadataDate,

		[XmpBasicProperty(XmpBasicType.Date, Category=XmpCategory.Internal)]
		ModifyDate,

		[XmpBasicProperty(XmpBasicType.Text)]
		Nickname,

		[XmpBasicProperty(XmpBasicType.Integer)]
		Rating,

		[XmpBasicProperty(XmpBasicType.Thumbnail, XmpQuantity.Alt, Category=XmpCategory.Internal)]
		Thumbnails
	}
}
