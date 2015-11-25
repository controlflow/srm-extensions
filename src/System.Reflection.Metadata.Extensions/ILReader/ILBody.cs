using JetBrains.Annotations;

namespace System.Reflection.Metadata.ILReader
{
  public abstract class ILBody
  {
    protected ILBody([NotNull] Instruction[] instructions, int methodBodySize)
    {
      Instructions = instructions;
      BodySize = methodBodySize;
    }

    [NotNull] public Instruction[] Instructions { get; }
    public int BodySize { get; }

    //internal abstract T ReadFromBody<T>(Func<BlobReader, T> reader);
  }
}