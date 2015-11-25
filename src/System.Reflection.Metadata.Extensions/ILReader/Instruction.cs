using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Model;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Reflection.Metadata.ILReader
{
  [StructLayout(LayoutKind.Auto)]
  [SuppressMessage("ReSharper", "NotResolvedInText")]
  public struct Instruction
  {
    internal Opcode myCode;
    internal int myOperand;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Opcode Code => myCode;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Int32Value
    {
      get
      {
        AssertInt32Value();
        return myOperand;
      }
    }

    [Conditional("DEBUG"), MethodImpl(MethodImplOptions.NoInlining)]
    private void AssertInt32Value()
    {
      if (myCode != Opcode.LdcI4)
        throw new ArgumentOutOfRangeException("myCode", myCode, "Unexpected opcode type");
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int ArgumentIndex
    {
      get
      {
        AssertArgumentIndex();
        return myOperand;
      }
    }

    [Conditional("DEBUG"), MethodImpl(MethodImplOptions.NoInlining)]
    private void AssertArgumentIndex()
    {
      switch (myCode)
      {
        case Opcode.Ldarg:
        case Opcode.Ldarga:
          return;

        default:
          throw new ArgumentOutOfRangeException("myCode", myCode, "Unexpected opcode type");
      }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int BranchTarget
    {
      get
      {
        AssertBranchTarget();
        return myOperand;
      }
    }

    [Conditional("DEBUG"), MethodImpl(MethodImplOptions.NoInlining)]
    private void AssertBranchTarget()
    {
      switch (myCode)
      {
        case Opcode.Beq:
        case Opcode.Bge:
        case Opcode.BgeUn:
        case Opcode.Bgt:
        case Opcode.BgtUn:
        case Opcode.Ble:
        case Opcode.BleUn:
        case Opcode.Blt:
        case Opcode.BltUn:
        case Opcode.BneUn:
        case Opcode.Br:
        case Opcode.Brfalse:
        case Opcode.Brtrue:
        case Opcode.Leave:
          return;
        default:
          throw new ArgumentOutOfRangeException("myCode", myCode, "Unexpected opcode type");
      }
    }

    // todo: int[] GetSwitchTargets(ILStream context)
    //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int[] GetSwitchTargets(ILBody body)
    {
      // todo: get from info
      //body.BodySize

      {
        AssertSwitchTargets();
        //return (int[]) myOperand;
        return null;
      }
    }

    [Conditional("DEBUG"), MethodImpl(MethodImplOptions.NoInlining)]
    private void AssertSwitchTargets()
    {
      switch (myCode)
      {
        case Opcode.Switch:
          return;
        default:
          throw new ArgumentOutOfRangeException("myCode", myCode, "Unexpected opcode type");
      }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Handle TypeHandle
    {
      get
      {
        AssertTypeHandle();
        return RawHandle.From((uint) myOperand);
      }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TypeToken
    {
      get
      {
        AssertTypeHandle();
        return myOperand;
      }
    }

    [Conditional("DEBUG"), MethodImpl(MethodImplOptions.NoInlining)]
    private void AssertTypeHandle()
    {
      switch (myCode)
      {
        case Opcode.Box:
        case Opcode.Unbox:
        case Opcode.UnboxAny:
        case Opcode.Castclass:
        case Opcode.Constrained:
        case Opcode.Cpobj:
          return;

        default:
          throw new ArgumentOutOfRangeException("myCode", myCode, "Unexpected opcode type");
      }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Handle MethodHandle
    {
      get
      {
        AssertMethodHandle();
        return RawHandle.From((uint) myOperand);
      }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int MethodToken
    {
      get
      {
        AssertMethodHandle();
        return myOperand;
      }
    }

    [Conditional("DEBUG"), MethodImpl(MethodImplOptions.NoInlining)]
    private void AssertMethodHandle()
    {
      switch (myCode)
      {
        case Opcode.Call:
        case Opcode.Callvirt:
          return;

        default:
          throw new ArgumentOutOfRangeException("myCode", myCode, "Unexpected opcode type");
      }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Handle SignatureHandle
    {
      get
      {
        AssertSignatureHandle();
        return RawHandle.From((uint) myOperand);
      }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int SignatureToken
    {
      get
      {
        AssertSignatureHandle();
        return myOperand;
      }
    }

    [Conditional("DEBUG"), MethodImpl(MethodImplOptions.NoInlining)]
    private void AssertSignatureHandle()
    {
      switch (myCode)
      {
        case Opcode.Calli:
          return;

        default:
          throw new ArgumentOutOfRangeException("myCode", myCode, "Unexpected opcode type");
      }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public ILType OperandType
    {
      get
      {
        AssertOperandType();
        return (ILType) myOperand;
      }
    }

    [Conditional("DEBUG"), MethodImpl(MethodImplOptions.NoInlining)]
    private void AssertOperandType()
    {
      switch (myCode)
      {
        case Opcode.Ldind:
        case Opcode.Stind:
        case Opcode.Ldelem:
        case Opcode.Stelem:
        case Opcode.Conv:
        case Opcode.ConvUn:
        case Opcode.ConvOvf:
        case Opcode.ConvOvfUn:
          return;

        default:
          throw new ArgumentOutOfRangeException("myCode", myCode, "Unexpected opcode type");
      }
    }

    public override string ToString()
    {
      var opcode = myCode.ToString().ToLowerInvariant();
      return $"{opcode} {myOperand}";
    }
  }
}