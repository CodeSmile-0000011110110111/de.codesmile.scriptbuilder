﻿// Copyright (C) 2021-2024 Steffen Itterheim
// Refer to included LICENSE file for terms and conditions.

using CodeSmile.CSharp.Definitions;
using System;
using System.Collections.Generic;

namespace CodeSmile.CSharp
{
	/// <summary>
	///     Create C# scripts reliably using a descriptive API that only leaves you to fill in the custom body lines of methods
	///     and other items.
	///     Supports nested types, usings, namespace, methods, constructors, fields, properties, indexers and more in the
	///     future.
	///     Most C# keywords such as abstract, partial, static are supported as well.
	/// </summary>
	public sealed partial class ScriptBuilder
	{
		public static void BuildAttributes(IndentStringBuilder builder, String[] attributes,
			Boolean compactStyle = false)
		{
			if (attributes != null && attributes.Length > 0)
			{
				for (var i = 0; i < attributes.Length; i++)
					if (compactStyle)
					{
						if (i == 0)
							builder.Append("[");
						else
							builder.Append(", ");

						builder.Append(attributes[i]);

						if (i + 1 == attributes.Length)
							builder.Append("] ");
					}
					else
					{
						builder.AppendIndented("[");
						builder.Append(attributes[i]);
						builder.AppendLine("]");
					}
			}
		}

		public ScriptBuilder(String nameSpace, String[] usings = null)
		{
			if (String.IsNullOrWhiteSpace(nameSpace))
				throw new ArgumentException("nameSpace");
			if (nameSpace.IsValidCSharpIdentifier() == false)
				throw new ArgumentException($"namespace {nameSpace}' is not a valid C# identifier");

			_nameSpace = nameSpace;

			_usings = new List<String>();
			if (usings != null)
				_usings.AddRange(usings);
		}

		public void AddType(TypeDefinition typeDef) => _types.Add(typeDef);
		public void AddTypes(TypeDefinition[] typeDefs) => _types.AddRange(typeDefs);

		public override String ToString() => String.IsNullOrWhiteSpace(_toString) ? Build() : _toString;

		public String ToString(Boolean spacesForTabs) =>
			String.IsNullOrWhiteSpace(_toString) ? Build(spacesForTabs) : _toString;
	}
}
