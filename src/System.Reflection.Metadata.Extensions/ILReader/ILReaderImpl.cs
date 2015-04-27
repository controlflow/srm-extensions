using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Reflection.Metadata.ILReader
{
  public static partial class ILReaderImpl
  {
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void UnexpectedOpcode()
    {
      throw new InvalidOperationException("Unexpected opcode");
    }

    private static int[] ReadSwitch(ref BlobReader reader)
    {
      var casesCount = reader.ReadInt32();
      var switchBranches = new int[casesCount];
      var endOffset = reader.Offset + casesCount * 4;

      for (var index = 0; index < switchBranches.Length; index++)
      {
        switchBranches[index] = reader.ReadInt32() + endOffset;
      }

      return switchBranches;
    }
  }
}