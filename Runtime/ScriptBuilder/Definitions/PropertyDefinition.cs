// Copyright (C) 2021-2024 Steffen Itterheim
// Refer to included LICENSE file for terms and conditions.

using CodeSmile.CSharp.Keywords;
using System;

namespace CodeSmile.CSharp.Definitions
{
	public sealed class PropertyDefinition
	{
		public Access Access { get; }
		public String Type { get; }
		public String Identifier { get; }
		public Boolean ReadOnly { get; }
		public String BackingFieldIdentifier { get; }

		public PropertyDefinition(Access modifier, String type, String identifier, String backingFieldIdentifier = null,
			Boolean readOnly = false)
		{
			if (String.IsNullOrWhiteSpace(type))
				throw new ArgumentException("type");
			if (String.IsNullOrWhiteSpace(identifier))
				throw new ArgumentException("identifier");

			Access = modifier;
			Type = type.Trim();
			Identifier = identifier.SanitizeIdentifier();
			ReadOnly = readOnly;
			BackingFieldIdentifier = backingFieldIdentifier.SanitizeIdentifier();
		}

		internal String Build(Boolean spacesForTabs = false)
		{
			var builder = new IndentStringBuilder(spacesForTabs);
			builder.Indentation = 2;

			builder.AppendIndented(new[] { Access.GetName(), Type, " ", Identifier });

			var hasBackingField = String.IsNullOrWhiteSpace(BackingFieldIdentifier) == false;
			if (ReadOnly && hasBackingField)
			{
				// make it an expression-bodied property 
				builder.Append(new[] { " => ", BackingFieldIdentifier, ";" });
			}
			else if (hasBackingField)
			{
				builder.Append(new[]
				{
					" { get { return ", BackingFieldIdentifier, "; } set { ", BackingFieldIdentifier, " = value; } }",
				});
			}
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

		public override String ToString() => Build();
		public String ToString(Boolean spacesForTabs) => Build(spacesForTabs);
	}
}
