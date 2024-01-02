namespace DataIO.Script.Builder
{
	/// <summary>
	/// ScriptBuilder method keywords used before method return type.
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