using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace System.Reflection.Metadata.ILReader
{
  [StructLayout(LayoutKind.Auto)]
  public struct Instruction
  {
    private readonly int myOffset;
    private readonly Opcode myCode;
    private readonly int myIntOperand;
    private readonly object myOperand;

    public Instruction(int offset, Opcode code)
    {
      myOffset = offset;
      myCode = code;
      myIntOperand = 0;
      myOperand = null;
    }

    public Instruction(int offset, Opcode code, int operand) : this(offset, code)
    {
      myIntOperand = operand;
    }

    public Instruction(int offset, Opcode code, long operand) : this(offset, code)
    {
      myOperand = operand;
    }

    public Instruction(int offset, Opcode code, [NotNull] int[] switchLabels) : this(offset, code)
    {
      myOperand = switchLabels;
    }

    public int Offset
    {
      get { return myOffset; }
    }

    public Opcode Code
    {
      get { return myCode; }
    }

    // todo: typed operands accessors

    public override string ToString()
    {
      var opcode = myCode.ToString().ToLowerInvariant();
      return string.Format("IL{0:X4}: {1} {2}", myOffset, opcode, myOperand ?? myIntOperand);
    }
  }
}