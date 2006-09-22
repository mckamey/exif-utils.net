using System;

namespace PhotoLib.Model.Exif
{
	/// <summary>
	/// Encodes the GDI+ representation of EXIF properties.
	/// </summary>
	/// <remarks>
	/// This is not built yet.
	/// </remarks>
	internal static class ExifEncoder
	{
		public static System.Drawing.Imaging.PropertyItem ToPropertyItem(ExifProperty exif)
		{
			Type propType = typeof(System.Drawing.Imaging.PropertyItem);
			System.Reflection.ConstructorInfo ctor = propType.GetConstructor(new Type[0]);
			System.Drawing.Imaging.PropertyItem propertyItem = ctor.Invoke(new object[0]) as System.Drawing.Imaging.PropertyItem;

			// set value here
			
			return propertyItem;
		}

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
	}
}
