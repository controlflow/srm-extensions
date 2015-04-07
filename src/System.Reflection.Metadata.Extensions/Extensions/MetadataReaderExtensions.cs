﻿using System.Collections.Generic;
using JetBrains.Annotations;

namespace System.Reflection.Metadata.Extensions
{
  public static class MetadataReaderExtensions
  {
    [NotNull]
    public static IEnumerable<MethodDefinition> GetMethodDefinitions([NotNull] this MetadataReader metadataReader)
    {
      foreach (var definitionHandle in metadataReader.MethodDefinitions)
      {
        yield return metadataReader.GetMethodDefinition(definitionHandle);
      }
    }


  }
}