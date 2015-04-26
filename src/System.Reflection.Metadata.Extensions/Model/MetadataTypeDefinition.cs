using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;
using JetBrains.Annotations;

namespace System.Reflection.Metadata.Model
{
  // todo: attributes

  [DebuggerDisplay("{DebugView,nq}")]
  public struct MetadataTypeDefinition
  {
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [NotNull] private readonly MetadataReader myMetadataReader;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private TypeDefinition myTypeDefinition;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private ImmutableArray<MetadataMethod> myMethods;

    public MetadataTypeDefinition([NotNull] MetadataReader metadataReader, TypeDefinition typeDefinition)
    {
      myMetadataReader = metadataReader;
      myTypeDefinition = typeDefinition;
      myMethods = default(ImmutableArray<MetadataMethod>);
    }

    public TypeDefinition Definition
    {
      get { return myTypeDefinition; }
    }

    [NotNull] public string Name
    {
      get { return myMetadataReader.GetString(myTypeDefinition.Name); }
    }

    [NotNull] public string FullName
    {
      get
      {
        var declaringType = myTypeDefinition.GetDeclaringType();
        if (!declaringType.IsNil)
        {
          var builder = new StringBuilder();
          BuildFullName(builder, declaringType);
          builder.Append('+').Append(Name);
          return builder.ToString();
        }

        var namespaceHandle = myTypeDefinition.Namespace;
        if (namespaceHandle.IsNil) return Name;

        var nameSpace = myMetadataReader.GetString(namespaceHandle);
        return nameSpace + "." + Name;
      }
    }

    private void BuildFullName([NotNull] StringBuilder builder, TypeDefinitionHandle type)
    {
      var typeDefinition = myMetadataReader.GetTypeDefinition(type);

      var containingType = typeDefinition.GetDeclaringType();
      if (containingType.IsNil)
      {
        var namespaceHandle = typeDefinition.Namespace;
        if (!namespaceHandle.IsNil)
        {
          var namespaceName = myMetadataReader.GetString(namespaceHandle);
          builder.Append(namespaceName).Append('.');
        }
      }
      else
      {
        BuildFullName(builder, containingType);
        builder.Append('+');
      }

      var shortName = myMetadataReader.GetString(typeDefinition.Name);
      builder.Append(shortName);
    }

    [NotNull] public string Namespace
    {
      get
      {
        var type = myTypeDefinition;

        for (var declaringType = type.GetDeclaringType(); !declaringType.IsNil;)
        {
          type = myMetadataReader.GetTypeDefinition(declaringType);
          declaringType = type.GetDeclaringType();
        }

        var namespaceHandle = type.Namespace;
        if (namespaceHandle.IsNil) return string.Empty;

        return myMetadataReader.GetString(namespaceHandle);
      }
    }

    public MetadataTypeDefinition? ContainingType
    {
      get
      {
        var declaringType = myTypeDefinition.GetDeclaringType();
        if (declaringType.IsNil) return null;

        var typeDefinition = myMetadataReader.GetTypeDefinition(declaringType);
        return new MetadataTypeDefinition(myMetadataReader, typeDefinition);
      }
    }

    public bool IsClass
    {
      get { return !IsInterface; }
    }

    public bool IsInterface
    {
      get { return (myTypeDefinition.Attributes & TypeAttributes.Interface) != 0; }
    }

    public bool IsAbstract
    {
      get { return (myTypeDefinition.Attributes & TypeAttributes.Abstract) != 0; }
    }

    public bool IsSealed
    {
      get { return (myTypeDefinition.Attributes & TypeAttributes.Sealed) != 0; }
    }

    public bool IsNested
    {
      get { return !myTypeDefinition.GetDeclaringType().IsNil; }
    }

    public MetadataVisibility Visibility
    {
      get
      {
        var attributes = myTypeDefinition.Attributes & TypeAttributes.VisibilityMask;
        switch (attributes)
        {
          case TypeAttributes.NestedAssembly:
            return MetadataVisibility.Internal;
          case TypeAttributes.NestedFamANDAssem:
            return MetadataVisibility.ProtectedAndInternal;
          case TypeAttributes.NestedFamily:
            return MetadataVisibility.Protected;
          case TypeAttributes.NestedFamORAssem:
            return MetadataVisibility.ProtectedOrInternal;
          case TypeAttributes.NestedPublic:
          case TypeAttributes.Public:
            return MetadataVisibility.Public;
          default:
            return MetadataVisibility.Private;
        }
      }
    }

    public ImmutableArray<MetadataMethod> Methods
    {
      get
      {
        if (myMethods.IsDefault)
        {
          var handleCollection = myTypeDefinition.GetMethods();
          var builder = ImmutableArray.CreateBuilder<MetadataMethod>(handleCollection.Count);
          var metadataReader = myMetadataReader;

          foreach (var definitionHandle in handleCollection)
          {
            var methodDefinition = metadataReader.GetMethodDefinition(definitionHandle);
            builder.Add(new MetadataMethod(metadataReader, methodDefinition));
          }

          myMethods = builder.MoveToImmutable();
        }

        return myMethods;
      }
    }

    public bool IsValueType
    {
      get
      {
        if ((myTypeDefinition.Attributes & TypeAttributes.Interface) != 0) return false;

        var baseTypeHandle = myTypeDefinition.BaseType;
        if (baseTypeHandle.Kind == HandleKind.TypeDefinition)
        {
          var typeDefinition = myMetadataReader.GetTypeDefinition((TypeDefinitionHandle) baseTypeHandle);

          var namespaceHandle = typeDefinition.Namespace;
          if (namespaceHandle.IsNil) return false;

          var namespaceName = myMetadataReader.GetString(namespaceHandle);
          if (namespaceName != "System") return false;

          var typeName = myMetadataReader.GetString(typeDefinition.Name);
          return typeName == "ValueType" || typeName == "Enum";
        }

        if (baseTypeHandle.Kind == HandleKind.TypeReference)
        {
          var typeReference = myMetadataReader.GetTypeReference((TypeReferenceHandle) baseTypeHandle);

          var namespaceHandle = typeReference.Namespace;
          if (namespaceHandle.IsNil) return false;

          var namespaceName = myMetadataReader.GetString(namespaceHandle);
          if (namespaceName != "System") return false;

          var typeName = myMetadataReader.GetString(typeReference.Name);
          return typeName == "ValueType" || typeName == "Enum";
        }

        return false;
      }
    }

    public MetadataTypeDefinition? GetBaseTypeDefinition()
    {
      var baseTypeHadle = myTypeDefinition.BaseType;
      if (baseTypeHadle.Kind == HandleKind.TypeDefinition)
      {
        var typeDefinition = myMetadataReader.GetTypeDefinition((TypeDefinitionHandle) baseTypeHadle);

        return new MetadataTypeDefinition(myMetadataReader, typeDefinition);
      }

      return null;
    }

    // TODO: GetBaseTypeSpecification
    // TODO: GetBaseTypeReference

    [NotNull, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebugView
    {
      get { return "[typedef] " + FullName; }
    }

    public override string ToString()
    {
      return FullName;
    }
  }
}