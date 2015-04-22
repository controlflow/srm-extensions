using System.Collections.Generic;
using JetBrains.Annotations;

namespace System.Reflection.Metadata.Extensions
{
  public static class MetadataReaderExtensions
  {
    [NotNull]
    public static IEnumerable<MetadataTypeDefinition> GetMetadataTypeDefinitions([NotNull] this MetadataReader metadataReader)
    {
      foreach (var definitionHandle in metadataReader.TypeDefinitions)
      {
        var typeDefinition = metadataReader.GetTypeDefinition(definitionHandle);
        yield return new MetadataTypeDefinition(metadataReader, typeDefinition);
      }
    }

    [NotNull]
    public static IEnumerable<MetadataTypeReference> GetMetadataTypeReferences([NotNull] this MetadataReader metadataReader)
    {
      foreach (var definitionHandle in metadataReader.TypeReferences)
      {
        var typeReference = metadataReader.GetTypeReference(definitionHandle);
        yield return new MetadataTypeReference(metadataReader, typeReference);
      }
    }

    [NotNull]
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