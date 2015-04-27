﻿using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Model;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace System.Reflection.Metadata.ILReader
{
  [StructLayout(LayoutKind.Auto)]
  [SuppressMessage("ReSharper", "NotResolvedInText")]
  public struct Instruction
  {
    internal Opcode myCode; // todo: rename to Code
    internal int myIntOperand; // todo: rename to Operand

    // todo: remove!
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Offset
    {
      //get { return myOffset; }
      get { return 0; }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Opcode Code
    {
      get { return myCode; }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int ValueInt32
    {
      get
      {
        AssertInt32();
        return myIntOperand;
      }
    }

    [Conditional("DEBUG")]
    private void AssertInt32()
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
        return myIntOperand;
      }
    }

    [Conditional("DEBUG")]
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
        return myIntOperand;
      }
    }

    [Conditional("DEBUG")]
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
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int[] SwitchTargets
    {
      get
      {
        AssertSwitchTargets();
        //return (int[]) myOperand;
        return null;
      }
    }

    [Conditional("DEBUG")]
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
        return RawHandle.From((uint) myIntOperand);
      }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int TypeToken
    {
      get
      {
        AssertTypeHandle();
        return myIntOperand;
      }
    }

    [Conditional("DEBUG")]
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
        return RawHandle.From((uint) myIntOperand);
      }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int MethodToken
    {
      get
      {
        AssertMethodHandle();
        return myIntOperand;
      }
    }

    [Conditional("DEBUG")]
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
        return RawHandle.From((uint) myIntOperand);
      }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int SignatureToken
    {
      get
      {
        AssertSignatureHandle();
        return myIntOperand;
      }
    }

    [Conditional("DEBUG")]
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
        return (ILType) myIntOperand;
      }
    }

    [Conditional("DEBUG")]
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

    // todo: typed operands accessors

    public override string ToString()
    {
      var opcode = myCode.ToString().ToLowerInvariant();
      return string.Format("IL{0:X4}: {1} {2}", Offset, opcode, 
        //myOperand ?? 
        myIntOperand);
    }
  }
}