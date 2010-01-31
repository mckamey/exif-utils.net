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
	[XmpNamespace("http://ns.adobe.com/xmp/1.0/DynamicMedia/", "xmpDM")]
	public enum XmpDynamicMediaSchema
	{
		[XmpBasicProperty(XmpBasicType.URI, Category=XmpCategory.Internal, Name="absPeakAudioFilePath")]
		AbsPeakAudioFilePath,

		[XmpBasicProperty(XmpBasicType.Text, Name="album")]
		Album,

		[XmpBasicProperty(XmpBasicType.Text, Name="altTapeName")]
		AltTapeName,

		[XmpVideoMediaProperty(XmpVideoMediaType.Timecode, Name="altTimecode")]
		AltTimecode,

		[XmpBasicProperty(XmpBasicType.Text, Name="artist")]
		Artist,

		[XmpBasicProperty(XmpBasicType.Date, Category=XmpCategory.Internal, Name="audioModDate")]
		AudioModDate,

		[XmpBasicProperty(XmpBasicType.Integer, Category=XmpCategory.Internal, Name="audioSampleRate")]
		AudioSampleRate,

		[XmpBasicProperty(XmpBasicType.Text, Category=XmpCategory.Internal, Name="audioSampleType")]
		AudioSampleType,

		[XmpBasicProperty(XmpBasicType.Text, Category=XmpCategory.Internal, Name="audioChannelType")]
		AudioChannelType,

		[XmpBasicProperty(XmpBasicType.Text, Category=XmpCategory.Internal, Name="audioCompressor")]
		AudioCompressor,

		[XmpVideoMediaProperty(XmpVideoMediaType.BeatSpliceStretch, Category=XmpCategory.Internal, Name="beatSpliceParams")]
		BeatSpliceParams,

		[XmpBasicProperty(XmpBasicType.Text, Name="composer")]
		Composer,

		[XmpVideoMediaProperty(XmpVideoMediaType.Media, XmpQuantity.Bag, Category=XmpCategory.Internal, Name="contributedMedia")]
		ContributedMedia,

		[XmpBasicProperty(XmpBasicType.Text, Name="copyright")]
		Copyright,

		[XmpVideoMediaProperty(XmpVideoMediaType.Time, Category=XmpCategory.Internal, Name="duration")]
		Duration,

		[XmpBasicProperty(XmpBasicType.Text, Name="engineer")]
		Engineer,

		[ExifProperty(ExifType.Rational, Category=XmpCategory.Internal, Name="fileDataRate")]
		FileDataRate,

		[XmpBasicProperty(XmpBasicType.Text, Name="genre")]
		Genre,

		[XmpBasicProperty(XmpBasicType.Text, Name="instrument")]
		Instrument,

		[XmpVideoMediaProperty(XmpVideoMediaType.Time, Category=XmpCategory.Internal, Name="introTime")]
		IntroTime,

		// TODO: enum as Text
		[XmpBasicProperty(XmpBasicType.Text, Category=XmpCategory.Internal, Name="key")]
		Key,

		[XmpBasicProperty(XmpBasicType.Text, Name="logComment")]
		LogComment,

		[XmpBasicProperty(XmpBasicType.Boolean, Category=XmpCategory.Internal, Name="loop")]
		Loop,

		[XmpBasicProperty(XmpBasicType.Real, Category=XmpCategory.Internal, Name="numberOfBeats")]
		NumberOfBeats,

		[XmpVideoMediaProperty(XmpVideoMediaType.Marker, XmpQuantity.Seq, Category=XmpCategory.Internal, Name="markers")]
		Markers,

		[XmpBasicProperty(XmpBasicType.Date, Category=XmpCategory.Internal, Name="metadataModDate")]
		MetadataModDate,

		[XmpVideoMediaProperty(XmpVideoMediaType.Time, Category=XmpCategory.Internal, Name="outCue")]
		OutCue,

		[XmpVideoMediaProperty(XmpVideoMediaType.ProjectLink, Category=XmpCategory.Internal, Name="projectRef")]
		ProjectRef,

		// TODO: enum as Text
		[XmpBasicProperty(XmpBasicType.Text, Category=XmpCategory.Internal, Name="pullDown")]
		PullDown,

		[XmpBasicProperty(XmpBasicType.URI, Category=XmpCategory.Internal, Name="relativePeakAudioFilePath")]
		RelativePeakAudioFilePath,

		[XmpVideoMediaProperty(XmpVideoMediaType.Time, Category=XmpCategory.Internal, Name="relativeTimestamp")]
		RelativeTimestamp,

		[XmpBasicProperty(XmpBasicType.Date, Name="releaseDate")]
		ReleaseDate,

		[XmpVideoMediaProperty(XmpVideoMediaType.ResampleStretch, Category=XmpCategory.Internal, Name="resampleParams")]
		ResampleParams,

		// TODO: enum as Text
		[XmpBasicProperty(XmpBasicType.Text, Category=XmpCategory.Internal, Name="scaleType")]
		ScaleType,

		[XmpBasicProperty(XmpBasicType.Text, Name="scene")]
		Scene,

		[XmpBasicProperty(XmpBasicType.Date, Name="shotDate")]
		ShotDate,

		[XmpBasicProperty(XmpBasicType.Text, Name="shotLocation")]
		ShotLocation,

		[XmpBasicProperty(XmpBasicType.Text, Name="shotName")]
		ShotName,

		[XmpBasicProperty(XmpBasicType.Text, Name="speakerPlacement")]
		SpeakerPlacement,

		[XmpVideoMediaProperty(XmpVideoMediaType.Timecode, Category=XmpCategory.Internal, Name="startTimecode")]
		StartTimecode,

		// TODO: enum as Text
		[XmpBasicProperty(XmpBasicType.Text, Category=XmpCategory.Internal, Name="stretchMode")]
		StretchMode,

		[XmpBasicProperty(XmpBasicType.Text, Name="tapeName")]
		TapeName,

		[XmpBasicProperty(XmpBasicType.Text, Category=XmpCategory.Internal, Name="tempo")]
		Tempo,

		[XmpVideoMediaProperty(XmpVideoMediaType.Timecode, Category=XmpCategory.Internal, Name="timeScaleParams")]
		TimeScaleParams,

		// TODO: enum as Text
		[XmpBasicProperty(XmpBasicType.Text, Category=XmpCategory.Internal, Name="timeSignature")]
		TimeSignature,

		[XmpBasicProperty(XmpBasicType.Integer, Name="trackNumber")]
		TrackNumber,

		[XmpVideoMediaProperty(XmpVideoMediaType.Track, XmpQuantity.Bag, Category=XmpCategory.Internal, Name="Tracks")]
		Tracks,

		// TODO: enum as Text
		[XmpBasicProperty(XmpBasicType.Text, Name="videoAlphaMode")]
		VideoAlphaMode,

		[XmpBasicProperty(XmpBasicType.Colorant, Name="videoAlphaPremultipleColor")]
		VideoAlphaPremultipleColor,

		[XmpBasicProperty(XmpBasicType.Boolean, Category=XmpCategory.Internal, Name="videoAlphaUnityIsTransparent")]
		VideoAlphaUnityIsTransparent,

		// TODO: enum as Text
		[XmpBasicProperty(XmpBasicType.Text, Category=XmpCategory.Internal, Name="videoColorSpace")]
		VideoColorSpace,

		[XmpBasicProperty(XmpBasicType.Text, Category=XmpCategory.Internal, Name="videoCompressor")]
		VideoCompressor,

		// TODO: enum as Text
		[XmpBasicProperty(XmpBasicType.Text, Category=XmpCategory.Internal, Name="videoFieldOrder")]
		VideoFieldOrder,

		// TODO: enum as Text
		[XmpBasicProperty(XmpBasicType.Text, Category=XmpCategory.Internal, Name="videoFrameRate")]
		VideoFrameRate,

		[XmpBasicProperty(XmpBasicType.Dimensions, Category=XmpCategory.Internal, Name="videoFrameSize")]
		VideoFrameSize,

		[XmpBasicProperty(XmpBasicType.Date, Category=XmpCategory.Internal, Name="videoModDate")]
		VideoModDate,

		// TODO: enum as Text
		[XmpBasicProperty(XmpBasicType.Text, Category=XmpCategory.Internal, Name="videoPixelDepth")]
		VideoPixelDepth,

		[ExifProperty(ExifType.Rational, Category=XmpCategory.Internal, Name="videoPixelAspectRatio")]
		VideoPixelAspectRatio
	}
}
