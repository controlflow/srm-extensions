using System.Collections.Generic;
using JetBrains.Annotations;

namespace System.Reflection.Metadata.ILReader
{
  public class ILBody
  {
    public ILBody([NotNull] Instruction[] instructions, [NotNull] List<InstructionJump> sortedJumps, int methodBodySize)
    {
      Instructions = instructions;
      SortedJumps = sortedJumps;
      BodySize = methodBodySize;
    }

    [NotNull] public Instruction[] Instructions { get; }
    [NotNull] public List<InstructionJump> SortedJumps { get; }
    public int BodySize { get; }
  }
}