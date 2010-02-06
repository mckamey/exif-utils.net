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
using System.Collections.Generic;
using System.Linq;

using XmpUtils.Xmp;
using XmpUtils.Xmp.ValueTypes.ExifTagValues;
using XmpUtils.Xmp.Schemas;
using XmpUtils;

namespace XmpDemo
{
	/// <summary>
	/// A strongly-typed adapter for common XMP properties
	/// </summary>
	public class XmpMetadata
	{
		#region Properties

		/// <summary>
		/// Gets and sets the aperture
		/// </summary>
		public decimal Aperture
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets the color space
		/// </summary>
		public ExifTagColorSpace ColorSpace
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets the copyright
		/// </summary>
		public string Copyright
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets the artist
		/// </summary>
		public string Creator
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets the date the photo was taken
		/// </summary>
		public DateTime DateTaken
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets the exposure bias
		/// </summary>
		public Rational<int> ExposureBias
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets the exposure mode
		/// </summary>
		public ExifTagExposureMode ExposureMode
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets the exposure program
		/// </summary>
		public ExifTagExposureProgram ExposureProgram
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets the flash
		/// </summary>
		public IDictionary<string, object> Flash
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets the focal length
		/// </summary>
		public decimal FocalLength
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets the GPS altitude
		/// </summary>
		public decimal GpsAltitude
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets the GPS latitude
		/// </summary>
		public GpsCoordinate GpsLatitude
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets the GPS longitude
		/// </summary>
		public GpsCoordinate GpsLongitude
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets the image description
		/// </summary>
		public string ImageDescription
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets the image height
		/// </summary>
		public int ImageHeight
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets the image title
		/// </summary>
		public string ImageTitle
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets the image width
		/// </summary>
		public int ImageWidth
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets the ISO speed
		/// </summary>
		public int ISOSpeed
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets the metering mode
		/// </summary>
		public ExifTagMeteringMode MeteringMode
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets the camera make
		/// </summary>
		public string Make
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets the camera model
		/// </summary>
		public string Model
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets the orientation
		/// </summary>
		public ExifTagOrientation Orientation
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets the shutter speed in seconds
		/// </summary>
		public Rational<uint> ShutterSpeed
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets the keywords or tags
		/// </summary>
		public IEnumerable<string> Tags
		{
			get;
			set;
		}

		/// <summary>
		/// Gets and sets the white balance
		/// </summary>
		public ExifTagWhiteBalance WhiteBalance
		{
			get;
			set;
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="properties">EXIF properties from which to populate</param>
		public static XmpMetadata Create(XmpPropertyCollection properties)
		{
		    if (properties == null)
		    {
		        throw new ArgumentNullException("properties");
		    }

			XmpMetadata xmp = new XmpMetadata();

		    // References:
		    // http://www.media.mit.edu/pia/Research/deepview/exif.html
		    // http://en.wikipedia.org/wiki/APEX_system
		    // http://en.wikipedia.org/wiki/Exposure_value

		    object rawValue;

		    #region Aperture

		    rawValue = properties[ExifSchema.FNumber];
		    if (rawValue is IConvertible)
		    {
		        // f/x.x
		        xmp.Aperture = Convert.ToDecimal(rawValue);
		    }
		    else
		    {
		        rawValue = properties[ExifSchema.ApertureValue];
		        if (rawValue is IConvertible)
		        {
		            // f/x.x
		            xmp.Aperture = (decimal)Math.Pow(2.0, Convert.ToDouble(rawValue)/2.0);
		        }
		    }

		    #endregion Aperture

		    xmp.Creator = Convert.ToString(properties[ExifTiffSchema.Artist]);

		    #region ColorSpace

		    rawValue = properties[ExifSchema.ColorSpace];
		    if (rawValue is Enum)
		    {
		        xmp.ColorSpace = (ExifTagColorSpace)rawValue;
		    }

		    #endregion ColorSpace

		    xmp.Copyright = Convert.ToString(properties[ExifTiffSchema.Copyright]);

		    #region DateTaken

		    rawValue = properties[ExifSchema.DateTimeOriginal];
		    if (rawValue is DateTime)
		    {
		        xmp.DateTaken = (DateTime)rawValue;
		    }
		    else
		    {
		        rawValue = properties[ExifSchema.DateTimeDigitized];
		        if (rawValue is DateTime)
		        {
		            xmp.DateTaken = (DateTime)rawValue;
		        }
		        else
		        {
					rawValue = properties[ExifTiffSchema.DateTime];
		            if (rawValue is DateTime)
		            {
		                xmp.DateTaken = (DateTime)rawValue;
		            }
		        }
		    }

		    #endregion DateTaken

		    #region ExposureBias

		    rawValue = properties[ExifSchema.ExposureBiasValue];
		    if (rawValue is Rational<int>)
		    {
		        xmp.ExposureBias = (Rational<int>)rawValue;
		    }

		    #endregion ExposureBias

		    #region ExposureMode

		    rawValue = properties[ExifSchema.ExposureMode];
		    if (rawValue is Enum)
		    {
		        xmp.ExposureMode = (ExifTagExposureMode)rawValue;
		    }

		    #endregion ExposureMode

		    #region ExposureProgram

		    rawValue = properties[ExifSchema.ExposureProgram];
		    if (rawValue is Enum)
		    {
		        xmp.ExposureProgram = (ExifTagExposureProgram)rawValue;
		    }

		    #endregion ExposureProgram

		    #region Flash

			xmp.Flash = properties[ExifSchema.Flash] as IDictionary<string, object>;

		    #endregion Flash

		    #region FocalLength

		    rawValue = properties[ExifSchema.FocalLength];
		    if (rawValue is IConvertible)
		    {
		        xmp.FocalLength = Convert.ToDecimal(rawValue);
		    }
		    else
		    {
		        rawValue = properties[ExifSchema.FocalLengthIn35mmFilm];
		        if (rawValue is IConvertible)
		        {
		            xmp.FocalLength = Convert.ToDecimal(rawValue);
		        }
		    }

		    #endregion FocalLength

		    #region GpsAltitude

		    rawValue = properties[ExifSchema.GPSAltitude];
		    if (rawValue is IConvertible)
		    {
		        xmp.GpsAltitude = Convert.ToDecimal(rawValue);
		    }

		    #endregion GpsAltitude

		    #region GpsLatitude

		    rawValue = properties[ExifSchema.GPSLatitude];
		    if (rawValue == null)
		    {
		        rawValue = properties[ExifSchema.GPSDestLatitude];
		    }
		    if (rawValue != null)
		    {
		        xmp.GpsLatitude = rawValue as GpsCoordinate;
		    }

		    #endregion GpsLatitude

		    #region GpsLongitude

		    rawValue = properties[ExifSchema.GPSLongitude];
			if (rawValue != null)
			{
		        rawValue = properties[ExifSchema.GPSDestLongitude];
		    }
			if (rawValue != null)
			{
				xmp.GpsLongitude = rawValue as GpsCoordinate;
		    }

		    #endregion GpsLongitude

			xmp.ImageDescription = Convert.ToString(properties[ExifTiffSchema.ImageDescription]);

		    #region ImageHeight

			rawValue = properties[ExifTiffSchema.ImageLength];
		    if (rawValue is IConvertible)
		    {
		        xmp.ImageHeight = Convert.ToInt32(rawValue);
		    }

		    #endregion ImageHeight

		    #region ImageWidth

			rawValue = properties[ExifTiffSchema.ImageWidth];
		    if (rawValue is IConvertible)
		    {
		        xmp.ImageWidth = Convert.ToInt32(rawValue);
		    }

		    #endregion ImageWidth

		    xmp.ImageTitle = Convert.ToString(properties[ExifSchema.ImageTitle]);

		    #region ISOSpeed

		    rawValue = properties[ExifSchema.ISOSpeedRatings];
		    if (rawValue is Array)
		    {
		        Array array = (Array)rawValue;
		        if (array.Length > 0)
		        {
		            rawValue = array.GetValue(0);
		        }
		    }
		    if (rawValue is IConvertible)
		    {
		        xmp.ISOSpeed = Convert.ToInt32(rawValue);
		    }

		    #endregion ISOSpeed

		    xmp.Make = Convert.ToString(properties[ExifTiffSchema.Make]);
			xmp.Model = Convert.ToString(properties[ExifTiffSchema.Model]);

		    #region MeteringMode

		    rawValue = properties[ExifSchema.MeteringMode];
		    if (rawValue is Enum)
		    {
		        xmp.MeteringMode = (ExifTagMeteringMode)rawValue;
		    }

		    #endregion MeteringMode

		    #region Orientation

		    rawValue = properties[ExifTiffSchema.Orientation];
		    if (rawValue is Enum)
		    {
		        xmp.Orientation = (ExifTagOrientation)rawValue;
		    }

		    #endregion Orientation

		    #region ShutterSpeed

		    rawValue = properties[ExifSchema.ExposureTime];
		    if (rawValue is Rational<uint>)
		    {
		        xmp.ShutterSpeed = (Rational<uint>)rawValue;
		    }
		    else
		    {
		        rawValue = properties[ExifSchema.ShutterSpeedValue];
		        if (rawValue is Rational<int>)
		        {
		            xmp.ShutterSpeed = Rational<uint>.Approximate((decimal)Math.Pow(2.0, -Convert.ToDouble(rawValue)));
		        }
		    }

		    #endregion ShutterSpeed

		    #region WhiteBalance

		    xmp.Tags = properties.GetValue(DublinCoreSchema.Subject, default(IEnumerable<string>));

		    #endregion WhiteBalance

		    #region WhiteBalance

			xmp.WhiteBalance = properties.GetValue(ExifSchema.WhiteBalance, default(ExifTagWhiteBalance));

		    #endregion WhiteBalance

			return xmp;
		}

		public void Apply(XmpPropertyCollection properties)
		{
			// TODO: set changed properties back onto collection

			properties[DublinCoreSchema.Creator] = this.Creator;

			properties[DublinCoreSchema.Rights] = this.Copyright;

			properties[DublinCoreSchema.Subject] = this.Tags;
		}

		#endregion Methods
	}
}
