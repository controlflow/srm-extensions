using System.Collections.Concurrent;
using System.IO;
using System.Reflection.PortableExecutable;
using JetBrains.Annotations;
using NUnit.Framework;

namespace System.Reflection.Metadata.Extensions.Tests.MetadataModel
{
  public abstract class MetadataModelTestBase
  {
    [NotNull] private readonly ConcurrentDictionary<Assembly, LoadedAssembly> myLoadedAssemblies = new ConcurrentDictionary<Assembly, LoadedAssembly>();

    private struct LoadedAssembly : IDisposable
    {
      [NotNull] public readonly PEReader PEReader;
      [NotNull] public readonly MetadataReader MetadataReader;

      public LoadedAssembly([NotNull] PEReader reader, [NotNull] MetadataReader metadataReader)
      {
        MetadataReader = metadataReader;
        PEReader = reader;
      }

      public void Dispose()
      {
        PEReader.Dispose();
      }
    }

    [TearDown]
    public void CloseAssembly()
    {
      foreach (var loadedAssembly in myLoadedAssemblies.Values)
        loadedAssembly.Dispose();

      myLoadedAssemblies.Clear();
    }

    protected MetadataTypeDefinition GetMetadataDefinition([NotNull] Type type)
    {
      Assert.NotNull(type, "type != null");
      Assert.IsFalse(type.IsConstructedGenericType, "type.IsConstructedGenericType");

      var loadedAssembly = myLoadedAssemblies.GetOrAdd(type.Assembly, LoadAssembly);
      var metadataTypes = loadedAssembly.MetadataReader.GetMetadataTypeDefinitions();

      foreach (var metadataType in metadataTypes)
      {
        if (metadataType.FullName == type.FullName) return metadataType;
      }

      throw new AssertionException(string.Format("Type is not found: {0}", type.FullName));
    }

    private static LoadedAssembly LoadAssembly([NotNull] Assembly assembly)
    {
      var assemblyStream = new FileStream(assembly.Location, FileMode.Open, FileAccess.Read, FileShare.Read);
      var reader = new PEReader(assemblyStream, PEStreamOptions.PrefetchEntireImage);
      var metadataReader = reader.GetMetadataReader();

      return new LoadedAssembly(reader, metadataReader);
    }
  }
}