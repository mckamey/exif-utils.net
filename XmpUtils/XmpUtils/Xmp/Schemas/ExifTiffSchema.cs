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
using System.ComponentModel;

using XmpUtils.Xmp.ValueTypes;

namespace XmpUtils.Xmp.Schemas
{
	/// <summary>
	/// Tagged Image File Format Rev 6.0 Schema
	/// </summary>
	[XmpNamespace("http://ns.adobe.com/tiff/1.0/", "tiff")]
	public enum ExifTiffSchema
	{
		#region Image Data Structure

		[XmpBasicProperty(XmpBasicType.Integer)]
		ImageWidth=0x0100,

		[XmpBasicProperty(XmpBasicType.Integer)]
		ImageLength=0x0101,

		[XmpBasicProperty(XmpBasicType.Integer, XmpQuantity.Bag)]
		BitsPerSample=0x0102,

		[XmpBasicProperty(XmpBasicType.Integer)]
		Compression=0x0103,

		[XmpBasicProperty(XmpBasicType.Integer)]
		PhotometricInterpretation=0x0106,

		[XmpBasicProperty(XmpBasicType.Integer)]
		Orientation=0x0112,

		[XmpBasicProperty(XmpBasicType.Integer)]
		SamplesPerPixel=0x0115,

		[XmpBasicProperty(XmpBasicType.Integer)]
		PlanarConfiguration=0x011C,

		[XmpBasicProperty(XmpBasicType.Integer, XmpQuantity.Seq)]
		YCbCrSubSampling=0x0212,

		[XmpBasicProperty(XmpBasicType.Integer)]
		YCbCrPositioning=0x0213,

		[ExifProperty(ExifType.Rational)]
		XResolution=0x011A,

		[ExifProperty(ExifType.Rational)]
		YResolution=0x011B,

		[XmpBasicProperty(XmpBasicType.Integer)]
		ResolutionUnit=0x0128,

		#endregion Image Data Structure

		#region Image Data Characteristics

		[XmpBasicProperty(XmpBasicType.Integer, XmpQuantity.Seq)]
		TransferFunction=0x012D,

		[ExifProperty(ExifType.Rational, XmpQuantity.Seq)]
		WhitePoint=0x013E,

		[ExifProperty(ExifType.Rational, XmpQuantity.Seq)]
		PrimaryChromaticities=0x013F,

		[ExifProperty(ExifType.Rational, XmpQuantity.Seq)]
		YCbCrCoefficients=0x0211,

		[ExifProperty(ExifType.Rational, XmpQuantity.Seq)]
		ReferenceBlackWhite=0x0214,

		#endregion Image Data Characteristics

		#region Other

		[XmpBasicProperty(XmpBasicType.Date)]
		DateTime=0x0132,

		[XmpBasicProperty(XmpBasicType.LangAlt, XmpQuantity.Alt)]
		ImageDescription=0x010E,

		[XmpBasicProperty(XmpBasicType.ProperName)]
		Make=0x010F,

		[XmpBasicProperty(XmpBasicType.ProperName)]
		Model=0x0110,

		[XmpMediaManagementProperty(XmpMediaManagementType.AgentName)]
		Software=0x0131,

		[XmpBasicProperty(XmpBasicType.ProperName)]
		Artist=0x013B,

		[XmpBasicProperty(XmpBasicType.LangAlt, XmpQuantity.Alt)]
		Copyright=0x8298,

		InteroperabilityIndex=0x5041,

		#endregion Other

		#region Recording Offset

		StripOffsets=0x0111,

		RowsPerStrip=0x0116,

		StripBytesCount=0x0117,

		JpegInterchangeFormat=0x0201,

		JpegInterchangeLength=0x0202

		#endregion Recording Offset
	}
}
