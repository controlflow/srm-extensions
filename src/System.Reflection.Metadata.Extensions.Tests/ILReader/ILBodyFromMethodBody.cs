using System.Collections.Generic;
using System.Reflection.Metadata.ILReader;
using JetBrains.Annotations;

namespace System.Reflection.Metadata.Extensions.Tests.ILReader
{
  public class ILBodyFromMethodBody : ILBody
  {
    public ILBodyFromMethodBody([NotNull] Instruction[] instructions, int methodBodySize)
      : base(instructions, methodBodySize)
    {
    }

    [CanBeNull]
    public static unsafe ILBodyFromMethodBody TryCreate([NotNull] MethodBody methodBody)
    {
      var ilStream = methodBody.GetILAsByteArray();

      fixed (byte* stream = ilStream)
      {
        var blobReader = new BlobReader(stream, ilStream.Length);

        var info = new BodyInspectionInfo();
        info.SortedJumps = new List<MsilJump>();

        try
        {
          ILReaderImpl.Pass1(blobReader, ref info);
        }
        catch (InvalidOperationException) { return null; }

        var instructions = ILReaderImpl.Pass2(blobReader, info.InstructionsCount);
        return new ILBodyFromMethodBody(instructions, ilStream.Length);
      }
    }
  }
}