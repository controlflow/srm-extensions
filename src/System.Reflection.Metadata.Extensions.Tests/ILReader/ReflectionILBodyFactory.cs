using System.Reflection.Metadata.ILReader;
using JetBrains.Annotations;

namespace System.Reflection.Metadata.Extensions.Tests.ILReader
{
  public class ReflectionILBodyFactory
  {
    public static unsafe ILBody TryCreate([NotNull] MethodBody methodBody, [NotNull] IILBodyReaderAllocator allocator)
    {
      var ilStream = methodBody.GetILAsByteArray();

      fixed (byte* stream = ilStream)
      {
        var blobReader = new BlobReader(stream, ilStream.Length);

        return ILReaderImpl.TryRead(blobReader, allocator);
      }
    }
  }
}