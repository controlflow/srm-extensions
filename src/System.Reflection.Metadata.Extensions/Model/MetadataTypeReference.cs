using System.Diagnostics;
using System.Text;
using JetBrains.Annotations;

namespace System.Reflection.Metadata.Model
{
  [DebuggerDisplay("{DebugView,nq}")]
  public struct MetadataTypeReference
  {
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [NotNull] private readonly MetadataReader myMetadataReader;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private TypeReference myTypeReference;

    public MetadataTypeReference([NotNull] MetadataReader metadataReader, TypeReference typeReference)
    {
      myMetadataReader = metadataReader;
      myTypeReference = typeReference;
    }

    public TypeReference Reference => myTypeReference;

    [NotNull] public string Name => myMetadataReader.GetString(myTypeReference.Name);

    [NotNull] public string FullName
    {
      get
      {
        var resolutionScope = myTypeReference.ResolutionScope;
        if (resolutionScope.Kind == HandleKind.TypeReference)
        {
          var builder = new StringBuilder();
          BuildFullName(builder, (TypeReferenceHandle) resolutionScope);
          builder.Append('+').Append(Name);
          return builder.ToString();
        }

        var namespaceHandle = myTypeReference.Namespace;
        if (namespaceHandle.IsNil) return Name;

        var nameSpace = myMetadataReader.GetString(namespaceHandle);
        return nameSpace + "." + Name;
      }
    }

    private void BuildFullName([NotNull] StringBuilder builder, TypeReferenceHandle type)
    {
      var typeReference = myMetadataReader.GetTypeReference(type);

      var resolutionScope = typeReference.ResolutionScope;
      if (resolutionScope.Kind == HandleKind.TypeReference)
      {
        BuildFullName(builder, (TypeReferenceHandle) resolutionScope);
        builder.Append('+');
      }
      else
      {
        var namespaceHandle = typeReference.Namespace;
        if (!namespaceHandle.IsNil)
        {
          var namespaceName = myMetadataReader.GetString(namespaceHandle);
          builder.Append(namespaceName).Append('.');
        }
      }

      var shortName = myMetadataReader.GetString(typeReference.Name);
      builder.Append(shortName);
    }

    [NotNull] public string Namespace
    {
      get
      {
        var type = myTypeReference;

        for (var resolutionScope = type.ResolutionScope;
             !resolutionScope.IsNil && resolutionScope.Kind == HandleKind.TypeReference; )
        {
          type = myMetadataReader.GetTypeReference((TypeReferenceHandle) resolutionScope);
          resolutionScope = type.ResolutionScope;
        }

        var namespaceHandle = type.Namespace;
        if (namespaceHandle.IsNil) return string.Empty;

        return myMetadataReader.GetString(namespaceHandle);
      }
    }

    public MetadataTypeReference? ContainingType
    {
      get
      {
        var resolutionScope = myTypeReference.ResolutionScope;
        if (resolutionScope.Kind != HandleKind.TypeReference) return null;

        var typeReference = myMetadataReader.GetTypeReference((TypeReferenceHandle) resolutionScope);
        return new MetadataTypeReference(myMetadataReader, typeReference);
      }
    }

    public bool IsNested
    {
      get
      {
        var resolutionScope = myTypeReference.ResolutionScope;
        return resolutionScope.Kind == HandleKind.TypeReference;
      }
    }

    public MetadataAssemblyReference Assembly
    {
      get
      {
        var resolutionScope = myTypeReference.ResolutionScope;
        while (resolutionScope.Kind == HandleKind.TypeReference)
        {
          var typeReference = myMetadataReader.GetTypeReference((TypeReferenceHandle) resolutionScope);
          resolutionScope = typeReference.ResolutionScope;
        }

        if (resolutionScope.Kind == HandleKind.AssemblyReference)
        {
          var assemblyReference = myMetadataReader.GetAssemblyReference((AssemblyReferenceHandle) resolutionScope);
          return new MetadataAssemblyReference(myMetadataReader, assemblyReference);
        }

        throw new NotImplementedException();
      }
    }

    // todo: assembly resolved
    public MetadataTypeDefinition? ResolveTypeDefinition()
    {
      var resolutionScope = myTypeReference.ResolutionScope;
      if (resolutionScope.IsNil)
      {
        var exportedType = myMetadataReader.GetExportedType((ExportedTypeHandle) resolutionScope);
        //exportedType.GetTypeDefinitionId()
      }

      if (resolutionScope.Kind == HandleKind.AssemblyReference)
      {
        var assemblyReference = myMetadataReader.GetAssemblyReference((AssemblyReferenceHandle) resolutionScope);
        
      }

      // todo: other cases
      return null;
    }

    [NotNull, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebugView => "[typeref] " + FullName;

    public override string ToString()
    {
      return FullName;
    }
  }
}