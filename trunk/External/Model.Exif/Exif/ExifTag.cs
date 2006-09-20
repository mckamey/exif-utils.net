using System;

using PhotoLib.Model.Exif.TagValues;

namespace PhotoLib.Model.Exif
{
	/// <summary>
	/// Defines EXIF Property Tag ID values.
	/// </summary>
	/// <remarks>
	/// Organization based on: http://www.exif.org/Exif2-2.PDF
	/// http://msdn.microsoft.com/library/en-us/gdicpp/GDIPlus/GDIPlusreference/constants/imagepropertytagconstants/propertytagsinnumericalorder.asp
	/// http://msdn.microsoft.com/library/en-us/gdicpp/GDIPlus/GDIPlusreference/constants/imagepropertytagconstants/propertyitemdescriptions.asp
	/// http://www.sno.phy.queensu.ca/~phil/exiftool/TagNames/EXIF.html
	/// </remarks>
	public enum ExifTag : int
	{
		#region Unknown

		Unknown = Int32.MinValue,

		#endregion Unknown

		#region EXIF IFD

		ExifIFDOffset=0x8769,
		GpsIFDOffset=0x8825,
		InteropIFDOffset=0xA005,

		#endregion EXIF IFD

		#region Interoperability IFD Fields

		RelatedImageFileFormat=0x1000,
		RelatedImageLength=0x1001,

		#endregion Interoperability IFD Fields

		#region TIFF Rev 6.0

		#region Image Data Structure

		ImageWidth=0x0100,
		ImageHeight=0x0101,
		BitsPerSample=0x0102,
		[ExifDataType(typeof(ExifTagCompression))]
		Compression=0x0103,
		[ExifDataType(typeof(ExifTagPhotometricInterpretation))]
		PhotometricInterpretation=0x0106,
		[ExifDataType(typeof(ExifTagOrientation))]
		Orientation=0x0112,
		SamplesPerPixel=0x0115,
		[ExifDataType(typeof(ExifTagPlanarConfiguration))]
		PlanarConfiguration=0x011C,
		YCbCrSubSampling=0x0212,
		[ExifDataType(typeof(ExifTagYCbCrPositioning))]
		YCbCrPositioning=0x0213,
		XResolution=0x011A,
		YResolution=0x011B,
		[ExifDataType(typeof(ExifTagResolutionUnit))]
		ResolutionUnit=0x0128,

		#endregion Image Data Structure

		#region Recording Offset

		StripOffsets=0x0111,
		RowsPerStrip=0x0116,
		StripBytesCount=0x0117,
		JPEGInterchangeFormat=0x0201,
		JPEGInterchangeLength=0x0202,

		#endregion Recording Offset

		#region Image Data Characteristics

		TransferFunction=0x012D,
		WhitePoint=0x013E,
		PrimaryChromaticities=0x013F,
		YCbCrCoefficients=0x0211,
		ReferenceBlackWhite=0x0214,

		#endregion Image Data Characteristics

		#region Other

		[ExifDataType(typeof(DateTime))]
		DateTime=0x0132,
		ImageDescription=0x010E,
		Make=0x010F,
		Model=0x0110,
		Software=0x0131,
		Artist=0x013B,
		Copyright=0x8298,
		InteroperabilityIndex=0x5041,

		#endregion Other

		#endregion TIFF Rev 6.0

		#region EXIF IFD

		#region Version

		ExifVersion=0x9000,
		FlashpixVersion=0xA000,

		#endregion Version

		#region Image Data Characteristics

		[ExifDataType(typeof(ExifTagColorSpace))]
		ColorSpace=0xA001,

		#endregion Image Data Characteristics

		#region Image Configuration

		ComponentsConfig=0x9101,
		CompressedBitsPerPixel=0x9102,
		CompressedImageWidth=0xA002,
		CompressedImageHeight=0xA003,

		#endregion Image Configuration

		#region User Information

		MakerNote=0x927C,
		UserComment=0x9286,

		#endregion User Information

		#region Related File Information

		RelatedAudioFile=0xA004,

		#endregion Related File Information

		#region Date and Time

		[ExifDataType(typeof(DateTime))]
		DateTimeOriginal=0x9003,
		[ExifDataType(typeof(DateTime))]
		DateTimeDigitized=0x9004,
		SubSecTime=0x9290,
		SubSecTimeOriginal=0x9291,
		SubSecTimeDigitized=0x9292,

		#endregion Date and Time

		#region Picture Taking Conditions

		ExposureTime=0x829A,
		FNumber=0x829D,
		[ExifDataType(typeof(ExifTagExposureProgram))]
		ExposureProgram=0x8822,
		SpectralSensitivity=0x8824,
		ISOSpeed=0x8827,
		OECF=0x8828,
		ShutterSpeed=0x9201,
		Aperture=0x9202,
		Brightness=0x9203,
		ExposureBias=0x9204,
		MaxAperture=0x9205,
		SubjectDistance=0x9206,
		[ExifDataType(typeof(ExifTagMeteringMode))]
		MeteringMode=0x9207,
		[ExifDataType(typeof(ExifTagLightSource))]
		LightSource=0x9208,
		[ExifDataType(typeof(ExifTagFlash))]
		Flash=0x9209,
		FocalLength=0x920A,
		SubjectArea=0x9214,
		FlashEnergy=0xA20B,
		SpatialFreqencyResponse=0xA20C,
		FocalPlaneXResolution=0xA20E,
		FocalPlaneYResolution=0xA20F,
		[ExifDataType(typeof(ExifTagResolutionUnit))]
		FocalPlaneResolutionUnit=0xA210,
		SubjectLocation=0xA214,
		ExposureIndex=0xA215,
		[ExifDataType(typeof(ExifTagSensingMethod))]
		SensingMethod=0xA217,
		[ExifDataType(typeof(ExifTagFileSource))]
		FileSource=0xA300,
		[ExifDataType(typeof(ExifTagSceneType))]
		SceneType=0xA301,
		CfaPattern=0xA302,
		[ExifDataType(typeof(ExifTagCustomRendered))]
		CustomRendered=0xA401,
		[ExifDataType(typeof(ExifTagExposureMode))]
		ExposureMode=0xA402,
		[ExifDataType(typeof(ExifTagWhiteBalance))]
		WhiteBalance=0xA403,
		DigitalZoomRatio=0xA404,
		FocalLengthIn35mmFilm=0xA405,
		[ExifDataType(typeof(ExifTagSceneCaptureType))]
		SceneCaptureType=0xA406,
		[ExifDataType(typeof(ExifTagGainControl))]
		GainControl=0xA407,
		[ExifDataType(typeof(ExifTagContrast))]
		Contrast=0xA408,
		[ExifDataType(typeof(ExifTagSaturation))]
		Saturation=0xA409,
		[ExifDataType(typeof(ExifTagSharpness))]
		Sharpness=0xA40A,
		DeviceSettingDescription=0xA40B,
		[ExifDataType(typeof(ExifTagSubjectDistanceRange))]
		SubjectDistanceRange=0xA40C,

		#endregion Picture Taking Conditions

		#region Other

		ImageUniqueID = 0xA420,

		#endregion Other

		#endregion EXIF IFD

		#region Global Positioning System (GPS)

		GpsVersionID=0x0000,
		GpsLatitudeRef=0x0001,
		GpsLatitude=0x0002,
		GpsLongitudeRef=0x0003,
		GpsLongitude=0x0004,
		[ExifDataType(typeof(ExifTagGpsAltitudeRef))]
		GpsAltitudeRef=0x0005,
		GpsAltitude=0x0006,
		GpsTimeStamp=0x0007,
		GpsSatellites=0x0008,
		GpsStatus=0x0009,
		GpsMeasureMode=0x000A,
		GpsDOP=0x000B,
		GpsSpeedRef=0x000C,
		GpsSpeed=0x000D,
		GpsTrackRef=0x000E,
		GpsTrack=0x000F,
		GpsImgDirectionRef=0x0010,
		GpsImgDirection=0x0011,
		GpsMapDatum=0x0012,
		GpsDestLatitudeRef=0x0013,
		GpsDestLatitude=0x0014,
		GpsDestLongitudeRef=0x0015,
		GpsDestLongitude=0x0016,
		GpsDestBearingRef=0x0017,
		GpsDestBearing=0x0018,
		GpsDestDistanceRef=0x0019,
		GpsDestDistance=0x001A,
		GpsProcessingMethod=0x001B,
		GpsAreaInformation=0x001C,
		[ExifDataType(typeof(DateTime))]
		GpsDateStamp=0x001D,
		[ExifDataType(typeof(ExifTagGpsDifferential))]
		GpsDifferential=0x001E,

		#endregion Global Positioning System (GPS)

		#region Thumbnail

		ThumbnailImageHeight=0x5021,
		ThumbnailImageWidth=0x5020,
		ThumbnailBitsPerSample=0x5022,
		[ExifDataType(typeof(ExifTagCompression))]
		ThumbnailCompression=0x5023,
		ThumbnailPhotometricInterpretation=0x5024,
		ThumbnailImageDescription=0x5025,
		ThumbnailMake=0x5026,
		ThumbnailModel=0x5027,
		ThumbnailStripOffsets=0x5028,
		[ExifDataType(typeof(ExifTagOrientation))]
		ThumbnailOrientation=0x5029,
		ThumbnailSamplesPerPixel=0x502A,
		ThumbnailRowsPerStrip=0x502B,
		ThumbnailStripBytesCount=0x502C,
		ThumbnailResolutionX=0x502D,
		ThumbnailResolutionY=0x502E,
		ThumbnailPlanarConfig=0x502F,
		[ExifDataType(typeof(ExifTagResolutionUnit))]
		ThumbnailResolutionUnit=0x5030,
		ThumbnailTransferFunction=0x5031,
		ThumbnailSoftware=0x5032,
		[ExifDataType(typeof(DateTime))]
		ThumbnailDateTime=0x5033,
		ThumbnailArtist=0x5034,
		ThumbnailWhitePoint=0x5035,
		ThumbnailPrimaryChromaticities=0x5036,
		ThumbnailYCbCrCoefficients=0x5037,
		ThumbnailYCbCrSubSampling=0x5038,
		ThumbnailYCbCrPositioning=0x5039,
		ThumbnailReferenceBlackWhite=0x503A,
		ThumbnailCopyright=0x503B,

		ThumbnailColorDepth = 0x5015,
		ThumbnailCompressedSize = 0x5019,
		ThumbnailData=0x501B,
		ThumbnailFormat = 0x5012,
		ThumbnailHeight = 0x5014,
		ThumbnailPlanes = 0x5016,
		ThumbnailRawBytes = 0x5017,
		ThumbnailSize = 0x5018,
		ThumbnailWidth = 0x5013,

		#endregion Thumbnail

		#region Other

		CellHeight = 0x0109,
		CellWidth = 0x0108,
		ChrominanceTable = 0x5091,
		[ExifDataType(typeof(ExifTagCleanFaxData))]
		CleanFaxData=0x0147,
		ColorMap=0x0140,
		ColorTransferFunction = 0x501A,
		DocumentName = 0x010D,
		DotRange = 0x0150,
		ExtraSamples = 0x0152,
		[ExifDataType(typeof(ExifTagFillOrder))]
		FillOrder=0x010A,
		FrameDelay = 0x5100,
		FreeByteCounts = 0x0121,
		FreeOffset = 0x0120,
		Gamma = 0x0301,
		GlobalPalette = 0x5102,
		GrayResponseCurve = 0x0123,
		GrayResponseUnit = 0x0122,
		GridSize = 0x5011,
		HalftoneDegree = 0x500C,
		HalftoneHints = 0x0141,
		HalftoneLPI = 0x500A,
		HalftoneLPIUnit = 0x500B,
		HalftoneMisc = 0x500E,
		HalftoneScreen = 0x500F,
		HalftoneShape = 0x500D,
		HostComputer = 0x013C,
		ICCProfile = 0x8773,
		ICCProfileDescriptor = 0x0302,
		ImageTitle = 0x0320,
		IndexBackground = 0x5103,
		IndexTransparent = 0x5104,
		InkNames = 0x014D,
		[ExifDataType(typeof(ExifTagInkSet))]
		InkSet=0x014C,
		JPEGACTables = 0x0209,
		JPEGDCTables = 0x0208,
		JPEGLosslessPredictors = 0x0205,
		JPEGPointTransforms = 0x0206,
		[ExifDataType(typeof(ExifTagJPEGProc))]
		JPEGProc=0x0200,
		JPEGQTables = 0x0207,
		JPEGQuality = 0x5010,
		JPEGRestartInterval = 0x0203,
		LoopCount = 0x5101,
		LuminanceTable = 0x5090,
		MaxSampleValue = 0x0119,
		MinSampleValue = 0x0118,
		NewSubfileType = 0x00FE,
		NumberOfInks = 0x014E,
		PageName = 0x011D,
		PageNumber = 0x0129,
		PaletteHistogram = 0x5113,
		PixelPerUnitX = 0x5111,
		PixelPerUnitY = 0x5112,
		PixelUnit = 0x5110,
		[ExifDataType(typeof(ExifTagPredictor))]
		Predictor=0x013D,
		PrintFlags = 0x5005,
		PrintFlagsBleedWidth = 0x5008,
		PrintFlagsBleedWidthScale = 0x5009,
		PrintFlagsCrop = 0x5007,
		PrintFlagsVersion = 0x5006,
		ResolutionXLengthUnit = 0x5003,
		ResolutionXUnit = 0x5001,
		ResolutionYLengthUnit = 0x5004,
		ResolutionYUnit = 0x5002,
		[ExifDataType(typeof(ExifTagSampleFormat))]
		SampleFormat=0x0153,
		SMaxSampleValue = 0x0155,
		SMinSampleValue = 0x0154,
		SRGBRenderingIntent = 0x0303,
		SubfileType = 0x00FF,
		T4Option = 0x0124,
		T6Option = 0x0125,
		TargetPrinter = 0x0151,
		[ExifDataType(typeof(ExifTagThreshholding))]
		Threshholding=0x0107,
		TileByteCounts = 0x0145,
		TileLength = 0x0143,
		TileOffset = 0x0144,
		TileWidth = 0x0142,
		TransferRange = 0x0156,
		XPosition = 0x011E,
		YPosition = 0x011F,

		#endregion Other

		#region Microsoft Fields

		[ExifDataType(typeof(System.Text.UnicodeEncoding))]
		MSTitle=0x9C9B,
		[ExifDataType(typeof(System.Text.UnicodeEncoding))]
		MSComments=0x9C9C,
		[ExifDataType(typeof(System.Text.UnicodeEncoding))]
		MSAuthor=0x9C9D,
		[ExifDataType(typeof(System.Text.UnicodeEncoding))]
		MSKeywords=0x9C9E,
		[ExifDataType(typeof(System.Text.UnicodeEncoding))]
		MSSubject=0x9C9F,

		#endregion Microsoft Fields
	}
}
