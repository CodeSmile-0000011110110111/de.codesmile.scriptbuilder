// Copyright (C) 2021-2024 Steffen Itterheim
// Refer to included LICENSE file for terms and conditions.

using CodeSmile.CSharp.Keywords;
using System;
using System.Collections.Generic;

namespace CodeSmile.CSharp.Definitions
{
	public sealed class FieldDefinition
	{
		public Access Access { get; }
		public String Type { get; }
		public String Identifier { get; }
		public String[] Attributes { get; }

		public FieldDefinition(Access modifier, String type, String identifier, String[] attributes = null)
		{
			if (String.IsNullOrWhiteSpace(type))
				throw new ArgumentException("type");
			if (String.IsNullOrWhiteSpace(identifier))
				throw new ArgumentException("identifier");

			Access = modifier;
			Type = type.Trim();
			Identifier = identifier.SanitizeIdentifier();

			if (attributes != null)
			{
				var trimmedAttributes = new List<String>();
				for (var i = 0; i < attributes.Length; i++)
					if (attributes[i] != null)
						trimmedAttributes.Add(attributes[i].Trim());

				Attributes = trimmedAttributes.ToArray();
			}
		}

		internal String Build(Boolean spacesForTabs = false)
		{
			var builder = new IndentStringBuilder(spacesForTabs);
			builder.Indentation = 2;

			builder.AppendIndentation();
			ScriptBuilder.BuildAttributes(builder, Attributes, true);
			builder.AppendLine(new[] { Access.GetName(), Type, " ", Identifier, ";" });
			return builder.ToString();
		}

		public override String ToString() => Build();
		public String ToString(Boolean spacesForTabs) => Build(spacesForTabs);
	}
}
