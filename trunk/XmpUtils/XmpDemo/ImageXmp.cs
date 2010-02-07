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
using System.ComponentModel;
using System.Text;

using XmpUtils;
using XmpUtils.Xmp;
using XmpUtils.Xmp.Schemas;
using XmpUtils.Xmp.ValueTypes.ExifTagValues;

namespace XmpDemo
{
	/// <summary>
	/// An app-specific view of common image XMP properties
	/// </summary>
	public class ImageXmp :
		INotifyPropertyChanging,
		INotifyPropertyChanged
	{
		#region Fields

		private decimal aperture;
		private string copyright;
		private string creator;
		private DateTime dateTaken;
		private string description;
		private int exposureBias_Numerator;
		private int exposureBias_Denominator;
		private ExifTagExposureMode exposureMode;
		private ExifTagExposureProgram exposureProgram;
		private ExifTagFlash flash;
		private decimal focalLength;
		private decimal gpsAltitude;
		private GpsCoordinate gpsLatitude;
		private GpsCoordinate gpsLongitude;
		private int imageHeight;
		private int imageWidth;
		private int isoSpeed;
		private ExifTagMeteringMode meteringMode;
		private string make;
		private string model;
		private ExifTagOrientation orientation;
		private uint shutterSpeed_Numerator;
		private uint shutterSpeed_Denominator;
		private IEnumerable<string> tags;
		private string title;
		private ExifTagWhiteBalance whiteBalance;

		#endregion Properties

		#region Properties

		/// <summary>
		/// Gets and sets the aperture
		/// </summary>
		public decimal Aperture
		{
			get { return this.aperture; }
			set
			{
				if (this.aperture == value)
				{
					return;
				}

				this.OnPropertyChanging("Aperture");
				this.aperture = value;
				this.OnPropertyChanged("Aperture");
			}
		}

		/// <summary>
		/// Gets and sets the copyright
		/// </summary>
		public string Copyright
		{
			get { return this.copyright; }
			set
			{
				if (this.copyright == value)
				{
					return;
				}

				this.OnPropertyChanging("Copyright");
				this.copyright = value;
				this.OnPropertyChanged("Copyright");
			}
		}

		/// <summary>
		/// Gets and sets the image creator
		/// </summary>
		public string Creator
		{
			get { return this.creator; }
			set
			{
				if (this.creator == value)
				{
					return;
				}

				this.OnPropertyChanging("Creator");
				this.creator = value;
				this.OnPropertyChanged("Creator");
			}
		}

		/// <summary>
		/// Gets and sets the date the photo was taken
		/// </summary>
		public DateTime DateTaken
		{
			get { return this.dateTaken; }
			set
			{
				if (this.dateTaken == value)
				{
					return;
				}

				this.OnPropertyChanging("DateTaken");
				this.dateTaken = value;
				this.OnPropertyChanged("DateTaken");
			}
		}

		/// <summary>
		/// Gets and sets the image description
		/// </summary>
		public string Description
		{
			get { return this.description; }
			set
			{
				if (this.description == value)
				{
					return;
				}

				this.OnPropertyChanging("Description");
				this.description = value;
				this.OnPropertyChanged("Description");
			}
		}

		/// <summary>
		/// Gets and sets the exposure bias
		/// </summary>
		public Rational<int> ExposureBias
		{
			get { return new Rational<int>(this.exposureBias_Numerator, this.exposureBias_Denominator, true); }
			set
			{
				this.ExposureBias_Numerator = value.Numerator;
				this.ExposureBias_Denominator = value.Denominator;
			}
		}

		/// <summary>
		/// For DB storage only
		/// </summary>
		public int ExposureBias_Numerator
		{
			get { return this.exposureBias_Numerator; }
			set
			{
				if (this.exposureBias_Numerator == value)
				{
					return;
				}

				this.OnPropertyChanging("ExposureBias_Numerator");
				this.OnPropertyChanging("ExposureBias_Value");
				this.exposureBias_Numerator = value;
				this.OnPropertyChanged("ExposureBias_Numerator");
				this.OnPropertyChanged("ExposureBias_Value");
			}
		}

		/// <summary>
		/// For DB storage only
		/// </summary>
		public int ExposureBias_Denominator
		{
			get { return this.exposureBias_Denominator; }
			set
			{
				if (this.exposureBias_Denominator == value)
				{
					return;
				}

				this.OnPropertyChanging("ExposureBias_Denominator");
				this.OnPropertyChanging("ExposureBias_Value");
				this.exposureBias_Denominator = value;
				this.OnPropertyChanged("ExposureBias_Denominator");
				this.OnPropertyChanged("ExposureBias_Value");
			}
		}

		/// <summary>
		/// For DB storage only
		/// </summary>
		public decimal ExposureBias_Value
		{
			get { return Math.Round(Convert.ToDecimal(this.ExposureBias), 5); }
			set { }
		}

		/// <summary>
		/// Gets and sets the exposure mode
		/// </summary>
		public ExifTagExposureMode ExposureMode
		{
			get { return this.exposureMode; }
			set
			{
				if (this.exposureMode == value)
				{
					return;
				}

				this.OnPropertyChanging("ExposureMode");
				this.exposureMode = value;
				this.OnPropertyChanged("ExposureMode");
			}
		}

		/// <summary>
		/// Gets and sets the exposure program
		/// </summary>
		public ExifTagExposureProgram ExposureProgram
		{
			get { return this.exposureProgram; }
			set
			{
				if (this.exposureProgram == value)
				{
					return;
				}

				this.OnPropertyChanging("ExposureProgram");
				this.exposureProgram = value;
				this.OnPropertyChanged("ExposureProgram");
			}
		}

		/// <summary>
		/// Gets and sets the flash
		/// </summary>
		public ExifTagFlash Flash
		{
			get { return this.flash; }
			set
			{
				if (this.flash == value)
				{
					return;
				}

				this.OnPropertyChanging("Flash");
				this.flash = value;
				this.OnPropertyChanged("Flash");
			}
		}

		/// <summary>
		/// Gets and sets the focal length
		/// </summary>
		public decimal FocalLength
		{
			get { return this.focalLength; }
			set
			{
				if (this.focalLength == value)
				{
					return;
				}

				this.OnPropertyChanging("FocalLength");
				this.focalLength = value;
				this.OnPropertyChanged("FocalLength");
			}
		}

		/// <summary>
		/// Gets and sets the GPS altitude
		/// </summary>
		public decimal GpsAltitude
		{
			get { return this.gpsAltitude; }
			set
			{
				if (this.gpsAltitude == value)
				{
					return;
				}

				this.OnPropertyChanging("GpsAltitude");
				this.gpsAltitude = value;
				this.OnPropertyChanged("GpsAltitude");
			}
		}

		/// <summary>
		/// Gets and sets the GPS latitude
		/// </summary>
		public GpsCoordinate GpsLatitude
		{
			get { return this.gpsLatitude; }
			set
			{
				if (this.gpsLatitude == value)
				{
					return;
				}

				this.OnPropertyChanging("GpsLatitude");
				this.gpsLatitude = value;
				this.OnPropertyChanged("GpsLatitude");
			}
		}

		/// <summary>
		/// Gets and sets the GPS longitude
		/// </summary>
		public GpsCoordinate GpsLongitude
		{
			get { return this.gpsLongitude; }
			set
			{
				if (this.gpsLongitude == value)
				{
					return;
				}

				this.OnPropertyChanging("GpsLongitude");
				this.gpsLongitude = value;
				this.OnPropertyChanged("GpsLongitude");
			}
		}

		/// <summary>
		/// Gets and sets the image height
		/// </summary>
		public int ImageHeight
		{
			get { return this.imageHeight; }
			set
			{
				if (this.imageHeight == value)
				{
					return;
				}

				this.OnPropertyChanging("ImageHeight");
				this.imageHeight = value;
				this.OnPropertyChanged("ImageHeight");
			}
		}

		/// <summary>
		/// Gets and sets the image width
		/// </summary>
		public int ImageWidth
		{
			get { return this.imageWidth; }
			set
			{
				if (this.imageWidth == value)
				{
					return;
				}

				this.OnPropertyChanging("ImageWidth");
				this.imageWidth = value;
				this.OnPropertyChanged("ImageWidth");
			}
		}

		/// <summary>
		/// Gets and sets the ISO speed
		/// </summary>
		public int ISOSpeed
		{
			get { return this.isoSpeed; }
			set
			{
				if (this.isoSpeed == value)
				{
					return;
				}

				this.OnPropertyChanging("ISOSpeed");
				this.isoSpeed = value;
				this.OnPropertyChanged("ISOSpeed");
			}
		}

		/// <summary>
		/// Gets and sets the metering mode
		/// </summary>
		public ExifTagMeteringMode MeteringMode
		{
			get { return this.meteringMode; }
			set
			{
				if (this.meteringMode == value)
				{
					return;
				}

				this.OnPropertyChanging("MeteringMode");
				this.meteringMode = value;
				this.OnPropertyChanged("MeteringMode");
			}
		}

		/// <summary>
		/// Gets and sets the camera make
		/// </summary>
		public string Make
		{
			get { return this.make; }
			set
			{
				if (this.make == value)
				{
					return;
				}

				this.OnPropertyChanging("Make");
				this.make = value;
				this.OnPropertyChanged("Make");
			}
		}

		/// <summary>
		/// Gets and sets the camera model
		/// </summary>
		public string Model
		{
			get { return this.model; }
			set
			{
				if (this.model == value)
				{
					return;
				}

				this.OnPropertyChanging("Model");
				this.model = value;
				this.OnPropertyChanged("Model");
			}
		}

		/// <summary>
		/// Gets and sets the orientation
		/// </summary>
		public ExifTagOrientation Orientation
		{
			get { return this.orientation; }
			set
			{
				if (this.orientation == value)
				{
					return;
				}

				this.OnPropertyChanging("Orientation");
				this.orientation = value;
				this.OnPropertyChanged("Orientation");
			}
		}

		/// <summary>
		/// Gets and sets the shutter speed in seconds
		/// </summary>
		public Rational<uint> ShutterSpeed
		{
			get { return new Rational<uint>(this.shutterSpeed_Numerator, this.shutterSpeed_Denominator, true); }
			set
			{
				this.ShutterSpeed_Numerator = value.Numerator;
				this.ShutterSpeed_Denominator = value.Denominator;
			}
		}

		/// <summary>
		/// For DB storage only
		/// </summary>
		public uint ShutterSpeed_Numerator
		{
			get { return this.shutterSpeed_Numerator; }
			set
			{
				if (this.shutterSpeed_Numerator == value)
				{
					return;
				}

				this.OnPropertyChanging("ShutterSpeed_Numerator");
				this.OnPropertyChanging("ShutterSpeed_Value");
				this.shutterSpeed_Numerator = value;
				this.OnPropertyChanged("ShutterSpeed_Numerator");
				this.OnPropertyChanged("ShutterSpeed_Value");
			}
		}

		/// <summary>
		/// For DB storage only
		/// </summary>
		public uint ShutterSpeed_Denominator
		{
			get { return this.shutterSpeed_Denominator; }
			set
			{
				if (this.shutterSpeed_Denominator == value)
				{
					return;
				}

				this.OnPropertyChanging("ShutterSpeed_Denominator");
				this.OnPropertyChanging("ShutterSpeed_Value");
				this.shutterSpeed_Denominator = value;
				this.OnPropertyChanged("ShutterSpeed_Denominator");
				this.OnPropertyChanged("ShutterSpeed_Value");
			}
		}

		/// <summary>
		/// For DB storage only
		/// </summary>
		public decimal ShutterSpeed_Value
		{
			get { return Math.Round(Convert.ToDecimal(this.ShutterSpeed), 5); }
			set { }
		}

		/// <summary>
		/// Gets and sets the keywords or tags
		/// </summary>
		public IEnumerable<string> Tags
		{
			get { return this.tags; }
			set
			{
				if (this.tags == value)
				{
					return;
				}

				this.OnPropertyChanging("Tags");
				this.tags = value;
				this.OnPropertyChanged("Tags");
			}
		}

		/// <summary>
		/// Gets and sets the image title
		/// </summary>
		public string Title
		{
			get { return this.title; }
			set
			{
				if (this.title == value)
				{
					return;
				}

				this.OnPropertyChanging("Title");
				this.title = value;
				this.OnPropertyChanged("Title");
			}
		}

		/// <summary>
		/// Gets and sets the white balance
		/// </summary>
		public ExifTagWhiteBalance WhiteBalance
		{
			get { return this.whiteBalance; }
			set
			{
				if (this.whiteBalance == value)
				{
					return;
				}

				this.OnPropertyChanging("WhiteBalance");
				this.whiteBalance = value;
				this.OnPropertyChanged("WhiteBalance");
			}
		}

		#endregion Properties

		#region Factory Method

		/// <summary>
		/// Factory method
		/// </summary>
		/// <param name="properties">EXIF properties from which to populate</param>
		public static ImageXmp Create(XmpPropertyCollection properties)
		{
		    if (properties == null)
		    {
		        throw new ArgumentNullException("properties");
		    }

		    // References:
		    // http://www.media.mit.edu/pia/Research/deepview/exif.html
		    // http://en.wikipedia.org/wiki/APEX_system
		    // http://en.wikipedia.org/wiki/Exposure_value

			decimal decimalValue;
			DateTime dateValue;
			string stringValue;
			GpsCoordinate gpsValue;
			IDictionary<string, object> dictionaryValue;

			ImageXmp xmp = new ImageXmp();

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
				properties.TryGetValue(ExifTiffSchema.Artist, out stringValue) ||
				properties.TryGetValue(ExifSchema.MSAuthor, out stringValue))
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
				properties.TryGetValue(ExifTiffSchema.ImageDescription, out stringValue) ||
				properties.TryGetValue(ExifSchema.MSSubject, out stringValue))
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

			if (properties.TryGetValue(ExifSchema.Flash, out dictionaryValue))
			{
				// decode object into EXIF enum
				ExifTagFlash flashValue = default(ExifTagFlash);

				if (dictionaryValue.ContainsKey("Fired") &&
					StringComparer.OrdinalIgnoreCase.Equals(Convert.ToString(dictionaryValue["Fired"]), Boolean.TrueString))
				{
					flashValue |= ExifTagFlash.FlashFired;
				}

				if (dictionaryValue.ContainsKey("Function") &&
					StringComparer.OrdinalIgnoreCase.Equals(Convert.ToString(dictionaryValue["Function"]), Boolean.TrueString))
				{
					flashValue |= ExifTagFlash.NoFlashFunction;
				}

				if (dictionaryValue.ContainsKey("Mode"))
				{
					switch (Convert.ToString(dictionaryValue["Mode"]))
					{
						case "1":
						{
							flashValue |= ExifTagFlash.ModeOn;
							break;
						}
						case "2":
						{
							flashValue |= ExifTagFlash.ModeOff;
							break;
						}
						case "3":
						{
							flashValue |= ExifTagFlash.ModeAuto;
							break;
						}
					}
				}

				if (dictionaryValue.ContainsKey("RedEyeMode") &&
					StringComparer.OrdinalIgnoreCase.Equals(Convert.ToString(dictionaryValue["RedEyeMode"]), Boolean.TrueString))
				{
					flashValue |= ExifTagFlash.RedEyeReduction;
				}

				if (dictionaryValue.ContainsKey("Return"))
				{
					switch (Convert.ToString(dictionaryValue["Return"]))
					{
						case "2":
						{
							flashValue |= ExifTagFlash.ReturnNotDetected;
							break;
						}
						case "3":
						{
							flashValue |= ExifTagFlash.ReturnDetected;
							break;
						}
					}
				}

				xmp.Flash = flashValue;
			}

		    #endregion Flash

		    #region FocalLength

			if (properties.TryGetValue(ExifSchema.FocalLength, out decimalValue) ||
				properties.TryGetValue(ExifSchema.FocalLengthIn35mmFilm, out decimalValue))
		    {
		        xmp.FocalLength = Decimal.Round(decimalValue, 1);
		    }

		    #endregion FocalLength

		    #region GpsAltitude

			if (properties.TryGetValue(ExifSchema.GPSAltitude, out decimalValue))
		    {
				xmp.GpsAltitude = Decimal.Round(decimalValue, 5);
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
			// TODO: ExifSchema.MSKeywords

			#endregion Tags

			#region Title

			if (properties.TryGetValue(DublinCoreSchema.Title, out stringValue) ||
				properties.TryGetValue(ExifSchema.ImageTitle, out stringValue) ||
				properties.TryGetValue(ExifSchema.MSTitle, out stringValue))
			{
				xmp.Title = stringValue;
			}

			#endregion Title

			#region WhiteBalance

			xmp.WhiteBalance = properties.GetValue(ExifSchema.WhiteBalance, default(ExifTagWhiteBalance));

		    #endregion WhiteBalance

			return xmp;
		}

		#endregion Factory Method

		#region INotifyPropertyChanging Members

		public event PropertyChangingEventHandler PropertyChanging;

		protected virtual void OnPropertyChanging(string propertyName)
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
			}
		}

		#endregion INotifyPropertyChanging Members

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion INotifyPropertyChanged Members

		#region Object Overrides

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			bool needsDelim = false;

			if (!String.IsNullOrEmpty(this.Model))
			{
				if (needsDelim)
				{
					builder.Append(", ");
				}
				else
				{
					needsDelim = true;
				}
				builder.Append(this.Model);
			}

			if (this.FocalLength != Decimal.Zero)
			{
				if (needsDelim)
				{
					builder.Append(", ");
				}
				else
				{
					needsDelim = true;
				}
				builder.Append(this.FocalLength);
				builder.Append("mm");
			}

			if (this.Aperture != Decimal.Zero)
			{
				if (needsDelim)
				{
					builder.Append(", ");
				}
				else
				{
					needsDelim = true;
				}
				builder.Append("f/");
				builder.Append(this.Aperture.ToString("N1"));
			}

			if (this.ShutterSpeed.Numerator > 0 &&
				this.ShutterSpeed.Denominator > 0)
			{
				if (needsDelim)
				{
					builder.Append(", ");
				}
				else
				{
					needsDelim = true;
				}
				if (this.ShutterSpeed.Numerator > this.ShutterSpeed.Denominator)
				{
					builder.Append(Convert.ToDecimal(this.ShutterSpeed));
				}
				else
				{
					builder.Append(this.ShutterSpeed);
				}
				builder.Append(" sec");
			}

			if (this.ISOSpeed != 0)
			{
				if (needsDelim)
				{
					builder.Append(", ");
				}
				else
				{
					needsDelim = true;
				}
				builder.Append("ISO-");
				builder.Append(this.ISOSpeed);
			}

			if (this.ImageWidth > 0 &&
				this.ImageHeight > 0)
			{
				if (needsDelim)
				{
					builder.Append(", ");
				}
				else
				{
					needsDelim = true;
				}
				builder.Append(this.ImageWidth);
				builder.Append(" x ");
				builder.Append(this.ImageHeight);
			}

			if (this.Orientation > ExifTagOrientation.Normal)
			{
				if (needsDelim)
				{
					builder.Append(", ");
				}
				else
				{
					needsDelim = true;
				}
				builder.Append(this.Orientation);
			}

			if (needsDelim)
			{
				builder.Append(", ");
			}
			else
			{
				needsDelim = true;
			}
			builder.Append(this.DateTaken.ToString("yyyy-MM-dd HH:mm:ss"));

			return builder.ToString();
		}

		#endregion Object Overrides
	}
}
