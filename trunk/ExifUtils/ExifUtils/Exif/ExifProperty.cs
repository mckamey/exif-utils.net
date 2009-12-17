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
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Serialization;
using System.Drawing;
using System.Drawing.Imaging;

using ExifUtils.Exif.IO;
using ExifUtils.Exif.TagValues;
using ExifUtils.Exif.TypeConverters;

namespace ExifUtils.Exif
{
	/// <summary>
	/// Represents a single EXIF property.
	/// </summary>
	/// <remarks>
	/// Should try to serialize as EXIF+RDF http://www.w3.org/2003/12/exif/
	/// </remarks>
	[Serializable]
	[TypeConverter(typeof(ExifConverter))]
	public class ExifProperty
	{
		#region Fields

		private int id = (int)ExifTag.Unknown;
		private ExifType type = ExifType.Unknown;
		private object value = null;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor
		/// </summary>
		public ExifProperty()
		{
		}

		/// <summary>
		/// Ctor
		/// </summary>
		public ExifProperty(ExifTag tag, object value)
		{
			this.Tag = tag;
			this.value = value;
		}

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="property"></param>
		public ExifProperty(PropertyItem property)
		{
			this.id = property.Id;
			this.type = (ExifType)property.Type;
			this.value = ExifDecoder.FromPropertyItem(property);
		}

		#endregion Init

		#region Properties

		/// <summary>
		/// Gets and sets the Property ID according to the Exif specification for DCF images.
		/// </summary>
		[Category("Key")]
		[DisplayName("Exif ID")]
		[Description("The Property ID according to the Exif specification for DCF images.")]
		[XmlAttribute("ExifID"), DefaultValue((int)ExifTag.Unknown)]
		public int ID
		{
			get { return this.id; }
			set { this.id = value; }
		}

		/// <summary>
		/// Gets and sets the property name according to the Exif specification for DCF images.
		/// </summary>
		/// <remarks>
		/// Note: If the ExifTag value specifies the ExifType then ExifProperty.Type is automatically set.
		/// If the ExifTag does not specify the ExifType then ExifProperty.Type is left unchanged.
		/// </remarks>
		[Category("Key")]
		[DisplayName("Exif Tag")]
		[Description("The property name according to the Exif specification for DCF images.")]
		[XmlAttribute("ExifTag"), DefaultValue(ExifTag.Unknown)]
		public ExifTag Tag
		{
			get
			{
				if (Enum.IsDefined(typeof(ExifTag), this.id))
					return (ExifTag)this.id;
				else
					return ExifTag.Unknown;
			}
			set
			{
				if (value != ExifTag.Unknown)
				{
					this.id = (int)value;
					ExifType exifType = ExifDataTypeAttribute.GetExifType(value);
					if (exifType != ExifType.Unknown)
					{
						this.Type = exifType;
					}
				}
			}
		}

		/// <summary>
		/// Gets and sets the EXIF data type.
		/// </summary>
		[Category("Value")]
		[Browsable(false)]
		[XmlAttribute("ExifType"), DefaultValue(ExifType.Unknown)]
		public ExifType Type
		{
			get { return this.type; }
			set { this.type = value; }
		}

		/// <summary>
		/// Gets and sets the EXIF value.
		/// </summary>
		[XmlElement(typeof(Byte))]
		[XmlElement(typeof(Byte[]))]
		[XmlElement(typeof(UInt16))]
		[XmlElement(typeof(UInt16[]))]
		[XmlElement(typeof(UInt32))]
		[XmlElement(typeof(UInt32[]))]
		[XmlElement(typeof(Int32))]
		[XmlElement(typeof(Int32[]))]
		[XmlElement(typeof(String))]
		[XmlElement(typeof(String[]))]
		[XmlElement(typeof(Rational<int>))]
		[XmlElement(typeof(Rational<int>[]))]
		[XmlElement(typeof(Rational<uint>))]
		[XmlElement(typeof(Rational<uint>[]))]
		[XmlElement(typeof(DateTime))]
		[XmlElement(typeof(UnicodeEncoding))]
		[XmlElement(typeof(ExifTagColorSpace))]
		[XmlElement(typeof(ExifTagCleanFaxData))]
		[XmlElement(typeof(ExifTagCompression))]
		[XmlElement(typeof(ExifTagContrast))]
		[XmlElement(typeof(ExifTagCustomRendered))]
		[XmlElement(typeof(ExifTagExposureMode))]
		[XmlElement(typeof(ExifTagExposureProgram))]
		[XmlElement(typeof(ExifTagFileSource))]
		[XmlElement(typeof(ExifTagFillOrder))]
		[XmlElement(typeof(ExifTagFlash))]
		[XmlElement(typeof(ExifTagGainControl))]
		[XmlElement(typeof(ExifTagGpsAltitudeRef))]
		[XmlElement(typeof(ExifTagGpsDifferential))]
		[XmlElement(typeof(ExifTagInkSet))]
		[XmlElement(typeof(ExifTagJPEGProc))]
		[XmlElement(typeof(ExifTagLightSource))]
		[XmlElement(typeof(ExifTagMeteringMode))]
		[XmlElement(typeof(ExifTagOrientation))]
		[XmlElement(typeof(ExifTagPhotometricInterpretation))]
		[XmlElement(typeof(ExifTagPlanarConfiguration))]
		[XmlElement(typeof(ExifTagPredictor))]
		[XmlElement(typeof(ExifTagResolutionUnit))]
		[XmlElement(typeof(ExifTagSampleFormat))]
		[XmlElement(typeof(ExifTagSaturation))]
		[XmlElement(typeof(ExifTagSceneCaptureType))]
		[XmlElement(typeof(ExifTagSceneType))]
		[XmlElement(typeof(ExifTagSensingMethod))]
		[XmlElement(typeof(ExifTagSharpness))]
		[XmlElement(typeof(ExifTagSubjectDistanceRange))]
		[XmlElement(typeof(ExifTagThreshholding))]
		[XmlElement(typeof(ExifTagWhiteBalance))]
		[XmlElement(typeof(ExifTagYCbCrPositioning))]
		[Category("Value")]
		public object Value
		{
			get { return this.value; }
			set { this.value = value; }
		}

		/// <summary>
		/// Gets the formatted text representation of the value.
		/// </summary>
		[DisplayName("Display Value")]
		[Category("Value")]
		public string DisplayValue
		{
			get { return this.FormatValue(); }
		}

		/// <summary>
		/// Gets the formatted text representation of the label.
		/// </summary>
		[DisplayName("Display Name")]
		[Category("Value")]
		public string DisplayName
		{
			get
			{
				if (this.Tag != ExifTag.Unknown)
				{
					string label = Utility.GetDescription(this.Tag);
					if (String.IsNullOrEmpty(label))
					{
						label = this.Tag.ToString("g");
					}
					return label;
				}
				else
				{
					return String.Format("Exif_0x{0:x4}", this.ID);
				}
			}
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// <remarks>
		/// References:
		/// http://www.media.mit.edu/pia/Research/deepview/exif.html
		/// http://en.wikipedia.org/wiki/APEX_system
		/// http://en.wikipedia.org/wiki/Exposure_value
		/// </remarks>
		protected string FormatValue()
		{
			object rawValue = this.Value;
			switch (this.Tag)
			{
				case ExifTag.ISOSpeed:
				{
					if (rawValue is Array)
					{
						Array array = (Array)rawValue;
						if (array.Length < 1 || !(array.GetValue(0) is IConvertible))
						{
							goto default;
						}
						rawValue = array.GetValue(0);
					}

					if (!(rawValue is IConvertible))
					{
						goto default;
					}

					return String.Format("ISO-{0:###0}", Convert.ToDecimal(rawValue));
				}
				case ExifTag.Aperture:
				case ExifTag.MaxAperture:
				{
					// f/x.x
					double fStop = Math.Pow(2.0, Convert.ToDouble(rawValue)/2.0);
					return String.Format("f/{0:#0.0}", fStop);
				}
				case ExifTag.FNumber:
				{
					// f/x.x
					return String.Format("f/{0:#0.0}", Convert.ToDecimal(rawValue));
				}
				case ExifTag.FocalLength:
				case ExifTag.FocalLengthIn35mmFilm:
				{
					return String.Format("{0:#0.0} mm", Convert.ToDecimal(rawValue));
				}
				case ExifTag.ShutterSpeed:
				{
					if (!(rawValue is Rational<int>))
					{
						goto default;
					}

					Rational<int> shutter = (Rational<int>)rawValue;
					if (shutter.Numerator > 0)
					{
						double speed = Math.Pow(2.0, Convert.ToDouble(shutter));
						return String.Format("1/{0:####0} sec", speed);
					}
					else
					{
						double speed = Math.Pow(2.0, -Convert.ToDouble(shutter));
						return String.Format("{0:####0.0#} sec", speed);
					}
				}
				case ExifTag.ExposureTime:
				{
					if (!(rawValue is Rational<uint>))
					{
						goto default;
					}

					Rational<uint> exposure = (Rational<uint>)rawValue;
					if (exposure.Numerator == exposure.Denominator)
					{
						return String.Format("{0} sec", Convert.ToInt32(rawValue));
					}
					if (exposure.Numerator < exposure.Denominator)
					{
						return String.Format("1/{0:####0} sec", 1m/Convert.ToDecimal(rawValue));
					}
					else
					{
						return String.Format("{0:####0.0#} sec", Convert.ToDecimal(rawValue));
					}
				}
				case ExifTag.XResolution:
				case ExifTag.YResolution:
				case ExifTag.ThumbnailXResolution:
				case ExifTag.ThumbnailYResolution:
				case ExifTag.FocalPlaneXResolution:
				case ExifTag.FocalPlaneYResolution:
				{
					return String.Format("{0:###0} dpi", Convert.ToDecimal(rawValue));
				}
				case ExifTag.ImageHeight:
				case ExifTag.ImageWidth:
				case ExifTag.CompressedImageHeight:
				case ExifTag.CompressedImageWidth:
				case ExifTag.ThumbnailHeight:
				case ExifTag.ThumbnailWidth:
				{
					return String.Format("{0:###0} pixels", Convert.ToDecimal(rawValue));
				}
				case ExifTag.SubjectDistance:
				{
					return String.Format("{0:###0.0} m", Convert.ToDecimal(rawValue));
				}
				case ExifTag.ExposureBias:
				case ExifTag.Brightness:
				{
					return String.Format("{0:###0.0} EV", Convert.ToDecimal(rawValue));
				}
				case ExifTag.CompressedBitsPerPixel:
				{
					return String.Format("{0:###0.0} bits", Convert.ToDecimal(rawValue));
				}
				case ExifTag.DigitalZoomRatio:
				{
					return Convert.ToString(rawValue).Replace('/', ':');
				}
				case ExifTag.GpsLatitude:
				case ExifTag.GpsLongitude:
				case ExifTag.GpsDestLatitude:
				case ExifTag.GpsDestLongitude:
				{
					if (!(rawValue is Array))
					{
						goto default;
					}

					Array array = (Array)rawValue;
					if (array.Length < 1)
					{
						return String.Empty;
					}
					else if (array.Length != 3)
					{
						goto default;
					}

					decimal value = Convert.ToDecimal(array.GetValue(0))+ Decimal.Divide(Convert.ToDecimal(array.GetValue(1)), 60m) + Decimal.Divide(Convert.ToDecimal(array.GetValue(2)), 3600m);
					return value.ToString("0.0######");
				}
				case ExifTag.GpsTimeStamp:
				{
					Array array = (Array)rawValue;
					if (array.Length < 1)
					{
						return String.Empty;
					}

					string[] time = new string[array.Length];
					for (int i=0; i<array.Length; i++)
					{
						time[i] = Convert.ToDecimal(array.GetValue(i)).ToString();
					}
					return String.Join(":", time);
				}
				default:
				{
					if (rawValue is Enum)
					{
						string description = Utility.GetDescription((Enum)rawValue);
						if (!String.IsNullOrEmpty(description))
						{
							return description;
						}
					}
					else if (rawValue is Array)
					{
						Array array = (Array)rawValue;
						if (array.Length < 1)
						{
							return String.Empty;
						}

						Type type = array.GetValue(0).GetType();
						if (!type.IsPrimitive || type == typeof(char) || type == typeof(float) || type == typeof(double))
						{
							return Convert.ToString(rawValue);
						}

						//const int ElemsPerRow = 40;
						int charSize = 2*System.Runtime.InteropServices.Marshal.SizeOf(type);
						string format = "{0:X"+(charSize)+"}";
						StringBuilder builder = new StringBuilder(((charSize+1)*array.Length)/*+(2*array.Length/ElemsPerRow)*/);
						for (int i=0; i<array.Length; i++)
						{
							builder.AppendFormat(format, array.GetValue(i));
							//if ((i+1)%ElemsPerRow == 0)
							//{
							//    builder.AppendLine();
							//}
							//else
							{
								builder.Append(" ");
							}
						}
						return builder.ToString();
					}

					return Convert.ToString(rawValue);
				}
			}
		}

		#endregion Methods

		#region Object Overrides

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("{0}: {1}", this.DisplayName, this.DisplayValue);
		}

		#endregion Object Overrides
	}
}
