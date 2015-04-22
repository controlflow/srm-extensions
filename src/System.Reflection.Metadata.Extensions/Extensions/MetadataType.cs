using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;
using JetBrains.Annotations;

namespace System.Reflection.Metadata.Extensions
{
  [DebuggerDisplay("{FullName,nq}")]
  public struct MetadataType
  {
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [NotNull] private readonly MetadataReader myMetadataReader;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private TypeDefinition myTypeDefinition;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private ImmutableArray<MetadataMethod> myMethods;

    public MetadataType([NotNull] MetadataReader metadataReader, TypeDefinition typeDefinition)
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

        var handle = myTypeDefinition.Namespace;
        if (handle.IsNil) return Name;

        var nameSpace = myMetadataReader.GetString(myTypeDefinition.Namespace);
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

    // todo: what about nested types?
    [NotNull] public string Namespace
    {
      get
      {
        var handle = myTypeDefinition.Namespace;
        if (handle.IsNil) return string.Empty;

        return myMetadataReader.GetString(myTypeDefinition.Namespace);
      }
    }

    public MetadataType? ContainingType
    {
      get
      {
        var declaringType = myTypeDefinition.GetDeclaringType();
        if (declaringType.IsNil) return null;

        var typeDefinition = myMetadataReader.GetTypeDefinition(declaringType);
        return new MetadataType(myMetadataReader, typeDefinition);
      }
    }

    public bool IsType
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

        var baseType = myTypeDefinition.BaseType;
        if (baseType.Kind == HandleKind.TypeDefinition)
        {
          var typeDefinition = myMetadataReader.GetTypeDefinition((TypeDefinitionHandle)baseType);

          var namespaceDefinitionHandle = typeDefinition.Namespace;
          if (!namespaceDefinitionHandle.IsNil)
          {
            var definition = myMetadataReader.GetString(namespaceDefinitionHandle);
            if (definition == "System")
            {
              var s = myMetadataReader.GetString(typeDefinition.Name);
              return s == "ValueType" || s == "Enum";
            }
          }
        }
        else if (baseType.Kind == HandleKind.TypeReference)
        {
          var typeReference = myMetadataReader.GetTypeReference((TypeReferenceHandle)baseType);

          var namespaceDefinitionHandle = typeReference.Namespace;
          if (!namespaceDefinitionHandle.IsNil)
          {
            var definition = myMetadataReader.GetString(namespaceDefinitionHandle);
            if (definition == "System")
            {
              var s = myMetadataReader.GetString(typeReference.Name);
              return s == "ValueType" || s == "Enum";
            }
          }
        }

        return false;
      }
    }

    public MetadataType? GetBaseType()
    {
      var baseType = myTypeDefinition.BaseType;
      if (baseType.Kind == HandleKind.TypeDefinition)
      {
        var typeDefinition = myMetadataReader.GetTypeDefinition((TypeDefinitionHandle) baseType);

        return new MetadataType(myMetadataReader, typeDefinition);
      }
      else if (baseType.Kind == HandleKind.TypeReference)
      {
        var typeReference = myMetadataReader.GetTypeReference((TypeReferenceHandle) baseType);

        //typeReference.ResolutionScope
      }


      return null;
    }

    public override string ToString()
    {
      return FullName;
    }
  }
}