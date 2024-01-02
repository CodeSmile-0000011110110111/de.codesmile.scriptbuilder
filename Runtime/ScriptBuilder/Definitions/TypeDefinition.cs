// Copyright (C) 2021-2024 Steffen Itterheim
// Refer to included LICENSE file for terms and conditions.

using CodeSmile.CSharp.Keywords;
using System;
using System.Collections.Generic;

namespace CodeSmile.CSharp.Definitions
{
	public sealed class TypeDefinition
	{
		private readonly List<FieldDefinition> _fields = new();
		private readonly List<PropertyDefinition> _properties = new();
		private readonly List<ConstructorDefinition> _constructors = new();
		private readonly List<MethodDefinition> _methods = new();
		private readonly List<IndexerDefinition> _indexers = new();

		public Access Access { get; }
		public Class Class { get; }
		public String Identifier { get; }
		public String BaseClass { get; }
		public String[] Interfaces { get; }
		public String[] Attributes { get; }

		public TypeDefinition(Access modifier, Class @class, String identifier, String[] attributes)
			:
			this(modifier, @class, identifier, null, null, attributes) {}

		public TypeDefinition(Access modifier, Class @class, String identifier, String[] interfaces,
			String[] attributes)
			:
			this(modifier, @class, identifier, null, interfaces, attributes) {}

		public TypeDefinition(Access modifier, Class @class, String identifier, String baseClass = null,
			String[] interfaces = null, String[] attributes = null)
		{
			if (String.IsNullOrWhiteSpace(identifier))
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
				var trimmedInterfaces = new List<String>();
				for (var i = 0; i < interfaces.Length; i++)
					if (interfaces[i] != null)
					{
						trimmedInterfaces.Add(interfaces[i].Trim());
						if (trimmedInterfaces[i].IsValidCSharpIdentifier(true) == false)
							throw new Exception($"'{trimmedInterfaces[i]}' is not a valid C# class or interface name");
					}

				Interfaces = trimmedInterfaces.ToArray();
			}

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

		private void BuildFields(IndentStringBuilder builder, Boolean spacesForTabs)
		{
			if (_fields.Count > 0)
			{
				foreach (var fieldDefinition in _fields)
					builder.Append(fieldDefinition.Build(spacesForTabs));
			}
		}

		private void BuildProperties(IndentStringBuilder builder, Boolean spacesForTabs)
		{
			if (_properties.Count > 0)
			{
				if (ShouldAppendLine(typeof(PropertyDefinition)))
					builder.AppendLine();
				foreach (var propertyDefinition in _properties)
					builder.Append(propertyDefinition.Build(spacesForTabs));
			}
		}

		private void BuildConstructors(IndentStringBuilder builder, Boolean spacesForTabs)
		{
			if (_constructors.Count > 0)
			{
				if (ShouldAppendLine(typeof(ConstructorDefinition)))
					builder.AppendLine();
				foreach (var constructorDefinition in _constructors)
					builder.Append(constructorDefinition.Build(spacesForTabs));
			}
		}

		private void BuildMethods(IndentStringBuilder builder, Boolean spacesForTabs)
		{
			if (_methods.Count > 0)
			{
				if (ShouldAppendLine(typeof(MethodDefinition)))
					builder.AppendLine();
				foreach (var methodDefinition in _methods)
					builder.Append(methodDefinition.Build(spacesForTabs));
			}
		}

		private void BuildIndexers(IndentStringBuilder builder, Boolean spacesForTabs)
		{
			if (_indexers.Count > 0)
			{
				if (ShouldAppendLine(typeof(IndexerDefinition)))
					builder.AppendLine();
				foreach (var indexerDefinition in _indexers)
					builder.Append(indexerDefinition.Build(spacesForTabs));
			}
		}

		private Boolean ShouldAppendLine(Type defType)
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

		public override String ToString() => Build();
		public String ToString(Boolean spacesForTabs) => Build(spacesForTabs);

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
