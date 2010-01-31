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

using XmpUtils.Xmp.ValueTypes;

namespace XmpUtils.Xmp.Schemas
{
	[XmpNamespace("http://ns.adobe.com/camera-raw-settings/1.0/", "crs")]
	public enum CameraRawSchema
	{
		[XmpBasicProperty(XmpBasicType.Boolean)]
		AutoBrightness,

		[XmpBasicProperty(XmpBasicType.Boolean)]
		AutoContrast,

		[XmpBasicProperty(XmpBasicType.Boolean)]
		AutoExposure,

		[XmpBasicProperty(XmpBasicType.Boolean)]
		AutoShadows,

		[XmpBasicProperty(XmpBasicType.Integer)]
		BlueHue,

		[XmpBasicProperty(XmpBasicType.Integer)]
		BlueSaturation,

		[XmpBasicProperty(XmpBasicType.Integer)]
		Brightness,

		[XmpBasicProperty(XmpBasicType.Text)]
		CameraProfile,

		[XmpBasicProperty(XmpBasicType.Integer)]
		ChromaticAberrationB,

		[XmpBasicProperty(XmpBasicType.Integer)]
		ChromaticAberrationR,

		[XmpBasicProperty(XmpBasicType.Integer)]
		ColorNoiseReduction,

		[XmpBasicProperty(XmpBasicType.Integer)]
		Contrast,

		[XmpBasicProperty(XmpBasicType.Real)]
		CropTop,

		[XmpBasicProperty(XmpBasicType.Real)]
		CropLeft,

		[XmpBasicProperty(XmpBasicType.Real)]
		CropBottom,

		[XmpBasicProperty(XmpBasicType.Real)]
		CropRight,

		[XmpBasicProperty(XmpBasicType.Real)]
		CropAngle,

		[XmpBasicProperty(XmpBasicType.Real)]
		CropWidth,

		[XmpBasicProperty(XmpBasicType.Real)]
		CropHeight,

		// TODO: enum as Integer
		[XmpBasicProperty(XmpBasicType.Integer)]
		CropUnits,

		[XmpBasicProperty(XmpBasicType.Real)]
		Exposure,

		[XmpBasicProperty(XmpBasicType.Integer)]
		GreenHue,

		[XmpBasicProperty(XmpBasicType.Integer)]
		GreenSaturation,

		[XmpBasicProperty(XmpBasicType.Boolean)]
		HasCrop,

		[XmpBasicProperty(XmpBasicType.Boolean)]
		HasSettings,

		[XmpBasicProperty(XmpBasicType.Integer)]
		LuminanceSmoothing,

		[XmpBasicProperty(XmpBasicType.Text)]
		RawFileName,

		[XmpBasicProperty(XmpBasicType.Integer)]
		RedHue,

		[XmpBasicProperty(XmpBasicType.Integer)]
		RedSaturation,

		[XmpBasicProperty(XmpBasicType.Integer)]
		Saturation,

		[XmpBasicProperty(XmpBasicType.Integer)]
		Shadows,

		[XmpBasicProperty(XmpBasicType.Integer)]
		ShadowTint,

		[XmpBasicProperty(XmpBasicType.Integer)]
		Sharpness,

		[XmpBasicProperty(XmpBasicType.Integer)]
		Temperature,

		[XmpBasicProperty(XmpBasicType.Integer)]
		Tint,

		[XmpBasicProperty(XmpBasicType.Text, XmpQuantity.Seq)]
		ToneCurve,

		// TODO: enum as Text
		[XmpBasicProperty(XmpBasicType.Text, Category=XmpCategory.Internal)]
		ToneCurveName,

		[XmpBasicProperty(XmpBasicType.Text, Category=XmpCategory.Internal)]
		Version,

		[XmpBasicProperty(XmpBasicType.Integer)]
		VignetteAmount,

		[XmpBasicProperty(XmpBasicType.Integer)]
		VignetteMidpoint,

		// TODO: enum as Text
		[XmpBasicProperty(XmpBasicType.Text)]
		WhiteBalance,

		#region additional CRS properties not listed in spec

		FillLight, // 0

		Vibrance, // +45

		HighlightRecovery, // 0

		Clarity, // +25

		Defringe, // 0

		HueAdjustmentRed, // 0

		HueAdjustmentOrange, // 0

		HueAdjustmentYellow, // 0

		HueAdjustmentGreen, // 0

		HueAdjustmentAqua, // 0

		HueAdjustmentBlue, // 0

		HueAdjustmentPurple, // 0

		HueAdjustmentMagenta, // 0

		SaturationAdjustmentRed, // 0

		SaturationAdjustmentOrange, // 0

		SaturationAdjustmentYellow, // 0

		SaturationAdjustmentGreen, // 0

		SaturationAdjustmentAqua, // 0

		SaturationAdjustmentBlue, // 0

		SaturationAdjustmentPurple, // 0

		SaturationAdjustmentMagenta, // 0

		LuminanceAdjustmentRed, // 0

		LuminanceAdjustmentOrange, // 0

		LuminanceAdjustmentYellow, // 0

		LuminanceAdjustmentGreen, // 0

		LuminanceAdjustmentAqua, // 0

		LuminanceAdjustmentBlue, // 0

		LuminanceAdjustmentPurple, // 0

		LuminanceAdjustmentMagenta, // 0

		SplitToningShadowHue, // 0

		SplitToningShadowSaturation, // 0

		SplitToningHighlightHue, // 0

		SplitToningHighlightSaturation, // 0

		SplitToningBalance, // 0

		ParametricShadows, // 0

		ParametricDarks, // 0

		ParametricLights, // 0

		ParametricHighlights, // 0

		ParametricShadowSplit, // 25

		ParametricMidtoneSplit, // 50

		ParametricHighlightSplit, // 75

		SharpenRadius, // +1.0

		SharpenDetail, // 50

		SharpenEdgeMasking, // 0

		PostCropVignetteAmount, // 0

		ConvertToGrayscale, // False

		AlreadyApplied, // True

		IncrementalTemperature, // 0

		IncrementalTint, // 0

		CropUnit // 0

		#endregion additional CRS properties not listed in spec
	}
}
