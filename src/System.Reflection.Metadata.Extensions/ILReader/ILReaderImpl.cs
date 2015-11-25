using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace System.Reflection.Metadata.ILReader
{
  public static partial class ILReaderImpl
  {
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void UnexpectedOpcode(int offset)
    {
      throw new InvalidOperationException($"Unexpected opcode at offset: 0x{offset:X}");
    }

    [StructLayout(LayoutKind.Auto)]
    internal struct FirstPassInfo
    {
      public int InstructionsCount;
      [CanBeNull] public List<InstructionJump> SortedJumps;

      [NotNull] private static readonly List<InstructionJump> UnreachableJump = new List<InstructionJump> { InstructionJump.Unreachable };

      [NotNull] public List<InstructionJump> GetJumpsOrUnreachable()
      {
        return SortedJumps ?? UnreachableJump;
      }
    }

    [StructLayout(LayoutKind.Auto)]
    [DebuggerDisplay("{Target} <- {Source}")]
    internal struct InstructionJump : IComparable<InstructionJump>
    {
      public readonly int Target; // offset or index
      public readonly int Source;

      public InstructionJump(int target, int source)
      {
        Target = target;
        Source = source;
      }

      public int CompareTo(InstructionJump other)
      {
        // ReSharper disable once ImpureMethodCallOnReadonlyValueField
        return Target.CompareTo(other.Target);
      }

      public static InstructionJump Unreachable = new InstructionJump(-1, -1);
    }

    [CanBeNull]
    public static Instruction[] TryRead(BlobReader reader)
    {
      try
      {
        var info = InspectionPass(reader);
        return DecodingPass(reader, info);
      }
      catch (InvalidOperationException)
      {
        return null;
      }
    }
  }
}