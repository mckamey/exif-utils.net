using System;
using System.Collections;

namespace PseudoCode.Common
{
	/// <summary>
	/// Enumeration Utilities
	/// </summary>
	public class EnumHelper
	{
		public static Enum[] GetEnumValues(Type enumType, object value)
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
	}
}
