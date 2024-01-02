using System;
using System.Text;

namespace DataIO.Tools
{
	/// <summary>
	/// Wrapper for StringBuilder that makes it easy to create indent formatted text files, like scripts.
	/// 
	/// Has methods for appending string[] so it gets more comfortable to add many smaller strings, example:
	/// builder.AppendIndented(new [] {$"{this}", " appends ", "several", " strings", " at ", "once", dot});
	///
	/// OpenBlock and CloseBlock add lines with curly brackets and increment/decrement indentation.
	/// 
	/// You can also use the original StringBuilder methods without indentation through this wrapper.
	/// </summary>
	public sealed class IndentStringBuilder
	{
		private static readonly string[] TabIndentation = { "", "\t", "\t\t", "\t\t\t", "\t\t\t\t", "\t\t\t\t\t", "\t\t\t\t\t\t", "\t\t\t\t\t\t\t" };
		private static readonly string[] SpaceIndentation =
		{
			"", "    ", "        ", "            ", "                ", "                    ", "                        ", "                            ",
		};

		private readonly StringBuilder _builder = new StringBuilder();

		private string[] _indentationStrings;

		/// <summary>
		/// current indentation level in 'number of Tabs'
		/// </summary>
		public int Indentation { get; set; }

		/// <summary>
		/// if true, 4x whitespace will be used in place of tabs '\t'
		/// </summary>
		public bool SpacesForTabs { get; }

		/// <summary>
		/// Create a new StringBuilder with indentation support. 
		/// </summary>
		/// <param name="spacesForTabs">set to true if you prefer 4x whitespace for each indentation level, otherwise will use Tabs '\t'</param>
		public IndentStringBuilder(bool spacesForTabs = false) => SpacesForTabs = spacesForTabs;

		/// <summary>
		/// get the resulting string
		/// </summary>
		/// <returns></returns>
		public override string ToString() => _builder.ToString();

		/// <summary>
		/// is true as long as nothing has been appended yet
		/// </summary>
		/// <returns></returns>
		public bool IsEmpty() => _builder.Length == 0;

		/// <summary>
		/// regular StringBuilder Append
		/// </summary>
		/// <param name="text"></param>
		public void Append(string text)
		{
			if (string.IsNullOrEmpty(text) == false)
				_builder.Append(text);
		}

		/// <summary>
		/// Appends each string in the array
		/// </summary>
		/// <param name="texts"></param>
		public void Append(string[] texts)
		{
			foreach (var text in texts)
				if (string.IsNullOrEmpty(text) == false)
					_builder.Append(text);
		}

		/// <summary>
		/// appends text with leading indentation based on current Indentation value
		/// </summary>
		/// <param name="text"></param>
		public void AppendIndented(string text)
		{
			AppendIndentation();
			if (string.IsNullOrEmpty(text) == false)
				_builder.Append(text);
		}

		/// <summary>
		/// adds and indent, then appends each string in the array
		/// </summary>
		/// <param name="texts"></param>
		public void AppendIndented(string[] texts)
		{
			AppendIndentation();
			foreach (var text in texts)
				if (string.IsNullOrEmpty(text) == false)
					_builder.Append(text);
		}

		/// <summary>
		/// regular StringBuilder AppendLine()
		/// </summary>
		public void AppendLine() => _builder.AppendLine();

		/// <summary>
		/// regular StringBuilder AppendLine("")
		/// </summary>
		/// <param name="text"></param>
		public void AppendLine(string text)
		{
			if (string.IsNullOrEmpty(text) == false)
				_builder.AppendLine(text);
			else
				_builder.AppendLine();
		}

		/// <summary>
		/// appends 'count' number of empty lines (without indentation)
		/// </summary>
		/// <param name="count"></param>
		public void AppendLines(int count = 1)
		{
			for (var i = 0; i < count; i++)
				_builder.AppendLine();
		}

		/// <summary>
		/// appends the texts, then adds a newline
		/// </summary>
		/// <param name="texts"></param>
		public void AppendLine(string[] texts)
		{
			foreach (var text in texts)
				if (string.IsNullOrEmpty(text) == false)
					_builder.Append(text);

			_builder.AppendLine();
		}

		/// <summary>
		/// appends text with leading indentation based on current Indentation value, ends with newline
		/// </summary>
		/// <param name="text"></param>
		public void AppendIndentedLine(string text)
		{
			if (string.IsNullOrEmpty(text) == false)
			{
				AppendIndentation();
				_builder.Append(text);
			}

			_builder.AppendLine();
		}

		/// <summary>
		/// Appends indentation, then appends each text and at the end appends a newline
		/// </summary>
		/// <param name="texts"></param>
		public void AppendIndentedLine(string[] texts)
		{
			if (texts != null && texts.Length > 0)
				AppendIndentation();

			foreach (var text in texts)
				if (string.IsNullOrEmpty(text) == false)
					_builder.Append(text);

			_builder.AppendLine();
		}

		/// <summary>
		/// just appends the current Indentation, same as calling AppendIndented("")
		/// can be useful in cases where you know you need to indent but don't know yet what follows next,
		/// or if for some reason you need the same indentation in the middle of a line
		/// </summary>
		public void AppendIndentation() => _builder.Append(CheckAndGetIndent());

		/// <summary>
		/// appends an opening '{' with the current Indentation, then increments Indentation 
		/// </summary>
		public void OpenBlock()
		{
			AppendIndentation();
			_builder.AppendLine("{");
			Indentation++;
		}

		/// <summary>
		/// decrements Indentation, then appends a closing '}' 
		/// </summary>
		public void CloseBlock()
		{
			Indentation--;
			AppendIndentation();
			_builder.AppendLine("}");
		}

		/// <summary>
		/// appends 'count' number of spaces (default = 1)
		/// </summary>
		/// <param name="count"></param>
		public void AppendWhiteSpace(int count = 1)
		{
			for (var i = 0; i < count; i++)
				_builder.Append(" ");
		}

		private string CheckAndGetIndent()
		{
			if (_indentationStrings == null)
				_indentationStrings = SpacesForTabs ? SpaceIndentation : TabIndentation;

			if (Indentation < 0)
				throw new Exception($"Indentation must not be negative: {Indentation}");
			if (Indentation >= _indentationStrings.Length)
				throw new Exception($"Indentation level must be less than {_indentationStrings.Length}, got {Indentation}");

			return _indentationStrings[Indentation];
		}
	}
}