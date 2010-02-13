//#define DIAGNOSTICS

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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Media.Imaging;

using XmpUtils.Xmp.Schemas;
using XmpUtils.Xmp.ValueTypes;
using XmpUtils.Xmp.ValueTypes.ExifTagValues;

namespace XmpUtils.Xmp
{
	/// <summary>
	/// Extracts XMP properties out of an image file
	/// </summary>
	internal class XmpExtractor
	{
		#region Constants

		private const string XmpDateFormat = "yyyy'-'MM'-'ddTHH':'mm':'ss.FFFK";
		private static readonly string[] ExifDateFormats =
		{
			"yyyy':'MM':'dd HH':'mm':'ss",
			"yyyy':'MM':'dd"
		};

		#endregion Constants

		#region Aggregation Methods

		public IEnumerable<XmpProperty> Extract(string filename, IEnumerable<Enum> schemas)
		{
			using (Stream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				return this.Extract(stream, schemas);
			}
		}

		public IEnumerable<XmpProperty> Extract(Stream stream, IEnumerable<Enum> schemas)
		{
			BitmapMetadata metadata = this.LoadMetadata(stream);

			if (schemas != null && schemas.Any())
			{
				return this.ExtractSchemas(metadata, schemas).ToList();
			}

			// none means all
			IEnumerable value = this.ProcessMetadata(metadata, "/", 0);
			return this.AggregateProperties(value).ToList();
		}

		private IEnumerable<XmpProperty> ExtractSchemas(BitmapMetadata metadata, IEnumerable<Enum> schemas)
		{
			foreach (Enum schema in schemas)
			{
				string name = null;
				object value = null;

				foreach (string query in this.GetQueryForSchema(schema))
				{
					if (String.IsNullOrEmpty(query))
					{
						continue;
					}

					value = metadata.GetQuery(query);
					if (value == null)
					{
						continue;
					}

					name = query;
					break;
				}

				if (String.IsNullOrEmpty(name))
				{
					continue;
				}
				name = name.Substring(name.LastIndexOf('/')+1);

#if DIAGNOSTICS
				Console.WriteLine("{0} => {1}: {2}", name, value != null ? value.GetType().Name : "null", Convert.ToString(value));
#endif

				if (value is BitmapMetadata)
				{
					value = this.ProcessMetadata((BitmapMetadata)value, '/'+name, 0);
				}
				else if (value is BitmapMetadataBlob)
				{
					value = ((BitmapMetadataBlob)value).GetBlobValue();
				}

				if (value == null)
				{
					continue;
				}

				yield return XmpPropertyCollection.ProcessValue(new XmpProperty
				{
					Schema = schema,
					Value = value
				});
			}
		}

		private IEnumerable<XmpProperty> AggregateProperties(IEnumerable value)
		{
			if (value == null)
			{
				yield break;
			}

			if (value is IDictionary<string, object>)
			{
				// unwrap generic container
				value = ((IDictionary<string, object>)value).Values;
			}

			// filter out any values which are not enumerable
			foreach (IEnumerable item in value.OfType<IEnumerable>())
			{
				// each item is either a list of properties or a sequence holding a list of properties
				IEnumerable<XmpProperty> properties =
					(item is IEnumerable<XmpProperty>) ?
					(IEnumerable<XmpProperty>)item :
					this.AggregateProperties(item);

				// aggregate into single sequence
				foreach (XmpProperty property in properties)
				{
					yield return XmpPropertyCollection.ProcessValue(property);
				}
			}
		}

		private IEnumerable<string> GetQueryForSchema(Enum schema)
		{
			if (schema is ExifSchema)
			{
				yield return "/app1/{ushort=0}/exif/{ushort="+schema.ToString("D")+"}";
				yield return "/app1/ifd/exif/{ushort="+schema.ToString("D")+"}";
			}
			else if (schema is ExifTiffSchema)
			{
				yield return "/app1/{ushort=0}/{ushort="+schema.ToString("D")+"}";
				yield return "/app1/ifd/{ushort="+schema.ToString("D")+"}";
			}

			string name;
			FieldInfo fieldInfo;
			Type type = AttributeUtility.GetEnumInfo(schema, out name, out fieldInfo);

			// check for namespace on property enum, then on type
			XmpNamespaceAttribute xns = AttributeUtility
				.FindAttributes<XmpNamespaceAttribute>(fieldInfo, type)
				.FirstOrDefault() ?? XmpNamespaceAttribute.Empty;

			// check for property info on property enum only
			XmpPropertyAttribute xp = AttributeUtility
				.FindAttributes<XmpPropertyAttribute>(fieldInfo)
				.FirstOrDefault();

			if (xp != null && !String.IsNullOrEmpty(xp.Name))
			{
				name = xp.Name;
			}

			if (xns == null || !xns.Prefixes.Any())
			{
				yield return "/xmp/"+name;
			}
			else
			{
				foreach (string prefix in xns.Prefixes)
				{
					yield return "/xmp/"+prefix+':'+name;
				}
			}
		}

		#endregion Aggregation Methods

		#region Extraction Methods

		private IEnumerable ProcessMetadata(BitmapMetadata metadata, string objName, int depth)
		{
#if DIAGNOSTICS
			Console.Write(new String('\t', depth));
			Console.WriteLine("{0} => {1}", objName, metadata.Format);
#endif

			switch (metadata.Format)
			{
				case "xmp":
				{
					return this.ProcessXmp(metadata, 0.8m, depth+1);
				}
				case "exif":
				{
					return this.ProcessAsXmp(metadata, typeof(ExifSchema), 0.2m, depth+1);
				}
				case "gps":
				{
					IEnumerable<XmpProperty> gps = this.ProcessAsXmp(metadata, typeof(ExifSchema), 0.2m, depth+1);
					return this.ProcessGps(gps, 0.2m);
				}
				case "ifd":
				{
					return this.ProcessAsXmp(metadata, typeof(ExifTiffSchema), 0.4m, depth+1);
				}
				// TODO: build out IPTC properties
				//case "iptc":
				//{
				//    return this.ProcessAsXmp(metadata, typeof(IptcSchema), 0.6m, depth+1);
				//}
				case "thumb":
				case "chrominance":
				case "luminance":
				{
					// these are suppressed in XMP
					return null;
				}
				case "xmpbag":
				case "xmpseq":
				{
					return this.ProcessArray(metadata, depth+1);
				}
				case "xmpalt":
				case "xmpstruct":
				default:
				{
					return this.ProcessBlock(metadata, depth+1);
				}
			}
		}

		private Dictionary<string, object> ProcessBlock(BitmapMetadata metadata, int depth)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();

			foreach (string name in this.GetNamesSafely(metadata))
			{
				object value = metadata.GetQuery(name);

#if DIAGNOSTICS
				Console.Write(new String('\t', depth));
				Console.WriteLine("{0} => {1}: {2}", name, value != null ? value.GetType().Name : "null", Convert.ToString(value));
#endif

				if (value is BitmapMetadata)
				{
					value = this.ProcessMetadata((BitmapMetadata)value, name, depth);
				}
				else if (value is BitmapMetadataBlob)
				{
					value = ((BitmapMetadataBlob)value).GetBlobValue();
				}

				if (value == null)
				{
					continue;
				}

				string key = name.TrimStart('/');
				key = key.Substring(key.LastIndexOf(':')+1);
				dictionary[key] = value;
			}

			return dictionary;
		}

		private IEnumerable ProcessArray(BitmapMetadata metadata, int depth)
		{
			ArrayList array = new ArrayList();

			foreach (string name in this.GetNamesSafely(metadata))
			{
				object value = metadata.GetQuery(name);

#if DIAGNOSTICS
				Console.Write(new String('\t', depth));
				Console.WriteLine("{0} => {1}: {2}", name, value != null ? value.GetType().Name : "null", Convert.ToString(value));
#endif

				if (value is BitmapMetadata)
				{
					value = this.ProcessMetadata((BitmapMetadata)value, name, depth);
				}
				else if (value is BitmapMetadataBlob)
				{
					value = ((BitmapMetadataBlob)value).GetBlobValue();
				}

				if (value == null)
				{
					continue;
				}

				array.Add(value);
			}

			return array;
		}

		private IEnumerable<XmpProperty> ProcessXmp(BitmapMetadata metadata, decimal priority, int depth)
		{
			foreach (string name in this.GetNamesSafely(metadata))
			{
				// http://msdn.microsoft.com/en-us/library/ee719796(VS.85).aspx
				// http://search.cpan.org/
				object value = metadata.GetQuery(name);

#if DIAGNOSTICS
				Console.Write(new String('\t', depth));
				Console.WriteLine("{0} => {1}: {2}", name, value != null ? value.GetType().Name : "null", Convert.ToString(value));
#endif

				if (value is BitmapMetadata)
				{
					value = this.ProcessMetadata((BitmapMetadata)value, name, depth);
				}
				else if (value is BitmapMetadataBlob)
				{
					value = ((BitmapMetadataBlob)value).GetBlobValue();
				}

				// TODO: evaluate if nested properties make sense here
				IEnumerable<XmpProperty> valueProps = value as IEnumerable<XmpProperty>;
				if (valueProps != null)
				{
					foreach (XmpProperty prop in valueProps)
					{
						yield return prop;
					}
					continue;
				}

				Enum schema = (Enum)XmpNamespaceUtility.Instance.ParsePrefix(name.TrimStart('/'));
				if (schema == null)
				{
					continue;
				}

				if (value == null)
				{
					continue;
				}

				yield return new XmpProperty
				{
					Schema = schema,
					Value = value,
					Priority = priority
				};
			}
		}

		private IEnumerable<XmpProperty> ProcessAsXmp(BitmapMetadata metadata, Type enumType, decimal priority, int depth)
		{
			foreach (string name in this.GetNamesSafely(metadata))
			{
				object value = metadata.GetQuery(name);

#if DIAGNOSTICS
				Console.Write(new String('\t', depth));
				Console.WriteLine("{0} => {1}: {2}", name, value != null ? value.GetType().Name : "null", Convert.ToString(value));
#endif

				if (value is BitmapMetadata)
				{
					value = this.ProcessMetadata((BitmapMetadata)value, name, depth);
				}
				else if (value is BitmapMetadataBlob)
				{
					value = ((BitmapMetadataBlob)value).GetBlobValue();
				}

				IEnumerable<XmpProperty> valueProps = value as IEnumerable<XmpProperty>;
				if (valueProps != null)
				{
					foreach (XmpProperty prop in valueProps)
					{
						yield return prop;
					}
					continue;
				}

				Enum schema = this.ParseSchema(enumType, name);
				if (schema == null)
				{
					continue;
				}

				XmpProperty property = new XmpProperty
				{
					Schema = schema,
					Priority = priority
				};

				property.Value = this.ProcessValue(property, value);

				if (property.Value == null)
				{
					continue;
				}

				yield return property;
			}
		}

		/// <summary>
		/// Converts EXIF / TIFF values into XMP style
		/// </summary>
		/// <param name="property"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		private object ProcessValue(XmpProperty property, object value)
		{
			if (value == null)
			{
				return value;
			}

			if (property.ValueType is XmpBasicType)
			{
				switch ((XmpBasicType)property.ValueType)
				{
					case XmpBasicType.Date:
					{
						DateTime date;

						Array array = value as Array;
						if (array != null && array.Length == 3)
						{
							try
							{
								var seconds =
									60m * (60m * Convert.ToDecimal(this.ProcessRational(property, array.GetValue(0))) +
									Convert.ToDecimal(this.ProcessRational(property, array.GetValue(1)))) +
									Convert.ToDecimal(this.ProcessRational(property, array.GetValue(2)));

								date = DateTime.MinValue.AddSeconds(Convert.ToDouble(seconds));
								value = date.ToString(XmpDateFormat);
							}
							catch { }
						}
						else if (DateTime.TryParseExact(
								Convert.ToString(value),
								ExifDateFormats,
								DateTimeFormatInfo.InvariantInfo,
								DateTimeStyles.AssumeLocal,
								out date))
						{
							// clean up to ISO-8601
							value = date.ToString(XmpDateFormat);
						}
						break;
					}
					case XmpBasicType.LangAlt:
					case XmpBasicType.Text:
					case XmpBasicType.ProperName:
					{
						if (value is byte[])
						{
							if (property.Schema is ExifSchema &&
								((ExifSchema)property.Schema) == ExifSchema.GPSVersionID)
							{
								// GPSVersionID represents version as byte[]
								value = String.Join(".", ((byte[])value).Select(b => b.ToString()).ToArray());
							}
							else
							{
								value = new String(Encoding.UTF8.GetChars((byte[])value));
							}

							value = this.TrimNullTerm((string)value);
						}
						else if (!(value is string) && value is IEnumerable)
						{
							var strList = ((IEnumerable)value).OfType<string>().Select(str => this.TrimNullTerm(str)).Where(str => !String.IsNullOrEmpty(str));

							if (property.Quantity == XmpQuantity.Single)
							{
								value = strList.FirstOrDefault();
							}
							else
							{
								value = strList;
							}
						}
						else if (value is string)
						{
							value = this.TrimNullTerm((string)value);
						}
						break;
					}
				}
			}
			else if (property.ValueType is ExifType)
			{
				switch ((ExifType)property.ValueType)
				{
					case ExifType.GpsCoordinate:
					{
						Array array = value as Array;
						if (array != null && array.Length == 3)
						{
							try
							{
								GpsCoordinate gps = new GpsCoordinate();
								gps.Degrees = (Rational<uint>)this.ProcessRational(property, array.GetValue(0));
								gps.Minutes = (Rational<uint>)this.ProcessRational(property, array.GetValue(1));
								gps.Seconds = (Rational<uint>)this.ProcessRational(property, array.GetValue(2));
								value = gps.ToString("X");
							}
							catch { }
						}
						break;
					}
					case ExifType.Rational:
					{
						Array array = value as Array;
						if (array == null)
						{
							value = Convert.ToString(this.ProcessRational(property, value));
						}
						else
						{
							string[] strArray = new string[array.Length];
							for (int i=0; i<array.Length; i++)
							{
								strArray[i] = Convert.ToString(this.ProcessRational(property, array.GetValue(i)));
							}
							value = array;
						}
						break;
					}
					case ExifType.Flash:
					{
						ExifTagFlash flash;
						if (value is ushort)
						{
							flash = (ExifTagFlash)value;
						}
						else if (value is string)
						{
							try
							{
								flash = (ExifTagFlash)Enum.Parse(typeof(ExifTagFlash), Convert.ToString(value));
							}
							catch
							{
								break;
							}
						}
						else
						{
							break;
						}

						value = new Dictionary<string, object>
						{
							{ "Fired", Convert.ToString((flash & ExifTagFlash.FlashFired) == ExifTagFlash.FlashFired) },
							{ "Return", (int)(flash & (ExifTagFlash.ReturnNotDetected|ExifTagFlash.ReturnDetected)) >> 1 },
							{ "Mode", (int)(flash & (ExifTagFlash.ModeOn|ExifTagFlash.ModeOff|ExifTagFlash.ModeAuto)) >> 3 },
							{ "Function", Convert.ToString((flash & ExifTagFlash.NoFlashFunction) == ExifTagFlash.NoFlashFunction) },
							{ "RedEyeMode", Convert.ToString((flash & ExifTagFlash.RedEyeReduction) == ExifTagFlash.RedEyeReduction) }
						};
						break;
					}
				}
			}

			return value;
		}

		/// <summary>
		/// Consolidates EXIF GPS values into XMP style
		/// </summary>
		/// <param name="gps"></param>
		/// <param name="priority"></param>
		/// <returns></returns>
		private IEnumerable<XmpProperty> ProcessGps(IEnumerable<XmpProperty> gps, decimal priority)
		{
			GpsCoordinate latitude = null, longitude = null, destLatitude = null, destLongitude = null;
			string latitudeRef = null, longitudeRef = null, destLatitudeRef = null, destLongitudeRef = null;

			foreach (XmpProperty property in gps)
			{
				if (!(property.Schema is ExifSchema))
				{
					yield return property;
					continue;
				}

				switch ((ExifSchema)property.Schema)
				{
					case ExifSchema.GPSLatitude:
					{
						GpsCoordinate.TryParse(Convert.ToString(property.Value), out latitude);
						break;
					}
					case ExifSchema.GPSLatitudeRef:
					{
						latitudeRef = Convert.ToString(property.Value);
						break;
					}

					case ExifSchema.GPSLongitude:
					{
						GpsCoordinate.TryParse(Convert.ToString(property.Value), out longitude);
						break;
					}
					case ExifSchema.GPSLongitudeRef:
					{
						longitudeRef = Convert.ToString(property.Value);
						break;
					}

					case ExifSchema.GPSDestLatitude:
					{
						GpsCoordinate.TryParse(Convert.ToString(property.Value), out destLatitude);
						break;
					}
					case ExifSchema.GPSDestLatitudeRef:
					{
						destLatitudeRef = Convert.ToString(property.Value);
						break;
					}

					case ExifSchema.GPSDestLongitude:
					{
						GpsCoordinate.TryParse(Convert.ToString(property.Value), out destLongitude);
						break;
					}
					case ExifSchema.GPSDestLongitudeRef:
					{
						destLongitudeRef = Convert.ToString(property.Value);
						break;
					}

					case ExifSchema.GPSDateStamp:
					case ExifSchema.GPSTimeStamp:

					case ExifSchema.GPSAltitude:
					case ExifSchema.GPSAltitudeRef:

					case ExifSchema.GPSDestBearing:
					case ExifSchema.GPSDestBearingRef:

					case ExifSchema.GPSDestDistance:
					case ExifSchema.GPSDestDistanceRef:

					case ExifSchema.GPSImgDirection:
					case ExifSchema.GPSImgDirectionRef:

					case ExifSchema.GPSSpeed:
					case ExifSchema.GPSSpeedRef:

					case ExifSchema.GPSTrack:
					case ExifSchema.GPSTrackRef:
					default:
					{
						// TODO: process other dual properties
						yield return property;
						break;
					}
				}
			}

			if (latitude != null)
			{
				if (!String.IsNullOrEmpty(latitudeRef))
				{
					latitude.Direction = latitudeRef;
				}

				yield return new XmpProperty
				{
					Schema = ExifSchema.GPSLatitude,
					Value = latitude.ToString("X"),
					Priority = priority
				};
			}

			if (longitude != null)
			{
				if (!String.IsNullOrEmpty(longitudeRef))
				{
					longitude.Direction = longitudeRef;
				}

				yield return new XmpProperty
				{
					Schema = ExifSchema.GPSLongitude,
					Value = longitude.ToString("X"),
					Priority = priority
				};
			}

			if (destLatitude != null)
			{
				if (!String.IsNullOrEmpty(destLatitudeRef))
				{
					destLatitude.Direction = destLatitudeRef;
				}

				yield return new XmpProperty
				{
					Schema = ExifSchema.GPSDestLatitude,
					Value = destLatitude.ToString("X"),
					Priority = priority
				};
			}

			if (destLongitude != null)
			{
				if (!String.IsNullOrEmpty(destLongitudeRef))
				{
					destLongitude.Direction = destLongitudeRef;
				}

				yield return new XmpProperty
				{
					Schema = ExifSchema.GPSDestLongitude,
					Value = destLongitude.ToString("X"),
					Priority = priority
				};
			}
		}

		private string TrimNullTerm(string value)
		{
			// safeguard against null terminated byte[]
			int end = value.IndexOf('\0');
			if (end >= 0)
			{
				return value.Substring(0, end);
			}
			return value;
		}

		/// <summary>
		/// Converts EXIF Rationals into XMP style
		/// </summary>
		/// <param name="property"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		private object ProcessRational(XmpProperty property, object value)
		{
			if (value == null)
			{
				return null;
			}

			switch (Type.GetTypeCode(value.GetType()))
			{
				case TypeCode.UInt64:
				{
					ulong rational = (ulong)value;
					uint numerator = (uint)(rational & 0xFFFFFFFFL);
					uint denominator = (uint)(rational >> 32);
					value = new Rational<uint>(numerator, denominator, false);
					break;
				}
				case TypeCode.Int64:
				{
					long rational = (long)value;
					int numerator = (int)(rational & 0xFFFFFFFFL);
					int denominator = (int)(rational >> 32);
					value = new Rational<int>(numerator, denominator, false);
					break;
				}
				case TypeCode.UInt32:
				{
					uint rational = (uint)value;
					ushort numerator = (ushort)(rational & 0xFFFF);
					ushort denominator = (ushort)(rational >> 16);
					value = new Rational<ushort>(numerator, denominator, false);
					break;
				}
				case TypeCode.Int32:
				{
					int rational = (int)value;
					short numerator = (short)(rational & 0xFFFF);
					short denominator = (short)(rational >> 16);
					value = new Rational<short>(numerator, denominator, false);
					break;
				}
				case TypeCode.UInt16:
				{
					ushort rational = (ushort)value;
					byte numerator = (byte)(rational & 0xFF);
					byte denominator = (byte)(rational >> 8);
					value = new Rational<byte>(numerator, denominator, false);
					break;
				}
				case TypeCode.Int16:
				{
					short rational = (short)value;
					sbyte numerator = (sbyte)(rational & 0xFF);
					sbyte denominator = (sbyte)(rational >> 8);
					value = new Rational<sbyte>(numerator, denominator, false);
					break;
				}
				default:
				{
					break;
				}
			}

			return value;
		}

		private Enum ParseSchema(Type enumType, string name)
		{
			name = name.TrimStart('/', '{').TrimEnd('}');
			string[] parts = name.Split(new char[] { '=' }, 2, StringSplitOptions.RemoveEmptyEntries);

			if (parts.Length < 2)
			{
				return null;
			}

			switch (parts[0])
			{
				case "byte":
				{
					byte value;

					if (!Byte.TryParse(parts[1], out value) ||
						!Enum.IsDefined(enumType, value))
					{
						return null;
					}

					return (Enum)Enum.ToObject(enumType, value);
				}
				case "sbyte":
				{
					sbyte value;

					if (!SByte.TryParse(parts[1], out value) ||
						!Enum.IsDefined(enumType, value))
					{
						return null;
					}

					return (Enum)Enum.ToObject(enumType, value);
				}
				case "ushort":
				{
					ushort value;

					if (!UInt16.TryParse(parts[1], out value) ||
						!Enum.IsDefined(enumType, value))
					{
						return null;
					}

					return (Enum)Enum.ToObject(enumType, value);
				}
				case "short":
				{
					short value;

					if (!Int16.TryParse(parts[1], out value) ||
						!Enum.IsDefined(enumType, value))
					{
						return null;
					}

					return (Enum)Enum.ToObject(enumType, value);
				}
				case "uint":
				{
					uint value;

					if (!UInt32.TryParse(parts[1], out value) ||
						!Enum.IsDefined(enumType, value))
					{
						return null;
					}

					return (Enum)Enum.ToObject(enumType, value);
				}
				case "int":
				{
					int value;

					if (!Int32.TryParse(parts[1], out value) ||
						!Enum.IsDefined(enumType, value))
					{
						return null;
					}

					return (Enum)Enum.ToObject(enumType, value);
				}
				case "ulong":
				{
					ulong value;

					if (!UInt64.TryParse(parts[1], out value) ||
						!Enum.IsDefined(enumType, value))
					{
						return null;
					}

					return (Enum)Enum.ToObject(enumType, value);
				}
				default:
				case "long":
				{
					long value;

					if (!Int64.TryParse(parts[1], out value) ||
						!Enum.IsDefined(enumType, value))
					{
						return null;
					}

					return (Enum)Enum.ToObject(enumType, value);
				}
			}
		}

		#endregion Extraction Methods

		#region IO Methods

		private BitmapMetadata LoadMetadata(Stream stream)
		{
			BitmapDecoder decoder = BitmapDecoder.Create(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnDemand);

			if (decoder.Frames[0] != null)
			{
				return decoder.Frames[0].Metadata as BitmapMetadata;
			}

			return null;
		}

		/// <summary>
		/// Workaround for COM null reference exception in Metadata iteration
		/// </summary>
		/// <param name="metadata"></param>
		/// <returns></returns>
		private IEnumerable<string> GetNamesSafely(BitmapMetadata metadata)
		{
			IEnumerator<string> enumerator = ((IEnumerable<string>)metadata).GetEnumerator();

			bool hasMore;
			try
			{
				hasMore = enumerator.MoveNext();
			}
			catch
			{
				hasMore = false;
			}

			while (hasMore)
			{
				yield return enumerator.Current;

				try
				{
					hasMore = enumerator.MoveNext();
				}
				catch
				{
					hasMore = false;
				}
			}
		}

		#endregion IO Methods
	}
}
