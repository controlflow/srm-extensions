using System.Collections.Generic;
using System.Reflection.Metadata.Decoding;
using System.Reflection.PortableExecutable;
using JetBrains.Annotations;

namespace System.Reflection.Metadata.ILReader
{
  public sealed class MetadataILBody : ILBody
  {
    private readonly MethodDefinition myMethodDefinition;
    [NotNull] private readonly MethodBodyBlock myMethodBodyBlock;

    private MetadataILBody(MethodDefinition methodDefinition, [NotNull] MethodBodyBlock methodBodyBlock, [NotNull] Instruction[] instructions) : base(instructions, methodBodyBlock.Size)
    {
      myMethodDefinition = methodDefinition;
      myMethodBodyBlock = methodBodyBlock;
    }

    public MethodDefinition Definition
    {
      get { return myMethodDefinition; }
    }

    [NotNull] public MethodBodyBlock BodyBlock
    {
      get { return myMethodBodyBlock; }
    }

    [CanBeNull] // todo: pass MetadataReader?
    public static ILBody TryCreate([NotNull] PEReader reader, MethodDefinition methodDefinition)
    {
      var virtualAddress = methodDefinition.RelativeVirtualAddress;
      if (virtualAddress == 0) return null; // abstract method

      MethodBodyBlock methodBodyBlock;
      try
      {
        methodBodyBlock = reader.GetMethodBody(virtualAddress);
      }
      catch (BadImageFormatException) { return null; }
      catch (InvalidOperationException) { return null; }

      // blob reader is then passed _by value_, this is important
      var blobReader = methodBodyBlock.GetILReader();

      var info = new BodyInspectionInfo();
      info.SortedJumps = new List<MsilJump>();

      try
      {
        ILReaderImpl.Pass1(blobReader, ref info);
      }
      catch (InvalidOperationException) { return null; }

      // todo: pass inside
      var metadataReader = reader.GetMetadataReader(MetadataReaderOptions.None);

      var standaloneSignatureHandle = methodBodyBlock.LocalSignature;
      if (!standaloneSignatureHandle.IsNil)
      {
        var standaloneSignature = metadataReader.GetStandaloneSignature(standaloneSignatureHandle);
        var localSignature = SignatureDecoder.DecodeLocalSignature(
          standaloneSignatureHandle, new MetadataModelTypeProvider(metadataReader));
      }

      var instructions = ILReaderImpl.Pass2(blobReader, info.InstructionsCount);
      foreach (var instruction in instructions)
      {
        
      }

      return new MetadataILBody(methodDefinition, methodBodyBlock, instructions);
    }
  }
}