using System.Collections.Generic;
using JetBrains.Annotations;

namespace System.Reflection.Metadata.ILReader
{
  public interface IILBodyReaderAllocator
  {
    [NotNull] Instruction[] AllocateInstructionsArray(int size);
    [NotNull] List<InstructionJump> AllocateJumpsList();
  }

  public sealed class TrivialILBodyReaderAllocator : IILBodyReaderAllocator
  {
    private TrivialILBodyReaderAllocator() { }
    [NotNull] public static readonly IILBodyReaderAllocator Instance = new TrivialILBodyReaderAllocator();

    public Instruction[] AllocateInstructionsArray(int size) => new Instruction[size];
    public List<InstructionJump> AllocateJumpsList() => new List<InstructionJump>(4);
  }

  public sealed class ThreadLocalPooledILBodyReaderAllocator : IILBodyReaderAllocator
  {
    [ThreadStatic, CanBeNull] private static Dictionary<int, Instruction[]> InstructionArraysCache;
    [ThreadStatic, CanBeNull] private static List<InstructionJump> InstructionJumpsListCache;

    private ThreadLocalPooledILBodyReaderAllocator() { }
    [NotNull] public static readonly IILBodyReaderAllocator Instance = new ThreadLocalPooledILBodyReaderAllocator();

    public Instruction[] AllocateInstructionsArray(int size)
    {
      if (size > 200) return new Instruction[size];

      var instructionsCache = InstructionArraysCache ?? (InstructionArraysCache = new Dictionary<int, Instruction[]>());

      Instruction[] array;
      if (instructionsCache.TryGetValue(size, out array)) return array;

      instructionsCache[size] = array = new Instruction[size];
      return array;
    }

    public List<InstructionJump> AllocateJumpsList()
    {
      var instructionJumps = InstructionJumpsListCache ?? (InstructionJumpsListCache = new List<InstructionJump>());
      instructionJumps.Clear();

      return instructionJumps;
    }

    public static void Clear()
    {
      InstructionArraysCache?.Clear();
      InstructionJumpsListCache = null;
    }
  }
}