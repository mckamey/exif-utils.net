using System;
using System.Collections;

namespace PseudoCode.Common
{
	/// <summary>
	/// Enumeration Utilities
	/// </summary>
	public class EnumHelper
	{
		#region GetFlagList

		/// <summary>
		/// Splits a bitwise-OR'd set of enums into a list.
		/// </summary>
		/// <param name="enumType">the enum type</param>
		/// <param name="value">the combined value</param>
		/// <returns>list of flag enums</returns>
		public static Enum[] GetFlagList(Type enumType, object value)
		{
			Enum[] enums = null;

			ulong longVal = Convert.ToUInt64(value);
			string[] enumNames = Enum.GetNames(enumType);
			Array enumValues = Enum.GetValues(enumType);

			ArrayList enumList = new ArrayList(enumValues.Length);

			// check for empty
			if (longVal == 0L)
			{
				enums = new Enum[1];

				// Return the value of empty, or zero if none exists
				if (Convert.ToUInt64(enumValues.GetValue(0)) == 0L)
					enums[0] = enumValues.GetValue(0) as Enum;
				else
					enums[0] = null;
				return enums;
			}

			for (int i = 0; i < enumValues.Length; i++)
			{
				ulong enumValue = Convert.ToUInt64(enumValues.GetValue(i));

				if ((i == 0) && (enumValue == 0L))
					continue;
 
				// matches a value in enumeration
				if ((longVal & enumValue) == enumValue)
				{
					// remove from val
					longVal -= enumValue;

					// add enum to list
					enumList.Add(enumValues.GetValue(i)); 
				}
			}
 
			if (longVal != 0L)
				enumList.Add(longVal); 

			enums = new Enum[enumList.Count];
			enumList.CopyTo(enums);
			return enums; 
		}

		#endregion GetFlagList

		#region GetEnumList

		/// <summary>
		/// Gets the list of all enum values from an enum type.
		/// </summary>
		/// <param name="enumType"></param>
		/// <returns></returns>
		public static Enum[] GetEnumList(System.Type enumType)
		{
			System.Array enumValues = Enum.GetValues(enumType);
			Enum[] enumList = new Enum[enumValues.Length];

			for(int i=0; i<enumValues.Length; i++)
			{
				enumList[i] = (Enum)Enum.ToObject(enumType, enumValues.GetValue(i));
			}

			return enumList;
		}

		#endregion GetEnumList

		#region GetEnumNames

		/// <summary>
		/// Convenience method.  Same as Enum.GetNames(enumType);
		/// </summary>
		/// <param name="enumType"></param>
		/// <returns></returns>
		public static string[] GetEnumNames(System.Type enumType)
		{
			return Enum.GetNames(enumType);
		}

		#endregion GetEnumNames

		#region GetEnumValues

		/// <summary>
		/// Convenience method.  Same as Enum.GetValues(enumType) except typed.
		/// </summary>
		/// <param name="enumType"></param>
		/// <returns>Int32[]</returns>
		public static Int32[] GetInt32Values(System.Type enumType)
		{
			return Enum.GetValues(enumType) as Int32[];
		}

		/// <summary>
		/// Convenience method.  Same as Enum.GetValues(enumType) except typed.
		/// </summary>
		/// <param name="enumType"></param>
		/// <returns>Byte[]</returns>
		public static Byte[] GetByteValues(System.Type enumType)
		{
			return Enum.GetValues(enumType) as Byte[];
		}

		/// <summary>
		/// Convenience method.  Same as Enum.GetValues(enumType) except typed.
		/// </summary>
		/// <param name="enumType"></param>
		/// <returns>Int64[]</returns>
		public static Int64[] GetInt64Values(System.Type enumType)
		{
			return Enum.GetValues(enumType) as Int64[];
		}

		#endregion GetEnumValues

		#region GetDescriptions

		/// <summary>
		/// Gets the list of all enum descriptions from an enum type.
		/// </summary>
		/// <param name="enumType"></param>
		/// <returns></returns>
		/// <remarks>Splits combined flag enums.</remarks>
		public static string[] GetDescriptions(System.Type enumType)
		{
			return EnumHelper.GetDescriptions(enumType, false);
		}

		/// <summary>
		/// Gets the list of all enum descriptions from an enum type.
		/// </summary>
		/// <param name="enumType"></param>
		/// <param name="splitFlags">Whether to split combined flag enums.</param>
		/// <returns></returns>
		public static string[] GetDescriptions(System.Type enumType, bool splitFlags)
		{
			Enum[] enumList = EnumHelper.GetEnumList(enumType);
			string[] enumDecriptions = new string[enumList.Length];

			for(int i=0; i<enumList.Length; i++)
			{
				enumDecriptions[i] = Helper.GetDescription(enumList[i], splitFlags);
			}

			return enumDecriptions;
		}

		#endregion GetDescriptions

		#region IsFlagsEnum
		
		/// <summary>
		/// Checks if an enum is able to be combined as bit flags.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static bool IsFlagsEnum(Type type)
		{
			if (!type.IsEnum)
				return false;

			Attribute flags = Helper.GetAttribute(type, typeof(FlagsAttribute), true);
			return (flags != null);
		}

		#endregion IsFlagsEnum

		#region Parse

		/// <summary>
		/// Parses a string into an enum and allows a default value.
		/// </summary>
		/// <param name="enumType"></param>
		/// <param name="strValue"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static Enum Parse(Type enumType, string strValue, Enum defaultValue)
		{
			object objValue = defaultValue;
			try
			{
				objValue = Enum.Parse(enumType, strValue, true);
				if (!enumType.IsInstanceOfType(objValue))
					objValue = defaultValue;
			}
			catch { }

			return (Enum)objValue;
		}

		#endregion Parse
	}
}
