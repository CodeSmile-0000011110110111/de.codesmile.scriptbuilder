// Copyright (C) 2021-2024 Steffen Itterheim
// Refer to included LICENSE file for terms and conditions.

using CodeSmile.CSharp.Keywords;
using System;

namespace CodeSmile.CSharp.Definitions
{
	public sealed class MethodDefinition
	{
		public Access Access { get; }
		public Method Method { get; }
		public String MethodName { get; }
		public ParameterDefinition[] Parameters { get; }
		public String ReturnType { get; }
		public String[] BodyLines { get; }

		public MethodDefinition(Access modifier, Method method, String returnType, String methodName,
			String[] bodyLines = null)
			:
			this(modifier, method, returnType, methodName, null, bodyLines) {}

		public MethodDefinition(Access modifier, Method method, String returnType, String methodName,
			ParameterDefinition[] parameters, String[] bodyLines = null)
		{
			if (String.IsNullOrWhiteSpace(methodName))
				throw new ArgumentException("methodName");
			if (methodName.IsValidCSharpIdentifier() == false)
				throw new ArgumentException($"'{MethodName}' is not a valid C# identifier");

			Access = modifier;
			Method = method;
			MethodName = methodName;
			Parameters = parameters;
			ReturnType = String.IsNullOrWhiteSpace(returnType) ? "void" : returnType;
			BodyLines = bodyLines;
		}

		internal String Build(Boolean spacesForTabs = false)
		{
			var builder = new IndentStringBuilder(spacesForTabs);
			builder.Indentation = 2;

			builder.AppendIndented(new[] { Access.GetName(), Method.GetName(), ReturnType, " ", MethodName, "(" });
			if (Parameters != null)
			{
				for (var i = 0; i < Parameters.Length; i++)
				{
					if (i != 0)
						builder.Append(", ");

					builder.Append(Parameters[i].Build(spacesForTabs));
				}
			}
			builder.AppendLine(")");

			builder.OpenBlock();
			if (BodyLines != null)
			{
				foreach (var line in BodyLines)
					builder.AppendIndentedLine(line);
			}
			builder.CloseBlock();

			return builder.ToString();
		}

		public override String ToString() => Build();
		public String ToString(Boolean spacesForTabs) => Build(spacesForTabs);
	}
}
