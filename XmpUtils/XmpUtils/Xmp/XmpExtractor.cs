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
using System.Text;
using System.Windows.Media.Imaging;

using XmpUtils.Xmp.Schemas;
using XmpUtils.Xmp.ValueTypes;

namespace XmpUtils.Xmp
{
	/// <summary>
	/// Extracts XMP properties out of an image file
	/// </summary>
	public class XmpExtractor
	{
		#region Constants

		private const string XmpDateFormat = "yyyy'-'MM'-'ddTHH':'mm':'ss.FFFK";
		private const string ExifDateFormat = "yyyy':'MM':'dd HH':'mm':'ss";

		#endregion Constants

		#region Aggregation Methods

		public IEnumerable<XmpProperty> Extract(string filename)
		{
			using (Stream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				return this.Extract(stream);
			}
		}

		public IEnumerable<XmpProperty> Extract(Stream stream)
		{
			BitmapMetadata metadata = this.LoadMetadata(stream);

			object value = this.ProcessMetadata(metadata, "/", 0);

			return this.AggregateProperties(value);
		}

		private IEnumerable<XmpProperty> AggregateProperties(object value)
		{
			IEnumerable list = value as IEnumerable;
			if (list == null)
			{
				yield break;
			}

			foreach (object item in list)
			{
				IEnumerable<XmpProperty> properties = item as IEnumerable<XmpProperty>;
				if (properties == null)
				{
					foreach (XmpProperty property in this.AggregateProperties(item))
					{
						yield return property;
					}
				}
				else
				{
					foreach (XmpProperty property in properties)
					{
						yield return property;
					}
				}
			}
		}

		#endregion Aggregation Methods

		#region Extraction Methods

		private object ProcessMetadata(BitmapMetadata metadata, string objName, int depth)
		{
#if DEBUG
			Console.Write(new String('\t', depth));
			Console.WriteLine("{0} => {1}", objName, metadata.Format);
#endif

			switch (metadata.Format)
			{
				case "xmpbag":
				case "xmpseq":
				{
					return this.ProcessArray(metadata, depth+1);
				}
				case "xmpalt":
				case "xmpstruct":
				{
					return this.ProcessXmpStruct(metadata, depth+1);
				}
				case "xmp":
				{
					return this.ProcessXmp(metadata, 0.8m, depth+1);
				}
				case "exif":
				case "gps":
				{
					return this.ProcessBlock(metadata, typeof(ExifSchema), 0.2m, depth+1);
				}
				case "ifd":
				{
					return this.ProcessBlock(metadata, typeof(ExifTiffSchema), 0.4m, depth+1);
				}
				// TODO: build out IPTC properties
				//case "iptc":
				//{
				//    return ProcessBlock(metadata, typeof(IptcSchema), 0.6m, depth+1);
				//}
				case "thumb":
				case "chrominance":
				case "luminance":
				{
					// these are suppressed in XMP
					return null;
				}
				default:
				{
					return this.ProcessArray(metadata, depth+1);
				}
			}
		}

		private IEnumerable<XmpProperty> ProcessBlock(BitmapMetadata metadata, Type enumType, decimal priority, int depth)
		{
			List<XmpProperty> properties = new List<XmpProperty>();

			foreach (string name in metadata)
			{
				object value = metadata.GetQuery(name);
				if (value == null)
				{
					continue;
				}

#if DEBUG
				Console.Write(new String('\t', depth));
				Console.WriteLine("{0} => {1}: {2}", name, value.GetType(), Convert.ToString(value));
#endif

				if (value is BitmapMetadata)
				{
					value = this.ProcessMetadata((BitmapMetadata)value, name, depth);
					IDictionary<string, object> dictionary = value as IDictionary<string, object>;
					if (dictionary != null)
					{
						value =
							(from key in dictionary.Keys
							 let val = dictionary[key]
							 select new
							 {
								 Key = key.Substring(key.LastIndexOf(':')+1),
								 Value = val
							 }).ToDictionary(item => item.Key, item => item.Value);
					}
				}

				if (value is BitmapMetadataBlob)
				{
					value = ((BitmapMetadataBlob)value).GetBlobValue();
				}

				if (value == null)
				{
					continue;
				}

				IEnumerable<XmpProperty> valueProps = value as IEnumerable<XmpProperty>;
				if (valueProps != null)
				{
					properties.AddRange(valueProps);
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

				if (property.Value != null)
				{
					properties.Add(property);
				}
			}

			return properties;
		}

		private object ProcessValue(XmpProperty property, object value)
		{
			switch (Type.GetTypeCode(property.DataType))
			{
				case TypeCode.String:
				{
					if (property.Quantity != XmpQuantity.Single &&
						!(value is Array))
					{
						value = new object[] { value };
					}

					if (property.Quantity == XmpQuantity.Single &&
						value is byte[] &&
						property.ValueType is XmpBasicType &&
						((XmpBasicType)property.ValueType) == XmpBasicType.Text)
					{
						value = new String(Encoding.UTF8.GetChars((byte[])value));
					}

					if (property.ValueType is ExifType)
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
									for (int i=0; i<array.Length; i++)
									{
										array.SetValue(Convert.ToString(this.ProcessRational(property, array.GetValue(i))), i);
									}
									value = array;
								}
								break;
							}
						}
					}
					break;
				}
				case TypeCode.DateTime:
				{
					DateTime date;
					if (DateTime.TryParseExact(
							Convert.ToString(value),
							ExifDateFormat,
							DateTimeFormatInfo.InvariantInfo,
							DateTimeStyles.AssumeLocal,
							out date))
					{
						// clean up to ISO-8601
						value = date.ToString(XmpDateFormat);
					}
					break;
				}
				case TypeCode.Object:
				{
					if (property.ValueType is XmpBasicType &&
						((XmpBasicType)property.ValueType) == XmpBasicType.LangAlt)
					{
						string str;
						if (value is byte[])
						{
							str = new String(Encoding.UTF8.GetChars((byte[])value));
							int end = str.IndexOf('\0');
							if (end >= 0)
							{
								str = str.Substring(0, end);
							}
							value = str;
						}
						else
						{
							str = Convert.ToString(value);
						}

						if (String.IsNullOrEmpty(str))
						{
							value = null;
						}
						else
						{
							value = new Dictionary<string, object>
							{
								{ "x-default", str }
							};
						}
					}
					else
					{
						// TODO
					}
					break;
				}
				default:
				{
					break;
				}
			}

			return value;
		}

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

		private IEnumerable<XmpProperty> ProcessXmp(BitmapMetadata metadata, decimal priority, int depth)
		{
			List<XmpProperty> properties = new List<XmpProperty>();

			foreach (string name in metadata)
			{
				// http://msdn.microsoft.com/en-us/library/ee719796(VS.85).aspx
				// http://search.cpan.org/
				object value = metadata.GetQuery(name);
				if (value == null)
				{
					continue;
				}

#if DEBUG
				Console.Write(new String('\t', depth));
				Console.WriteLine("{0} => {1}: {2}", name, value.GetType(), Convert.ToString(value));
#endif

				Enum schema = (Enum)XmpNamespaceUtility.Instance.Parse(name.TrimStart('/'));
				if (schema == null)
				{
					continue;
				}

				XmpProperty property = new XmpProperty
				{
					Schema = schema,
					Priority = priority
				};

				if (value is BitmapMetadata)
				{
					value = this.ProcessMetadata((BitmapMetadata)value, name, depth);
					IDictionary<string, object> dictionary = value as IDictionary<string, object>;
					if (dictionary != null)
					{
						value =
							(from key in dictionary.Keys
							 let val = dictionary[key]
							 select new
							 {
								 Key = key.Substring(key.LastIndexOf(':')+1),
								 Value = val
							 }).ToDictionary(item => item.Key, item => item.Value);
					}
				}

				if (value is BitmapMetadataBlob)
				{
					value = ((BitmapMetadataBlob)value).GetBlobValue();
				}

				if (value == null)
				{
					continue;
				}

				property.Value = value;
				properties.Add(property);
			}

			return properties;
		}

		private Dictionary<string, object> ProcessXmpStruct(BitmapMetadata metadata, int depth)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();

			foreach (string name in metadata)
			{
				object value = metadata.GetQuery(name);
				if (value == null)
				{
					continue;
				}
				if (value is BitmapMetadata)
				{
					value = this.ProcessMetadata((BitmapMetadata)value, name, depth);
					continue;
				}

				if (value is BitmapMetadataBlob)
				{
					value = ((BitmapMetadataBlob)value).GetBlobValue();
				}

				dictionary[name.TrimStart('/')] = value;

#if DEBUG
				Console.Write(new String('\t', depth));
				Console.WriteLine("{0} => {1}: {2}", name, value.GetType(), Convert.ToString(value));
#endif
			}

			return dictionary;
		}

		private Array ProcessArray(BitmapMetadata metadata, int depth)
		{
			ArrayList array = new ArrayList();

			foreach (string name in metadata)
			{
				object value = metadata.GetQuery(name);
				if (value == null)
				{
					continue;
				}
				if (value is BitmapMetadata)
				{
					value = this.ProcessMetadata((BitmapMetadata)value, name, depth);
				}

				if (value is BitmapMetadataBlob)
				{
					value = ((BitmapMetadataBlob)value).GetBlobValue();
				}

				array.Add(value);

#if DEBUG
				Console.Write(new String('\t', depth));
				Console.WriteLine("{0} => {1}: {2}", name, value != null ? value.GetType().Name : "null", Convert.ToString(value));
#endif
			}

			return array.ToArray();
		}

		private Enum ParseSchema(Type enumType, string name)
		{
			name = name.TrimStart('/', '{').TrimEnd('}');
			string[] parts = name.Split(new char[]{'='}, 2, StringSplitOptions.RemoveEmptyEntries);

			if (parts.Length < 2)
			{
				return null;
			}

			switch (parts[0])
			{
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

		#endregion IO Methods
	}
}
