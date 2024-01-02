// Copyright (C) 2021-2022 Steffen Itterheim
// Usage is bound to the Unity Asset Store Terms of Service and EULA: https://unity3d.com/legal/as_terms

using DataIO.Extensions.System;
using DataIO.Tools;
using System;

namespace DataIO.Script.Builder
{
	public sealed class ParameterDefinition
	{
		public string Type { get; }
		public string Identifier { get; }

		public ParameterDefinition(string type, string identifier)
		{
			if (string.IsNullOrWhiteSpace(type))
				throw new ArgumentException("type");
			if (string.IsNullOrWhiteSpace(identifier))
				throw new ArgumentException("identifier");

			Type = type.Trim();
			Identifier = identifier.SanitizeIdentifier();
		}

		internal string Build(bool spacesForTabs = false)
		{
			var builder = new IndentStringBuilder(spacesForTabs);
			builder.Append(new[] { Type, " ", Identifier });
			return builder.ToString();
		}

		public override string ToString() => Build();
		public string ToString(bool spacesForTabs) => Build(spacesForTabs);
	}
}