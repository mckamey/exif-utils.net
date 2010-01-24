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
	[XmpNamespace("http://ns.adobe.com/exif/1.0/", "exif")]
	public enum ExifSchema : ushort
	{
		#region EXIF IFD

		[Description("EXIF IFD Offset")]
		ExifIFDOffset=0x8769,

		[Description("GPS IFD Offset")]
		GpsIFDOffset=0x8825,

		[Description("Interop IFD Offset")]
		InteropIFDOffset=0xA005,

		#endregion EXIF IFD

		#region Interoperability IFD Fields

		[Description("Related Image File Format")]
		RelatedImageFileFormat=0x1000,

		[Description("Related Image Length")]
		RelatedImageLength=0x1001,

		#endregion Interoperability IFD Fields

		#region EXIF IFD

		#region Version

		[XmpBasicProperty(XmpBasicType.Text)]
		ExifVersion=0x9000,

		[XmpBasicProperty(XmpBasicType.Text)]
		FlashpixVersion=0xA000,

		#endregion Version

		#region Image Data Characteristics

		[XmpBasicProperty(XmpBasicType.Integer)]
		ColorSpace=0xA001,

		#endregion Image Data Characteristics

		#region Image Configuration

		[XmpBasicProperty(XmpBasicType.Integer, XmpQuantity.Seq)]
		ComponentsConfiguration=0x9101,

		[ExifProperty(ExifType.Rational)]
		CompressedBitsPerPixel=0x9102,

		[XmpBasicProperty(XmpBasicType.Integer)]
		PixelXDimension=0xA002,

		[XmpBasicProperty(XmpBasicType.Integer)]
		PixelYDimension=0xA003,

		#endregion Image Configuration

		#region User Information

		MakerNote=0x927C,

		[XmpBasicProperty(XmpBasicType.LangAlt, XmpQuantity.Alt)]
		UserComment=0x9286,

		#endregion User Information

		#region Related File Information

		[XmpBasicProperty(XmpBasicType.Text)]
		RelatedSoundFile=0xA004,

		#endregion Related File Information

		#region Date and Time

		[XmpBasicProperty(XmpBasicType.Date)]
		DateTimeOriginal=0x9003,

		[XmpBasicProperty(XmpBasicType.Date)]
		DateTimeDigitized=0x9004,

		SubSecTime=0x9290,

		SubSecTimeOriginal=0x9291,

		SubSecTimeDigitized=0x9292,

		#endregion Date and Time

		#region Picture Taking Conditions

		[ExifProperty(ExifType.Rational)]
		ExposureTime=0x829A,

		[ExifProperty(ExifType.Rational)]
		FNumber=0x829D,

		[XmpBasicProperty(XmpBasicType.Integer)]
		ExposureProgram=0x8822,

		[XmpBasicProperty(XmpBasicType.Text)]
		SpectralSensitivity=0x8824,

		[XmpBasicProperty(XmpBasicType.Integer, XmpQuantity.Seq)]
		ISOSpeedRatings=0x8827,

		[ExifProperty(ExifType.OECF_SFR)]
		OECF=0x8828,

		[ExifProperty(ExifType.Rational)]
		ShutterSpeedValue=0x9201,

		[ExifProperty(ExifType.Rational)]
		ApertureValue=0x9202,

		[ExifProperty(ExifType.Rational)]
		BrightnessValue=0x9203,

		[ExifProperty(ExifType.Rational)]
		ExposureBiasValue=0x9204,

		[ExifProperty(ExifType.Rational)]
		MaxApertureValue=0x9205,

		[ExifProperty(ExifType.Rational)]
		SubjectDistance=0x9206,

		[XmpBasicProperty(XmpBasicType.Integer)]
		MeteringMode=0x9207,

		[XmpBasicProperty(XmpBasicType.Integer)]
		LightSource=0x9208,

		[ExifProperty(ExifType.Flash, XmpQuantity.Struct)]
		Flash=0x9209,

		[ExifProperty(ExifType.Rational)]
		FocalLength=0x920A,

		[XmpBasicProperty(XmpBasicType.Integer, XmpQuantity.Seq)]
		SubjectArea=0x9214,

		[ExifProperty(ExifType.Rational)]
		FlashEnergy=0xA20B,

		[ExifProperty(ExifType.OECF_SFR)]
		SpatialFreqencyResponse=0xA20C,

		[ExifProperty(ExifType.Rational)]
		FocalPlaneXResolution=0xA20E,

		[ExifProperty(ExifType.Rational)]
		FocalPlaneYResolution=0xA20F,

		[XmpBasicProperty(XmpBasicType.Integer)]
		FocalPlaneResolutionUnit=0xA210,

		[XmpBasicProperty(XmpBasicType.Integer, XmpQuantity.Seq)]
		SubjectLocation=0xA214,

		[ExifProperty(ExifType.Rational)]
		ExposureIndex=0xA215,

		[XmpBasicProperty(XmpBasicType.Integer)]
		SensingMethod=0xA217,

		[XmpBasicProperty(XmpBasicType.Integer)]
		FileSource=0xA300,

		[XmpBasicProperty(XmpBasicType.Integer)]
		SceneType=0xA301,

		[ExifProperty(ExifType.CFAPattern)]
		CFAPattern=0xA302,

		[XmpBasicProperty(XmpBasicType.Integer)]
		CustomRendered=0xA401,

		[XmpBasicProperty(XmpBasicType.Integer)]
		ExposureMode=0xA402,

		[XmpBasicProperty(XmpBasicType.Integer)]
		WhiteBalance=0xA403,

		[ExifProperty(ExifType.Rational)]
		DigitalZoomRatio=0xA404,

		[XmpBasicProperty(XmpBasicType.Integer)]
		FocalLengthIn35mmFilm=0xA405,

		[XmpBasicProperty(XmpBasicType.Integer)]
		SceneCaptureType=0xA406,

		[XmpBasicProperty(XmpBasicType.Integer)]
		GainControl=0xA407,

		[XmpBasicProperty(XmpBasicType.Integer)]
		Contrast=0xA408,

		[XmpBasicProperty(XmpBasicType.Integer)]
		Saturation=0xA409,

		[XmpBasicProperty(XmpBasicType.Integer)]
		Sharpness=0xA40A,

		[XmpBasicProperty(XmpBasicType.Integer)]
		DeviceSettingDescription=0xA40B,

		[XmpBasicProperty(XmpBasicType.Integer)]
		SubjectDistanceRange=0xA40C,

		#endregion Picture Taking Conditions

		#region Other

		[XmpBasicProperty(XmpBasicType.Text)]
		ImageUniqueID=0xA420,

		#endregion Other

		#endregion EXIF IFD

		#region Global Positioning System (GPS)

		[XmpBasicProperty(XmpBasicType.Text)]
		GPSVersionID=0x0000,

		GPSLatitudeRef=0x0001,

		[ExifProperty(ExifType.GPSCoordinate)]
		GPSLatitude=0x0002,

		GPSLongitudeRef=0x0003,

		[ExifProperty(ExifType.GPSCoordinate)]
		GPSLongitude=0x0004,

		GPSAltitudeRef=0x0005,

		[ExifProperty(ExifType.Rational)]
		GPSAltitude=0x0006,

		[XmpBasicProperty(XmpBasicType.Date)]
		GPSTimeStamp=0x0007,

		[XmpBasicProperty(XmpBasicType.Text)]
		GPSSatellites=0x0008,

		[XmpBasicProperty(XmpBasicType.Text)]
		GPSStatus=0x0009,

		[XmpBasicProperty(XmpBasicType.Text)]
		GPSMeasureMode=0x000A,

		[ExifProperty(ExifType.Rational)]
		GPSDOP=0x000B,

		[XmpBasicProperty(XmpBasicType.Text)]
		GPSSpeedRef=0x000C,

		[ExifProperty(ExifType.Rational)]
		GPSSpeed=0x000D,

		[XmpBasicProperty(XmpBasicType.Text)]
		GPSTrackRef=0x000E,

		[ExifProperty(ExifType.Rational)]
		GPSTrack=0x000F,

		[XmpBasicProperty(XmpBasicType.Text)]
		GPSImgDirectionRef=0x0010,

		[ExifProperty(ExifType.Rational)]
		GPSImgDirection=0x0011,

		[XmpBasicProperty(XmpBasicType.Text)]
		GPSMapDatum=0x0012,

		[XmpBasicProperty(XmpBasicType.Text)]
		GPSDestLatitudeRef=0x0013,

		[ExifProperty(ExifType.GPSCoordinate)]
		GPSDestLatitude=0x0014,

		[XmpBasicProperty(XmpBasicType.Text)]
		GPSDestLongitudeRef=0x0015,

		[ExifProperty(ExifType.GPSCoordinate)]
		GPSDestLongitude=0x0016,

		[XmpBasicProperty(XmpBasicType.Text)]
		GPSDestBearingRef=0x0017,

		[ExifProperty(ExifType.Rational)]
		GPSDestBearing=0x0018,

		[XmpBasicProperty(XmpBasicType.Text)]
		GPSDestDistanceRef=0x0019,

		[ExifProperty(ExifType.Rational)]
		GPSDestDistance=0x001A,

		[XmpBasicProperty(XmpBasicType.Text)]
		GPSProcessingMethod=0x001B,

		[XmpBasicProperty(XmpBasicType.Text)]
		GPSAreaInformation=0x001C,

		[XmpBasicProperty(XmpBasicType.Date)]
		GPSDateStamp=0x001D,

		[XmpBasicProperty(XmpBasicType.Text)]
		GPSDifferential=0x001E,

		#endregion Global Positioning System (GPS)

		#region Thumbnail

		[Description("Thumbnail Image Height")]
		ThumbnailImageHeight=0x5021,

		[Description("Thumbnail Image Width")]
		ThumbnailImageWidth=0x5020,

		[Description("Thumbnail Bits Per Sample")]
		ThumbnailBitsPerSample=0x5022,

		[Description("Thumbnail Compression")]
		ThumbnailCompression=0x5023,

		[Description("Thumbnail Photometric Interpretation")]
		ThumbnailPhotometricInterpretation=0x5024,

		[Description("Thumbnail Image Description")]
		ThumbnailImageDescription=0x5025,

		[Description("Thumbnail Make")]
		ThumbnailMake=0x5026,

		[Description("Thumbnail Model")]
		ThumbnailModel=0x5027,

		[Description("Thumbnail Strip Offsets")]
		ThumbnailStripOffsets=0x5028,

		[Description("Thumbnail Orientation")]
		ThumbnailOrientation=0x5029,

		[Description("Thumbnail Samples Per Pixel")]
		ThumbnailSamplesPerPixel=0x502A,

		[Description("Thumbnail RowsPerStrip")]
		ThumbnailRowsPerStrip=0x502B,

		[Description("Thumbnail Strip Bytes Count")]
		ThumbnailStripBytesCount=0x502C,

		[Description("Thumbnail Horizontal Resolution")]
		ThumbnailXResolution=0x502D,

		[Description("Thumbnail Vertical Resolution")]
		ThumbnailYResolution=0x502E,

		[Description("Thumbnail Planar Config")]
		ThumbnailPlanarConfig=0x502F,

		[Description("Thumbnail Resolution Unit")]
		ThumbnailResolutionUnit=0x5030,

		[Description("Thumbnail Transfer Function")]
		ThumbnailTransferFunction=0x5031,

		[Description("Thumbnail Software")]
		ThumbnailSoftware=0x5032,

		[Description("Thumbnail DateStamp")]
		ThumbnailDateTime=0x5033,

		[Description("Thumbnail Artist")]
		ThumbnailArtist=0x5034,

		[Description("Thumbnail WhitePoint")]
		ThumbnailWhitePoint=0x5035,

		[Description("Thumbnail Primary Chromaticities")]
		ThumbnailPrimaryChromaticities=0x5036,

		[Description("Thumbnail YCbCr Coefficients")]
		ThumbnailYCbCrCoefficients=0x5037,

		[Description("Thumbnail YCbCr SubSampling")]
		ThumbnailYCbCrSubSampling=0x5038,

		[Description("Thumbnail YCbCr Positioning")]
		ThumbnailYCbCrPositioning=0x5039,

		[Description("Thumbnail Reference Black White")]
		ThumbnailReferenceBlackWhite=0x503A,

		[Description("Thumbnail Copyright")]
		ThumbnailCopyright=0x503B,

		[Description("Thumbnail Color Depth")]
		ThumbnailColorDepth=0x5015,

		[Description("Thumbnail Compressed Size")]
		ThumbnailCompressedSize=0x5019,

		[Description("Thumbnail Data")]
		ThumbnailData=0x501B,

		[Description("Thumbnail Format")]
		ThumbnailFormat=0x5012,

		[Description("Thumbnail Height")]
		ThumbnailHeight=0x5014,

		[Description("Thumbnail Planes")]
		ThumbnailPlanes=0x5016,

		[Description("Thumbnail Raw Bytes")]
		ThumbnailRawBytes=0x5017,

		[Description("Thumbnail Size")]
		Thumbnail=0x5018,

		[Description("Thumbnail Width")]
		ThumbnailWidth=0x5013,

		#endregion Thumbnail

		#region Other

		[Description("Cell Height")]
		CellHeight=0x0109,

		[Description("Cell Width")]
		CellWidth=0x0108,

		[Description("Chrominance Table")]
		ChrominanceTable=0x5091,

		[Description("Clean Fax Data")]
		CleanFaxData=0x0147,

		[Description("Color Map")]
		ColorMap=0x0140,

		[Description("Color Transfer Function")]
		ColorTransferFunction=0x501A,

		[Description("Document Name")]
		DocumentName=0x010D,

		[Description("Dot Range")]
		DotRange=0x0150,

		[Description("Extra Samples")]
		ExtraSamples=0x0152,

		[Description("Fill Order")]
		FillOrder=0x010A,

		[Description("Frame Delay")]
		FrameDelay=0x5100,

		[Description("Free Byte Counts")]
		FreeByteCounts=0x0121,

		[Description("Free Offset")]
		FreeOffset=0x0120,

		Gamma=0x0301,

		[Description("Global Palette")]
		GlobalPalette=0x5102,

		[Description("Gray Response Curve")]
		GrayResponseCurve=0x0123,

		[Description("Gray Response Unit")]
		GrayResponseUnit=0x0122,

		[Description("Grid Size")]
		GridSize=0x5011,

		[Description("Halftone Degree")]
		HalftoneDegree=0x500C,

		[Description("Halftone Hints")]
		HalftoneHints=0x0141,

		[Description("Halftone LPI")]
		HalftoneLPI=0x500A,

		[Description("Halftone LPI Unit")]
		HalftoneLPIUnit=0x500B,

		[Description("Halftone Misc")]
		HalftoneMisc=0x500E,

		[Description("Halftone Screen")]
		HalftoneScreen=0x500F,

		[Description("Halftone Shape")]
		HalftoneShape=0x500D,

		[Description("Host Computer")]
		HostComputer=0x013C,

		[Description("ICC Profile")]
		ICCProfile=0x8773,

		[Description("ICC Profile Descriptor")]
		ICCProfileDescriptor=0x0302,

		[Description("Image Title")]
		ImageTitle=0x0320,

		[Description("Index Background")]
		IndexBackground=0x5103,

		[Description("Index Transparent")]
		IndexTransparent=0x5104,

		[Description("Ink Names")]
		InkNames=0x014D,

		[Description("Ink Set")]
		InkSet=0x014C,

		[Description("JPEG AC Tables")]
		JpegACTables=0x0209,

		[Description("JPEG DC Tables")]
		JpegDCTables=0x0208,

		[Description("JPEG Lossless Predictors")]
		JpegLosslessPredictors=0x0205,

		[Description("JPEG Point Transforms")]
		JpegPointTransforms=0x0206,

		[Description("JPEG Proc")]
		JpegProc=0x0200,

		[Description("JPEG Q Tables")]
		JpegQTables=0x0207,

		[Description("JPEG Quality")]
		JpegQuality=0x5010,

		[Description("JPEG Restart Interval")]
		JpegRestartInterval=0x0203,

		[Description("Loop Count")]
		LoopCount=0x5101,

		[Description("Luminance Table")]
		LuminanceTable=0x5090,

		[Description("Max Sample Value")]
		MaxSampleValue=0x0119,

		[Description("Min Sample Value")]
		MinSampleValue=0x0118,

		[Description("New Subfile Type")]
		NewSubfileType=0x00FE,

		[Description("Number Of Inks")]
		NumberOfInks=0x014E,

		[Description("Page Name")]
		PageName=0x011D,

		[Description("Page Number")]
		PageNumber=0x0129,

		[Description("Palette Histogram")]
		PaletteHistogram=0x5113,

		[Description("Pixel Per Unit X")]
		PixelPerUnitX=0x5111,

		[Description("Pixel Per Unit Y")]
		PixelPerUnitY=0x5112,

		[Description("Pixel Unit")]
		PixelUnit=0x5110,

		Predictor=0x013D,

		[Description("Print Flags")]
		PrintFlags=0x5005,

		[Description("Print Flags Bleed Width")]
		PrintFlagsBleedWidth=0x5008,

		[Description("Print Flags Bleed Width Scale")]
		PrintFlagsBleedWidthScale=0x5009,

		[Description("Print Flags Crop")]
		PrintFlagsCrop=0x5007,

		[Description("Print Flags Version")]
		PrintFlagsVersion=0x5006,

		[Description("Horizontal Resolution Length Unit")]
		ResolutionXLengthUnit=0x5003,

		[Description("Horizontal Resolution Unit")]
		ResolutionXUnit=0x5001,

		[Description("Vertical Resolution Length Unit")]
		ResolutionYLengthUnit=0x5004,

		[Description("Vertical Resolution Unit")]
		ResolutionYUnit=0x5002,

		[Description("Sample Format")]
		SampleFormat=0x0153,

		[Description("SMax Sample Value")]
		SMaxSampleValue=0x0155,

		[Description("SMin Sample Value")]
		SMinSampleValue=0x0154,

		[Description("sRGB Rendering Intent")]
		SRGBRenderingIntent=0x0303,

		[Description("Subfile Type")]
		SubfileType=0x00FF,

		[Description("T4 Option")]
		T4Option=0x0124,

		[Description("T6 Option")]
		T6Option=0x0125,

		[Description("Target Printer")]
		TargetPrinter=0x0151,

		Threshholding=0x0107,

		[Description("Tile Byte Counts")]
		TileByteCounts=0x0145,

		[Description("Tile Length")]
		TileLength=0x0143,

		[Description("Tile Offset")]
		TileOffset=0x0144,

		[Description("Tile Width")]
		TileWidth=0x0142,

		[Description("Transfer Range")]
		TransferRange=0x0156,

		[Description("Horizontal Position")]
		XPosition=0x011E,

		[Description("Vertical Position")]
		YPosition=0x011F,

		[Description("Application Notes")]
		ApplicationNotes=0x02BC,

		#endregion Other

		#region Microsoft Fields

		[Description("Title")]
		MSTitle=0x9C9B,

		[Description("Comments")]
		MSComments=0x9C9C,

		[Description("Author")]
		MSAuthor=0x9C9D,

		[Description("Keywords")]
		MSKeywords=0x9C9E,

		[Description("Subject")]
		MSSubject=0x9C9F

		#endregion Microsoft Fields
	}
}
