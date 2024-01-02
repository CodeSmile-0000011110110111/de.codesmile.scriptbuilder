// Copyright (C) 2021-2022 Steffen Itterheim
// Usage is bound to the Unity Asset Store Terms of Service and EULA: https://unity3d.com/legal/as_terms

using DataIO.Extensions.System;
using DataIO.Tools;
using System;

namespace DataIO.Script.Builder
{
	public sealed class MethodDefinition
	{
		public Access Access { get; }
		public Method Method { get; }
		public string MethodName { get; }
		public ParameterDefinition[] Parameters { get; }
		public string ReturnType { get; }
		public string[] BodyLines { get; }

		public MethodDefinition(Access modifier, Method method, string returnType, string methodName, string[] bodyLines = null)
			:
			this(modifier, method, returnType, methodName, null, bodyLines) {}

		public MethodDefinition(Access modifier, Method method, string returnType, string methodName,
			ParameterDefinition[] parameters, string[] bodyLines = null)
		{
			if (string.IsNullOrWhiteSpace(methodName))
				throw new ArgumentException("methodName");
			if (methodName.IsValidCSharpIdentifier() == false)
				throw new ArgumentException($"'{MethodName}' is not a valid C# identifier");

			Access = modifier;
			Method = method;
			MethodName = methodName;
			Parameters = parameters;
			ReturnType = string.IsNullOrWhiteSpace(returnType) ? "void" : returnType;
			BodyLines = bodyLines;
		}

		internal string Build(bool spacesForTabs = false)
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

		public override string ToString() => Build();
		public string ToString(bool spacesForTabs) => Build(spacesForTabs);
	}
}