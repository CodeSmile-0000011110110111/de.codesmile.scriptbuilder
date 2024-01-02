// Copyright (C) 2021-2022 Steffen Itterheim
// Usage is bound to the Unity Asset Store Terms of Service and EULA: https://unity3d.com/legal/as_terms

using DataIO.Extensions.System;
using DataIO.Tools;
using System;

namespace DataIO.Script.Builder
{
	public sealed class IndexerDefinition
	{
		public Access Access { get; }
		public string ReturnType { get; }
		public ParameterDefinition Index { get; }
		public string GetterLine { get; }
		public string SetterLine { get; }

		public IndexerDefinition(Access modifier, string returnType, ParameterDefinition index, string getterLine, string setterLine = null)
		{
			if (string.IsNullOrWhiteSpace(returnType))
				throw new ArgumentException("returnType");
			if (index == null)
				throw new ArgumentException("index");
			if (string.IsNullOrWhiteSpace(getterLine))
				throw new ArgumentException("getterLine");

			Access = modifier;
			ReturnType = returnType;
			Index = index;
			GetterLine = getterLine;
			SetterLine = setterLine;
		}

		internal string Build(bool spacesForTabs = false)
		{
			var builder = new IndentStringBuilder(spacesForTabs);
			builder.Indentation = 2;

			builder.AppendIndentedLine(new[] { Access.GetName(), ReturnType, " this[", Index.Type, " ", Index.Identifier, "]" });

			builder.OpenBlock();
			builder.AppendIndented("get => ");
			builder.AppendLine(GetterLine);
			if (SetterLine != null)
			{
				builder.AppendIndented("set => ");
				builder.AppendLine(SetterLine);
			}
			builder.CloseBlock();

			return builder.ToString();
		}

		public override string ToString() => Build();
		public string ToString(bool spacesForTabs) => Build(spacesForTabs);
	}
}