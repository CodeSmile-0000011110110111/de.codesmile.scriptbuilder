// Copyright (C) 2021-2024 Steffen Itterheim
// Refer to included LICENSE file for terms and conditions.

using CodeSmile.CSharp.Keywords;
using System;

namespace CodeSmile.CSharp.Definitions
{
	public sealed class ConstructorDefinition
	{
		public Access Access { get; }
		public ParameterDefinition[] Parameters { get; }
		public String[] BaseParameters { get; }
		public String[] BodyLines { get; }
		public String TypeName { get; internal set; }

		public ConstructorDefinition(Access modifier, String[] bodyLines)
			: this(modifier, null, null, bodyLines) {}

		public ConstructorDefinition(Access modifier, ParameterDefinition[] parameters, String[] bodyLines)
			: this(modifier,
				parameters,
				null,
				bodyLines) {}

		public ConstructorDefinition(Access modifier, String[] baseParameters, String[] bodyLines)
			: this(modifier, null, baseParameters, bodyLines) {}

		public ConstructorDefinition(Access modifier, ParameterDefinition[] parameters, String[] baseParameters,
			String[] bodyLines)
		{
			Access = modifier;
			Parameters = parameters;
			BaseParameters = baseParameters;
			BodyLines = bodyLines;
		}

		internal String Build(Boolean spacesForTabs = false)
		{
			var builder = new IndentStringBuilder(spacesForTabs);
			builder.Indentation = 2;

			builder.AppendIndented(Access.GetName());
			builder.Append(new[] { TypeName, "(" });
			if (Parameters != null)
			{
				for (var i = 0; i < Parameters.Length; i++)
				{
					var param = Parameters[i];
					if (i != 0)
						builder.Append(", ");

					builder.Append(new[] { param.Type, " ", param.Identifier });
				}
			}
			builder.Append(")");

			if (BaseParameters != null)
			{
				builder.Append(" : base(");
				for (var i = 0; i < BaseParameters.Length; i++)
				{
					var param = BaseParameters[i];
					if (i != 0)
						builder.Append(", ");

					builder.Append(param);
				}
				builder.Append(")");
			}

			builder.AppendLine();

			builder.OpenBlock();
			if (BodyLines != null)
			{
				foreach (var line in BodyLines)
					builder.AppendIndentedLine(line);
			}
			builder.CloseBlock();

			builder.AppendLine();

			return builder.ToString();
		}

		public override String ToString() => Build();
		public String ToString(Boolean spacesForTabs) => Build(spacesForTabs);
	}
}
