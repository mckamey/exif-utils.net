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

		// TODO: enum
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

		// TODO: Point = {Integer, Integer}
		[XmpBasicProperty(XmpBasicType.Integer, XmpQuantity.Seq)]
		ToneCurve,

		// TODO: enum
		[XmpBasicProperty(XmpBasicType.Text, Category=XmpCategory.Internal)]
		ToneCurveName,

		[XmpBasicProperty(XmpBasicType.Text, Category=XmpCategory.Internal)]
		Version,

		[XmpBasicProperty(XmpBasicType.Integer)]
		VignetteAmount,

		[XmpBasicProperty(XmpBasicType.Integer)]
		VignetteMidpoint,

		// TODO: enum
		[XmpBasicProperty(XmpBasicType.Text)]
		WhiteBalance
	}
}
