namespace DataIO.Script.Builder
{
	/// <summary>
	/// Access modifiers for ScriptBuilder
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