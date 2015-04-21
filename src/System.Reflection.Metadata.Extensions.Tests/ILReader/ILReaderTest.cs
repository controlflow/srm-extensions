using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata.ILReader;
using JetBrains.Annotations;
using NUnit.Framework;

namespace System.Reflection.Metadata.Extensions.Tests.ILReader
{
  [TestFixture]
  public unsafe class ILReaderTest
  {
    private ModuleBuilder myDynamicModule;
    private int myLastMethodIndex;

    [Test] public void ReadAdd()
    {
      AssertReader(
        gen =>
        {
          gen.Emit(OpCodes.Ldc_I4_M1);
          gen.Emit(OpCodes.Ldc_I4_0);
          gen.Emit(OpCodes.Ldc_I4_1);
          gen.Emit(OpCodes.Ldc_I4_2);
          gen.Emit(OpCodes.Add);
          gen.Emit(OpCodes.Add_Ovf);
          gen.Emit(OpCodes.Add_Ovf_Un);
          gen.Emit(OpCodes.Ret);
        },
        il =>
        {
          Assert.That(il.Count, Is.EqualTo(8));
          Assert.AreEqual(il[0].Code, Opcode.LdcI4);
          Assert.AreEqual(il[0].ValueInt32, -1);
          Assert.AreEqual(il[1].Code, Opcode.LdcI4);
          Assert.AreEqual(il[1].ValueInt32, 0);
          Assert.AreEqual(il[2].Code, Opcode.LdcI4);
          Assert.AreEqual(il[2].ValueInt32, 1);
          Assert.AreEqual(il[3].Code, Opcode.LdcI4);
          Assert.AreEqual(il[3].ValueInt32, 2);
          Assert.AreEqual(il[4].Code, Opcode.Add);
          Assert.AreEqual(il[5].Code, Opcode.AddOvf);
          Assert.AreEqual(il[6].Code, Opcode.AddOvfUn);
          Assert.AreEqual(il[7].Code, Opcode.Ret);
        });
    }

    [Test] public void ReadAndOrXorNeg()
    {
      AssertReader(
        gen =>
        {
          gen.Emit(OpCodes.Ldc_I4_3);
          gen.Emit(OpCodes.Ldc_I4_4);
          gen.Emit(OpCodes.Ldc_I4_5);
          gen.Emit(OpCodes.Ldc_I4_6);
          gen.Emit(OpCodes.And);
          gen.Emit(OpCodes.Or);
          gen.Emit(OpCodes.Xor);
          gen.Emit(OpCodes.Neg);
          gen.Emit(OpCodes.Ret);
        },
        il =>
        {
          Assert.That(il.Count, Is.EqualTo(9));
          Assert.AreEqual(il[0].Code, Opcode.LdcI4);
          Assert.AreEqual(il[0].ValueInt32, 3);
          Assert.AreEqual(il[1].Code, Opcode.LdcI4);
          Assert.AreEqual(il[1].ValueInt32, 4);
          Assert.AreEqual(il[2].Code, Opcode.LdcI4);
          Assert.AreEqual(il[2].ValueInt32, 5);
          Assert.AreEqual(il[3].Code, Opcode.LdcI4);
          Assert.AreEqual(il[3].ValueInt32, 6);
          Assert.AreEqual(il[4].Code, Opcode.And);
          Assert.AreEqual(il[5].Code, Opcode.Or);
          Assert.AreEqual(il[6].Code, Opcode.Xor);
          Assert.AreEqual(il[7].Code, Opcode.Neg);
          Assert.AreEqual(il[8].Code, Opcode.Ret);
        });
    }

    [Test] public void ReadArglist()
    {
      AssertReader(
        gen =>
        {
          gen.Emit(OpCodes.Arglist);
          gen.Emit(OpCodes.Pop);
          gen.Emit(OpCodes.Ret);
        },
        il =>
        {
          Assert.That(il.Count, Is.EqualTo(3));
          Assert.AreEqual(il[0].Code, Opcode.Arglist);
          Assert.AreEqual(il[1].Code, Opcode.Pop);
          Assert.AreEqual(il[2].Code, Opcode.Ret);
        });
    }

    [Test] public void ReadBeqBgeBgtBltBle()
    {
      AssertRelational(OpCodes.Beq, Opcode.Beq);
      AssertRelational(OpCodes.Beq_S, Opcode.Beq);
      AssertRelational(OpCodes.Bge, Opcode.Bge);
      AssertRelational(OpCodes.Bge_S, Opcode.Bge);
      AssertRelational(OpCodes.Bge_Un, Opcode.BgeUn);
      AssertRelational(OpCodes.Bge_Un_S, Opcode.BgeUn);
      AssertRelational(OpCodes.Bgt, Opcode.Bgt);
      AssertRelational(OpCodes.Bgt_S, Opcode.Bgt);
      AssertRelational(OpCodes.Bgt_Un, Opcode.BgtUn);
      AssertRelational(OpCodes.Bgt_Un_S, Opcode.BgtUn);
      AssertRelational(OpCodes.Ble, Opcode.Ble);
      AssertRelational(OpCodes.Ble_S, Opcode.Ble);
      AssertRelational(OpCodes.Ble_Un, Opcode.BleUn);
      AssertRelational(OpCodes.Ble_Un_S, Opcode.BleUn);
      AssertRelational(OpCodes.Blt, Opcode.Blt);
      AssertRelational(OpCodes.Blt_S, Opcode.Blt);
      AssertRelational(OpCodes.Blt_Un, Opcode.BltUn);
      AssertRelational(OpCodes.Blt_Un_S, Opcode.BltUn);
      AssertRelational(OpCodes.Bne_Un, Opcode.BneUn);
      AssertRelational(OpCodes.Bne_Un_S, Opcode.BneUn);
    }

    [Test] public void ReadBoxUnboxAny()
    {
      AssertReader<Func<int, int>>(
        gen =>
        {
          gen.Emit(OpCodes.Ldarg_0);
          gen.Emit(OpCodes.Box, typeof(int));
          gen.Emit(OpCodes.Dup);
          gen.Emit(OpCodes.Unbox, typeof(int));
          gen.Emit(OpCodes.Ldind_I4);
          gen.Emit(OpCodes.Pop);
          gen.Emit(OpCodes.Unbox_Any, typeof(int));
          gen.Emit(OpCodes.Ret);
        },
        il =>
        {
          Assert.That(il.Count, Is.EqualTo(8));
          Assert.AreEqual(il[0].Code, Opcode.Ldarg);
          Assert.AreEqual(il[0].ArgumentIndex, 0);
          Assert.AreEqual(il[1].Code, Opcode.Box);
          Assert.AreEqual(il[2].Code, Opcode.Dup);
          Assert.AreEqual(il[3].Code, Opcode.Unbox);
          Assert.AreEqual(il[4].Code, Opcode.Ldind);
          Assert.AreEqual(il[4].OperandType, ILType.Int32);
          Assert.AreEqual(il[5].Code, Opcode.Pop);
          Assert.AreEqual(il[6].Code, Opcode.UnboxAny);
          Assert.AreEqual(il[7].Code, Opcode.Ret);

          Assert.AreEqual(il[1].TypeToken, il[3].TypeToken);
          Assert.AreEqual(il[1].TypeHandle, il[3].TypeHandle);
          Assert.AreEqual(il[1].TypeToken, il[6].TypeToken);
          Assert.AreEqual(il[1].TypeHandle, il[6].TypeHandle);
        });
    }

    [Test] public void ReadBrBreak()
    {
      AssertReader(
        gen =>
        {
          var label = gen.DefineLabel();
          gen.Emit(OpCodes.Br, label);
          gen.Emit(OpCodes.Break);
          gen.MarkLabel(label);
          gen.Emit(OpCodes.Ret);
        },
        il =>
        {
          Assert.That(il.Count, Is.EqualTo(3));
          Assert.AreEqual(il[0].Code, Opcode.Br);
          Assert.AreEqual(il[0].BranchTarget, il[2].Offset);
          Assert.AreEqual(il[1].Code, Opcode.Break);
          Assert.AreEqual(il[2].Code, Opcode.Ret);
        });
    }

    private void AssertRelational(OpCode opCode, Opcode opcode)
    {
      AssertReader<Func<bool>>(
        gen =>
        {
          var someLabel = gen.DefineLabel();
          gen.MarkLabel(someLabel);
          gen.Emit(OpCodes.Ldc_I4_7);
          gen.Emit(OpCodes.Ldc_I4_8);
          gen.Emit(opCode, someLabel);
          gen.Emit(OpCodes.Ret);
        },
        il =>
        {
          Assert.That(il.Count, Is.EqualTo(4));
          Assert.AreEqual(il[0].Code, Opcode.LdcI4);
          Assert.AreEqual(il[0].ValueInt32, 7);
          Assert.AreEqual(il[1].Code, Opcode.LdcI4);
          Assert.AreEqual(il[1].ValueInt32, 8);
          Assert.AreEqual(il[2].Code, opcode);
          Assert.AreEqual(il[2].BranchTarget, il[0].Offset);
          Assert.AreEqual(il[3].Code, Opcode.Ret);
        });
    }

    [TestFixtureSetUp]
    public void CreateDynamicAssembly()
    {
      var tempAssemblyName = new AssemblyName("Foo");
      var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(tempAssemblyName, AssemblyBuilderAccess.RunAndCollect);
      myDynamicModule = assemblyBuilder.DefineDynamicModule("Foo");
    }

    private void AssertReader([NotNull] Action<ILGenerator> ilGenerator, [NotNull] Action<List<Instruction>> assertion)
    {
      AssertReader<Action>(ilGenerator, assertion);
    }

    private void AssertReader<TDelegate>([NotNull] Action<ILGenerator> ilGenerator, [NotNull] Action<List<Instruction>> assertion)
      where TDelegate : class
    {
      var delegateType = typeof(TDelegate);
      Debug.Assert(delegateType.IsSubclassOf(typeof(Delegate)), "delegateType.IsSubclassOf(typeof(Delegate))");

      var invokeMethod = delegateType.GetMethod("Invoke", BindingFlags.Public | BindingFlags.Instance);
      var parameterTypes = invokeMethod.GetParameters().Select( x=> x.ParameterType).ToArray();

      var dynamicName = string.Format("Method{0}", myLastMethodIndex++);
      var typeBuilder = myDynamicModule.DefineType(dynamicName);
      var methodBuilder = typeBuilder.DefineMethod(
        dynamicName, MethodAttributes.Public | MethodAttributes.Static, invokeMethod.ReturnType, parameterTypes);

      var generator = methodBuilder.GetILGenerator();
      ilGenerator(generator);

      var type = typeBuilder.CreateType();

      var methodInfo = type.GetMethod(dynamicName, BindingFlags.Public | BindingFlags.Static);
      var methodBody = methodInfo.GetMethodBody();
      Assert.IsNotNull(methodBody);

      var ilStream = methodBody.GetILAsByteArray();
      var ilLength = ilStream.Length;

      fixed (byte* ptr = &ilStream[0])
      {
        var blobReader = new BlobReader(ptr, ilLength);

        var instructions = new List<Instruction>();
        ILReaderImpl.Read(blobReader, instructions);
        assertion(instructions);
      }
    }
  }
}