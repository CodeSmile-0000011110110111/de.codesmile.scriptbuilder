// Copyright (C) 2021-2022 Steffen Itterheim
// Usage is bound to the Unity Asset Store Terms of Service and EULA: https://unity3d.com/legal/as_terms

using DataIO.Extensions.System;
using DataIO.Tools;
using System;

namespace DataIO.Script.Builder
{
	public sealed class PropertyDefinition
	{
		public Access Access { get; }
		public string Type { get; }
		public string Identifier { get; }
		public bool ReadOnly { get; }
		public string BackingFieldIdentifier { get; }

		public PropertyDefinition(Access modifier, string type, string identifier, string backingFieldIdentifier = null, bool readOnly = false)
		{
			if (string.IsNullOrWhiteSpace(type))
				throw new ArgumentException("type");
			if (string.IsNullOrWhiteSpace(identifier))
				throw new ArgumentException("identifier");

			Access = modifier;
			Type = type.Trim();
			Identifier = identifier.SanitizeIdentifier();
			ReadOnly = readOnly;
			BackingFieldIdentifier = backingFieldIdentifier.SanitizeIdentifier();
		}

		internal string Build(bool spacesForTabs = false)
		{
			var builder = new IndentStringBuilder(spacesForTabs);
			builder.Indentation = 2;

			builder.AppendIndented(new[] { Access.GetName(), Type, " ", Identifier });

			var hasBackingField = string.IsNullOrWhiteSpace(BackingFieldIdentifier) == false;
			if (ReadOnly && hasBackingField)
			{
				// make it an expression-bodied property 
				builder.Append(new[] { " => ", BackingFieldIdentifier, ";" });
			}
			else if (hasBackingField)
				builder.Append(new[] { " { get { return ", BackingFieldIdentifier, "; } set { ", BackingFieldIdentifier, " = value; } }" });
			else
			{
				builder.Append(" { get; ");
				if (ReadOnly)
					builder.Append("private ");
				builder.Append("set; }");
			}

			builder.AppendLine();

			return builder.ToString();
		}

		public override string ToString() => Build();
		public string ToString(bool spacesForTabs) => Build(spacesForTabs);
	}
}