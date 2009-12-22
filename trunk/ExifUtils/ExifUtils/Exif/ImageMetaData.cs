#region License
/*---------------------------------------------------------------------------------*\

	Distributed under the terms of an MIT-style license:

	The MIT License

	Copyright (c) 2005-2009 Stephen M. McKamey

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
using System.Drawing;

using ExifUtils.Exif.IO;
using ExifUtils.Exif.TagValues;

namespace ExifUtils.Exif
{
	/// <summary>
	/// A strongly-typed adapter for common EXIF properties
	/// </summary>
	public class ImageMetaData
	{
		#region Constants

		private static readonly ExifTag[] StandardTags =
		{
			ExifTag.Aperture,
			ExifTag.Artist,
			ExifTag.ColorSpace,
			ExifTag.CompressedImageHeight,
			ExifTag.CompressedImageWidth,
			ExifTag.Copyright,
			ExifTag.DateTime,
			ExifTag.DateTimeDigitized,
			ExifTag.DateTimeOriginal,
			ExifTag.ExposureBias,
			ExifTag.ExposureMode,
			ExifTag.ExposureProgram,
			ExifTag.ExposureTime,
			ExifTag.Flash,
			ExifTag.FNumber,
			ExifTag.FocalLength,
			ExifTag.FocalLengthIn35mmFilm,
			ExifTag.GpsAltitude,
			ExifTag.GpsDestLatitude,
			ExifTag.GpsDestLatitudeRef,
			ExifTag.GpsDestLongitude,
			ExifTag.GpsDestLongitudeRef,
			ExifTag.GpsLatitude,
			ExifTag.GpsLatitudeRef,
			ExifTag.GpsLongitude,
			ExifTag.GpsLongitudeRef,
			ExifTag.ImageDescription,
			ExifTag.ImageTitle,
			ExifTag.ImageWidth,
			ExifTag.ISOSpeed,
			ExifTag.Make,
			ExifTag.MeteringMode,
			ExifTag.Model,
			ExifTag.MSAuthor,
			ExifTag.MSComments,
			ExifTag.MSKeywords,
			ExifTag.MSSubject,
			ExifTag.MSTitle,
			ExifTag.Orientation,
			ExifTag.ShutterSpeed,
			ExifTag.WhiteBalance
		};

		#endregion Constants

		#region Fields

		private decimal aperture;
		private string artist;
		private ExifTagColorSpace colorSpace;
		private string copyright;
		private DateTime dateTaken;
		private Rational<int> exposureBias;
		private ExifTagExposureMode exposureMode;
		private ExifTagExposureProgram exposureProgram;
		private ExifTagFlash flash;
		private decimal focalLength;
		private decimal gpsAltitude;
		private decimal gpsLatitude;
		private decimal gpsLongitude;
		private string imageDescription;
		private int imageHeight;
		private string imageTitle;
		private int imageWidth;
		private int isoSpeed;
		private ExifTagMeteringMode meteringMode;
		private string make;
		private string model;
		private string msAuthor;
		private string msComments;
		private string msKeywords;
		private string msSubject;
		private string msTitle;
		private ExifTagOrientation orientation;
		private Rational<uint> shutterSpeed;
		private ExifTagWhiteBalance whiteBalance;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="image">image from which to populate properties</param>
		public ImageMetaData(Bitmap image)
			: this(ExifReader.GetExifData(image, ImageMetaData.StandardTags))
		{
			if (image != null)
			{
				// override EXIF with actual values
				if (image.Height > 0)
				{
					this.ImageHeight = image.Height;
				}

				if (image.Width > 0)
				{
					this.ImageWidth = image.Width;
				}
			}
		}

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="properties">EXIF properties from which to populate</param>
		public ImageMetaData(ExifPropertyCollection properties)
		{
			if (properties == null)
			{
				throw new ArgumentNullException("properties");
			}

			// References:
			// http://www.media.mit.edu/pia/Research/deepview/exif.html
			// http://en.wikipedia.org/wiki/APEX_system
			// http://en.wikipedia.org/wiki/Exposure_value

			object rawValue;

			#region Aperture

			rawValue = properties[ExifTag.FNumber].Value;
			if (rawValue is IConvertible)
			{
				// f/x.x
				this.Aperture = Convert.ToDecimal(rawValue);
			}
			else
			{
				rawValue = properties[ExifTag.Aperture].Value;
				if (rawValue is IConvertible)
				{
					// f/x.x
					this.Aperture = (decimal)Math.Pow(2.0, Convert.ToDouble(rawValue)/2.0);
				}
			}

			#endregion Aperture

			this.Artist = Convert.ToString(properties[ExifTag.Artist].Value);

			#region ColorSpace

			rawValue = properties[ExifTag.ColorSpace].Value;
			if (rawValue is Enum)
			{
				this.ColorSpace = (ExifTagColorSpace)rawValue;
			}

			#endregion ColorSpace

			this.Copyright = Convert.ToString(properties[ExifTag.Copyright].Value);

			#region DateTaken

			rawValue = properties[ExifTag.DateTimeOriginal].Value;
			if (rawValue is DateTime)
			{
				this.DateTaken = (DateTime)rawValue;
			}
			else
			{
				rawValue = properties[ExifTag.DateTimeDigitized].Value;
				if (rawValue is DateTime)
				{
					this.DateTaken = (DateTime)rawValue;
				}
				else
				{
					rawValue = properties[ExifTag.DateTime].Value;
					if (rawValue is DateTime)
					{
						this.DateTaken = (DateTime)rawValue;
					}
				}
			}

			#endregion DateTaken

			#region ExposureBias

			rawValue = properties[ExifTag.ExposureBias].Value;
			if (rawValue is Rational<int>)
			{
				this.ExposureBias = (Rational<int>)rawValue;
			}

			#endregion ExposureBias

			#region ExposureMode

			rawValue = properties[ExifTag.ExposureMode].Value;
			if (rawValue is Enum)
			{
				this.ExposureMode = (ExifTagExposureMode)rawValue;
			}

			#endregion ExposureMode

			#region ExposureProgram

			rawValue = properties[ExifTag.ExposureProgram].Value;
			if (rawValue is Enum)
			{
				this.ExposureProgram = (ExifTagExposureProgram)rawValue;
			}

			#endregion ExposureProgram

			#region Flash

			rawValue = properties[ExifTag.Flash].Value;
			if (rawValue is Enum)
			{
				this.Flash = (ExifTagFlash)rawValue;
			}

			#endregion Flash

			#region FocalLength

			rawValue = properties[ExifTag.FocalLength].Value;
			if (rawValue is IConvertible)
			{
				this.FocalLength = Convert.ToDecimal(rawValue);
			}
			else
			{
				rawValue = properties[ExifTag.FocalLengthIn35mmFilm].Value;
				if (rawValue is IConvertible)
				{
					this.FocalLength = Convert.ToDecimal(rawValue);
				}
			}

			#endregion FocalLength

			#region GpsAltitude

			rawValue = properties[ExifTag.GpsAltitude].Value;
			if (rawValue is IConvertible)
			{
				this.GpsAltitude = Convert.ToDecimal(rawValue);
			}

			#endregion GpsAltitude

			bool isNeg;

			#region GpsLatitude

			isNeg = StringComparer.OrdinalIgnoreCase.Equals(Convert.ToString(properties[ExifTag.GpsLatitudeRef].Value), "S");
			rawValue = properties[ExifTag.GpsLatitude].Value;
			if (!(rawValue is Array))
			{
				isNeg = StringComparer.OrdinalIgnoreCase.Equals(Convert.ToString(properties[ExifTag.GpsDestLatitudeRef].Value), "S");
				rawValue = properties[ExifTag.GpsDestLatitude].Value;
			}
			if (rawValue is Array)
			{
				Array array = (Array)rawValue;
				if (array.Length == 3)
				{
					this.GpsLatitude = Convert.ToDecimal(array.GetValue(0))+ Decimal.Divide(Convert.ToDecimal(array.GetValue(1)), 60m) + Decimal.Divide(Convert.ToDecimal(array.GetValue(2)), 3600m);
					if (isNeg)
					{
						this.GpsLatitude = -this.GpsLatitude;
					}
				}
			}

			#endregion GpsLatitude

			#region GpsLongitude

			isNeg = StringComparer.OrdinalIgnoreCase.Equals(Convert.ToString(properties[ExifTag.GpsLongitudeRef].Value), "W");
			rawValue = properties[ExifTag.GpsLongitude].Value;
			if (!(rawValue is Array))
			{
				isNeg = StringComparer.OrdinalIgnoreCase.Equals(Convert.ToString(properties[ExifTag.GpsDestLongitudeRef].Value), "W");
				rawValue = properties[ExifTag.GpsDestLongitude].Value;
			}
			if (rawValue is Array)
			{
				Array array = (Array)rawValue;
				if (array.Length == 3)
				{
					this.GpsLongitude = Convert.ToDecimal(array.GetValue(0))+ Decimal.Divide(Convert.ToDecimal(array.GetValue(1)), 60m) + Decimal.Divide(Convert.ToDecimal(array.GetValue(2)), 3600m);
					if (isNeg)
					{
						this.GpsLongitude = -this.GpsLongitude;
					}
				}
			}

			#endregion GpsLongitude

			this.ImageDescription = Convert.ToString(properties[ExifTag.ImageDescription].Value);

			#region ImageHeight

			rawValue = properties[ExifTag.ImageHeight].Value;
			if (rawValue is IConvertible)
			{
				this.ImageHeight = Convert.ToInt32(rawValue);
			}
			else
			{
				rawValue = properties[ExifTag.CompressedImageHeight].Value;
				if (rawValue is IConvertible)
				{
					this.ImageHeight = Convert.ToInt32(rawValue);
				}
			}

			#endregion ImageHeight

			#region ImageWidth

			rawValue = properties[ExifTag.ImageWidth].Value;
			if (rawValue is IConvertible)
			{
				this.ImageWidth = Convert.ToInt32(rawValue);
			}
			else
			{
				rawValue = properties[ExifTag.CompressedImageWidth].Value;
				if (rawValue is IConvertible)
				{
					this.ImageWidth = Convert.ToInt32(rawValue);
				}
			}

			#endregion ImageWidth

			this.ImageTitle = Convert.ToString(properties[ExifTag.ImageTitle].Value);

			#region ISOSpeed

			rawValue = properties[ExifTag.ISOSpeed].Value;
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
				this.ISOSpeed = Convert.ToInt32(rawValue);
			}

			#endregion ISOSpeed

			this.Make = Convert.ToString(properties[ExifTag.Make].Value);
			this.Model = Convert.ToString(properties[ExifTag.Model].Value);

			#region MeteringMode

			rawValue = properties[ExifTag.MeteringMode].Value;
			if (rawValue is Enum)
			{
				this.MeteringMode = (ExifTagMeteringMode)rawValue;
			}

			#endregion MeteringMode

			this.MSAuthor = Convert.ToString(properties[ExifTag.MSAuthor].Value);
			this.MSComments = Convert.ToString(properties[ExifTag.MSComments].Value);
			this.MSKeywords = Convert.ToString(properties[ExifTag.MSKeywords].Value);
			this.MSSubject = Convert.ToString(properties[ExifTag.MSSubject].Value);
			this.MSTitle = Convert.ToString(properties[ExifTag.MSTitle].Value);

			#region Orientation

			rawValue = properties[ExifTag.Orientation].Value;
			if (rawValue is Enum)
			{
				this.Orientation = (ExifTagOrientation)rawValue;
			}

			#endregion Orientation

			#region ShutterSpeed

			rawValue = properties[ExifTag.ExposureTime].Value;
			if (rawValue is Rational<uint>)
			{
				this.ShutterSpeed = (Rational<uint>)rawValue;
			}
			else
			{
				rawValue = properties[ExifTag.ShutterSpeed].Value;
				if (rawValue is Rational<int>)
				{
					this.ShutterSpeed = Rational<uint>.Approximate((decimal)Math.Pow(2.0, -Convert.ToDouble(rawValue)));
				}
			}

			#endregion ShutterSpeed

			#region WhiteBalance

			rawValue = properties[ExifTag.WhiteBalance].Value;
			if (rawValue is Enum)
			{
				this.WhiteBalance = (ExifTagWhiteBalance)rawValue;
			}

			#endregion WhiteBalance
		}

		#endregion Init

		#region Properties

		/// <summary>
		/// Gets and sets the aperture
		/// </summary>
		public decimal Aperture
		{
			get { return this.aperture; }
			set { this.aperture = value; }
		}

		/// <summary>
		/// Gets and sets the artist
		/// </summary>
		public string Artist
		{
			get { return this.artist; }
			set { this.artist = value; }
		}

		/// <summary>
		/// Gets and sets the color space
		/// </summary>
		public ExifTagColorSpace ColorSpace
		{
			get { return this.colorSpace; }
			set { this.colorSpace = value; }
		}

		/// <summary>
		/// Gets and sets the copyright
		/// </summary>
		public string Copyright
		{
			get { return this.copyright; }
			set { this.copyright = value; }
		}

		/// <summary>
		/// Gets and sets the date the photo was taken
		/// </summary>
		public DateTime DateTaken
		{
			get { return this.dateTaken; }
			set { this.dateTaken = value; }
		}

		/// <summary>
		/// Gets and sets the exposure bias
		/// </summary>
		public Rational<int> ExposureBias
		{
			get { return this.exposureBias; }
			set { this.exposureBias = value; }
		}

		/// <summary>
		/// Gets and sets the exposure mode
		/// </summary>
		public ExifTagExposureMode ExposureMode
		{
			get { return this.exposureMode; }
			set { this.exposureMode = value; }
		}

		/// <summary>
		/// Gets and sets the exposure program
		/// </summary>
		public ExifTagExposureProgram ExposureProgram
		{
			get { return this.exposureProgram; }
			set { this.exposureProgram = value; }
		}

		/// <summary>
		/// Gets and sets the flash
		/// </summary>
		public ExifTagFlash Flash
		{
			get { return this.flash; }
			set { this.flash = value; }
		}

		/// <summary>
		/// Gets and sets the focal length
		/// </summary>
		public decimal FocalLength
		{
			get { return this.focalLength; }
			set { this.focalLength = value; }
		}

		/// <summary>
		/// Gets and sets the GPS altitude
		/// </summary>
		public decimal GpsAltitude
		{
			get { return this.gpsAltitude; }
			set { this.gpsAltitude = value; }
		}

		/// <summary>
		/// Gets and sets the GPS latitude
		/// </summary>
		public decimal GpsLatitude
		{
			get { return this.gpsLatitude; }
			set { this.gpsLatitude = value; }
		}

		/// <summary>
		/// Gets and sets the GPS longitude
		/// </summary>
		public decimal GpsLongitude
		{
			get { return this.gpsLongitude; }
			set { this.gpsLongitude = value; }
		}

		/// <summary>
		/// Gets and sets the image description
		/// </summary>
		public string ImageDescription
		{
			get { return this.imageDescription; }
			set { this.imageDescription = value; }
		}

		/// <summary>
		/// Gets and sets the image height
		/// </summary>
		public int ImageHeight
		{
			get { return this.imageHeight; }
			set { this.imageHeight = value; }
		}

		/// <summary>
		/// Gets and sets the image title
		/// </summary>
		public string ImageTitle
		{
			get { return this.imageTitle; }
			set { this.imageTitle = value; }
		}

		/// <summary>
		/// Gets and sets the image width
		/// </summary>
		public int ImageWidth
		{
			get { return this.imageWidth; }
			set { this.imageWidth = value; }
		}

		/// <summary>
		/// Gets and sets the ISO speed
		/// </summary>
		public int ISOSpeed
		{
			get { return this.isoSpeed; }
			set { this.isoSpeed = value; }
		}

		/// <summary>
		/// Gets and sets the metering mode
		/// </summary>
		public ExifTagMeteringMode MeteringMode
		{
			get { return this.meteringMode; }
			set { this.meteringMode = value; }
		}

		/// <summary>
		/// Gets and sets the camera make
		/// </summary>
		public string Make
		{
			get { return this.make; }
			set { this.make = value; }
		}

		/// <summary>
		/// Gets and sets the camera model
		/// </summary>
		public string Model
		{
			get { return this.model; }
			set { this.model = value; }
		}

		/// <summary>
		/// Gets and sets the author
		/// </summary>
		public string MSAuthor
		{
			get { return this.msAuthor; }
			set { this.msAuthor = value; }
		}

		/// <summary>
		/// Gets and sets comments
		/// </summary>
		public string MSComments
		{
			get { return this.msComments; }
			set { this.msComments = value; }
		}

		/// <summary>
		/// Gets and sets keywords
		/// </summary>
		public string MSKeywords
		{
			get { return this.msKeywords; }
			set { this.msKeywords = value; }
		}

		/// <summary>
		/// Gets and sets the subject
		/// </summary>
		public string MSSubject
		{
			get { return this.msSubject; }
			set { this.msSubject = value; }
		}

		/// <summary>
		/// Gets and sets the title
		/// </summary>
		public string MSTitle
		{
			get { return this.msTitle; }
			set { this.msTitle = value; }
		}

		/// <summary>
		/// Gets and sets the orientation
		/// </summary>
		public ExifTagOrientation Orientation
		{
			get { return this.orientation; }
			set { this.orientation = value; }
		}

		/// <summary>
		/// Gets and sets the shutter speed in seconds
		/// </summary>
		public Rational<uint> ShutterSpeed
		{
			get { return this.shutterSpeed; }
			set { this.shutterSpeed = value; }
		}

		/// <summary>
		/// Gets and sets the white balance
		/// </summary>
		public ExifTagWhiteBalance WhiteBalance
		{
			get { return this.whiteBalance; }
			set { this.whiteBalance = value; }
		}

		#endregion Properties
	}
}
