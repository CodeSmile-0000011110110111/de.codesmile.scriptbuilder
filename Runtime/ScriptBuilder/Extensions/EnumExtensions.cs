// Copyright (C) 2021-2024 Steffen Itterheim
// Refer to included LICENSE file for terms and conditions.

using System;

namespace CodeSmile.CSharp
{
	/// <summary>
	///     Extensions for System.Enum types.
	/// </summary>
	public static class EnumExtensions
	{
		/// <summary>
		///     Reads [EnumName("..")] attribute, if any, from a given enum value and returns that string.
		///     Useful if you need a different (display) name for an enum than the actual enum value.
		/// </summary>
		/// <param name="enumValue"></param>
		/// <returns></returns>
		public static String GetName(this Enum enumValue)
		{
			var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
			var attributes = fieldInfo.GetCustomAttributes(typeof(EnumNameAttribute), false) as EnumNameAttribute[];
			var name = attributes.Length > 0 ? attributes[0].Name : null;
			return name != null ? name : enumValue.ToString();
		}
	}
}
