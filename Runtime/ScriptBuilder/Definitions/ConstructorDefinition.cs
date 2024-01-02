// Copyright (C) 2021-2022 Steffen Itterheim
// Usage is bound to the Unity Asset Store Terms of Service and EULA: https://unity3d.com/legal/as_terms

using DataIO.Extensions.System;
using DataIO.Tools;

namespace DataIO.Script.Builder
{
	public sealed class ConstructorDefinition
	{
		public Access Access { get; }
		public ParameterDefinition[] Parameters { get; }
		public string[] BaseParameters { get; }
		public string[] BodyLines { get; }
		public string TypeName { get; internal set; }

		public ConstructorDefinition(Access modifier, string[] bodyLines)
			: this(modifier, null, null, bodyLines) {}

		public ConstructorDefinition(Access modifier, ParameterDefinition[] parameters, string[] bodyLines)
			: this(modifier,
				parameters,
				null,
				bodyLines) {}

		public ConstructorDefinition(Access modifier, string[] baseParameters, string[] bodyLines)
			: this(modifier, null, baseParameters, bodyLines) {}

		public ConstructorDefinition(Access modifier, ParameterDefinition[] parameters, string[] baseParameters, string[] bodyLines)
		{
			Access = modifier;
			Parameters = parameters;
			BaseParameters = baseParameters;
			BodyLines = bodyLines;
		}

		internal string Build(bool spacesForTabs = false)
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

		public override string ToString() => Build();
		public string ToString(bool spacesForTabs) => Build(spacesForTabs);
	}
}