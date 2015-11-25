using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.Reflection.Metadata.ILReader
{
  [StructLayout(LayoutKind.Auto)]
  [DebuggerDisplay("{Target} <- {Source}")]
  public struct InstructionJump : IComparable<InstructionJump>
  {
    public readonly int Target;
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
}