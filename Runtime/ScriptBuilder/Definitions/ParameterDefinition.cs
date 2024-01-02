// Copyright (C) 2021-2024 Steffen Itterheim
// Refer to included LICENSE file for terms and conditions.

using System;

namespace CodeSmile.CSharp.Definitions
{
	public sealed class ParameterDefinition
	{
		public String Type { get; }
		public String Identifier { get; }

		public ParameterDefinition(String type, String identifier)
		{
			if (String.IsNullOrWhiteSpace(type))
				throw new ArgumentException("type");
			if (String.IsNullOrWhiteSpace(identifier))
				throw new ArgumentException("identifier");

			Type = type.Trim();
			Identifier = identifier.SanitizeIdentifier();
		}

		internal String Build(Boolean spacesForTabs = false)
		{
			var builder = new IndentStringBuilder(spacesForTabs);
			builder.Append(new[] { Type, " ", Identifier });
			return builder.ToString();
		}

		public override String ToString() => Build();
		public String ToString(Boolean spacesForTabs) => Build(spacesForTabs);
	}
}
