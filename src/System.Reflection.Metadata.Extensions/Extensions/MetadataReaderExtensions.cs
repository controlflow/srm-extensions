using System.Collections.Generic;
using JetBrains.Annotations;

namespace System.Reflection.Metadata.Extensions
{
  public static class MetadataReaderExtensions
  {
    [NotNull]
    public static IEnumerable<MetadataType> GetMetadataTypes([NotNull] this MetadataReader metadataReader)
    {
      foreach (var definitionHandle in metadataReader.TypeDefinitions)
      {
        var typeDefinition = metadataReader.GetTypeDefinition(definitionHandle);

        yield return new MetadataType(metadataReader, typeDefinition);
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