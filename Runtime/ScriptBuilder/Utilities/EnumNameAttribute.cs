using System;

namespace DataIO
{
	/// <summary>
	/// Get the EnumName string from an enum value's [EnumName("..")] attribute by calling Enum.GetName(value).
	/// </summary>
	public sealed class EnumNameAttribute : Attribute
	{
		public string Name { get; }
		public EnumNameAttribute(string name) => Name = name;
	}
}