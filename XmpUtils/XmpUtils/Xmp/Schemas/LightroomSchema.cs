using System;

using XmpUtils.Xmp;
using XmpUtils.Xmp.ValueTypes;

namespace XmpUtils.Xmp.Schemas
{
	[XmpNamespace("http://ns.adobe.com/lightroom/1.0/", "lr")]
	public enum LightroomSchema
	{
		[XmpBasicProperty(XmpBasicType.Text, XmpQuantity.Bag, Name="hierarchicalSubject")]
		HierarchicalSubject
	}
}
