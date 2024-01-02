// Copyright (C) 2021-2024 Steffen Itterheim
// Refer to included LICENSE file for terms and conditions.

using System;

namespace CodeSmile.CSharp
{
	/// <summary>
	///     Get the EnumName string from an enum value's [EnumName("..")] attribute by calling Enum.GetName(value).
	/// </summary>
	public sealed class EnumNameAttribute : Attribute
	{
		public String Name { get; }
		public EnumNameAttribute(String name) => Name = name;
	}
}
