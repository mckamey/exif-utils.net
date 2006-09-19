using System;
using System.ComponentModel;
using System.Xml.Serialization;

using PhotoLib.Model.Exif.TagValues;

namespace PhotoLib.Model.Exif
{
	/// <summary>
	/// Represents a single EXIF property.
	/// </summary>
	/// <remarks>
	/// Should try to serialize as EXIF+RDF http://www.w3.org/2003/12/exif/
	/// </remarks>
	[Serializable]
	public class ExifProperty
	{
		#region Fields

		private int id = (int)ExifTag.Unknown;
		private ExifType type = ExifType.Raw;
		private object value = null;

		#endregion Fields

		#region Init

		/// <summary>
		/// Ctor.
		/// </summary>
		public ExifProperty()
		{
		}

		/// <summary>
		/// Ctor.
		/// </summary>
		/// <param name="property"></param>
		public ExifProperty(System.Drawing.Imaging.PropertyItem property)
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
					this.id = (int)value;
			}
		}

		/// <summary>
		/// Gets and sets the EXIF data type.
		/// </summary>
		[Category("Value")]
		[Browsable(false)]
		[XmlAttribute("ExifType"), DefaultValue(ExifType.Raw)]
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
		[XmlElement(typeof(System.Text.UnicodeEncoding))]
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
		/// Gets the formatted text representation.
		/// </summary>
		[DisplayName("Display Text")]
		[Category("Value")]
		public string DisplayText
		{
			get { this.FormatValue(); }
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// <remarks>
		/// Referencing: http://www.media.mit.edu/pia/Research/deepview/exif.html
		/// </remarks>
		protected string FormatValue()
		{
			object rawValue = this.Value;
			switch (this.Tag)
			{
			/*
			 shutter speed, fstop, iso setting,
			 metering mode, lightsource, flash, focal length,
			 exposure bias, brightness value, exposure program
			 colorspace, metering mode, lightsource

			 */
				case ExifTag.Aperture:
				{
					try
					{
						Rational<uint> aperture = (Rational<uint>)rawValue;
						double fStop = Math.Pow(2.0, ((double)aperture)/2.0);
						return String.Format("f/{0:#.0}", fStop);
					}
					catch
					{
						goto default;
					}
				}
				case ExifTag.FNumber:
				{
					return String.Format("f/{0:#.0}", Convert.ToDecimal(rawValue));
				}
				case ExifTag.ExposureTime:
				{
					return String.Format("{0} sec");
				}
				default:
				{
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
			System.Text.StringBuilder builder = new System.Text.StringBuilder();
			if (this.Tag != ExifTag.Unknown)
				builder.AppendFormat("{0:g}: ", this.Tag);
			else
				builder.AppendFormat("Exif_0x{0:x4}: ", this.ID);
			builder.Append(this.DisplayText);

			return builder.ToString();
		}

		#endregion Object Overrides
	}
}
