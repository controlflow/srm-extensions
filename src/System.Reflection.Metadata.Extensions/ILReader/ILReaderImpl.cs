using System.Collections.Generic;
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

    [CanBeNull]
    public static ILBody TryRead(BlobReader reader, [NotNull] IILBodyReaderAllocator allocator)
    {
      try
      {
        var info = InspectionPass(reader, allocator);
        var instructions = DecodingPass(reader, info, allocator);

        return new ILBody(instructions, info.SortedJumps, reader.Length);
      }
      catch (InvalidOperationException)
      {
        return null;
      }
    }
  }
}