using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.ILReader;
using System.Reflection.Metadata.Model;
using System.Reflection.PortableExecutable;

internal class Program
{
  private static void Main()
  {
    var dllFiles = Directory.GetFiles(@"C:\Work\ReSharper\bin.resharper", "*.dll");

    long ilBytes = 0, instructionsCount = 0;
    var stopwatch = Stopwatch.StartNew();

    for (int i = 0; i < 10; i++)
    foreach (var dllFile in dllFiles)
    {
      using (var dllStream = new FileStream(dllFile, FileMode.Open, FileAccess.Read, FileShare.Read))
      using (var peReader = new PEReader(dllStream, PEStreamOptions.PrefetchEntireImage))
      {
        if (!peReader.HasMetadata) continue;

        var metadataReader = peReader.GetMetadataReader();

        foreach (var methodDefinition in metadataReader.GetMethodDefinitions())
        {
          var containingType = methodDefinition.ContainingType;
          if (containingType.HasValue)
          {
            //var fullName = containingType.Value.FullName;
            //var methodName = methodDefinition.Name;
            //GC.KeepAlive(fullName + methodName);
          }

          var ilBody = MetadataILBody.TryCreate(peReader, methodDefinition.Definition, ThreadLocalPooledILBodyReaderAllocator.Instance);
          if (ilBody == null) continue;

          Process1(ilBody, metadataReader);

          ilBytes = Math.Max(ilBytes, ilBody.BodySize);
          instructionsCount += ilBody.Instructions.Length;
        }
      }
    }

    Console.WriteLine(ilBytes);
    Console.WriteLine(instructionsCount);

    var collections = string.Join("/",
      Enumerable.Range(0, GC.MaxGeneration + 1).Select(GC.CollectionCount));

    Console.WriteLine("GC: {0}", collections);
    Console.WriteLine("Time: {0}", stopwatch.Elapsed);
  }

  private static void Process1(ILBody body, MetadataReader metadataReader)
  {
    var instructions = body.Instructions;

    for (var index = 1; index < instructions.Length; index++)
    {
      if (instructions[index].Code == Opcode.Throw)
      {
        if (instructions[index - 1].Code == Opcode.Newobj)
        {
          //var methodHandle = instructions[index - 1].MethodHandle;

        }
      }
    }
  }
}
