// Copyright (C) 2021-2022 Steffen Itterheim
// Usage is bound to the Unity Asset Store Terms of Service and EULA: https://unity3d.com/legal/as_terms

using DataIO.Extensions.System;
using DataIO.Tools;
using System;
using System.Collections.Generic;

namespace DataIO.Script.Builder
{
	public sealed class TypeDefinition
	{
		private readonly List<FieldDefinition> _fields = new List<FieldDefinition>();
		private readonly List<PropertyDefinition> _properties = new List<PropertyDefinition>();
		private readonly List<ConstructorDefinition> _constructors = new List<ConstructorDefinition>();
		private readonly List<MethodDefinition> _methods = new List<MethodDefinition>();
		private readonly List<IndexerDefinition> _indexers = new List<IndexerDefinition>();

		public Access Access { get; }
		public Class Class { get; }
		public string Identifier { get; }
		public string BaseClass { get; }
		public string[] Interfaces { get; }
		public string[] Attributes { get; }

		public TypeDefinition(Access modifier, Class @class, string identifier, string[] attributes)
			:
			this(modifier, @class, identifier, null, null, attributes) {}

		public TypeDefinition(Access modifier, Class @class, string identifier, string[] interfaces,
			string[] attributes)
			:
			this(modifier, @class, identifier, null, interfaces, attributes) {}

		public TypeDefinition(Access modifier, Class @class, string identifier, string baseClass = null,
			string[] interfaces = null, string[] attributes = null)
		{
			if (string.IsNullOrWhiteSpace(identifier))
				throw new ArgumentException("identifier");
			if (identifier.IsValidCSharpIdentifier(true, true) == false)
				throw new ArgumentException($"identifier '{identifier}' is not a valid C# identifier");
			if (baseClass?.IsValidCSharpIdentifier(true, true) == false)
				throw new Exception($"baseClass '{BaseClass}' is not a valid C# type name");

			Access = modifier;
			Class = @class;
			Identifier = identifier.Trim();
			BaseClass = baseClass?.Trim();

			if (interfaces != null)
			{
				var trimmedInterfaces = new List<string>();
				for (var i = 0; i < interfaces.Length; i++)
				{
					if (interfaces[i] != null)
					{
						trimmedInterfaces.Add(interfaces[i].Trim());
						if (trimmedInterfaces[i].IsValidCSharpIdentifier(true) == false)
							throw new Exception($"'{trimmedInterfaces[i]}' is not a valid C# class or interface name");
					}
				}

				Interfaces = trimmedInterfaces.ToArray();
			}

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
			builder.Indentation = 1;

			ScriptBuilder.BuildAttributes(builder, Attributes);
			builder.AppendIndented(new[] { Access.GetName(), Class.GetName(), Identifier });

			if (BaseClass != null)
				builder.Append(new[] { " : ", BaseClass });

			if (Interfaces != null)
			{
				for (var i = 0; i < Interfaces.Length; i++)
					builder.Append(new[] { i == 0 && BaseClass == null ? " : " : ", ", Interfaces[i] });
			}

			builder.AppendLine();

			builder.OpenBlock();
			BuildFields(builder, spacesForTabs);
			BuildProperties(builder, spacesForTabs);
			BuildConstructors(builder, spacesForTabs);
			BuildMethods(builder, spacesForTabs);
			BuildIndexers(builder, spacesForTabs);
			builder.CloseBlock();

			return builder.ToString();
		}

		private void BuildFields(IndentStringBuilder builder, bool spacesForTabs)
		{
			if (_fields.Count > 0)
			{
				foreach (var fieldDefinition in _fields)
					builder.Append(fieldDefinition.Build(spacesForTabs));
			}
		}

		private void BuildProperties(IndentStringBuilder builder, bool spacesForTabs)
		{
			if (_properties.Count > 0)
			{
				if (ShouldAppendLine(typeof(PropertyDefinition)))
					builder.AppendLine();
				foreach (var propertyDefinition in _properties)
					builder.Append(propertyDefinition.Build(spacesForTabs));
			}
		}

		private void BuildConstructors(IndentStringBuilder builder, bool spacesForTabs)
		{
			if (_constructors.Count > 0)
			{
				if (ShouldAppendLine(typeof(ConstructorDefinition)))
					builder.AppendLine();
				foreach (var constructorDefinition in _constructors)
					builder.Append(constructorDefinition.Build(spacesForTabs));
			}
		}

		private void BuildMethods(IndentStringBuilder builder, bool spacesForTabs)
		{
			if (_methods.Count > 0)
			{
				if (ShouldAppendLine(typeof(MethodDefinition)))
					builder.AppendLine();
				foreach (var methodDefinition in _methods)
					builder.Append(methodDefinition.Build(spacesForTabs));
			}
		}

		private void BuildIndexers(IndentStringBuilder builder, bool spacesForTabs)
		{
			if (_indexers.Count > 0)
			{
				if (ShouldAppendLine(typeof(IndexerDefinition)))
					builder.AppendLine();
				foreach (var indexerDefinition in _indexers)
					builder.Append(indexerDefinition.Build(spacesForTabs));
			}
		}

		private bool ShouldAppendLine(Type defType)
		{
			// TODO: this assumes a fixed order ... which is okay, for now
			// solution: use a common interface for all, then add them all into a single list and iterate over that, appending lines only when type changes
			if (defType == typeof(PropertyDefinition))
				return _fields.Count > 0;
			if (defType == typeof(ConstructorDefinition))
				return _fields.Count > 0 || _properties.Count > 0;
			if (defType == typeof(MethodDefinition))
				return _fields.Count > 0 || _properties.Count > 0 || _constructors.Count > 0;
			if (defType == typeof(IndexerDefinition))
				return _fields.Count > 0 || _properties.Count > 0 || _constructors.Count > 0 || _methods.Count > 0;

			return false;
		}

		public override string ToString() => Build();
		public string ToString(bool spacesForTabs) => Build(spacesForTabs);

		public void AddField(FieldDefinition field) => _fields.Add(field);
		public void AddFields(FieldDefinition[] fields) => _fields.AddRange(fields);
		public void AddProperty(PropertyDefinition property) => _properties.Add(property);
		public void AddProperties(PropertyDefinition[] properties) => _properties.AddRange(properties);

		public void AddConstructor(ConstructorDefinition ctor)
		{
			ctor.TypeName = Identifier;
			_constructors.Add(ctor);
		}

		public void AddConstructors(ConstructorDefinition[] ctors)
		{
			foreach (var constructorDefinition in ctors)
				constructorDefinition.TypeName = Identifier;
			_constructors.AddRange(ctors);
		}

		public void AddMethod(MethodDefinition method) => _methods.Add(method);
		public void AddMethods(MethodDefinition[] methods) => _methods.AddRange(methods);
		public void AddIndexer(IndexerDefinition indexer) => _indexers.Add(indexer);
		public void AddIndexers(IndexerDefinition[] indexers) => _indexers.AddRange(indexers);
	}
}