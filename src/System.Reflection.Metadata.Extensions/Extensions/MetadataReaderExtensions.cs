using System.Collections.Generic;
using JetBrains.Annotations;

namespace System.Reflection.Metadata.Extensions
{
  public static class MetadataReaderExtensions
  {
    [NotNull, Pure]
    public static IEnumerable<MetadataTypeDefinition> GetMetadataTypeDefinitions([NotNull] this MetadataReader metadataReader)
    {
      foreach (var definitionHandle in metadataReader.TypeDefinitions)
      {
        var typeDefinition = metadataReader.GetTypeDefinition(definitionHandle);
        yield return new MetadataTypeDefinition(metadataReader, typeDefinition);
      }
    }

    [NotNull, Pure]
    public static IEnumerable<MetadataTypeReference> GetMetadataTypeReferences([NotNull] this MetadataReader metadataReader)
    {
      foreach (var referenceHandle in metadataReader.TypeReferences)
      {
        var typeReference = metadataReader.GetTypeReference(referenceHandle);
        yield return new MetadataTypeReference(metadataReader, typeReference);
      }
    }

    [NotNull, Pure]
    public static IEnumerable<MetadataAssemblyReference> GetMetadataAssemblyReferences([NotNull] this MetadataReader metadataReader)
    {
      foreach (var referenceHandle in metadataReader.AssemblyReferences)
      {
        var assemblyReference = metadataReader.GetAssemblyReference(referenceHandle);
        yield return new MetadataAssemblyReference(metadataReader, assemblyReference);
      }
    }

    [NotNull, Pure]
    public static IEnumerable<MetadataMethod> GetMethodDefinitions([NotNull] this MetadataReader metadataReader)
    {
      foreach (var definitionHandle in metadataReader.MethodDefinitions)
      {
        var definition = metadataReader.GetMethodDefinition(definitionHandle);
        yield return new MetadataMethod(metadataReader, definition);
      }
    }



  }
}