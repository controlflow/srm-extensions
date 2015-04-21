using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace System.Reflection.Metadata.ILReader
{
  public static class ILReaderImpl
  {
    public static void Read(BlobReader reader, [NotNull] List<Instruction> instructions)
    {
      while (reader.RemainingBytes > 0)
      {
        var offset = reader.Offset;

        switch (reader.ReadByte())
        {
          case 0x00: // nop
            instructions.Add(new Instruction(offset, Opcode.Nop));
            continue;
          case 0x01: // break
            instructions.Add(new Instruction(offset, Opcode.Break));
            continue;
          case 0x02: // ldarg.0
            instructions.Add(new Instruction(offset, Opcode.Ldarg, 0));
            continue;
          case 0x03: // ldarg.1
            instructions.Add(new Instruction(offset, Opcode.Ldarg, 1));
            continue;
          case 0x04: // ldarg.2
            instructions.Add(new Instruction(offset, Opcode.Ldarg, 2));
            continue;
          case 0x05: // ldarg.3
            instructions.Add(new Instruction(offset, Opcode.Ldarg, 3));
            continue;
          case 0x06: // ldloc.0
            instructions.Add(new Instruction(offset, Opcode.Ldloc, 0));
            continue;
          case 0x07: // ldloc.1
            instructions.Add(new Instruction(offset, Opcode.Ldloc, 1));
            continue;
          case 0x08: // ldloc.2
            instructions.Add(new Instruction(offset, Opcode.Ldloc, 2));
            continue;
          case 0x09: // ldloc.3
            instructions.Add(new Instruction(offset, Opcode.Ldloc, 3));
            continue;
          case 0x0A: // stloc.0
            instructions.Add(new Instruction(offset, Opcode.Stloc, 0));
            continue;
          case 0x0B: // stloc.1
            instructions.Add(new Instruction(offset, Opcode.Stloc, 1));
            continue;
          case 0x0C: // stloc.2
            instructions.Add(new Instruction(offset, Opcode.Stloc, 2));
            continue;
          case 0x0D: // stloc.3
            instructions.Add(new Instruction(offset, Opcode.Stloc, 3));
            continue;
          case 0x0E: // ldarg.s
            instructions.Add(new Instruction(offset, Opcode.Ldarg, reader.ReadByte()));
            continue;
          case 0x0F: // ldarga.s
            instructions.Add(new Instruction(offset, Opcode.Ldarga, reader.ReadByte()));
            continue;
          case 0x10: // starg.s
            instructions.Add(new Instruction(offset, Opcode.Starg, reader.ReadByte()));
            continue;
          case 0x11: // ldloc.s
            instructions.Add(new Instruction(offset, Opcode.Ldloc, reader.ReadByte()));
            continue;
          case 0x12: // ldloca.s
            instructions.Add(new Instruction(offset, Opcode.Ldloca, reader.ReadByte()));
            continue;
          case 0x13: // stloc.s
            instructions.Add(new Instruction(offset, Opcode.Stloc, reader.ReadByte()));
            continue;
          case 0x14: // ldnull
            instructions.Add(new Instruction(offset, Opcode.Ldnull));
            continue;
          case 0x15: // ldc.i4.m1
            instructions.Add(new Instruction(offset, Opcode.LdcI4, -1));
            continue;
          case 0x16: // ldc.i4.0
            instructions.Add(new Instruction(offset, Opcode.LdcI4, 0));
            continue;
          case 0x17: // ldc.i4.1
            instructions.Add(new Instruction(offset, Opcode.LdcI4, 1));
            continue;
          case 0x18: // ldc.i4.2
            instructions.Add(new Instruction(offset, Opcode.LdcI4, 2));
            continue;
          case 0x19: // ldc.i4.3
            instructions.Add(new Instruction(offset, Opcode.LdcI4, 3));
            continue;
          case 0x1A: // ldc.i4.4
            instructions.Add(new Instruction(offset, Opcode.LdcI4, 4));
            continue;
          case 0x1B: // ldc.i4.5
            instructions.Add(new Instruction(offset, Opcode.LdcI4, 5));
            continue;
          case 0x1C: // ldc.i4.6
            instructions.Add(new Instruction(offset, Opcode.LdcI4, 6));
            continue;
          case 0x1D: // ldc.i4.7
            instructions.Add(new Instruction(offset, Opcode.LdcI4, 7));
            continue;
          case 0x1E: // ldc.i4.8
            instructions.Add(new Instruction(offset, Opcode.LdcI4, 8));
            continue;
          case 0x1F: // ldc.i4.s
            instructions.Add(new Instruction(offset, Opcode.LdcI4, reader.ReadByte()));
            continue;
          case 0x20: // ldc.i4
            instructions.Add(new Instruction(offset, Opcode.LdcI4, reader.ReadInt32()));
            continue;
          case 0x21: // ldc.i8
            instructions.Add(new Instruction(offset, Opcode.LdcI8, reader.ReadInt64()));
            continue;
          case 0x22: // ldc.r4
            instructions.Add(new Instruction(offset, Opcode.LdcR4, reader.ReadInt32()));
            continue;
          case 0x23: // ldc.r8
            instructions.Add(new Instruction(offset, Opcode.LdcR8, reader.ReadInt64()));
            continue;
          case 0x25: // dup
            instructions.Add(new Instruction(offset, Opcode.Dup));
            continue;
          case 0x26: // pop
            instructions.Add(new Instruction(offset, Opcode.Pop));
            continue;
          case 0x27: // jmp
            instructions.Add(new Instruction(offset, Opcode.Jmp, reader.ReadInt32()));
            continue;
          case 0x28: // call
            instructions.Add(new Instruction(offset, Opcode.Call, reader.ReadInt32()));
            continue;
          case 0x29: // calli
            instructions.Add(new Instruction(offset, Opcode.Calli, reader.ReadInt32()));
            continue;
          case 0x2A: // ret
            instructions.Add(new Instruction(offset, Opcode.Ret));
            continue;
          case 0x2B: // br.s
            instructions.Add(new Instruction(offset, Opcode.Br, reader.ReadSByte() + reader.Offset));
            continue;
          case 0x2C: // brfalse.s
            instructions.Add(new Instruction(offset, Opcode.Brfalse, reader.ReadSByte() + reader.Offset));
            continue;
          case 0x2D: // brtrue.s
            instructions.Add(new Instruction(offset, Opcode.Brtrue, reader.ReadSByte() + reader.Offset));
            continue;
          case 0x2E: // beq.s
            instructions.Add(new Instruction(offset, Opcode.Beq, reader.ReadSByte() + reader.Offset));
            continue;
          case 0x2F: // bge.s
            instructions.Add(new Instruction(offset, Opcode.Bge, reader.ReadSByte() + reader.Offset));
            continue;
          case 0x30: // bgt.s
            instructions.Add(new Instruction(offset, Opcode.Bgt, reader.ReadSByte() + reader.Offset));
            continue;
          case 0x31: // ble.s
            instructions.Add(new Instruction(offset, Opcode.Ble, reader.ReadSByte() + reader.Offset));
            continue;
          case 0x32: // blt.s
            instructions.Add(new Instruction(offset, Opcode.Blt, reader.ReadSByte() + reader.Offset));
            continue;
          case 0x33: // bne.un.s
            instructions.Add(new Instruction(offset, Opcode.BneUn, reader.ReadSByte() + reader.Offset));
            continue;
          case 0x34: // bge.un.s
            instructions.Add(new Instruction(offset, Opcode.BgeUn, reader.ReadSByte() + reader.Offset));
            continue;
          case 0x35: // bgt.un.s
            instructions.Add(new Instruction(offset, Opcode.BgtUn, reader.ReadSByte() + reader.Offset));
            continue;
          case 0x36: // ble.un.s
            instructions.Add(new Instruction(offset, Opcode.BleUn, reader.ReadSByte() + reader.Offset));
            continue;
          case 0x37: // blt.un.s
            instructions.Add(new Instruction(offset, Opcode.BltUn, reader.ReadSByte() + reader.Offset));
            continue;
          case 0x38: // br
            instructions.Add(new Instruction(offset, Opcode.Br, reader.ReadInt32() + reader.Offset));
            continue;
          case 0x39: // brfalse
            instructions.Add(new Instruction(offset, Opcode.Brfalse, reader.ReadInt32() + reader.Offset));
            continue;
          case 0x3A: // brtrue
            instructions.Add(new Instruction(offset, Opcode.Brtrue, reader.ReadInt32() + reader.Offset));
            continue;
          case 0x3B: // beq
            instructions.Add(new Instruction(offset, Opcode.Beq, reader.ReadInt32() + reader.Offset));
            continue;
          case 0x3C: // bge
            instructions.Add(new Instruction(offset, Opcode.Bge, reader.ReadInt32() + reader.Offset));
            continue;
          case 0x3D: // bgt
            instructions.Add(new Instruction(offset, Opcode.Bgt, reader.ReadInt32() + reader.Offset));
            continue;
          case 0x3E: // ble
            instructions.Add(new Instruction(offset, Opcode.Ble, reader.ReadInt32() + reader.Offset));
            continue;
          case 0x3F: // blt
            instructions.Add(new Instruction(offset, Opcode.Blt, reader.ReadInt32() + reader.Offset));
            continue;
          case 0x40: // bne.un
            instructions.Add(new Instruction(offset, Opcode.BneUn, reader.ReadInt32() + reader.Offset));
            continue;
          case 0x41: // bge.un
            instructions.Add(new Instruction(offset, Opcode.BgeUn, reader.ReadInt32() + reader.Offset));
            continue;
          case 0x42: // bgt.un
            instructions.Add(new Instruction(offset, Opcode.BgtUn, reader.ReadInt32() + reader.Offset));
            continue;
          case 0x43: // ble.un
            instructions.Add(new Instruction(offset, Opcode.BleUn, reader.ReadInt32() + reader.Offset));
            continue;
          case 0x44: // blt.un
            instructions.Add(new Instruction(offset, Opcode.BltUn, reader.ReadInt32() + reader.Offset));
            continue;
          case 0x45: // switch
            instructions.Add(new Instruction(offset, Opcode.Switch, ReadSwitch(ref reader)));
            continue;
          case 0x46: // ldind.i1
            instructions.Add(new Instruction(offset, Opcode.Ldind, 1));
            continue;
          case 0x47: // ldind.u1
            instructions.Add(new Instruction(offset, Opcode.Ldind, 5));
            continue;
          case 0x48: // ldind.i2
            instructions.Add(new Instruction(offset, Opcode.Ldind, 2));
            continue;
          case 0x49: // ldind.u2
            instructions.Add(new Instruction(offset, Opcode.Ldind, 6));
            continue;
          case 0x4A: // ldind.i4
            instructions.Add(new Instruction(offset, Opcode.Ldind, 3));
            continue;
          case 0x4B: // ldind.u4
            instructions.Add(new Instruction(offset, Opcode.Ldind, 7));
            continue;
          case 0x4C: // ldind.i8
            instructions.Add(new Instruction(offset, Opcode.Ldind, 4));
            continue;
          case 0x4D: // ldind.i
            instructions.Add(new Instruction(offset, Opcode.Ldind, 0));
            continue;
          case 0x4E: // ldind.r4
            instructions.Add(new Instruction(offset, Opcode.Ldind, 9));
            continue;
          case 0x4F: // ldind.r8
            instructions.Add(new Instruction(offset, Opcode.Ldind, 10));
            continue;
          case 0x50: // ldind.ref
            instructions.Add(new Instruction(offset, Opcode.LdindRef));
            continue;
          case 0x51: // stind.ref
            instructions.Add(new Instruction(offset, Opcode.StindRef));
            continue;
          case 0x52: // stind.i1
            instructions.Add(new Instruction(offset, Opcode.Stind, 1));
            continue;
          case 0x53: // stind.i2
            instructions.Add(new Instruction(offset, Opcode.Stind, 2));
            continue;
          case 0x54: // stind.i4
            instructions.Add(new Instruction(offset, Opcode.Stind, 3));
            continue;
          case 0x55: // stind.i8
            instructions.Add(new Instruction(offset, Opcode.Stind, 4));
            continue;
          case 0x56: // stind.r4
            instructions.Add(new Instruction(offset, Opcode.Stind, 9));
            continue;
          case 0x57: // stind.r8
            instructions.Add(new Instruction(offset, Opcode.Stind, 10));
            continue;
          case 0x58: // add
            instructions.Add(new Instruction(offset, Opcode.Add));
            continue;
          case 0x59: // sub
            instructions.Add(new Instruction(offset, Opcode.Sub));
            continue;
          case 0x5A: // mul
            instructions.Add(new Instruction(offset, Opcode.Mul));
            continue;
          case 0x5B: // div
            instructions.Add(new Instruction(offset, Opcode.Div));
            continue;
          case 0x5C: // div.un
            instructions.Add(new Instruction(offset, Opcode.DivUn));
            continue;
          case 0x5D: // rem
            instructions.Add(new Instruction(offset, Opcode.Rem));
            continue;
          case 0x5E: // rem.un
            instructions.Add(new Instruction(offset, Opcode.RemUn));
            continue;
          case 0x5F: // and
            instructions.Add(new Instruction(offset, Opcode.And));
            continue;
          case 0x60: // or
            instructions.Add(new Instruction(offset, Opcode.Or));
            continue;
          case 0x61: // xor
            instructions.Add(new Instruction(offset, Opcode.Xor));
            continue;
          case 0x62: // shl
            instructions.Add(new Instruction(offset, Opcode.Shl));
            continue;
          case 0x63: // shr
            instructions.Add(new Instruction(offset, Opcode.Shr));
            continue;
          case 0x64: // shr.un
            instructions.Add(new Instruction(offset, Opcode.ShrUn));
            continue;
          case 0x65: // neg
            instructions.Add(new Instruction(offset, Opcode.Neg));
            continue;
          case 0x66: // not
            instructions.Add(new Instruction(offset, Opcode.Not));
            continue;
          case 0x67: // conv.i1
            instructions.Add(new Instruction(offset, Opcode.Conv, 1));
            continue;
          case 0x68: // conv.i2
            instructions.Add(new Instruction(offset, Opcode.Conv, 2));
            continue;
          case 0x69: // conv.i4
            instructions.Add(new Instruction(offset, Opcode.Conv, 3));
            continue;
          case 0x6A: // conv.i8
            instructions.Add(new Instruction(offset, Opcode.Conv, 4));
            continue;
          case 0x6B: // conv.r4
            instructions.Add(new Instruction(offset, Opcode.Conv, 9));
            continue;
          case 0x6C: // conv.r8
            instructions.Add(new Instruction(offset, Opcode.Conv, 10));
            continue;
          case 0x6D: // conv.u4
            instructions.Add(new Instruction(offset, Opcode.Conv, 7));
            continue;
          case 0x6E: // conv.u8
            instructions.Add(new Instruction(offset, Opcode.Conv, 8));
            continue;
          case 0x6F: // callvirt
            instructions.Add(new Instruction(offset, Opcode.Callvirt, reader.ReadInt32()));
            continue;
          case 0x70: // cpobj
            instructions.Add(new Instruction(offset, Opcode.Cpobj, reader.ReadInt32()));
            continue;
          case 0x71: // ldobj
            instructions.Add(new Instruction(offset, Opcode.Ldobj, reader.ReadInt32()));
            continue;
          case 0x72: // ldstr
            instructions.Add(new Instruction(offset, Opcode.Ldstr, reader.ReadInt32()));
            continue;
          case 0x73: // newobj
            instructions.Add(new Instruction(offset, Opcode.Newobj, reader.ReadInt32()));
            continue;
          case 0x74: // castclass
            instructions.Add(new Instruction(offset, Opcode.Castclass, reader.ReadInt32()));
            continue;
          case 0x75: // isinst
            instructions.Add(new Instruction(offset, Opcode.Isinst, reader.ReadInt32()));
            continue;
          case 0x76: // conv.r.un
            instructions.Add(new Instruction(offset, Opcode.ConvUn, 9));
            continue;
          case 0x79: // unbox
            instructions.Add(new Instruction(offset, Opcode.Unbox, reader.ReadInt32()));
            continue;
          case 0x7A: // throw
            instructions.Add(new Instruction(offset, Opcode.Throw));
            continue;
          case 0x7B: // ldfld
            instructions.Add(new Instruction(offset, Opcode.Ldfld, reader.ReadInt32()));
            continue;
          case 0x7C: // ldflda
            instructions.Add(new Instruction(offset, Opcode.Ldflda, reader.ReadInt32()));
            continue;
          case 0x7D: // stfld
            instructions.Add(new Instruction(offset, Opcode.Stfld, reader.ReadInt32()));
            continue;
          case 0x7E: // ldsfld
            instructions.Add(new Instruction(offset, Opcode.Ldsfld, reader.ReadInt32()));
            continue;
          case 0x7F: // ldsflda
            instructions.Add(new Instruction(offset, Opcode.Ldsflda, reader.ReadInt32()));
            continue;
          case 0x80: // stsfld
            instructions.Add(new Instruction(offset, Opcode.Stsfld, reader.ReadInt32()));
            continue;
          case 0x81: // stobj
            instructions.Add(new Instruction(offset, Opcode.Stobj, reader.ReadInt32()));
            continue;
          case 0x82: // conv.ovf.i1.un
            instructions.Add(new Instruction(offset, Opcode.ConvOvfUn, 1));
            continue;
          case 0x83: // conv.ovf.i2.un
            instructions.Add(new Instruction(offset, Opcode.ConvOvfUn, 2));
            continue;
          case 0x84: // conv.ovf.i4.un
            instructions.Add(new Instruction(offset, Opcode.ConvOvfUn, 3));
            continue;
          case 0x85: // conv.ovf.i8.un
            instructions.Add(new Instruction(offset, Opcode.ConvOvfUn, 4));
            continue;
          case 0x86: // conv.ovf.u1.un
            instructions.Add(new Instruction(offset, Opcode.ConvOvfUn, 5));
            continue;
          case 0x87: // conv.ovf.u2.un
            instructions.Add(new Instruction(offset, Opcode.ConvOvfUn, 6));
            continue;
          case 0x88: // conv.ovf.u4.un
            instructions.Add(new Instruction(offset, Opcode.ConvOvfUn, 7));
            continue;
          case 0x89: // conv.ovf.u8.un
            instructions.Add(new Instruction(offset, Opcode.ConvOvfUn, 8));
            continue;
          case 0x8A: // conv.ovf.i.un
            instructions.Add(new Instruction(offset, Opcode.ConvOvfUn, 0));
            continue;
          case 0x8B: // conv.ovf.u.un
            instructions.Add(new Instruction(offset, Opcode.ConvOvfUn, 11));
            continue;
          case 0x8C: // box
            instructions.Add(new Instruction(offset, Opcode.Box, reader.ReadInt32()));
            continue;
          case 0x8D: // newarr
            instructions.Add(new Instruction(offset, Opcode.Newarr, reader.ReadInt32()));
            continue;
          case 0x8E: // ldlen
            instructions.Add(new Instruction(offset, Opcode.Ldlen));
            continue;
          case 0x8F: // ldelema
            instructions.Add(new Instruction(offset, Opcode.Ldelema, reader.ReadInt32()));
            continue;
          case 0x90: // ldelem.i1
            instructions.Add(new Instruction(offset, Opcode.Ldelem, 1));
            continue;
          case 0x91: // ldelem.u1
            instructions.Add(new Instruction(offset, Opcode.Ldelem, 5));
            continue;
          case 0x92: // ldelem.i2
            instructions.Add(new Instruction(offset, Opcode.Ldelem, 2));
            continue;
          case 0x93: // ldelem.u2
            instructions.Add(new Instruction(offset, Opcode.Ldelem, 6));
            continue;
          case 0x94: // ldelem.i4
            instructions.Add(new Instruction(offset, Opcode.Ldelem, 3));
            continue;
          case 0x95: // ldelem.u4
            instructions.Add(new Instruction(offset, Opcode.Ldelem, 7));
            continue;
          case 0x96: // ldelem.i8
            instructions.Add(new Instruction(offset, Opcode.Ldelem, 4));
            continue;
          case 0x97: // ldelem.i
            instructions.Add(new Instruction(offset, Opcode.Ldelem, 0));
            continue;
          case 0x98: // ldelem.r4
            instructions.Add(new Instruction(offset, Opcode.Ldelem, 9));
            continue;
          case 0x99: // ldelem.r8
            instructions.Add(new Instruction(offset, Opcode.Ldelem, 10));
            continue;
          case 0x9A: // ldelem.ref
            instructions.Add(new Instruction(offset, Opcode.LdelemRef));
            continue;
          case 0x9B: // stelem.i
            instructions.Add(new Instruction(offset, Opcode.Stelem, 0));
            continue;
          case 0x9C: // stelem.i1
            instructions.Add(new Instruction(offset, Opcode.Stelem, 1));
            continue;
          case 0x9D: // stelem.i2
            instructions.Add(new Instruction(offset, Opcode.Stelem, 2));
            continue;
          case 0x9E: // stelem.i4
            instructions.Add(new Instruction(offset, Opcode.Stelem, 3));
            continue;
          case 0x9F: // stelem.i8
            instructions.Add(new Instruction(offset, Opcode.Stelem, 4));
            continue;
          case 0xA0: // stelem.r4
            instructions.Add(new Instruction(offset, Opcode.Stelem, 9));
            continue;
          case 0xA1: // stelem.r8
            instructions.Add(new Instruction(offset, Opcode.Stelem, 10));
            continue;
          case 0xA2: // stelem.ref
            instructions.Add(new Instruction(offset, Opcode.StelemRef));
            continue;
          case 0xA3: // ldelem
            instructions.Add(new Instruction(offset, Opcode.Ldelem, reader.ReadInt32()));
            continue;
          case 0xA4: // stelem
            instructions.Add(new Instruction(offset, Opcode.Stelem, reader.ReadInt32()));
            continue;
          case 0xA5: // unbox.any
            instructions.Add(new Instruction(offset, Opcode.UnboxAny, reader.ReadInt32()));
            continue;
          case 0xB3: // conv.ovf.i1
            instructions.Add(new Instruction(offset, Opcode.ConvOvf, 1));
            continue;
          case 0xB4: // conv.ovf.u1
            instructions.Add(new Instruction(offset, Opcode.ConvOvf, 5));
            continue;
          case 0xB5: // conv.ovf.i2
            instructions.Add(new Instruction(offset, Opcode.ConvOvf, 2));
            continue;
          case 0xB6: // conv.ovf.u2
            instructions.Add(new Instruction(offset, Opcode.ConvOvf, 6));
            continue;
          case 0xB7: // conv.ovf.i4
            instructions.Add(new Instruction(offset, Opcode.ConvOvf, 3));
            continue;
          case 0xB8: // conv.ovf.u4
            instructions.Add(new Instruction(offset, Opcode.ConvOvf, 7));
            continue;
          case 0xB9: // conv.ovf.i8
            instructions.Add(new Instruction(offset, Opcode.ConvOvf, 4));
            continue;
          case 0xBA: // conv.ovf.u8
            instructions.Add(new Instruction(offset, Opcode.ConvOvf, 8));
            continue;
          case 0xC2: // refanyval
            instructions.Add(new Instruction(offset, Opcode.Refanyval, reader.ReadInt32()));
            continue;
          case 0xC3: // ckfinite
            instructions.Add(new Instruction(offset, Opcode.Ckfinite));
            continue;
          case 0xC6: // mkrefany
            instructions.Add(new Instruction(offset, Opcode.Mkrefany, reader.ReadInt32()));
            continue;
          case 0xD0: // ldtoken
            instructions.Add(new Instruction(offset, Opcode.Ldtoken, reader.ReadInt32()));
            continue;
          case 0xD1: // conv.u2
            instructions.Add(new Instruction(offset, Opcode.Conv, 6));
            continue;
          case 0xD2: // conv.u1
            instructions.Add(new Instruction(offset, Opcode.Conv, 5));
            continue;
          case 0xD3: // conv.i
            instructions.Add(new Instruction(offset, Opcode.Conv, 0));
            continue;
          case 0xD4: // conv.ovf.i
            instructions.Add(new Instruction(offset, Opcode.ConvOvf, 0));
            continue;
          case 0xD5: // conv.ovf.u
            instructions.Add(new Instruction(offset, Opcode.ConvOvf, 11));
            continue;
          case 0xD6: // add.ovf
            instructions.Add(new Instruction(offset, Opcode.AddOvf));
            continue;
          case 0xD7: // add.ovf.un
            instructions.Add(new Instruction(offset, Opcode.AddOvfUn));
            continue;
          case 0xD8: // mul.ovf
            instructions.Add(new Instruction(offset, Opcode.MulOvf));
            continue;
          case 0xD9: // mul.ovf.un
            instructions.Add(new Instruction(offset, Opcode.MulOvfUn));
            continue;
          case 0xDA: // sub.ovf
            instructions.Add(new Instruction(offset, Opcode.SubOvf));
            continue;
          case 0xDB: // sub.ovf.un
            instructions.Add(new Instruction(offset, Opcode.SubOvfUn));
            continue;
          case 0xDC: // endfinally
            instructions.Add(new Instruction(offset, Opcode.Endfinally));
            continue;
          case 0xDD: // leave
            instructions.Add(new Instruction(offset, Opcode.Leave, reader.ReadInt32() + reader.Offset));
            continue;
          case 0xDE: // leave.s
            instructions.Add(new Instruction(offset, Opcode.Leave, reader.ReadSByte() + reader.Offset));
            continue;
          case 0xDF: // stind.i
            instructions.Add(new Instruction(offset, Opcode.Stind, 0));
            continue;
          case 0xE0: // conv.u
            instructions.Add(new Instruction(offset, Opcode.Conv, 11));
            continue;
          case 0xF8: // prefix7
            instructions.Add(new Instruction(offset, Opcode.Prefix7));
            continue;
          case 0xF9: // prefix6
            instructions.Add(new Instruction(offset, Opcode.Prefix6));
            continue;
          case 0xFA: // prefix5
            instructions.Add(new Instruction(offset, Opcode.Prefix5));
            continue;
          case 0xFB: // prefix4
            instructions.Add(new Instruction(offset, Opcode.Prefix4));
            continue;
          case 0xFC: // prefix3
            instructions.Add(new Instruction(offset, Opcode.Prefix3));
            continue;
          case 0xFD: // prefix2
            instructions.Add(new Instruction(offset, Opcode.Prefix2));
            continue;
          //case 0xFE: // prefix1
          //  instructions.Add(new Instruction(offset, Opcode.Prefix1));
          //  continue;
          case 0xFF: // prefixref
            instructions.Add(new Instruction(offset, Opcode.Prefixref));
            continue;
          case 0xFE:
            switch (reader.ReadByte())
            {
              case 0x00: // arglist
                instructions.Add(new Instruction(offset, Opcode.Arglist));
                continue;
              case 0x01: // ceq
                instructions.Add(new Instruction(offset, Opcode.Ceq));
                continue;
              case 0x02: // cgt
                instructions.Add(new Instruction(offset, Opcode.Cgt));
                continue;
              case 0x03: // cgt.un
                instructions.Add(new Instruction(offset, Opcode.CgtUn));
                continue;
              case 0x04: // clt
                instructions.Add(new Instruction(offset, Opcode.Clt));
                continue;
              case 0x05: // clt.un
                instructions.Add(new Instruction(offset, Opcode.CltUn));
                continue;
              case 0x06: // ldftn
                instructions.Add(new Instruction(offset, Opcode.Ldftn, reader.ReadInt32()));
                continue;
              case 0x07: // ldvirtftn
                instructions.Add(new Instruction(offset, Opcode.Ldvirtftn, reader.ReadInt32()));
                continue;
              case 0x09: // ldarg
                instructions.Add(new Instruction(offset, Opcode.Ldarg, reader.ReadInt16()));
                continue;
              case 0x0A: // ldarga
                instructions.Add(new Instruction(offset, Opcode.Ldarga, reader.ReadInt16()));
                continue;
              case 0x0B: // starg
                instructions.Add(new Instruction(offset, Opcode.Starg, reader.ReadInt16()));
                continue;
              case 0x0C: // ldloc
                instructions.Add(new Instruction(offset, Opcode.Ldloc, reader.ReadInt16()));
                continue;
              case 0x0D: // ldloca
                instructions.Add(new Instruction(offset, Opcode.Ldloca, reader.ReadInt16()));
                continue;
              case 0x0E: // stloc
                instructions.Add(new Instruction(offset, Opcode.Stloc, reader.ReadInt16()));
                continue;
              case 0x0F: // localloc
                instructions.Add(new Instruction(offset, Opcode.Localloc));
                continue;
              case 0x11: // endfilter
                instructions.Add(new Instruction(offset, Opcode.Endfilter));
                continue;
              case 0x12: // unaligned.
                instructions.Add(new Instruction(offset, Opcode.Unaligned, reader.ReadByte()));
                continue;
              case 0x13: // volatile.
                instructions.Add(new Instruction(offset, Opcode.Volatile));
                continue;
              case 0x14: // tail.
                instructions.Add(new Instruction(offset, Opcode.Tail));
                continue;
              case 0x15: // initobj
                instructions.Add(new Instruction(offset, Opcode.Initobj, reader.ReadInt32()));
                continue;
              case 0x16: // constrained.
                instructions.Add(new Instruction(offset, Opcode.Constrained, reader.ReadInt32()));
                continue;
              case 0x17: // cpblk
                instructions.Add(new Instruction(offset, Opcode.Cpblk));
                continue;
              case 0x18: // initblk
                instructions.Add(new Instruction(offset, Opcode.Initblk));
                continue;
              case 0x1A: // rethrow
                instructions.Add(new Instruction(offset, Opcode.Rethrow));
                continue;
              case 0x1C: // sizeof
                instructions.Add(new Instruction(offset, Opcode.Sizeof, reader.ReadInt32()));
                continue;
              case 0x1D: // refanytype
                instructions.Add(new Instruction(offset, Opcode.Refanytype));
                continue;
              case 0x1E: // readonly.
                instructions.Add(new Instruction(offset, Opcode.Readonly));
                continue;
              default:
                UnexpectedOpcode();
                continue;
            }
          default:
            UnexpectedOpcode();
            continue;
        }
      }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void UnexpectedOpcode()
    {
      throw new ArgumentException("Unexpected opcode");
    }

    private static int[] ReadSwitch(ref BlobReader reader)
    {
      var casesCount = reader.ReadInt32();
      var switchBranches = new int[casesCount];

      for (var index = 0; index < switchBranches.Length; index++)
      {
        switchBranches[index] = reader.ReadInt32();
      }

      return switchBranches;
    }
  }
}