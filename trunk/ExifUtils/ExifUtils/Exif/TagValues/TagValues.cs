#region License
/*---------------------------------------------------------------------------------*\

	Distributed under the terms of an MIT-style license:

	The MIT License

	Copyright (c) 2005-2006 Stephen M. McKamey

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

namespace ExifUtils.Exif.TagValues
{
	#region TIFF Rev 6.0 Tags

	public enum ExifTagCompression : ushort
	{
		Uncompressed = 1,
		CCITT1D = 2,
		[Description("T4 Group 3 Fax")]
		T4Group3Fax = 3,
		[Description("T6 Group 4 Fax")]
		T6Group4Fax = 4,
		LZW = 5,
		[Description("JPEG Compression")]
		JpegCompression = 6,
		[Description("JPEG")]
		Jpeg = 7,
		[Description("Adobe Deflate")]
		AdobeDeflate = 8,
		JBIGBW = 9,
		[Description("JBIG Color")]
		JBIGColor = 10,
		Next = 32766,
		CCIRLEW = 32771,
		[Description("Pack Bits")]
		PackBits = 32773,
		Thunderscan = 32809,
		IT8CTPAD = 32895,
		IT8LW = 32896,
		IT8MP = 32897,
		IT8BL = 32898,
		[Description("Pixar Film")]
		PixarFilm = 32908,
		[Description("Pixar Log")]
		PixarLog = 32909,
		Deflate = 32946,
		DCS = 32947,
		JBIG = 34661,
		[Description("SGI Log")]
		SGILog = 34676,
		[Description("SGI Log 24")]
		SGILog24 = 34677,
		[Description("JPEG 2000")]
		Jpeg2000 = 34712,
		[Description("Nikon NEF")]
		NikonNEF = 34713
	}

	public enum ExifTagPhotometricInterpretation : ushort
	{
		[Description("White Is Zero")]
		WhiteIsZero = 0,
		[Description("Black Is Zero")]
		BlackIsZero = 1,
		RGB = 2,
		[Description("RGB Palette")]
		RGBPalette = 3,
		[Description("Transparency Mask")]
		TransparencyMask = 4,
		CMYK = 5,
		YCbCr = 6,
		[Description("CIE Lab")]
		CIELab = 8,
		[Description("ICC Lab")]
		ICCLab = 9,
		[Description("ITU Lab")]
		ITULab = 10,
		[Description("Color Filter Array")]
		ColorFilterArray = 32803,
		[Description("Pixar Log L")]
		PixarLogL = 32844,
		[Description("Pixar Log Luv")]
		PixarLogLuv = 32845,
		[Description("Linear Raw")]
		LinearRaw = 34892
	}

	public enum ExifTagOrientation : ushort
	{
		Undefined = 0,
		Horizontal = 1,
		[Description("Horizontal Mirror")]
		HorizontalMirror = 2,
		[Description("Rotate 180")]
		Rotate180 = 3,
		[Description("Vertical Mirror")]
		VerticalMirror = 4,
		[Description("Horizontal Mirror Rotate 270-CW")]
		HorizontalMirrorRotate270CW = 5,
		[Description("Rotate 90-CW")]
		Rotate90CW = 6,
		[Description("Horizontal Mirror Rotate 90-CW")]
		HorizontalMirrorRotate90CW = 7,
		[Description("Rotate 270-CW")]
		Rotate270CW = 8
	}

	public enum ExifTagPlanarConfiguration : ushort
	{
		[Description("Chunky Format")]
		ChunkyFormat=1,
		[Description("Planar Format")]
		PlanarFormat=2
	}

	public enum ExifTagYCbCrPositioning : ushort
	{
		Centered = 1,
		CoSited = 2,
	}

	public enum ExifTagResolutionUnit : ushort
	{
		Undefined = 1,
		Inch = 2,
		Centimeter = 3
	}

	#endregion TIFF Rev 6.0 Tags

	#region EXIF IFD Tags

	public enum ExifTagColorSpace : ushort
	{
		sRGB = 0x0001,
		Uncalibrated = 0xFFFF
	}

	public enum ExifTagExposureProgram : ushort
	{
		Undefined = 0,
		Manual = 1,
		[Description("Normal Auto")]
		NormalAuto = 2,
		[Description("Aperture Priority Auto")]
		AperturePriorityAuto = 3,
		[Description("Shutter Priority Auto")]
		ShutterPriorityAuto = 4,
		[Description("Creative Slow Speed")]
		CreativeSlowSpeed = 5,
		[Description("Action High Speed")]
		ActionHighSpeed = 6,
		[Description("Portrait Mode")]
		PortraitMode = 7,
		[Description("Landscape Mode")]
		LandscapeMode = 8
	}

	public enum ExifTagMeteringMode : ushort
	{
		Undefined = 0,
		Average = 1,
		[Description("Center Weighted Average")]
		CenterWeightedAverage = 2,
		Spot = 3,
		MultiSpot = 4,
		Pattern = 5,
		Partial = 6,
		Other = 255
	}

	public enum ExifTagLightSource : ushort
	{
		Undefined = 0,
		Daylight = 1,
		Fluorescent = 2,
		Tungsten = 3,
		Flash = 4,
		[Description("Fine Weather")]
		FineWeather = 9,
		[Description("Cloudy Weather")]
		CloudyWeather = 10,
		Shade = 11,
		[Description("Daylight Fluorescent")]
		DaylightFluorescent = 12,
		[Description("Day White Fluorescent")]
		DayWhiteFluorescent = 13,
		[Description("Cool White Fluorescent")]
		CoolWhiteFluorescent = 14,
		[Description("White Fluorescent")]
		WhiteFluorescent = 15,
		[Description("Standard Light A")]
		StandardLightA = 17,
		[Description("Standard Light B")]
		StandardLightB = 18,
		[Description("Standard Light C")]
		StandardLightC = 19,
		D55 = 20,
		D65 = 21,
		D75 = 22,
		D50 = 23,
		[Description("ISO Studio Tungsten")]
		ISOStudioTungsten = 24,
		Other = 255
	}

	[Flags]
	public enum ExifTagFlash : ushort
	{
		[Description("Flash Not Fired")]
		FlashNotFired = 0x0000,
		[Description("Flash Fired")]
		FlashFired = 0x0001,
		[Description("No Flash Function")]
		NoFlashFunction = 0x0020,
		[Description("Red-Eye Reduction")]
		RedEyeReduction = 0x0040,
		[Description("Return Detected")]
		ReturnDetected = 0x0006,
		[Description("Return Not Detected")]
		ReturnNotDetected = 0x0004,
		[Description("Auto Flash")]
		ModeAuto = 0x0018,
		[Description("Flash On")]
		ModeOn = 0x0008,
		[Description("Flash Off")]
		ModeOff = 0x0010
	}

	public enum ExifTagSensingMethod : ushort
	{
		Undefined = 1,
		[Description("One Chip Color Area Sensor")]
		OneChipColorAreaSensor=2,
		[Description("Two Chip Color Area Sensor")]
		TwoChipColorAreaSensor = 3,
		[Description("Three Chip Color Area Sensor")]
		ThreeChipColorAreaSensor = 4,
		[Description("Color Sequential Area Sensor")]
		ColorSequentialAreaSensor = 5,
		[Description("Trilinear Sensor")]
		TrilinearSensor = 7,
		[Description("Color Sequential Linear Sensor")]
		ColorSequentialLinearSensor = 8
	}

	public enum ExifTagFileSource : byte
	{
		[Description("Film Scannner")]
		FilmScannner = 1,
		[Description("Reflection Print Scanner")]
		ReflectionPrintScanner = 2,
		[Description("Digital Still Camera")]
		DSC = 3
	}

	public enum ExifTagSceneType : byte
	{
		[Description("Directly Photographed Image")]
		DirectlyPhotographedImage = 1
	}

	public enum ExifTagCustomRendered : ushort
	{
		[Description("Normal Process")]
		NormalProcess = 0,
		[Description("Custom Process")]
		CustomProcess = 1
	}

	public enum ExifTagExposureMode : ushort
	{
		[Description("Auto Exposure")]
		AutoExposure = 0,
		[Description("Manual Exposure")]
		ManualExposure = 1,
		[Description("Auto Bracket")]
		AutoBracket = 2
	}

	public enum ExifTagWhiteBalance : ushort
	{
		[Description("Auto White Balance")]
		AutoWhiteBalance = 0,
		[Description("Manual White Balance")]
		ManualWhiteBalance = 1
	}

	public enum ExifTagSceneCaptureType : ushort
	{
		Standard = 0,
		Landscape = 1,
		Portrait = 2,
		[Description("Night Scene")]
		NightScene = 3
	}

	public enum ExifTagGainControl : ushort
	{
		None = 0,
		[Description("Low Gain Up")]
		LowGainUp = 1,
		[Description("High Gain Up")]
		HighGainUp = 2,
		[Description("Low Gain Down")]
		LowGainDown = 3,
		[Description("High Gain Down")]
		HighGainDown = 4
	}

	public enum ExifTagContrast : ushort
	{
		Normal = 0,
		Soft = 1,
		Hard = 2
	}

	public enum ExifTagSaturation : ushort
	{
		Normal = 0,
		[Description("Low Saturation")]
		LowSaturation = 1,
		[Description("High Saturation")]
		HighSaturation = 2
	}

	public enum ExifTagSharpness : ushort
	{
		Normal = 0,
		Soft = 1,
		Hard = 2
	}

	public enum ExifTagSubjectDistanceRange : ushort
	{
		Undefined = 0,
		Macro = 1,
		[Description("Close View")]
		CloseView = 2,
		[Description("Distant View")]
		DistantView = 3
	}

	public enum ExifTagPredictor : ushort
	{
		None = 0,
		[Description("Horizontal Differencing")]
		HorizontalDifferencing = 1
	}

	public enum ExifTagThreshholding : ushort
	{
		[Description("No Dither Or Halftone")]
		NoDitherOrHalftone = 1,
		[Description("Ordered Dither Or Halftone")]
		OrderedDitherOrHalftone = 2,
		[Description("Randomized Dither")]
		RandomizedDither = 3
	}

	public enum ExifTagFillOrder : ushort
	{
		Normal = 1,
		Reversed = 2
	}

	public enum ExifTagCleanFaxData : ushort
	{
		Clean = 0,
		Regenerated = 1,
		Unclean = 2
	}

	public enum ExifTagInkSet : ushort
	{
		CMYK = 1,
		[Description("Not CMYK")]
		NotCMYK = 2
	}

	public enum ExifTagSampleFormat : ushort
	{
		[Description("Unsigned Integer")]
		UnsignedInteger = 1,
		[Description("Two's Complement Signed Integer")]
		TwosComplementSignedInteger = 2,
		[Description("IEEE Floating Point")]
		IEEEFloatingPoint = 3,
		Undefined = 4,
		[Description("Complex Integer")]
		ComplexInteger = 5,
		[Description("IEEE Floating PointAlt")]
		IEEEFloatingPointAlt = 6,
	}

	public enum ExifTagJPEGProc : ushort
	{
		Baseline = 1,
		Lossless = 14,
	}

	#endregion EXIF IFD Tags

	#region Global Positioning System (GPS) Tags

	public enum ExifTagGpsAltitudeRef : byte
	{
		[Description("Sea Level")]
		SeaLevel=0,
		[Description("Sea Level Reference")]
		SeaLevelReference = 1,
	}

	public enum ExifTagGpsDifferential : ushort
	{
		[Description("No Differential Correction")]
		NoDifferentialCorrection = 0,
		[Description("Differential Correction")]
		DifferentialCorrection = 1,
	}

	#endregion Global Positioning System (GPS) Tags
}