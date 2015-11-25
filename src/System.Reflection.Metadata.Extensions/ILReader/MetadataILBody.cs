using System.Collections.Generic;
using System.Reflection.Metadata.Decoding;
using System.Reflection.PortableExecutable;
using JetBrains.Annotations;

namespace System.Reflection.Metadata.ILReader
{
  public sealed class MetadataILBody
  {
    //public MethodDefinition Definition { get; }
    //[NotNull] public MethodBodyBlock BodyBlock { get; }

    [CanBeNull] // todo: pass MetadataReader?
    public static ILBody TryCreate([NotNull] PEReader reader, MethodDefinition methodDefinition, [NotNull] IILBodyReaderAllocator allocator)
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

      var body = ILReaderImpl.TryRead(blobReader, allocator);
      if (body == null) return null;

      // todo: pass inside
      //var metadataReader = reader.GetMetadataReader(MetadataReaderOptions.None);

      var standaloneSignatureHandle = methodBodyBlock.LocalSignature;
      if (!standaloneSignatureHandle.IsNil)
      {
        //var standaloneSignature = metadataReader.GetStandaloneSignature(standaloneSignatureHandle);
        //var localSignature = SignatureDecoder.DecodeLocalSignature(
        //  standaloneSignatureHandle, new MetadataModelTypeProvider(metadataReader));
      }

      return body;
    }
  }
}