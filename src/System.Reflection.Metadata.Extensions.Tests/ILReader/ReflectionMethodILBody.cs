using System.Reflection.Metadata.ILReader;
using JetBrains.Annotations;

namespace System.Reflection.Metadata.Extensions.Tests.ILReader
{
  public class ReflectionMethodILBody : ILBody
  {
    public ReflectionMethodILBody([NotNull] Instruction[] instructions, int methodBodySize)
      : base(instructions, methodBodySize) { }

    [CanBeNull]
    public static unsafe ReflectionMethodILBody TryCreate([NotNull] MethodBody methodBody)
    {
      var ilStream = methodBody.GetILAsByteArray();

      fixed (byte* stream = ilStream)
      {
        var blobReader = new BlobReader(stream, ilStream.Length);

        var instructions = ILReaderImpl.TryRead(blobReader);
        if (instructions == null) return null;

        return new ReflectionMethodILBody(instructions, ilStream.Length);
      }
    }
  }
}