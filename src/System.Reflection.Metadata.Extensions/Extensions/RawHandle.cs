using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace System.Reflection.Metadata.Extensions
{
  [SuppressMessage("ReSharper", "UseObjectOrCollectionInitializer")]
  [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Local")]
  public class RawHandle
  {
    public static Handle From(uint value)
    {
      var foo = new Foo();
      foo.Value = value;
      return foo.Handle;
    }

    [StructLayout(LayoutKind.Explicit)]
    struct Foo
    {
      [FieldOffset(0)] public uint Value;
      [FieldOffset(0)] public Handle Handle;
    }
  }
}