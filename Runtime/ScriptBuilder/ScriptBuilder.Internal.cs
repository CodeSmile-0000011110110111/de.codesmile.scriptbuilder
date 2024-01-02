using DataIO.Extensions.System;
using DataIO.Tools;
using System;
using System.Collections.Generic;

namespace DataIO.Script.Builder
{
	public sealed partial class ScriptBuilder
	{
		private readonly string _nameSpace;
		private readonly List<string> _usings;
		private readonly List<TypeDefinition> _types = new List<TypeDefinition>();
		private string _toString;
		private IndentStringBuilder _builder;

		private ScriptBuilder() {}

		private void BuildUsings()
		{
			if (_usings == null || _usings.Count == 0)
				return;

			var sortedUsings = new List<string>(_usings);
			sortedUsings.Sort();

			foreach (var use in sortedUsings)
			{
				_builder.Append("using ");
				_builder.Append(use);
				_builder.AppendIndentedLine(";");
			}

			_builder.AppendLine();
		}

		private void BuildNamespace()
		{
			if (string.IsNullOrWhiteSpace(_nameSpace))
				throw new ArgumentException("Namespace cannot be empty");
			if (_nameSpace.IsValidCSharpIdentifier() == false)
				throw new ArgumentException($"Namespace '{_nameSpace}' is not a valid C# identifier");

			_builder.Append("namespace ");
			_builder.AppendIndentedLine(_nameSpace);
			_builder.AppendIndentedLine("{");
		}

		private void BuildTypes(bool spacesForTabs)
		{
			for (var i = 0; i < _types.Count; i++)
			{
				if (i > 0)
					_builder.AppendLine();

				_builder.Append(_types[i].Build(spacesForTabs));
			}
		}

		private void BuildEndOfFile() => _builder.AppendLine("}"); // end namespace

		private string Build(bool spacesForTabs = false)
		{
			_builder = new IndentStringBuilder(spacesForTabs);

			BuildUsings();
			BuildNamespace();
			BuildTypes(spacesForTabs);
			BuildEndOfFile();

			return _toString = _builder.ToString();
		}
	}
}