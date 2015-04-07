namespace System.Reflection.Metadata.ILReader
{
  public static class OpcodeExtensions
  {
    public static bool CanThrowException(this Opcode opcode)
    {
      switch (opcode)
      {
        case Opcode.Add:
        case Opcode.And:
        case Opcode.Or:
        case Opcode.Neg:
        case Opcode.Arglist:
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
          return false;

        case Opcode.AddOvf:
        case Opcode.AddOvfUn:
        case Opcode.Box:
        case Opcode.Unbox:
        case Opcode.UnboxAny:
          return true;

        
        case Opcode.Br:
        case Opcode.Break:
        case Opcode.Brfalse:
        case Opcode.Brtrue:
        case Opcode.Call:
        case Opcode.Calli:
        case Opcode.Callvirt:
        case Opcode.Castclass:
        case Opcode.Ceq:
        case Opcode.Cgt:
        case Opcode.CgtUn:
        case Opcode.Ckfinite:
        case Opcode.Clt:
        case Opcode.CltUn:
        case Opcode.Constrained:
        case Opcode.Conv:
        case Opcode.ConvOvf:
        case Opcode.ConvOvfUn:
        case Opcode.ConvUn:
        case Opcode.Cpblk:
        case Opcode.Cpobj:
        case Opcode.Div:
        case Opcode.DivUn:
        case Opcode.Dup:
        case Opcode.Endfilter:
        case Opcode.Endfinally:
        case Opcode.Initblk:
        case Opcode.Initobj:
        case Opcode.Isinst:
        case Opcode.Jmp:
        case Opcode.Ldarg:
        case Opcode.Ldarga:
        case Opcode.LdcI4:
        case Opcode.LdcI8:
        case Opcode.LdcR4:
        case Opcode.LdcR8:
        case Opcode.Ldelem:
        case Opcode.Ldelema:
        case Opcode.LdelemRef:
        case Opcode.Ldfld:
        case Opcode.Ldflda:
        case Opcode.Ldftn:
        case Opcode.Ldind:
        case Opcode.LdindRef:
        case Opcode.Ldlen:
        case Opcode.Ldloc:
        case Opcode.Ldloca:
        case Opcode.Ldnull:
        case Opcode.Ldobj:
        case Opcode.Ldsfld:
        case Opcode.Ldsflda:
        case Opcode.Ldstr:
        case Opcode.Ldtoken:
        case Opcode.Ldvirtftn:
        case Opcode.Leave:
        case Opcode.Localloc:
        case Opcode.Mkrefany:
        case Opcode.Mul:
        case Opcode.MulOvf:
        case Opcode.MulOvfUn:
        case Opcode.Newarr:
        case Opcode.Newobj:
        case Opcode.Nop:
        case Opcode.Not:
        case Opcode.Pop:
        case Opcode.Prefix1:
        case Opcode.Prefix2:
        case Opcode.Prefix3:
        case Opcode.Prefix4:
        case Opcode.Prefix5:
        case Opcode.Prefix6:
        case Opcode.Prefix7:
        case Opcode.Prefixref:
        case Opcode.Readonly:
        case Opcode.Refanytype:
        case Opcode.Refanyval:
        case Opcode.Rem:
        case Opcode.RemUn:
        case Opcode.Ret:
        case Opcode.Rethrow:
        case Opcode.Shl:
        case Opcode.Shr:
        case Opcode.ShrUn:
        case Opcode.Sizeof:
        case Opcode.Starg:
        case Opcode.Stelem:
        case Opcode.StelemRef:
        case Opcode.Stfld:
        case Opcode.Stind:
        case Opcode.StindRef:
        case Opcode.Stloc:
        case Opcode.Stobj:
        case Opcode.Stsfld:
        case Opcode.Sub:
        case Opcode.SubOvf:
        case Opcode.SubOvfUn:
        case Opcode.Switch:
        case Opcode.Tail:
        case Opcode.Throw:
        case Opcode.Unaligned:
        case Opcode.Volatile:
        case Opcode.Xor:
          return true;
      }

      return true;
    }
  }
}