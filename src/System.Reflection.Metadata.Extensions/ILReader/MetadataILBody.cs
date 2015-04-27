using System.Collections.Generic;
using System.Reflection.Metadata.Decoding;
using System.Reflection.Metadata.Model;
using System.Reflection.PortableExecutable;
using JetBrains.Annotations;

namespace System.Reflection.Metadata.ILReader
{
  public sealed class MetadataILBody : ILBody
  {
    private readonly MethodDefinition myMethodDefinition;
    [NotNull] private readonly MethodBodyBlock myMethodBodyBlock;

    public MetadataILBody(MethodDefinition methodDefinition, [NotNull] MethodBodyBlock methodBodyBlock, [NotNull] Instruction[] instructions) : base(instructions, methodBodyBlock.Size)
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

      var metadataReader = reader.GetMetadataReader(MetadataReaderOptions.None);

      var standaloneSignatureHandle = methodBodyBlock.LocalSignature;
      if (!standaloneSignatureHandle.IsNil)
      {
        var signature = metadataReader.GetStandaloneSignature(standaloneSignatureHandle);


        var localSignature = SignatureDecoder.DecodeLocalSignature(standaloneSignatureHandle, new Foo());

        

        var varSigReader = metadataReader.GetBlobReader(signature.Signature);
        var signatureHeader = varSigReader.ReadSignatureHeader();

        if (signatureHeader.Kind == SignatureKind.LocalVariables)
        {
          var variablesCount = varSigReader.ReadCompressedInteger();

          if (variablesCount > 0)
          {
            
          }

          //varSigReader.ReadSignatureTypeCode()

          //blobReader.ReadSignatureTypeCode()
        }

        //var variablesCount = signatureHeader.RawValue;

        var typeCode = varSigReader.ReadSignatureTypeCode();




      }

      var instructions = ILReaderImpl.Pass2(blobReader, info.InstructionsCount);
      return new MetadataILBody(methodDefinition, methodBodyBlock, instructions);
    }
  }

  public class Foo : ISignatureTypeProvider<MetadataTypeReference>
  {
  }
}