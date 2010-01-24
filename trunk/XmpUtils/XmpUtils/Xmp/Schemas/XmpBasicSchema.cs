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

		[XmpBasicProperty(XmpBasicType.URL)]
		BaseURL,

		[XmpBasicProperty(XmpBasicType.Date)]
		CreateDate,

		[XmpMediaManagementProperty(XmpMediaManagementType.AgentName)]
		CreatorTool,

		[XmpBasicProperty(XmpBasicType.Text, XmpQuantity.Bag)]
		Identifier,

		[XmpBasicProperty(XmpBasicType.Text)]
		Label,

		[XmpBasicProperty(XmpBasicType.Date)]
		MetadataDate,

		[XmpBasicProperty(XmpBasicType.Date)]
		ModifyDate,

		[XmpBasicProperty(XmpBasicType.Text)]
		Nickname,

		[XmpBasicProperty(XmpBasicType.Integer)]
		Rating,

		[XmpBasicProperty(XmpBasicType.Thumbnail, XmpQuantity.Alt)]
		Thumbnails
	}
}
