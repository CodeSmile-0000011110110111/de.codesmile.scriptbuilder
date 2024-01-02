using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DataIO.Extensions.System
{
	/// <summary>
	/// Extensions for System.String types.
	/// </summary>
	public static class StringExtensions
	{
		private static readonly string[] CSharpKeywords =
		{
			"var", "bool", "byte", "sbyte", "short", "ushort", "int", "uint", "long", "ulong", "double", "float", "decimal", "string", "char", "void",
			"object",
			"typeof", "sizeof", "null", "true", "false", "if", "else", "while", "for", "foreach", "do", "switch", "case", "default", "lock", "try",
			"throw",
			"catch", "finally", "goto", "break", "continue", "return", "public", "private", "internal", "protected", "static", "readonly", "sealed",
			"const", "fixed", "stackalloc", "volatile", "new", "override", "abstract", "virtual", "event", "extern", "ref", "out", "in", "is", "as",
			"params", "__arglist", "__makeref", "__reftype", "__refvalue", "this", "base", "namespace", "using", "class", "struct", "interface", "enum",
			"delegate", "checked", "unchecked", "unsafe", "operator", "implicit", "explicit",
		};

		/// <summary>
		/// Correctness check of a C# identifier (ie variable, method name) that works across platforms and doesn't use Regex.
		/// It may not catch all edge cases, but 99% of them.
		/// </summary>
		/// <param name="str"></param>
		/// <param name="allowNamespaces">Allows '.' as character as well</param>
		/// <returns></returns>
		public static bool IsValidCSharpIdentifier(this string str, bool allowNamespaces = false, bool allowGenerics = false)
		{
			if (str.Length == 0)
				return false;

			var c = ' ';
			for (var i = 0; i < str.Length; i++)
			{
				c = str[i];
				if ((allowNamespaces && c == '.') || (allowGenerics && c == '<') || c == '>' || c == '_' || (c >= 'a' && c <= 'z') ||
				    (c >= 'A' && c <= 'Z') || (i != 0 && c >= '0' && c <= '9') ||
				    (i == 0 && c == '@')) // @ can be a prefix to avoid clashing with C# keywords (ie "@namespace")
					continue;

				return false;
			}

			// it's technically valid but is it in the reserved keywords list?
			return CSharpKeywords.Contains(str) == false;
		}

		/// <summary>
		/// Replaces any illegal C# identifier character in the string with the given replacement character.
		/// If the string matches a known C# keyword, it gets prefixed with '@' to make it a legal identifier.  
		/// </summary>
		/// <param name="str"></param>
		/// <param name="replacement"></param>
		/// <returns></returns>
		public static string ReplaceIllegalCSharpIdentifierChars(this string str, char replacement = '_')
		{
			var sb = new StringBuilder();
			var c = ' ';
			for (var i = 0; i < str.Length; i++)
			{
				c = str[i];
				if ((c == '_' || (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (i != 0 && c >= '0' && c <= '9')) == false)
					c = replacement;

				sb.Append(c);
			}

			// if it's a C# keyword, prefix it:
			var result = sb.ToString().Trim(replacement);
			if (CSharpKeywords.Contains(result))
				result = $"@{result}";

			return result;
		}

		/// <summary>
		/// Replaces any illegal file system character in the string with replacement.
		/// </summary>
		/// <param name="str"></param>
		/// <param name="replacement"></param>
		/// <returns></returns>
		public static string ReplaceIllegalFileNameChars(this string str, char replacement = '_')
		{
			var modifiedString = str;
			var invalidChars = Path.GetInvalidFileNameChars();
			foreach (var c in invalidChars)
				modifiedString = modifiedString.Replace(c, replacement);

			return modifiedString.Trim(replacement);
		}

		/// <summary>
		/// Trims string and replaces illegal C# identifier characters and prints a Debug.LogWarning if it had to be altered.
		/// </summary>
		/// <param name="identifier"></param>
		/// <param name="noWarnings"></param>
		/// <returns></returns>
		public static string SanitizeIdentifier(this string identifier, bool noWarnings = false)
		{
			if (string.IsNullOrWhiteSpace(identifier))
				return null;

			identifier = identifier.Trim();
			if (identifier.IsValidCSharpIdentifier() == false)
			{
				var sanitizedIdentifier = identifier.ReplaceIllegalCSharpIdentifierChars();
				if (noWarnings == false)
					Debug.LogWarning($"'{identifier}' is not a valid C# identifier => replaced with '{sanitizedIdentifier}'");
				identifier = sanitizedIdentifier;
			}

			return identifier;
		}

		/// <summary>
		/// Removes any quotes and any kind of brackets around a SQL variable's name. Assumes the input contains only the variable part as in
		/// "(\"SomeName\")" or similar, it does not walk through an entire SQL command.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string SanitizeSQLVariableName(this string input) => input.Trim(' ', '(', ')', '[', ']').Trim('\"');

		/// <summary>
		/// Splits the string into lines, ie an array of strings where each entry is the contents of a line. Empty lines are kept.
		/// </summary>
		/// <param name="str"></param>
		/// <param name="newLine">Specify the type of newline, either "\r" or "\n" or "\r\n". If null, uses Environment.NewLine.</param>
		/// <returns></returns>
		public static string[] SplitLines(this string str, string newLine = null) =>
			str.Split(new[] { newLine == null ? Environment.NewLine : newLine }, StringSplitOptions.None);

		/// <summary>
		/// Makes the first char of the string lowercase.
		/// </summary>
		/// <param name="str"></param>
		/// <returns>the camel-cased string</returns>
		public static string ToLowerFirstChar(this string str)
		{
			if (string.IsNullOrEmpty(str) || char.IsLower(str[0]))
				return str;

			return char.ToLower(str[0]) + str.Substring(1);
		}

		/// <summary>
		/// Makes the first char of the string uppercase.
		/// </summary>
		/// <param name="str"></param>
		/// <returns>the camel-cased string</returns>
		public static string ToUpperFirstChar(this string str)
		{
			if (string.IsNullOrEmpty(str) || char.IsUpper(str[0]))
				return str;

			return char.ToUpper(str[0]) + str.Substring(1);
		}
	}
}