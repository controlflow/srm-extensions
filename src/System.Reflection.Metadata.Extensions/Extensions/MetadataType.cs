using System.Collections.Immutable;
using System.Diagnostics;
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
        var handle = myTypeDefinition.Namespace;
        if (handle.IsNil) return Name;

        var nameSpace = myMetadataReader.GetString(myTypeDefinition.Namespace);
        return nameSpace + "." + Name;
      }
    }

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

    public MetadataType? GetBaseType()
    {
      var baseType = myTypeDefinition.BaseType;
      if (baseType.Kind == HandleKind.TypeDefinition)
      {
        var typeDefinition = myMetadataReader.GetTypeDefinition((TypeDefinitionHandle) baseType);

        return new MetadataType(myMetadataReader, typeDefinition);
      }
      else if (baseType.Kind == HandleKind.TypeDefinition)
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