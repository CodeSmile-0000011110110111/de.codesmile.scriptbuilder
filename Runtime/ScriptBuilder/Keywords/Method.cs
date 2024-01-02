// Copyright (C) 2021-2024 Steffen Itterheim
// Refer to included LICENSE file for terms and conditions.

namespace CodeSmile.CSharp.Keywords
{
	/// <summary>
	///     ScriptBuilder method keywords used before method return type.
	/// </summary>
	public enum Method
	{
		[EnumName("")] None,
		[EnumName("static ")] Static,
		[EnumName("abstract ")] Abstract,
		[EnumName("virtual ")] Virtual,
		[EnumName("override ")] Override,
	}
}
