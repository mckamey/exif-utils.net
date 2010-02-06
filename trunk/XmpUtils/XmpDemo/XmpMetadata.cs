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
		/// Gets and sets the image description
		/// </summary>
		public string Description
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
		/// Gets and sets the image height
		/// </summary>
		public int ImageHeight
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
		/// Gets and sets the image title
		/// </summary>
		public string Title
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

			decimal decimalValue;
			DateTime dateValue;
			string stringValue;
			GpsCoordinate gpsValue;
			object rawValue;

		    #region Aperture

			if (properties.TryGetValue(ExifSchema.FNumber, out decimalValue))
		    {
		        // f/x.x
				xmp.Aperture = decimalValue;
		    }
			else if (properties.TryGetValue(ExifSchema.ApertureValue, out decimalValue))
		    {
	            // f/x.x
				xmp.Aperture = Decimal.Round((decimal)Math.Pow(2.0, Convert.ToDouble(decimalValue)/2.0), 1);
		    }

		    #endregion Aperture

			#region Creator

			if (properties.TryGetValue(DublinCoreSchema.Creator, out stringValue) ||
				properties.TryGetValue(ExifTiffSchema.Artist, out stringValue))
			{
				xmp.Creator = stringValue;
			}

			#endregion Creator

			#region Copyright

			if (properties.TryGetValue(DublinCoreSchema.Rights, out stringValue) ||
				properties.TryGetValue(ExifTiffSchema.Copyright, out stringValue))
			{
				xmp.Copyright = stringValue;
			}

			#endregion Copyright

			#region DateTaken

			if (properties.TryGetValue(ExifSchema.DateTimeOriginal, out dateValue) ||
				properties.TryGetValue(ExifSchema.DateTimeDigitized, out dateValue) ||
				properties.TryGetValue(ExifTiffSchema.DateTime, out dateValue))
			{
				xmp.DateTaken = dateValue;
			}

			#endregion DateTaken

			#region Description

			if (properties.TryGetValue(DublinCoreSchema.Description, out stringValue) ||
				properties.TryGetValue(ExifTiffSchema.ImageDescription, out stringValue))
			{
				xmp.Description = stringValue;
			}

			#endregion Description

			#region ExposureBias

			xmp.ExposureBias = properties.GetValue(ExifSchema.ExposureBiasValue, Rational<int>.Empty);

		    #endregion ExposureBias

		    #region ExposureMode

		    xmp.ExposureMode = properties.GetValue(ExifSchema.ExposureMode, default(ExifTagExposureMode));

		    #endregion ExposureMode

		    #region ExposureProgram

			xmp.ExposureProgram = properties.GetValue(ExifSchema.ExposureProgram, default(ExifTagExposureProgram));

		    #endregion ExposureProgram

		    #region Flash

			xmp.Flash = properties[ExifSchema.Flash] as IDictionary<string, object>;

		    #endregion Flash

		    #region FocalLength

			if (properties.TryGetValue(ExifSchema.FocalLength, out decimalValue) ||
				properties.TryGetValue(ExifSchema.FocalLengthIn35mmFilm, out decimalValue))
		    {
		        xmp.FocalLength = decimalValue;
		    }

		    #endregion FocalLength

		    #region GpsAltitude

			if (properties.TryGetValue(ExifSchema.GPSAltitude, out decimalValue))
		    {
				xmp.GpsAltitude = decimalValue;
		    }

		    #endregion GpsAltitude

		    #region GpsLatitude

			if (properties.TryGetValue(ExifSchema.GPSLatitude, out gpsValue) ||
				properties.TryGetValue(ExifSchema.GPSDestLatitude, out gpsValue))
		    {
				xmp.GpsLatitude = gpsValue;
		    }

		    #endregion GpsLatitude

		    #region GpsLongitude

			if (properties.TryGetValue(ExifSchema.GPSLongitude, out gpsValue) ||
				properties.TryGetValue(ExifSchema.GPSDestLongitude, out gpsValue))
			{
				xmp.GpsLongitude = gpsValue;
			}

		    #endregion GpsLongitude

			#region ImageHeight

			if (properties.TryGetValue(ExifTiffSchema.ImageLength, out decimalValue))
		    {
				xmp.ImageHeight = Convert.ToInt32(decimalValue);
		    }

		    #endregion ImageHeight

		    #region ImageWidth

			if (properties.TryGetValue(ExifTiffSchema.ImageWidth, out decimalValue))
			{
				xmp.ImageWidth = Convert.ToInt32(decimalValue);
			}

		    #endregion ImageWidth

			#region ISOSpeed

			if (properties.TryGetValue(ExifSchema.ISOSpeedRatings, out decimalValue))
			{
				xmp.ISOSpeed = Convert.ToInt32(decimalValue);
			}

			#endregion ISOSpeed

			#region Make

			if (properties.TryGetValue(ExifTiffSchema.Make, out stringValue))
			{
				xmp.Make = stringValue;
			}

			#endregion Make

			#region Make

			if (properties.TryGetValue(ExifTiffSchema.Model, out stringValue))
			{
				xmp.Model = stringValue;
			}

			#endregion Model

		    #region MeteringMode

			xmp.MeteringMode = properties.GetValue(ExifSchema.MeteringMode, default(ExifTagMeteringMode));

		    #endregion MeteringMode

		    #region Orientation

			xmp.Orientation = properties.GetValue(ExifTiffSchema.Orientation, default(ExifTagOrientation));

		    #endregion Orientation

		    #region ShutterSpeed

		    xmp.ShutterSpeed = properties.GetValue(ExifSchema.ExposureTime, Rational<uint>.Empty);
			if (xmp.ShutterSpeed.IsEmpty)
			{
				Rational<int> shutterSpeed = properties.GetValue(ExifSchema.ShutterSpeedValue, Rational<int>.Empty);
				if (!shutterSpeed.IsEmpty)
				{
					xmp.ShutterSpeed = Rational<uint>.Approximate((decimal)Math.Pow(2.0, -Convert.ToDouble(shutterSpeed)));
				}
			}

		    #endregion ShutterSpeed

			#region Tags

			xmp.Tags = properties.GetValue(DublinCoreSchema.Subject, default(IEnumerable<string>));

			#endregion Tags

			#region Title

			if (properties.TryGetValue(DublinCoreSchema.Title, out stringValue) ||
				properties.TryGetValue(ExifSchema.ImageTitle, out stringValue))
			{
				xmp.Title = stringValue;
			}

			#endregion Title

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
