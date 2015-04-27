using JetBrains.Annotations;

namespace System.Reflection.Metadata.ILReader
{
  public abstract class ILBody
  {
    [NotNull] private readonly Instruction[] myInstructions;
    private readonly int myMethodBodySize;

    protected ILBody([NotNull] Instruction[] instructions, int methodBodySize)
    {
      myInstructions = instructions;
      myMethodBodySize = methodBodySize;
    }

    [NotNull] public Instruction[] Instructions
    {
      get { return myInstructions; }
    }

    public int BodySize
    {
      get { return myMethodBodySize; }
    }
  }
}