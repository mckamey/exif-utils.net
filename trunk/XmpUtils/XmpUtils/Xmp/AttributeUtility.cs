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
using System.Collections.Generic;
using System.Reflection;

namespace XmpUtils.Xmp
{
	internal static class AttributeUtility
	{
		public class AttributedMember<T>
			where T : Attribute
		{
			public MemberInfo Member { get; set; }
			public T Attribute { get; set; }
		}

		public static Type GetEnumInfo(Enum value, out string name, out FieldInfo fieldInfo)
		{
			Type type = (value != null) ? value.GetType() : null;

			if (type == null || !type.IsEnum)
			{
				name = null;
				fieldInfo = null;
				return type;
			}

			name = Enum.GetName(type, value);
			fieldInfo = type.GetField(name);

			return type;
		}

		public static IEnumerable<T> FindAttributes<T>(params MemberInfo[] memberInfos)
			where T : Attribute
		{
			return AttributeUtility.FindAttributes<T>((IEnumerable<MemberInfo>)memberInfos);
		}

		public static IEnumerable<T> FindAttributes<T>(IEnumerable<MemberInfo> memberInfos)
			where T : Attribute
		{
			foreach (MemberInfo memberInfo in memberInfos)
			{
				if (memberInfo == null || !Attribute.IsDefined(memberInfo, typeof(T), true))
				{
					continue;
				}

				yield return (T)Attribute.GetCustomAttribute(memberInfo, typeof(T), true);
			}
		}

		public static IEnumerable<AttributedMember<T>> FindWithAttributes<T>(IEnumerable<MemberInfo> memberInfos)
			where T : Attribute
		{
			foreach (MemberInfo memberInfo in memberInfos)
			{
				if (memberInfo == null || !Attribute.IsDefined(memberInfo, typeof(T), true))
				{
					continue;
				}

				yield return new AttributedMember<T>
				{
					Member = memberInfo,
					Attribute = (T)Attribute.GetCustomAttribute(memberInfo, typeof(T), true)
				};
			}
		}
	}
}
