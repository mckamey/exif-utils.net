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
using System.Text;

namespace XmpUtils
{
	public class GpsCoordinate
	{
		#region Constants

		public const string North = "N";
		public const string East = "E";
		public const string West = "W";
		public const string South = "S";

		private static readonly char[] Directions = new char[] { 'N', 'E', 'W', 'S', 'n', 'e', 'w', 's' };

		private const string DecimalFormat = "0.0######";
		private const decimal SecPerMin = 60m;
		private const decimal MinPerDeg = 60m;

		#endregion Constants

		#region Fields

		private Rational<uint> degrees;
		private Rational<uint> minutes;
		private Rational<uint> seconds;
		private string direction;

		#endregion Fields

		#region Properties

		public Rational<uint> Degrees
		{
			get { return this.degrees; }
			set { this.degrees = value; }
		}

		public Rational<uint> Minutes
		{
			get { return this.minutes; }
			set { this.minutes = value; }
		}

		public Rational<uint> Seconds
		{
			get { return this.seconds; }
			set { this.seconds = value; }
		}

		public string Direction
		{
			get { return this.direction; }
			set
			{
				if (String.IsNullOrEmpty(value))
				{
					this.direction = null;
					return;
				}

				if (value.IndexOfAny(GpsCoordinate.Directions) != 0)
				{
					throw new ArgumentException("Invalid GPS direction, must be one of 'N', 'E', 'W', 'S'.");
				}

				this.direction = Char.ToUpperInvariant(value[0]).ToString();
			}
		}

		public decimal Value
		{
			get
			{
				decimal val = Convert.ToDecimal(this.Degrees)+ Decimal.Divide(Convert.ToDecimal(this.Minutes) + Decimal.Divide(Convert.ToDecimal(this.Seconds), SecPerMin), MinPerDeg);
				bool negative =
					StringComparer.OrdinalIgnoreCase.Equals(this.Direction, GpsCoordinate.West) ||
					StringComparer.OrdinalIgnoreCase.Equals(this.Direction, GpsCoordinate.South);

				return (negative == val < Decimal.Zero) ? val : -val;
			}
		}

		#endregion Properties

		#region Methods

		public void SetDegrees(decimal deg)
		{
			this.Degrees = Rational<uint>.Approximate(deg);
		}

		public void SetMinutes(decimal min)
		{
			this.Minutes = Rational<uint>.Approximate(min);
		}

		public void SetSeconds(decimal sec)
		{
			this.Seconds = Rational<uint>.Approximate(sec);
		}

		public static GpsCoordinate FromDecimal(decimal val)
		{
			decimal deg = Decimal.Truncate(val);
			decimal min = Decimal.Remainder(val, Decimal.One) * MinPerDeg;
			decimal sec = Decimal.Remainder(min, Decimal.One) * SecPerMin;
			min = Decimal.Truncate(min);

			return GpsCoordinate.FromDecimal(deg, min, sec);
		}

		public static GpsCoordinate FromDecimal(decimal deg, decimal min, decimal sec)
		{
			GpsCoordinate gps = new GpsCoordinate();

			gps.SetDegrees(deg);
			gps.SetMinutes(min);
			gps.SetSeconds(sec);

			return gps;
		}

		public static GpsCoordinate Parse(string value)
		{
			GpsCoordinate gps;
			if (!GpsCoordinate.TryParse(value, out gps))
			{
				throw new ArgumentException("Invalid GpsCoordinate");
			}

			return gps;
		}

		public static bool TryParse(string value, out GpsCoordinate gps)
		{
			if (String.IsNullOrEmpty(value))
			{
				gps = null;
				return false;
			}

			int index = 0,
				last = 0,
				length = value.Length;
			char ch;
			decimal deg, min, sec;

			#region parse degrees

			for (; index < length; index++)
			{
				ch = value[index];

				if (ch != '.' &&
					ch != '-' &&
					ch != '+' &&
					(ch < '0' || ch > '9'))
				{
					break;
				}
			}

			if (!Decimal.TryParse(value.Substring(last, index-last), out deg))
			{
				gps = null;
				return false;
			}

			for (; index < length; index++)
			{
				ch = value[index];
				if (ch != ',' &&
					ch != '\u00B0' &&
					ch != ' ')
				{
					break;
				}
			}
			last = index;

			#endregion parse degrees

			#region parse minutes

			if (last+1 >= length)
			{
				gps = GpsCoordinate.FromDecimal(deg);
				if (last < length &&
					value.IndexOfAny(GpsCoordinate.Directions, last, 1) == last)
				{
					gps.Direction = value.Substring(last);
				}
				return true;
			}

			for (; index < length; index++)
			{
				ch = value[index];

				if (ch != '.' &&
					ch != '-' &&
					ch != '+' &&
					(ch < '0' || ch > '9'))
				{
					break;
				}
			}

			if (!Decimal.TryParse(value.Substring(last, index-last), out min))
			{
				gps = null;
				return false;
			}

			for (; index < length; index++)
			{
				ch = value[index];
				if (ch != ',' &&
					ch != '\'' &&
					ch != ' ')
				{
					break;
				}
			}
			last = index;

			#endregion parse minutes

			#region parse seconds

			if (last+1 >= length)
			{
				gps = GpsCoordinate.FromDecimal(deg, min, 0m);
				if (last < length &&
					value.IndexOfAny(GpsCoordinate.Directions, last, 1) == last)
				{
					gps.Direction = value.Substring(last);
				}
				return true;
			}

			for (; index < length; index++)
			{
				ch = value[index];

				if (ch != '.' &&
					ch != '-' &&
					ch != '+' &&
					(ch < '0' || ch > '9'))
				{
					break;
				}
			}

			if (!Decimal.TryParse(value.Substring(last, index-last), out sec))
			{
				gps = null;
				return false;
			}

			for (; index < length; index++)
			{
				ch = value[index];
				if (ch != ',' &&
					ch != '"' &&
					ch != ' ')
				{
					break;
				}
			}
			last = index;

			#endregion parse seconds

			if (last+1 >= length)
			{
				gps = GpsCoordinate.FromDecimal(deg, min, sec);
				if (last < length &&
					value.IndexOfAny(GpsCoordinate.Directions, last, 1) == last)
				{
					gps.Direction = value.Substring(last);
				}
				return true;
			}

			gps = null;
			return false;
		}

		#endregion Methods

		#region Object Overrides

		/// <summary>
		/// Formats the GPS coordinate as an XMP GPSCoordinate string
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this.ToString(null);
		}

		/// <summary>
		/// Formats the GPS coordinate as an XMP GPSCoordinate string
		/// </summary>
		/// <returns></returns>
		/// <remarks>
		/// Accepts "X", "x" for XMP style formatting, or "D", "d" for degree style formatting, or "N", "n" for numeric
		/// </remarks>
		public string ToString(string format)
		{
			if (String.IsNullOrEmpty(format))
			{
				format = "X";
			}
			else
			{
				switch(format)
				{
					case "X":
					case "x":
					case "D":
					case "d":
					{
						format = format.ToUpperInvariant();
						break;
					}
					case "N":
					case "n":
					{
						return this.Value.ToString(GpsCoordinate.DecimalFormat);
					}
					default:
					{
						throw new ArgumentException("format");
					}
				}
			}

			StringBuilder gps = new StringBuilder();

			if (this.Degrees.IsEmpty || this.Minutes.IsEmpty || this.Seconds.IsEmpty || this.Degrees.Denominator != 1)
			{
				// use full decimal formatting
				if (String.IsNullOrEmpty(this.Direction))
				{
					gps.Append(this.Value.ToString(GpsCoordinate.DecimalFormat));
				}
				else
				{
					gps.Append(Math.Abs(this.Value).ToString(GpsCoordinate.DecimalFormat));
				}
				if (format == "D")
				{
					gps.Append("\u00B0");
				}
				if (!String.IsNullOrEmpty(this.Direction))
				{
					if (format == "D")
					{
						gps.Append(' ');
					}
					gps.Append(this.Direction);
				}
				return gps.ToString();
			}

			gps.Append(this.Degrees.Numerator);
			switch(format)
			{
				case "D":
				{
					gps.Append("\u00B0 ");
					break;
				}
				case "X":
				{
					gps.Append(',');
					break;
				}
			}

			// DD,MM.mmk
			if (this.Minutes.Denominator != 1 || this.Seconds.Denominator != 1)
			{
				decimal MMmm = Convert.ToDecimal(this.Minutes) + Decimal.Divide(Convert.ToDecimal(this.Seconds), SecPerMin);
				gps.Append(MMmm.ToString(GpsCoordinate.DecimalFormat));
				if (format == "D")
				{
					gps.Append('\'');
				}
				if (!String.IsNullOrEmpty(this.Direction))
				{
					if (format == "D")
					{
						gps.Append(' ');
					}
					gps.Append(this.Direction);
				}
				return gps.ToString();
			}

			// DD,MM,SSk
			gps.Append(this.Minutes.Numerator);
			switch (format)
			{
				case "D":
				{
					gps.Append("' ");
					break;
				}
				case "X":
				{
					gps.Append(',');
					break;
				}
			}

			gps.Append(this.Seconds.Numerator);
			if (format == "D")
			{
				gps.Append('"');
			}

			if (!String.IsNullOrEmpty(this.Direction))
			{
				if (format == "D")
				{
					gps.Append(' ');
				}
				gps.Append(this.Direction);
			}

			return gps.ToString();
		}

		#endregion Object Overrides
	}
}
