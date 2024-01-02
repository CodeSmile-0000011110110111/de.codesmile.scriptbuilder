// Copyright (C) 2021-2022 Steffen Itterheim
// Usage is bound to the Unity Asset Store Terms of Service and EULA: https://unity3d.com/legal/as_terms

using DataIO.Extensions.System;
using DataIO.Tools;
using System;
using System.Collections.Generic;

namespace DataIO.Script.Builder
{
	public sealed class FieldDefinition
	{
		public Access Access { get; }
		public string Type { get; }
		public string Identifier { get; }
		public string[] Attributes { get; }

		public FieldDefinition(Access modifier, string type, string identifier, string[] attributes = null)
		{
			if (string.IsNullOrWhiteSpace(type))
				throw new ArgumentException("type");
			if (string.IsNullOrWhiteSpace(identifier))
				throw new ArgumentException("identifier");

			Access = modifier;
			Type = type.Trim();
			Identifier = identifier.SanitizeIdentifier();

			if (attributes != null)
			{
				var trimmedAttributes = new List<string>();
				for (var i = 0; i < attributes.Length; i++)
				{
					if (attributes[i] != null)
						trimmedAttributes.Add(attributes[i].Trim());
				}

				Attributes = trimmedAttributes.ToArray();
			}
		}

		internal string Build(bool spacesForTabs = false)
		{
			var builder = new IndentStringBuilder(spacesForTabs);
			builder.Indentation = 2;

			builder.AppendIndentation();
			ScriptBuilder.BuildAttributes(builder, Attributes, true);
			builder.AppendLine(new[] { Access.GetName(), Type, " ", Identifier, ";" });
			return builder.ToString();
		}

		public override string ToString() => Build();
		public string ToString(bool spacesForTabs) => Build(spacesForTabs);
	}
}