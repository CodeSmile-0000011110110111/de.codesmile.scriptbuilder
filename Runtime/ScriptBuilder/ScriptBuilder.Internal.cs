// Copyright (C) 2021-2024 Steffen Itterheim
// Refer to included LICENSE file for terms and conditions.

using CodeSmile.CSharp.Definitions;
using System;
using System.Collections.Generic;

namespace CodeSmile.CSharp
{
	public sealed partial class ScriptBuilder
	{
		private readonly String _nameSpace;
		private readonly List<String> _usings;
		private readonly List<TypeDefinition> _types = new();
		private String _toString;
		private IndentStringBuilder _builder;

		private ScriptBuilder() {}

		private void BuildUsings()
		{
			if (_usings == null || _usings.Count == 0)
				return;

			var sortedUsings = new List<String>(_usings);
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
			if (String.IsNullOrWhiteSpace(_nameSpace))
				throw new ArgumentException("Namespace cannot be empty");
			if (_nameSpace.IsValidCSharpIdentifier() == false)
				throw new ArgumentException($"Namespace '{_nameSpace}' is not a valid C# identifier");

			_builder.Append("namespace ");
			_builder.AppendIndentedLine(_nameSpace);
			_builder.AppendIndentedLine("{");
		}

		private void BuildTypes(Boolean spacesForTabs)
		{
			for (var i = 0; i < _types.Count; i++)
			{
				if (i > 0)
					_builder.AppendLine();

				_builder.Append(_types[i].Build(spacesForTabs));
			}
		}

		private void BuildEndOfFile() => _builder.AppendLine("}"); // end namespace

		private String Build(Boolean spacesForTabs = false)
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
