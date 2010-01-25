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
	/// <summary>
	/// Tagged Image File Format Rev 6.0 Schema
	/// </summary>
	[XmpNamespace("http://ns.adobe.com/tiff/1.0/", "tiff")]
	public enum ExifTiffSchema : ushort
	{
		#region Image Data Structure

		[XmpBasicProperty(XmpBasicType.Integer, Category=XmpCategory.Internal)]
		ImageWidth=0x0100,

		[XmpBasicProperty(XmpBasicType.Integer, Category=XmpCategory.Internal)]
		ImageLength=0x0101,

		[XmpBasicProperty(XmpBasicType.Integer, XmpQuantity.Bag, Category=XmpCategory.Internal)]
		BitsPerSample=0x0102,

		// TODO: enum
		[XmpBasicProperty(XmpBasicType.Integer, Category=XmpCategory.Internal)]
		Compression=0x0103,

		// TODO: enum
		[XmpBasicProperty(XmpBasicType.Integer, Category=XmpCategory.Internal)]
		PhotometricInterpretation=0x0106,

		// TODO: enum
		[XmpBasicProperty(XmpBasicType.Integer, Category=XmpCategory.Internal)]
		Orientation=0x0112,

		[XmpBasicProperty(XmpBasicType.Integer, Category=XmpCategory.Internal)]
		SamplesPerPixel=0x0115,

		// TODO: enum
		[XmpBasicProperty(XmpBasicType.Integer, Category=XmpCategory.Internal)]
		PlanarConfiguration=0x011C,

		// TODO: enum
		[XmpBasicProperty(XmpBasicType.Integer, XmpQuantity.Seq, Category=XmpCategory.Internal)]
		YCbCrSubSampling=0x0212,

		// TODO: enum
		[XmpBasicProperty(XmpBasicType.Integer, Category=XmpCategory.Internal)]
		YCbCrPositioning=0x0213,

		[ExifProperty(ExifType.Rational, Category=XmpCategory.Internal)]
		XResolution=0x011A,

		// TODO: enum
		[ExifProperty(ExifType.Rational, Category=XmpCategory.Internal)]
		YResolution=0x011B,

		[XmpBasicProperty(XmpBasicType.Integer, Category=XmpCategory.Internal)]
		ResolutionUnit=0x0128,

		#endregion Image Data Structure

		#region Image Data Characteristics

		[XmpBasicProperty(XmpBasicType.Integer, XmpQuantity.Seq, Category=XmpCategory.Internal)]
		TransferFunction=0x012D,

		[ExifProperty(ExifType.Rational, XmpQuantity.Seq, Category=XmpCategory.Internal)]
		WhitePoint=0x013E,

		[ExifProperty(ExifType.Rational, XmpQuantity.Seq, Category=XmpCategory.Internal)]
		PrimaryChromaticities=0x013F,

		[ExifProperty(ExifType.Rational, XmpQuantity.Seq, Category=XmpCategory.Internal)]
		YCbCrCoefficients=0x0211,

		[ExifProperty(ExifType.Rational, XmpQuantity.Seq, Category=XmpCategory.Internal)]
		ReferenceBlackWhite=0x0214,

		#endregion Image Data Characteristics

		#region Other

		[XmpBasicProperty(XmpBasicType.Date, Category=XmpCategory.Internal)]
		DateTime=0x0132,

		[XmpBasicProperty(XmpBasicType.LangAlt)]
		ImageDescription=0x010E,

		[XmpBasicProperty(XmpBasicType.ProperName, Category=XmpCategory.Internal)]
		Make=0x010F,

		[XmpBasicProperty(XmpBasicType.ProperName, Category=XmpCategory.Internal)]
		Model=0x0110,

		[XmpMediaManagementProperty(XmpMediaManagementType.AgentName, Category=XmpCategory.Internal)]
		Software=0x0131,

		[XmpBasicProperty(XmpBasicType.ProperName)]
		Artist=0x013B,

		[XmpBasicProperty(XmpBasicType.LangAlt)]
		Copyright=0x8298,

		#endregion Other

		#region Recording Offset

		InteroperabilityIndex=0x5041,

		StripOffsets=0x0111,

		RowsPerStrip=0x0116,

		StripBytesCount=0x0117,

		JpegInterchangeFormat=0x0201,

		JpegInterchangeLength=0x0202

		#endregion Recording Offset
	}
}
