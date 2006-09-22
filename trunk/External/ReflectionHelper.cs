using System;

namespace PseudoCode
{
	/// <summary>
	/// Reflection Helper
	/// </summary>
	public class ReflectionHelper
	{
		#region Reflection Helper

		/// <summary>
		/// Gets an attribute of an object
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="attributeType"></param>
		/// <param name="inherit"></param>
		/// <returns></returns>
		public static Attribute GetAttribute(object obj, Type attributeType, bool inherit)
		{
			if (obj == null || attributeType == null)
				return null;

			object[] array;

			if (obj is Type)
				array = (obj as Type).GetCustomAttributes(attributeType, inherit);
			else if (obj is System.Reflection.MemberInfo)
				array = (obj as System.Reflection.MemberInfo).GetCustomAttributes(attributeType, inherit);
			else if (obj is Enum)
				array = obj.GetType().GetField(obj.ToString()).GetCustomAttributes(attributeType, inherit);
			else
				throw new NotSupportedException("object doesn't support attributes.");

			return array != null && array.Length > 0 ? array[0] as Attribute : null;
		}

		/// <summary>
		/// Gets an attribute of an enum, optionally splitting up combined flags.
		/// </summary>
		/// <param name="obj">the enum instance</param>
		/// <param name="splitFlags">Whether to split combined flag enums.</param>
		/// <returns></returns>
		public static string GetDescription(Enum obj, bool splitFlags)
		{
			if (splitFlags)
				return ReflectionHelper.GetDescription((Enum)obj);
			else
				return ReflectionHelper.GetDescription((object)obj);
		}

		/// <summary>
		/// Splits enum bit flags into list
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static string GetDescription(Enum obj)
		{
			if (!EnumHelper.IsFlagsEnum(obj.GetType()))
				return ReflectionHelper.GetDescription((object)obj);

			const string delimiter = ", ";

			Enum[] enumValues = EnumHelper.GetFlagList(obj.GetType(), obj);
			System.Text.StringBuilder builder = new System.Text.StringBuilder();

			for (int i=0; i<enumValues.Length; i++)
			{
				builder.Append(ReflectionHelper.GetDescription(enumValues[i] as object));

				if (i != enumValues.Length-1)
					builder.Append(delimiter);
			}

			return builder.ToString();
		}

		/// <summary>
		/// Gets the description of this object from its DescriptionAttribute.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static string GetDescription(object obj)
		{
			if (obj == null)
				return null;

			System.ComponentModel.DescriptionAttribute attribute = ReflectionHelper.GetAttribute(obj,
				typeof(System.ComponentModel.DescriptionAttribute), false)
				as System.ComponentModel.DescriptionAttribute;
			return attribute != null ? attribute.Description : null;
		}

		/// <summary>
		/// Gets the display name of this object from its DisplayNameAttribute.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static string GetDisplayName(object obj)
		{
			if (obj == null)
				return null;

			System.ComponentModel.DisplayNameAttribute attribute = ReflectionHelper.GetAttribute(obj,
				typeof(System.ComponentModel.DisplayNameAttribute), false)
				as System.ComponentModel.DisplayNameAttribute;
			return attribute != null ? attribute.DisplayName : null;
		}

		#endregion Reflection Helper
	}
}
