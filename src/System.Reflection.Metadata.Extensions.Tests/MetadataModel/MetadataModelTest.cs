using NUnit.Framework;

namespace System.Reflection.Metadata.Extensions.Tests.MetadataModel
{
  [TestFixture]
  public class MetadataModelTest : MetadataModelTestBase
  {
    [Test] public void TypeDefinition()
    {
      var type = typeof(MetadataModelTest);
      var metadataType = GetMetadataDefinition(type);
      Assert.AreEqual(metadataType.FullName, type.FullName);
      Assert.AreEqual(metadataType.Namespace, type.Namespace);
      Assert.AreEqual(metadataType.Name, type.Name);
      Assert.IsTrue(metadataType.IsType);
      Assert.IsFalse(metadataType.IsInterface);
      Assert.IsFalse(metadataType.IsAbstract);
      Assert.IsFalse(metadataType.IsSealed);
      Assert.IsFalse(metadataType.IsNested);
    }

    [Test] public void TestNestedType()
    {
      var type = typeof(Class);
      var metadataType = GetMetadataDefinition(type);
      Assert.IsTrue(metadataType.IsNested);
      Assert.AreEqual(type.FullName, type.FullName);
      Assert.AreEqual(type.Name, type.Name);
    }

    //[Test] public void TestGenericType()
    //{
    //  var type = typeof(Class);
    //  var metadataType = GetMetadataType(type);
    //  Assert.IsTrue(metadataType.IsNested);
    //  Assert.AreEqual(type.FullName, type.FullName);
    //  Assert.AreEqual(type.Name, type.Name);
    //}

    [Test] public void TestIsValueType()
    {
      Assert.IsTrue(GetMetadataDefinition(typeof(IInterface)).IsInterface);
      Assert.IsFalse(GetMetadataDefinition(typeof(Class)).IsInterface);
      Assert.IsFalse(GetMetadataDefinition(typeof(Class)).IsValueType);
      Assert.IsTrue(GetMetadataDefinition(typeof(Enumeration)).IsValueType);
      Assert.IsTrue(GetMetadataDefinition(typeof(Struct)).IsValueType);
      Assert.IsTrue(GetMetadataDefinition(typeof(Nullable<>)).IsValueType);
      Assert.IsTrue(GetMetadataDefinition(typeof(int)).IsValueType);
    }

    interface IInterface { }
    enum Enumeration { }
    class Class { }
    struct Struct { }
  }
}