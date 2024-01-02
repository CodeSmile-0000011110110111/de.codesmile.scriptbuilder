// Copyright (C) 2021-2024 Steffen Itterheim
// Refer to included LICENSE file for terms and conditions.

namespace CodeSmile.CSharp.Keywords
{
	/// <summary>
	///     ScriptBuilder C# keywords used before a class' name.
	/// </summary>
	public enum Class
	{
		[EnumName("")] None, // default, will raise an error

		[EnumName("interface ")] Interface,
		[EnumName("struct ")] Struct,
		[EnumName("class ")] Class,
		[EnumName("abstract class ")] AbstractClass,
		[EnumName("sealed class ")] SealedClass,
		[EnumName("static class ")] StaticClass,

		// partial variants
		[EnumName("partial interface ")] PartialInterface,
		[EnumName("partial struct ")] PartialStruct,
		[EnumName("partial class ")] PartialClass,
		[EnumName("abstract partial class ")] AbstractPartialClass,
		[EnumName("sealed partial class ")] SealedPartialClass,
		[EnumName("static partial class ")] StaticPartialClass,
	}
}
