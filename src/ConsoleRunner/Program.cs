using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.ILReader;
using System.Reflection.Metadata.Model;
using System.Reflection.PortableExecutable;

class Program
{
  static void Main()
  {
    var builder = new IlReaderBuilder();
    var text = builder.BuildReadMethod();
    //var text = builder.BuildCountMethod();

    var dllFiles = Directory.GetFiles(@"C:\Work\ReSharper\bin", "*.dll");
    //var dllFiles = new[] {typeof (object).Assembly.Location};

    long ilBytes = 0, instructionsCount = 0;
    var stopwatch = Stopwatch.StartNew();

    //for (int i = 0; i < 10; i++)
    foreach (var dllFile in dllFiles)
    {
      using (var dllStream = new FileStream(dllFile, FileMode.Open, FileAccess.Read, FileShare.Read))
      using (var peReader = new PEReader(dllStream, PEStreamOptions.PrefetchEntireImage))
      {
        var metadataReader = peReader.GetMetadataReader();

        foreach (var methodDefinition in metadataReader.GetMethodDefinitions())
        {
          var ilBody = MetadataILBody.TryCreate(peReader, methodDefinition.Definition);
          if (ilBody == null) continue;

          ilBytes = Math.Max(ilBytes, ilBody.BodySize);
          instructionsCount += ilBody.Instructions.Length;
        }
      }
    }

    Console.WriteLine(ilBytes);
    Console.WriteLine(instructionsCount);

    var collections = string.Join("/", Enumerable.Range(0, GC.MaxGeneration + 1).Select(GC.CollectionCount));
    Console.WriteLine("GC: {0}", collections);
    Console.WriteLine("Time: {0}", stopwatch.Elapsed);
  }
}
