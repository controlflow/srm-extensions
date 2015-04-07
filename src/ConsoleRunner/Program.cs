using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Extensions;
using System.Reflection.Metadata.ILReader;
using System.Reflection.PortableExecutable;

class Program
{
  static void Main()
  {
    var dllFiles = Directory.GetFiles(@"C:\Work\ReSharper\bin", "*.dll");


    long ilBytes = 0, instructionsCount = 0;
    var stopwatch = Stopwatch.StartNew();

    foreach (var dllFile in dllFiles)
    {
      using (var dllStream = new FileStream(dllFile, FileMode.Open, FileAccess.Read, FileShare.Read))
      using (var peReader = new PEReader(dllStream, PEStreamOptions.PrefetchEntireImage))
      {
        var metadataReader = peReader.GetMetadataReader();

        foreach (var methodDefinition in metadataReader.GetMethodDefinitions())
        {
          //var typeDefinition = metadataReader.GetTypeDefinition(methodDefinition.GetDeclaringType());

          //var methodName = metadataReader.GetString(methodDefinition.Name);
          //var typeName = metadataReader.GetString(typeDefinition.Name);
          //var namespaceName = metadataReader.GetString(typeDefinition.Namespace);

          var virtualAddress = methodDefinition.RelativeVirtualAddress;
          if (virtualAddress == 0) continue;

          var methodBodyBlock = peReader.GetMethodBody(virtualAddress);

          var ilReader = methodBodyBlock.GetILReader();

          ilBytes = Math.Max(ilBytes, ilReader.RemainingBytes);

          var instructions = new List<Instruction>(ilReader.RemainingBytes / 2);
          try
          {
            ILReaderImpl.Read(ilReader, instructions);
          }
          catch
          {
            foreach (var instruction in instructions)
            {
              Console.WriteLine(instruction);
            }
          
            throw;
          }

          instructionsCount += instructions.Count;

          //Console.WriteLine("{0}.{1}.{2}: {3}/{4}",
          //  namespaceName, typeName, methodName, methodBodyBlock.GetILBytes().Length, instructions.Count);
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
