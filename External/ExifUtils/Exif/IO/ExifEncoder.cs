using System;

namespace ExifUtils.Exif
{
	/// <summary>
	/// Encodes the GDI+ representation of EXIF properties.
	/// </summary>
	/// <remarks>
	/// This is not built yet.
	/// </remarks>
	internal static class ExifEncoder
	{
		#region Byte Encoding

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private static byte[] WriteUInt16(UInt16 value)
		{
			return BitConverter.GetBytes(value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private static byte[] WriteInt32(Int32 value)
		{
			return BitConverter.GetBytes(value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private static byte[] WriteUInt32(UInt32 value)
		{
			return BitConverter.GetBytes(value);
		}

		#endregion Byte Encoding

		#region Data Conversion

		private static byte[] ConvertData(Type targetType, object value)
		{
			return null;
		}

		#endregion Data Conversion
	}
}
