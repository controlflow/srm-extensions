namespace System.Reflection.Metadata.ILReader
{
  public enum Opcode : byte
  {
    /// <summary>Adds two values and pushes the result onto the evaluation stack.</summary>
    Add = 0,
    /// <summary>Adds two integers, performs an overflow check, and pushes the result onto the evaluation stack.</summary>
    AddOvf = 1,
    /// <summary>Adds two unsigned integer values, performs an overflow check, and pushes the result onto the evaluation stack.</summary>
    AddOvfUn = 2,
    /// <summary>Computes the bitwise AND of two values and pushes the result onto the evaluation stack.</summary>
    And = 3,
    /// <summary>Returns an unmanaged pointer to the argument list of the current method.</summary>
    Arglist = 4,
    /// <summary>Transfers control to a target instruction if two values are equal.</summary>
    Beq = 5,
    /// <summary>Transfers control to a target instruction if the first value is greater than or equal to the second value.</summary>
    Bge = 6,
    /// <summary>Transfers control to a target instruction if the first value is greater than the second value, when comparing unsigned integer values or unordered float values.</summary>
    BgeUn = 7,
    /// <summary>Transfers control to a target instruction if the first value is greater than the second value.</summary>
    Bgt = 8,
    /// <summary>Transfers control to a target instruction if the first value is greater than the second value, when comparing unsigned integer values or unordered float values.</summary>
    BgtUn = 9,
    /// <summary>Transfers control to a target instruction if the first value is less than or equal to the second value.</summary>
    Ble = 10,
    /// <summary>Transfers control to a target instruction if the first value is less than or equal to the second value, when comparing unsigned integer values or unordered float values.</summary>
    BleUn = 11,
    /// <summary>Transfers control to a target instruction if the first value is less than the second value.</summary>
    Blt = 12,
    /// <summary>Transfers control to a target instruction if the first value is less than the second value, when comparing unsigned integer values or unordered float values.</summary>
    BltUn = 13,
    /// <summary>Transfers control to a target instruction when two unsigned integer values or unordered float values are not equal.</summary>
    BneUn = 14,
    /// <summary>Converts a value type to an object reference (type O).</summary>
    Box = 15,
    /// <summary>Unconditionally transfers control to a target instruction.</summary>
    Br = 16,
    /// <summary>Signals the Common Language Infrastructure (CLI) to inform the debugger that a break point has been tripped.</summary>
    Break = 17,
    /// <summary>Transfers control to a target instruction if value is false, a null reference (Nothing in Visual Basic), or zero.</summary>
    Brfalse = 18,
    /// <summary>Transfers control to a target instruction if value is true, not null, or non-zero.</summary>
    Brtrue = 19,
    /// <summary>Calls the method indicated by the passed method descriptor.</summary>
    Call = 20,
    /// <summary>Calls the method indicated on the evaluation stack (as a pointer to an entry point) with arguments described by a calling convention.</summary>
    Calli = 21,
    /// <summary>Calls a late-bound method on an object, pushing the return value onto the evaluation stack.</summary>
    Callvirt = 22,
    /// <summary>Attempts to cast an object passed by reference to the specified class.</summary>
    Castclass = 23,
    /// <summary>Compares two values. If they are equal, the integer value 1 (int32) is pushed onto the evaluation stack; otherwise 0 (int32) is pushed onto the evaluation stack.</summary>
    Ceq = 24,
    /// <summary>Compares two values. If the first value is greater than the second, the integer value 1 (int32) is pushed onto the evaluation stack; otherwise 0 (int32) is pushed onto the evaluation stack.</summary>
    Cgt = 25,
    /// <summary>Compares two unsigned or unordered values. If the first value is greater than the second, the integer value 1 (int32) is pushed onto the evaluation stack; otherwise 0 (int32) is pushed onto the evaluation stack.</summary>
    CgtUn = 26,
    /// <summary>Throws <see cref="T:System.ArithmeticException"/> if value is not a finite number.</summary>
    Ckfinite = 27,
    /// <summary>Compares two values. If the first value is less than the second, the integer value 1 (int32) is pushed onto the evaluation stack; otherwise 0 (int32) is pushed onto the evaluation stack.</summary>
    Clt = 28,
    /// <summary>Compares the unsigned or unordered values 'value1' and 'value2'. If 'value1' is less than 'value2', then the integer value 1 (int32) is pushed onto the evaluation stack; otherwise 0 (int32) is pushed onto the evaluation stack.</summary>
    CltUn = 29,
    Constrained = 30,
    Conv = 31,
    ConvOvf = 32,
    ConvOvfUn = 33,
    ConvUn = 34,
    Cpblk = 35,
    Cpobj = 36,
    Div = 37,
    DivUn = 38,
    Dup = 39,
    Endfilter = 40,
    Endfinally = 41,
    Initblk = 42,
    Initobj = 43,
    Isinst = 44,
    Jmp = 45,
    Ldarg = 46,
    Ldarga = 47,
    LdcI4 = 48,
    LdcI8 = 49,
    LdcR4 = 50,
    LdcR8 = 51,
    Ldelem = 52,
    Ldelema = 53,
    LdelemRef = 54,
    Ldfld = 55,
    Ldflda = 56,
    /// <summary>Pushes an unmanaged pointer (type native int) to the native code implementing a specific method onto the evaluation stack.</summary>
    Ldftn = 57,
    Ldind = 58,
    LdindRef = 59,
    Ldlen = 60,
    Ldloc = 61,
    Ldloca = 62,
    Ldnull = 63,
    Ldobj = 64,
    Ldsfld = 65,
    Ldsflda = 66,
    Ldstr = 67,
    Ldtoken = 68,
    Ldvirtftn = 69,
    Leave = 70,
    Localloc = 71,
    Mkrefany = 72,
    Mul = 73,
    MulOvf = 74,
    MulOvfUn = 75,
    Neg = 76,
    Newarr = 77,
    Newobj = 78,
    Nop = 79,
    Not = 80,
    /// <summary>Compute the bitwise complement of the two integer values on top of the stack and pushes the result onto the evaluation stack.</summary>
    Or = 81,
    Pop = 82,
    Prefix1 = 83,
    Prefix2 = 84,
    Prefix3 = 85,
    Prefix4 = 86,
    Prefix5 = 87,
    Prefix6 = 88,
    Prefix7 = 89,
    Prefixref = 90,
    Readonly = 91,
    Refanytype = 92,
    Refanyval = 93,
    Rem = 94,
    RemUn = 95,
    Ret = 96,
    Rethrow = 97,
    Shl = 98,
    Shr = 99,
    ShrUn = 100,
    Sizeof = 101,
    Starg = 102,
    Stelem = 103,
    StelemRef = 104,
    Stfld = 105,
    Stind = 106,
    StindRef = 107,
    Stloc = 108,
    Stobj = 109,
    Stsfld = 110,
    Sub = 111,
    SubOvf = 112,
    SubOvfUn = 113,
    Switch = 114,
    /// <summary>Performs a postfixed method call instruction such that the current method's stack frame is removed before the actual call instruction is executed.</summary>
    Tail = 115,
    Throw = 116,
    Unaligned = 117,
    Unbox = 118,
    UnboxAny = 119,
    Volatile = 120,
    /// <summary>Computes the bitwise XOR of the top two values on the evaluation stack, pushing the result onto the evaluation stack.</summary>
    Xor = 121
  }
}