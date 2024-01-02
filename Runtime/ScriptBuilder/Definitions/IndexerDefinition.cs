// Copyright (C) 2021-2024 Steffen Itterheim
// Refer to included LICENSE file for terms and conditions.

using CodeSmile.CSharp.Keywords;
using System;

namespace CodeSmile.CSharp.Definitions
{
	public sealed class IndexerDefinition
	{
		public Access Access { get; }
		public String ReturnType { get; }
		public ParameterDefinition Index { get; }
		public String GetterLine { get; }
		public String SetterLine { get; }

		public IndexerDefinition(Access modifier, String returnType, ParameterDefinition index, String getterLine,
			String setterLine = null)
		{
			if (String.IsNullOrWhiteSpace(returnType))
				throw new ArgumentException("returnType");
			if (index == null)
				throw new ArgumentException("index");
			if (String.IsNullOrWhiteSpace(getterLine))
				throw new ArgumentException("getterLine");

			Access = modifier;
			ReturnType = returnType;
			Index = index;
			GetterLine = getterLine;
			SetterLine = setterLine;
		}

		internal String Build(Boolean spacesForTabs = false)
		{
			var builder = new IndentStringBuilder(spacesForTabs);
			builder.Indentation = 2;

			builder.AppendIndentedLine(new[]
				{ Access.GetName(), ReturnType, " this[", Index.Type, " ", Index.Identifier, "]" });

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

		public override String ToString() => Build();
		public String ToString(Boolean spacesForTabs) => Build(spacesForTabs);
	}
}
