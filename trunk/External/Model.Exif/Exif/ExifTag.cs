using System;
using System.ComponentModel;

using PhotoLib.Model.Exif.TagValues;

namespace PhotoLib.Model.Exif
{
	/// <summary>
	/// Defines EXIF Property Tag ID values.
	/// </summary>
	/// <remarks>
	/// References:
	/// http://www.exif.org/Exif2-2.PDF
	/// http://msdn.microsoft.com/library/en-us/gdicpp/GDIPlus/GDIPlusreference/constants/imagepropertytagconstants/propertytagsinnumericalorder.asp
	/// http://msdn.microsoft.com/library/en-us/gdicpp/GDIPlus/GDIPlusreference/constants/imagepropertytagconstants/propertyitemdescriptions.asp
	/// http://www.sno.phy.queensu.ca/~phil/exiftool/TagNames/EXIF.html
	/// http://search.cpan.org/src/EXIFTOOL/Image-ExifTool-6.36/html/TagNames/EXIF.html
	/// </remarks>
	public enum ExifTag : int
	{
		#region Unknown

		Unknown = Int32.MinValue,

		#endregion Unknown

		#region EXIF IFD

		[Description("EXIF IFD Offset")]
		ExifIFDOffset = 0x8769,
		[Description("GPS IFD Offset")]
		GpsIFDOffset = 0x8825,
		[Description("Interop IFD Offset")]
		InteropIFDOffset = 0xA005,

		#endregion EXIF IFD

		#region Interoperability IFD Fields

		[Description("Related Image File Format")]
		RelatedImageFileFormat = 0x1000,
		[Description("Related Image Length")]
		RelatedImageLength = 0x1001,

		#endregion Interoperability IFD Fields

		#region TIFF Rev 6.0

		#region Image Data Structure

		[Description("Image Width")]
		ImageWidth = 0x0100,
		[Description("Image Height")]
		ImageHeight = 0x0101,
		[Description("Bits Per Sample")]
		BitsPerSample = 0x0102,
		[ExifDataType(typeof(ExifTagCompression), ExifType.UInt16)]
		Compression = 0x0103,
		[ExifDataType(typeof(ExifTagPhotometricInterpretation), ExifType.UInt16)]
		[Description("Photometric Interpretation")]
		PhotometricInterpretation = 0x0106,
		[ExifDataType(typeof(ExifTagOrientation), ExifType.UInt16)]
		Orientation = 0x0112,
		[Description("Samples Per Pixel")]
		SamplesPerPixel = 0x0115,
		[ExifDataType(typeof(ExifTagPlanarConfiguration), ExifType.UInt16)]
		[Description("Planar Configuration")]
		PlanarConfiguration = 0x011C,
		[Description("YCbCr SubSampling")]
		YCbCrSubSampling = 0x0212,
		[ExifDataType(typeof(ExifTagYCbCrPositioning), ExifType.UInt16)]
		[Description("YCbCr Positioning")]
		YCbCrPositioning = 0x0213,
		[Description("Horizontal Resolution")]
		XResolution = 0x011A,
		[Description("Vertical Resolution")]
		YResolution = 0x011B,
		[ExifDataType(typeof(ExifTagResolutionUnit), ExifType.UInt16)]
		[Description("Resolution Unit")]
		ResolutionUnit = 0x0128,

		#endregion Image Data Structure

		#region Recording Offset

		[Description("Strip Offsets")]
		StripOffsets = 0x0111,
		[Description("Rows Per Strip")]
		RowsPerStrip = 0x0116,
		[Description("Strip Bytes Count")]
		StripBytesCount = 0x0117,
		[Description("JPEG Interchange Format")]
		JpegInterchangeFormat = 0x0201,
		[Description("JPEG Interchange Length")]
		JpegInterchangeLength = 0x0202,

		#endregion Recording Offset

		#region Image Data Characteristics

		[Description("Transfer Function")]
		TransferFunction=0x012D,
		WhitePoint = 0x013E,
		[Description("Primary Chromaticities")]
		PrimaryChromaticities = 0x013F,
		[Description("YCbCr Coefficients")]
		YCbCrCoefficients = 0x0211,
		[Description("Reference Black White")]
		ReferenceBlackWhite = 0x0214,

		#endregion Image Data Characteristics

		#region Other

		[ExifDataType(typeof(DateTime), ExifType.Ascii)]
		DateTime = 0x0132,
		[ExifDataType(ExifType.Ascii)]
		[Description("Image Description")]
		ImageDescription = 0x010E,
		Make = 0x010F,
		Model = 0x0110,
		Software = 0x0131,
		[ExifDataType(ExifType.Ascii)]
		Artist=0x013B,
		[ExifDataType(ExifType.Ascii)]
		Copyright=0x8298,
		[Description("Interoperability Index")]
		InteroperabilityIndex = 0x5041,

		#endregion Other

		#endregion TIFF Rev 6.0

		#region EXIF IFD

		#region Version

		[Description("EXIF Version")]
		ExifVersion = 0x9000,
		[Description("Flashpix Version")]
		FlashpixVersion = 0xA000,

		#endregion Version

		#region Image Data Characteristics

		[ExifDataType(typeof(ExifTagColorSpace), ExifType.UInt16)]
		ColorSpace = 0xA001,

		#endregion Image Data Characteristics

		#region Image Configuration

		[Description("Components Config")]
		ComponentsConfig = 0x9101,
		[Description("Compressed Bits Per Pixel")]
		CompressedBitsPerPixel = 0x9102,
		[Description("Compressed Image Width")]
		CompressedImageWidth = 0xA002,
		[Description("Compressed Image Height")]
		CompressedImageHeight = 0xA003,

		#endregion Image Configuration

		#region User Information

		[Description("Maker Note")]
		MakerNote = 0x927C,
		[Description("User Comment")]
		UserComment = 0x9286,

		#endregion User Information

		#region Related File Information

		[Description("Related Audio File")]
		RelatedAudioFile = 0xA004,

		#endregion Related File Information

		#region Date and Time

		[ExifDataType(typeof(DateTime), ExifType.Ascii)]
		[Description("DateTime Original")]
		DateTimeOriginal = 0x9003,
		[ExifDataType(typeof(DateTime), ExifType.Ascii)]
		[Description("DateTime Digitized")]
		DateTimeDigitized = 0x9004,
		[Description("SubSec Time")]
		SubSecTime = 0x9290,
		[Description("SubSec Time Original")]
		SubSecTimeOriginal = 0x9291,
		[Description("SubSec Time Digitized")]
		SubSecTimeDigitized = 0x9292,

		#endregion Date and Time

		#region Picture Taking Conditions

		[Description("Exposure Time")]
		ExposureTime = 0x829A,
		[Description("F-Stop")]
		FNumber = 0x829D,
		[ExifDataType(typeof(ExifTagExposureProgram), ExifType.UInt16)]
		[Description("Exposure Program")]
		ExposureProgram = 0x8822,
		[Description("Spectral Sensitivity")]
		SpectralSensitivity = 0x8824,
		[Description("ISO Speed")]
		ISOSpeed = 0x8827,
		OECF = 0x8828,
		[Description("Shutter Speed")]
		ShutterSpeed = 0x9201,
		Aperture = 0x9202,
		Brightness = 0x9203,
		[Description("Exposure Bias")]
		ExposureBias = 0x9204,
		[Description("Max Aperture")]
		MaxAperture = 0x9205,
		[Description("Subject Distance")]
		SubjectDistance = 0x9206,
		[ExifDataType(typeof(ExifTagMeteringMode), ExifType.UInt16)]
		[Description("Metering Mode")]
		MeteringMode = 0x9207,
		[ExifDataType(typeof(ExifTagLightSource), ExifType.UInt16)]
		[Description("Light Source")]
		LightSource = 0x9208,
		[ExifDataType(typeof(ExifTagFlash), ExifType.UInt16)]
		Flash = 0x9209,
		[Description("Focal Length")]
		FocalLength = 0x920A,
		[Description("Subject Area")]
		SubjectArea = 0x9214,
		[Description("Flash Energy")]
		FlashEnergy = 0xA20B,
		[Description("Spatial Freqency Response")]
		SpatialFreqencyResponse = 0xA20C,
		[Description("Focal Plane Horizontal Resolution")]
		FocalPlaneXResolution = 0xA20E,
		[Description("Focal Plane Vertical Resolution")]
		FocalPlaneYResolution = 0xA20F,
		[ExifDataType(typeof(ExifTagResolutionUnit), ExifType.UInt16)]
		[Description("Focal Plane Resolution Unit")]
		FocalPlaneResolutionUnit = 0xA210,
		[Description("Subject Location")]
		SubjectLocation = 0xA214,
		[Description("Exposure Index")]
		ExposureIndex = 0xA215,
		[ExifDataType(typeof(ExifTagSensingMethod), ExifType.UInt16)]
		[Description("Sensing Method")]
		SensingMethod = 0xA217,
		[ExifDataType(typeof(ExifTagFileSource), ExifType.UInt16)]
		[Description("File Source")]
		FileSource = 0xA300,
		[ExifDataType(typeof(ExifTagSceneType), ExifType.UInt16)]
		[Description("Scene Type")]
		SceneType = 0xA301,
		[Description("CFA Pattern")]
		CfaPattern = 0xA302,
		[ExifDataType(typeof(ExifTagCustomRendered), ExifType.UInt16)]
		[Description("Custom Rendered")]
		CustomRendered = 0xA401,
		[ExifDataType(typeof(ExifTagExposureMode), ExifType.UInt16)]
		[Description("Exposure Mode")]
		ExposureMode = 0xA402,
		[ExifDataType(typeof(ExifTagWhiteBalance), ExifType.UInt16)]
		[Description("White Balance")]
		WhiteBalance = 0xA403,
		[Description("Digital Zoom Ratio")]
		DigitalZoomRatio = 0xA404,
		[Description("Focal Length In 35mm Film")]
		FocalLengthIn35mmFilm = 0xA405,
		[ExifDataType(typeof(ExifTagSceneCaptureType), ExifType.UInt16)]
		[Description("Scene Capture Type")]
		SceneCaptureType = 0xA406,
		[ExifDataType(typeof(ExifTagGainControl), ExifType.UInt16)]
		[Description("Gain Control")]
		GainControl = 0xA407,
		[ExifDataType(typeof(ExifTagContrast), ExifType.UInt16)]
		Contrast = 0xA408,
		[ExifDataType(typeof(ExifTagSaturation), ExifType.UInt16)]
		Saturation = 0xA409,
		[ExifDataType(typeof(ExifTagSharpness), ExifType.UInt16)]
		Sharpness = 0xA40A,
		[Description("Device Setting Description")]
		DeviceSettingDescription = 0xA40B,
		[ExifDataType(typeof(ExifTagSubjectDistanceRange), ExifType.UInt16)]
		[Description("Subject Distance Range")]
		SubjectDistanceRange = 0xA40C,

		#endregion Picture Taking Conditions

		#region Other

		[Description("Image Unique ID")]
		ImageUniqueID = 0xA420,

		#endregion Other

		#endregion EXIF IFD

		#region Global Positioning System (GPS)

		[Description("GPS Version ID")]
		GpsVersionID = 0x0000,
		[Description("GPS Latitude Ref")]
		GpsLatitudeRef = 0x0001,
		[Description("GPS Latitude")]
		GpsLatitude = 0x0002,
		[Description("GPS Longitude Ref")]
		GpsLongitudeRef = 0x0003,
		[Description("GPS Longitude")]
		GpsLongitude = 0x0004,
		[ExifDataType(typeof(ExifTagGpsAltitudeRef), ExifType.UInt16)]
		[Description("GPS Altitude Ref")]
		GpsAltitudeRef = 0x0005,
		[Description("GPS Altitude")]
		GpsAltitude = 0x0006,
		[Description("GPS TimeStamp")]
		GpsTimeStamp = 0x0007,
		[Description("GPS Satellites")]
		GpsSatellites = 0x0008,
		[Description("GPS Status")]
		GpsStatus = 0x0009,
		[Description("GPS Measure Mode")]
		GpsMeasureMode = 0x000A,
		[Description("GPS DOP")]
		GpsDOP = 0x000B,
		[Description("GPS Speed Ref")]
		GpsSpeedRef = 0x000C,
		[Description("GPS Speed")]
		GpsSpeed = 0x000D,
		[Description("GPS Track Ref")]
		GpsTrackRef = 0x000E,
		[Description("GPS Track")]
		GpsTrack = 0x000F,
		[Description("GPS Image Direction Ref")]
		GpsImgDirectionRef = 0x0010,
		[Description("GPS Image Direction")]
		GpsImgDirection = 0x0011,
		[Description("GPS Map Datum")]
		GpsMapDatum = 0x0012,
		[Description("GPS Dest Latitude Ref")]
		GpsDestLatitudeRef = 0x0013,
		[Description("GPS Dest Latitude")]
		GpsDestLatitude = 0x0014,
		[Description("GPS Dest Longitude Ref")]
		GpsDestLongitudeRef = 0x0015,
		[Description("GPS Dest Longitude")]
		GpsDestLongitude = 0x0016,
		[Description("GPS Dest Bearing Ref")]
		GpsDestBearingRef = 0x0017,
		[Description("GPS Dest Bearing")]
		GpsDestBearing = 0x0018,
		[Description("GPS Dest Distance Ref")]
		GpsDestDistanceRef = 0x0019,
		[Description("GPS Dest Distance")]
		GpsDestDistance = 0x001A,
		[Description("GPS Processing Method")]
		GpsProcessingMethod = 0x001B,
		[Description("GPS Area Information")]
		GpsAreaInformation = 0x001C,
		[ExifDataType(typeof(DateTime), ExifType.Ascii)]
		[Description("GPS DateStamp")]
		GpsDateStamp = 0x001D,
		[ExifDataType(typeof(ExifTagGpsDifferential), ExifType.UInt16)]
		[Description("GPS Differential")]
		GpsDifferential = 0x001E,

		#endregion Global Positioning System (GPS)

		#region Thumbnail

		[Description("Thumbnail Image Height")]
		ThumbnailImageHeight = 0x5021,
		[Description("Thumbnail Image Width")]
		ThumbnailImageWidth = 0x5020,
		[Description("Thumbnail Bits Per Sample")]
		ThumbnailBitsPerSample = 0x5022,
		[ExifDataType(typeof(ExifTagCompression), ExifType.UInt16)]
		[Description("Thumbnail Compression")]
		ThumbnailCompression = 0x5023,
		[Description("Thumbnail Photometric Interpretation")]
		ThumbnailPhotometricInterpretation = 0x5024,
		[Description("Thumbnail Image Description")]
		ThumbnailImageDescription = 0x5025,
		[Description("Thumbnail Make")]
		ThumbnailMake = 0x5026,
		[Description("Thumbnail Model")]
		ThumbnailModel = 0x5027,
		[Description("Thumbnail Strip Offsets")]
		ThumbnailStripOffsets = 0x5028,
		[ExifDataType(typeof(ExifTagOrientation), ExifType.UInt16)]
		[Description("Thumbnail Orientation")]
		ThumbnailOrientation = 0x5029,
		[Description("Thumbnail Samples Per Pixel")]
		ThumbnailSamplesPerPixel = 0x502A,
		[Description("Thumbnail RowsPerStrip")]
		ThumbnailRowsPerStrip = 0x502B,
		[Description("Thumbnail Strip Bytes Count")]
		ThumbnailStripBytesCount = 0x502C,
		[Description("Thumbnail Horizontal Resolution")]
		ThumbnailXResolution = 0x502D,
		[Description("Thumbnail Vertical Resolution")]
		ThumbnailYResolution = 0x502E,
		[Description("Thumbnail Planar Config")]
		ThumbnailPlanarConfig = 0x502F,
		[ExifDataType(typeof(ExifTagResolutionUnit), ExifType.UInt16)]
		[Description("Thumbnail Resolution Unit")]
		ThumbnailResolutionUnit = 0x5030,
		[Description("Thumbnail Transfer Function")]
		ThumbnailTransferFunction = 0x5031,
		[Description("Thumbnail Software")]
		ThumbnailSoftware = 0x5032,
		[ExifDataType(typeof(DateTime), ExifType.Ascii)]
		[Description("Thumbnail DateTime")]
		ThumbnailDateTime = 0x5033,
		[Description("Thumbnail Artist")]
		ThumbnailArtist = 0x5034,
		[Description("Thumbnail WhitePoint")]
		ThumbnailWhitePoint = 0x5035,
		[Description("Thumbnail Primary Chromaticities")]
		ThumbnailPrimaryChromaticities = 0x5036,
		[Description("Thumbnail YCbCr Coefficients")]
		ThumbnailYCbCrCoefficients = 0x5037,
		[Description("Thumbnail YCbCr SubSampling")]
		ThumbnailYCbCrSubSampling = 0x5038,
		[Description("Thumbnail YCbCr Positioning")]
		ThumbnailYCbCrPositioning = 0x5039,
		[Description("Thumbnail Reference Black White")]
		ThumbnailReferenceBlackWhite = 0x503A,
		[Description("Thumbnail Copyright")]
		ThumbnailCopyright = 0x503B,

		[Description("Thumbnail Color Depth")]
		ThumbnailColorDepth = 0x5015,
		[Description("Thumbnail Compressed Size")]
		ThumbnailCompressedSize = 0x5019,
		[Description("Thumbnail Data")]
		ThumbnailData = 0x501B,
		[Description("Thumbnail Format")]
		ThumbnailFormat = 0x5012,
		[Description("Thumbnail Height")]
		ThumbnailHeight = 0x5014,
		[Description("Thumbnail Planes")]
		ThumbnailPlanes = 0x5016,
		[Description("Thumbnail Raw Bytes")]
		ThumbnailRawBytes = 0x5017,
		[Description("Thumbnail Size")]
		Thumbnail = 0x5018,
		[Description("Thumbnail Width")]
		ThumbnailWidth = 0x5013,

		#endregion Thumbnail

		#region Other

		[Description("Cell Height")]
		CellHeight = 0x0109,
		[Description("Cell Width")]
		CellWidth = 0x0108,
		[Description("Chrominance Table")]
		ChrominanceTable = 0x5091,
		[ExifDataType(typeof(ExifTagCleanFaxData), ExifType.UInt16)]
		[Description("Clean Fax Data")]
		CleanFaxData = 0x0147,
		[Description("Color Map")]
		ColorMap = 0x0140,
		[Description("Color Transfer Function")]
		ColorTransferFunction = 0x501A,
		[ExifDataType(ExifType.Ascii)]
		[Description("Document Name")]
		DocumentName = 0x010D,
		[Description("Dot Range")]
		DotRange = 0x0150,
		[Description("Extra Samples")]
		ExtraSamples = 0x0152,
		[ExifDataType(typeof(ExifTagFillOrder), ExifType.UInt16)]
		[Description("Fill Order")]
		FillOrder = 0x010A,
		[Description("Frame Delay")]
		FrameDelay = 0x5100,
		[Description("Free Byte Counts")]
		FreeByteCounts = 0x0121,
		[Description("Free Offset")]
		FreeOffset = 0x0120,
		Gamma = 0x0301,
		[Description("Global Palette")]
		GlobalPalette = 0x5102,
		[Description("Gray Response Curve")]
		GrayResponseCurve = 0x0123,
		[Description("Gray Response Unit")]
		GrayResponseUnit = 0x0122,
		[Description("Grid Size")]
		GridSize = 0x5011,
		[Description("Halftone Degree")]
		HalftoneDegree = 0x500C,
		[Description("Halftone Hints")]
		HalftoneHints = 0x0141,
		[Description("Halftone LPI")]
		HalftoneLPI = 0x500A,
		[Description("Halftone LPI Unit")]
		HalftoneLPIUnit = 0x500B,
		[Description("Halftone Misc")]
		HalftoneMisc = 0x500E,
		[Description("Halftone Screen")]
		HalftoneScreen = 0x500F,
		[Description("Halftone Shape")]
		HalftoneShape = 0x500D,
		[Description("Host Computer")]
		HostComputer = 0x013C,
		[Description("ICC Profile")]
		ICCProfile = 0x8773,
		[Description("ICC Profile Descriptor")]
		ICCProfileDescriptor = 0x0302,
		[ExifDataType(ExifType.Ascii)]
		[Description("Image Title")]
		ImageTitle = 0x0320,
		[Description("Index Background")]
		IndexBackground = 0x5103,
		[Description("Index Transparent")]
		IndexTransparent = 0x5104,
		[Description("Ink Names")]
		InkNames = 0x014D,
		[ExifDataType(typeof(ExifTagInkSet), ExifType.UInt16)]
		[Description("Ink Set")]
		InkSet = 0x014C,
		[Description("JPEG AC Tables")]
		JpegACTables = 0x0209,
		[Description("JPEG DC Tables")]
		JpegDCTables = 0x0208,
		[Description("JPEG Lossless Predictors")]
		JpegLosslessPredictors = 0x0205,
		[Description("JPEG Point Transforms")]
		JpegPointTransforms = 0x0206,
		[ExifDataType(typeof(ExifTagJPEGProc), ExifType.UInt16)]
		[Description("JPEG Proc")]
		JpegProc = 0x0200,
		[Description("JPEG Q Tables")]
		JpegQTables = 0x0207,
		[Description("JPEG Quality")]
		JpegQuality = 0x5010,
		[Description("JPEG Restart Interval")]
		JpegRestartInterval = 0x0203,
		[Description("Loop Count")]
		LoopCount = 0x5101,
		[Description("Luminance Table")]
		LuminanceTable = 0x5090,
		[Description("Max Sample Value")]
		MaxSampleValue = 0x0119,
		[Description("Min Sample Value")]
		MinSampleValue = 0x0118,
		[Description("New Subfile Type")]
		NewSubfileType = 0x00FE,
		[Description("Number Of Inks")]
		NumberOfInks = 0x014E,
		[Description("Page Name")]
		PageName = 0x011D,
		[Description("Page Number")]
		PageNumber = 0x0129,
		[Description("Palette Histogram")]
		PaletteHistogram = 0x5113,
		[Description("Pixel Per Unit X")]
		PixelPerUnitX = 0x5111,
		[Description("Pixel Per Unit Y")]
		PixelPerUnitY = 0x5112,
		[Description("Pixel Unit")]
		PixelUnit = 0x5110,
		[ExifDataType(typeof(ExifTagPredictor), ExifType.UInt16)]
		Predictor = 0x013D,
		[Description("Print Flags")]
		PrintFlags = 0x5005,
		[Description("Print Flags Bleed Width")]
		PrintFlagsBleedWidth = 0x5008,
		[Description("Print Flags Bleed Width Scale")]
		PrintFlagsBleedWidthScale = 0x5009,
		[Description("Print Flags Crop")]
		PrintFlagsCrop = 0x5007,
		[Description("Print Flags Version")]
		PrintFlagsVersion = 0x5006,
		[Description("Horizontal Resolution Length Unit")]
		ResolutionXLengthUnit = 0x5003,
		[Description("Horizontal Resolution Unit")]
		ResolutionXUnit = 0x5001,
		[Description("Vertical Resolution Length Unit")]
		ResolutionYLengthUnit = 0x5004,
		[Description("Vertical Resolution Unit")]
		ResolutionYUnit = 0x5002,
		[ExifDataType(typeof(ExifTagSampleFormat), ExifType.UInt16)]
		[Description("Sample Format")]
		SampleFormat = 0x0153,
		[Description("SMax Sample Value")]
		SMaxSampleValue = 0x0155,
		[Description("SMin Sample Value")]
		SMinSampleValue = 0x0154,
		[Description("sRGB Rendering Intent")]
		SRGBRenderingIntent = 0x0303,
		[Description("Subfile Type")]
		SubfileType = 0x00FF,
		[Description("T4 Option")]
		T4Option = 0x0124,
		[Description("T6 Option")]
		T6Option = 0x0125,
		[Description("Target Printer")]
		TargetPrinter = 0x0151,
		[ExifDataType(typeof(ExifTagThreshholding), ExifType.UInt16)]
		Threshholding = 0x0107,
		[Description("Tile Byte Counts")]
		TileByteCounts = 0x0145,
		[Description("Tile Length")]
		TileLength = 0x0143,
		[Description("Tile Offset")]
		TileOffset = 0x0144,
		[Description("Tile Width")]
		TileWidth = 0x0142,
		[Description("Transfer Range")]
		TransferRange = 0x0156,
		[Description("Horizontal Position")]
		XPosition = 0x011E,
		[Description("Vertical Position")]
		YPosition = 0x011F,

		[Description("Application Notes")]
		ApplicationNotes = 0x02BC,

		#endregion Other

		#region Microsoft Fields

		[ExifDataType(typeof(System.Text.UnicodeEncoding), ExifType.Byte)]
		[Description("Title")]
		MSTitle = 0x9C9B,
		[ExifDataType(typeof(System.Text.UnicodeEncoding), ExifType.Byte)]
		[Description("Comments")]
		MSComments = 0x9C9C,
		[ExifDataType(typeof(System.Text.UnicodeEncoding), ExifType.Byte)]
		[Description("Author")]
		MSAuthor = 0x9C9D,
		[ExifDataType(typeof(System.Text.UnicodeEncoding), ExifType.Byte)]
		[Description("Keywords")]
		MSKeywords = 0x9C9E,
		[ExifDataType(typeof(System.Text.UnicodeEncoding), ExifType.Byte)]
		[Description("Subject")]
		MSSubject = 0x9C9F

		#endregion Microsoft Fields
	}
}
