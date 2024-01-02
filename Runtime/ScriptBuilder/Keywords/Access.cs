// Copyright (C) 2021-2024 Steffen Itterheim
// Refer to included LICENSE file for terms and conditions.

namespace CodeSmile.CSharp.Keywords
{
	/// <summary>
	///     Access modifiers for ScriptBuilder
	/// </summary>
	public enum Access
	{
		[EnumName("public ")] Public,
		[EnumName("private ")] Private,
		[EnumName("protected ")] Protected,
		[EnumName("internal ")] Internal,
		[EnumName("protected internal ")] ProtectedInternal,
		[EnumName("private protected ")] PrivateProtected,
	}
}
