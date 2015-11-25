using System.Collections.Generic;

namespace System.Reflection.Metadata.ILReader
{
  public partial class ILReaderImpl
  {
    internal static FirstPassInfo InspectionPass(BlobReader reader)
    {
      var count = 0;
      List<InstructionJump> jumps = null;

      for (; reader.RemainingBytes > 0; count++)
      {
        switch (reader.ReadByte())
        {
          case 0x00: // nop
          case 0x01: // break
          case 0x02: // ldarg.0
          case 0x03: // ldarg.1
          case 0x04: // ldarg.2
          case 0x05: // ldarg.3
          case 0x06: // ldloc.0
          case 0x07: // ldloc.1
          case 0x08: // ldloc.2
          case 0x09: // ldloc.3
          case 0x0A: // stloc.0
          case 0x0B: // stloc.1
          case 0x0C: // stloc.2
          case 0x0D: // stloc.3
          case 0x14: // ldnull
          case 0x15: // ldc.i4.m1
          case 0x16: // ldc.i4.0
          case 0x17: // ldc.i4.1
          case 0x18: // ldc.i4.2
          case 0x19: // ldc.i4.3
          case 0x1A: // ldc.i4.4
          case 0x1B: // ldc.i4.5
          case 0x1C: // ldc.i4.6
          case 0x1D: // ldc.i4.7
          case 0x1E: // ldc.i4.8
          case 0x25: // dup
          case 0x26: // pop
          case 0x2A: // ret
          case 0x46: // ldind.i1
          case 0x47: // ldind.u1
          case 0x48: // ldind.i2
          case 0x49: // ldind.u2
          case 0x4A: // ldind.i4
          case 0x4B: // ldind.u4
          case 0x4C: // ldind.i8
          case 0x4D: // ldind.i
          case 0x4E: // ldind.r4
          case 0x4F: // ldind.r8
          case 0x50: // ldind.ref
          case 0x51: // stind.ref
          case 0x52: // stind.i1
          case 0x53: // stind.i2
          case 0x54: // stind.i4
          case 0x55: // stind.i8
          case 0x56: // stind.r4
          case 0x57: // stind.r8
          case 0x58: // add
          case 0x59: // sub
          case 0x5A: // mul
          case 0x5B: // div
          case 0x5C: // div.un
          case 0x5D: // rem
          case 0x5E: // rem.un
          case 0x5F: // and
          case 0x60: // or
          case 0x61: // xor
          case 0x62: // shl
          case 0x63: // shr
          case 0x64: // shr.un
          case 0x65: // neg
          case 0x66: // not
          case 0x67: // conv.i1
          case 0x68: // conv.i2
          case 0x69: // conv.i4
          case 0x6A: // conv.i8
          case 0x6B: // conv.r4
          case 0x6C: // conv.r8
          case 0x6D: // conv.u4
          case 0x6E: // conv.u8
          case 0x76: // conv.r.un
          case 0x7A: // throw
          case 0x82: // conv.ovf.i1.un
          case 0x83: // conv.ovf.i2.un
          case 0x84: // conv.ovf.i4.un
          case 0x85: // conv.ovf.i8.un
          case 0x86: // conv.ovf.u1.un
          case 0x87: // conv.ovf.u2.un
          case 0x88: // conv.ovf.u4.un
          case 0x89: // conv.ovf.u8.un
          case 0x8A: // conv.ovf.i.un
          case 0x8B: // conv.ovf.u.un
          case 0x8E: // ldlen
          case 0x90: // ldelem.i1
          case 0x91: // ldelem.u1
          case 0x92: // ldelem.i2
          case 0x93: // ldelem.u2
          case 0x94: // ldelem.i4
          case 0x95: // ldelem.u4
          case 0x96: // ldelem.i8
          case 0x97: // ldelem.i
          case 0x98: // ldelem.r4
          case 0x99: // ldelem.r8
          case 0x9A: // ldelem.ref
          case 0x9B: // stelem.i
          case 0x9C: // stelem.i1
          case 0x9D: // stelem.i2
          case 0x9E: // stelem.i4
          case 0x9F: // stelem.i8
          case 0xA0: // stelem.r4
          case 0xA1: // stelem.r8
          case 0xA2: // stelem.ref
          case 0xB3: // conv.ovf.i1
          case 0xB4: // conv.ovf.u1
          case 0xB5: // conv.ovf.i2
          case 0xB6: // conv.ovf.u2
          case 0xB7: // conv.ovf.i4
          case 0xB8: // conv.ovf.u4
          case 0xB9: // conv.ovf.i8
          case 0xBA: // conv.ovf.u8
          case 0xC3: // ckfinite
          case 0xD1: // conv.u2
          case 0xD2: // conv.u1
          case 0xD3: // conv.i
          case 0xD4: // conv.ovf.i
          case 0xD5: // conv.ovf.u
          case 0xD6: // add.ovf
          case 0xD7: // add.ovf.un
          case 0xD8: // mul.ovf
          case 0xD9: // mul.ovf.un
          case 0xDA: // sub.ovf
          case 0xDB: // sub.ovf.un
          case 0xDC: // endfinally
          case 0xDF: // stind.i
          case 0xE0: // conv.u
          case 0xF8: // prefix7
          case 0xF9: // prefix6
          case 0xFA: // prefix5
          case 0xFB: // prefix4
          case 0xFC: // prefix3
          case 0xFD: // prefix2
          case 0xFF: // prefixref
          {
            continue;
          }

          case 0x0E: // ldarg.s
          case 0x0F: // ldarga.s
          case 0x10: // starg.s
          case 0x11: // ldloc.s
          case 0x12: // ldloca.s
          case 0x13: // stloc.s
          case 0x1F: // ldc.i4.s
          {
            reader.ReadByte();
            continue;
          }

          case 0x3B: // beq
          case 0x3C: // bge
          case 0x3D: // bgt
          case 0x3E: // ble
          case 0x3F: // blt
          case 0x40: // bne.un
          case 0x41: // bge.un
          case 0x42: // bgt.un
          case 0x43: // ble.un
          case 0x44: // blt.un
          case 0x38: // br
          case 0x39: // brfalse
          case 0x3A: // brtrue
          case 0xDD: // leave
          {
            var targetOffset = reader.ReadInt32() + reader.Offset;

            jumps = jumps ?? new List<InstructionJump>(capacity: 4);
            jumps.Add(new InstructionJump(targetOffset, count));
            continue;
          }

          case 0x20: // ldc.i4
          case 0x22: // ldc.r4
          case 0x27: // jmp
          case 0x28: // call
          case 0x29: // calli
          case 0x6F: // callvirt
          case 0x70: // cpobj
          case 0x71: // ldobj
          case 0x72: // ldstr
          case 0x73: // newobj
          case 0x74: // castclass
          case 0x75: // isinst
          case 0x79: // unbox
          case 0x7B: // ldfld
          case 0x7C: // ldflda
          case 0x7D: // stfld
          case 0x7E: // ldsfld
          case 0x7F: // ldsflda
          case 0x80: // stsfld
          case 0x81: // stobj
          case 0x8C: // box
          case 0x8D: // newarr
          case 0x8F: // ldelema
          case 0xA3: // ldelem
          case 0xA4: // stelem
          case 0xA5: // unbox.any
          case 0xC2: // refanyval
          case 0xC6: // mkrefany
          case 0xD0: // ldtoken
          {
            reader.ReadInt32();
            continue;
          }

          case 0x21: // ldc.i8
          case 0x23: // ldc.r8
          {
            reader.ReadInt64();
            continue;
          }

          case 0x2B: // br.s
          case 0x2C: // brfalse.s
          case 0x2D: // brtrue.s
          case 0x2E: // beq.s
          case 0x2F: // bge.s
          case 0x30: // bgt.s
          case 0x31: // ble.s
          case 0x32: // blt.s
          case 0x33: // bne.un.s
          case 0x34: // bge.un.s
          case 0x35: // bgt.un.s
          case 0x36: // ble.un.s
          case 0x37: // blt.un.s
          case 0xDE: // leave.s
          {
            var targetOffset = reader.ReadSByte() + reader.Offset;

            jumps = jumps ?? new List<InstructionJump>(capacity: 4);
            jumps.Add(new InstructionJump(targetOffset, count));
            continue;
          }

          case 0x45: // switch
          {
            var casesCount = reader.ReadUInt32();
            jumps = jumps ?? new List<InstructionJump>(capacity: Math.Max((int) casesCount, 4));

            for (; casesCount > 0; casesCount--)
            {
              var targetOffset = reader.ReadInt32() + reader.Offset;
              jumps.Add(new InstructionJump(targetOffset, count));
            }

            continue;
          }

          case 254:
            switch (reader.ReadByte())
            {
              case 0x00: // arglist
              case 0x01: // ceq
              case 0x02: // cgt
              case 0x03: // cgt.un
              case 0x04: // clt
              case 0x05: // clt.un
              case 0x0F: // localloc
              case 0x11: // endfilter
              case 0x13: // volatile.
              case 0x14: // tail.
              case 0x17: // cpblk
              case 0x18: // initblk
              case 0x1A: // rethrow
              case 0x1D: // refanytype
              case 0x1E: // readonly.
                continue;
              case 0x06: // ldftn
              case 0x07: // ldvirtftn
              case 0x15: // initobj
              case 0x16: // constrained.
              case 0x1C: // sizeof
                reader.ReadInt32();
                continue;
              case 0x09: // ldarg
              case 0x0A: // ldarga
              case 0x0B: // starg
              case 0x0C: // ldloc
              case 0x0D: // ldloca
              case 0x0E: // stloc
                reader.ReadInt16();
                continue;
              case 0x12: // unaligned.
                reader.ReadByte();
                continue;
              default:
                UnexpectedOpcode(reader.Offset - 1);
                continue;
            }

          default:
          {
            UnexpectedOpcode(reader.Offset - 1);
            continue;
          }
        }
      }

      jumps?.Sort();

      var info = new FirstPassInfo();
      info.InstructionsCount = count;
      info.SortedJumps = jumps;

      return info;
    }
  }
}