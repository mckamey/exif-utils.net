using System;

namespace PhotoLib.Model.Exif.TagValues
{
	#region TIFF Rev 6.0 Tags

	public enum ExifTagCompression : ushort
	{
		Uncompressed=1,
		CCITT1D=2,
		T4Group3Fax=3,
		T6Group4Fax=4,
		LZW=5,
		JpegCompression=6,
		Jpeg=7,
		AdobeDeflate=8,
		JBIGBW=9,
		JBIGColor=10,
		Next=32766,
		CCIRLEW=32771,
		PackBits=32773,
		Thunderscan=32809,
		IT8CTPAD=32895,
		IT8LW=32896,
		IT8MP=32897,
		IT8BL=32898,
		PixarFilm=32908,
		PixarLog=32909,
		Deflate=32946,
		DCS=32947,
		JBIG=34661,
		SGILog=34676,
		SGILog24=34677,
		Jpeg2000=34712,
		NikonNEF=34713
	}

	public enum ExifTagPhotometricInterpretation : ushort
	{
		WhiteIsZero=0,
		BlackIsZero=1,
		RGB=2,
		RGBPalette=3,
		TransparencyMask=4,
		CMYK=5,
		YCbCr=6,
		CIELab=8,
		ICCLab=9,
		ITULab=10,
		ColorFilterArray=32803,
		PixarLogL=32844,
		PixarLogLuv=32845,
		LinearRaw=34892,
	}

	public enum ExifTagOrientation : ushort
	{
		Undefined=0,
		Horizontal=1,
		HorizontalMirror=2,
		Rotate180=3,
		VerticalMirror=4,
		HorizontalMirrorRotate270CW=5,
		Rotate90CW=6,
		HorizontalMirrorRotate90CW=7,
		Rotate270CW=8
	}

	public enum ExifTagPlanarConfiguration : ushort
	{
		ChunkyFormat=1,
		PlanarFormat=2,
	}

	public enum ExifTagYCbCrPositioning : ushort
	{
		Centered=1,
		CoSited=2,
	}

	public enum ExifTagResolutionUnit : ushort
	{
		Undefined=1,
		Inch=2,
		Centimeter=3,
	}

	#endregion TIFF Rev 6.0 Tags

	#region EXIF IFD Tags

	public enum ExifTagColorSpace : ushort
	{
		sRGB=0x0001,
		Uncalibrated=0xFFFF
	}

	public enum ExifTagExposureProgram : ushort
	{
		Undefined=0,
		Manual=1,
		NormalAuto=2,
		AperturePriorityAuto=3,
		ShutterPriorityAuto=4,
		CreativeSlowSpeed=5,
		ActionHighSpeed=6,
		PortraitMode=7,
		LandscapeMode=8
	}

	public enum ExifTagMeteringMode : ushort
	{
		Undefined=0,
		Average=1,
		CenterWeightedAverage=2,
		Spot=3,
		MultiSpot=4,
		Pattern=5,
		Partial=6,
		Other=255
	}

	public enum ExifTagLightSource : ushort
	{
		Undefined=0,
		Daylight=1,
		Fluorescent=2,
		Tungsten=3,
		Flash=4,
		FineWeather=9,
		CloudyWeather=10,
		Shade=11,
		DaylightFluorescent=12,
		DayWhiteFluorescent=13,
		CoolWhiteFluorescent=14,
		WhiteFluorescent=15,
		StandardLightA=17,
		StandardLightB=18,
		StandardLightC=19,
		D55=20,
		D65=21,
		D75=22,
		D50=23,
		ISOStudioTungsten=24,
		Other=255
	}

	[Flags]
	public enum ExifTagFlash : ushort
	{
		FlashNotFired=0x0000,
		FlashFired=0x0001,
		NoFlashFunction=0x0020,
		RedEyeReduction=0x0040,
		ReturnDetected=0x0006,
		ReturnNotDetected=0x0004,
		ModeAuto=0x0018,
		ModeOn=0x0008,
		ModeOff=0x0010,
	}

	public enum ExifTagSensingMethod : ushort
	{
		Undefined=1,
		OneChipColorAreaSensor=2,
		TwoChipColorAreaSensor=3,
		ThreeChipColorAreaSensor=4,
		ColorSequentialAreaSensor=5,
		TrilinearSensor=7,
		ColorSequentialLinearSensor=8,
	}

	public enum ExifTagFileSource : byte
	{
		DSC=3
	}

	public enum ExifTagSceneType : byte
	{
		DirectlyPhotographedImage=1
	}

	public enum ExifTagCustomRendered : ushort
	{
		NormalProcess=0,
		CustomProcess=1,
	}

	public enum ExifTagExposureMode : ushort
	{
		AutoExposure=0,
		ManualExposure=1,
		AutoBracket=2,
	}

	public enum ExifTagWhiteBalance : ushort
	{
		AutoWhiteBalance=0,
		ManualWhiteBalance=1
	}

	public enum ExifTagSceneCaptureType : ushort
	{
		Standard=0,
		Landscape=1,
		Portrait=2,
		NightScene=3,
	}

	public enum ExifTagGainControl : ushort
	{
		None=0,
		LowGainUp=1,
		HighGainUp=2,
		LowGainDown=3,
		HighGainDown=4,
	}

	public enum ExifTagContrast : ushort
	{
		Normal=0,
		Soft=1,
		Hard=2,
	}

	public enum ExifTagSaturation : ushort
	{
		Normal=0,
		LowSaturation=1,
		HighSaturation=2,
	}

	public enum ExifTagSharpness : ushort
	{
		Normal=0,
		Soft=1,
		Hard=2,
	}

	public enum ExifTagSubjectDistanceRange : ushort
	{
		Undefined=0,
		Macro=1,
		CloseView=2,
		DistantView=3,
	}

	public enum ExifTagPredictor : ushort
	{
		None=0,
		HorizontalDifferencing=1,
	}

	public enum ExifTagThreshholding : ushort
	{
		NoDitherOrHalftone=1,
		OrderedDitherOrHalftone=2,
		RandomizedDither=3,
	}

	public enum ExifTagFillOrder : ushort
	{
		Normal=1,
		Reversed=2,
	}

	public enum ExifTagCleanFaxData : ushort
	{
		Clean=0,
		Regenerated=1,
		Unclean=2,
	}

	public enum ExifTagInkSet : ushort
	{
		CMYK=1,
		NotCMYK=2,
	}

	public enum ExifTagSampleFormat : ushort
	{
		UnsignedInteger=1,
		TwosComplementSignedInteger=2,
		IEEEFloatingPoint=3,
		Undefined=4,
		ComplexInteger=5,
		IEEEFloatingPointAlt=6,
	}

	public enum ExifTagJPEGProc : ushort
	{
		Baseline=1,
		Lossless=14,
	}

	#endregion EXIF IFD Tags

	#region Global Positioning System (GPS) Tags

	public enum ExifTagGpsAltitudeRef : byte
	{
		SeaLevel=0,
		SeaLevelReference=1,
	}

	public enum ExifTagGpsDifferential : ushort
	{
		NoDifferentialCorrection=0,
		DifferentialCorrection=1,
	}

	#endregion Global Positioning System (GPS) Tags
}