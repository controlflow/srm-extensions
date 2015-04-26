using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;

class IlReaderBuilder
{
  private readonly StringBuilder myReadMethod = new StringBuilder();
  private readonly StringBuilder myCountMethod = new StringBuilder();
  //private readonly HashSet<string> myCases = new HashSet<string>(StringComparer.Ordinal);
  private readonly List<OpCode> myAllOpcodes;

  public IlReaderBuilder()
  {
    myAllOpcodes = typeof(OpCodes).GetFields()
      .Where(x => x.FieldType == typeof(OpCode))
      .Select(x => (OpCode)x.GetValue(null))
      .ToList();
  }

  const byte MultiByteOpCodePrefix = 0xFE;

  public string BuildReadMethod()
  {
    var readMethod = myReadMethod;
    readMethod.AppendLine("switch (reader.ReadByte()) {");

    // single-byte instructions
    foreach (var code in myAllOpcodes)
    {
      if (IsTwoByteOpcode(code)) continue;
      if (code.Value == MultiByteOpCodePrefix) continue;

      readMethod.AppendFormat("case 0x{0:X2}: // {1}", code.Value, code.Name).AppendLine();

      ReadOperand(code, readMethod);
      readMethod.AppendLine(" continue;");
    }

    readMethod.AppendFormat("case {0}:", MultiByteOpCodePrefix).AppendLine();
    readMethod.AppendLine("  switch (reader.ReadByte()) {");

    // two-byte instructions
    foreach (var code in myAllOpcodes)
    {
      if (!IsTwoByteOpcode(code)) continue;

      var value = code.Value & byte.MaxValue;
      readMethod.AppendFormat("  case 0x{0:X2}: // {1}", value, code.Name).AppendLine();
      readMethod.Append("  ");

      ReadOperand(code, readMethod);
      readMethod.AppendLine("  continue;");
    }

    // todo: remove?
    readMethod.AppendLine("  default:");
    readMethod.AppendLine("    UnexpectedOpcode();");
    readMethod.AppendLine("    continue;");
    readMethod.AppendLine("  }");

    readMethod.AppendLine("default:");
    readMethod.AppendLine("  UnexpectedOpcode();");
    readMethod.AppendLine("  continue;");
    readMethod.AppendLine("}");

    //var cases = string.Join(", ", myCases.OrderBy(x => x).Select((x, i) => string.Format("{0} = {1}", x, i)));
    //myReadMethod.AppendFormat("enum Opcode {{ {0} }}", cases);

    return readMethod.ToString();
  }

  private static bool IsTwoByteOpcode(OpCode code)
  {
    var upperByte = code.Value >> 8;
    return (upperByte & MultiByteOpCodePrefix) != 0;
  }

  public string BuildCountMethod()
  {
    var countMethod = myCountMethod;
    countMethod.AppendLine("switch (reader.ReadByte()) {");

    CountOpcodes(myAllOpcodes.Where(code => !IsTwoByteOpcode(code)));

    countMethod.AppendFormat("case {0}:", MultiByteOpCodePrefix).AppendLine();

    {
      countMethod.AppendLine("  switch (reader.ReadByte()) {");

      CountOpcodes(myAllOpcodes.Where(IsTwoByteOpcode));

      countMethod.AppendLine("  default:");
      countMethod.AppendLine("    UnexpectedOpcode();");
      countMethod.AppendLine("    continue;");
      countMethod.AppendLine("  }");
    }

    countMethod.AppendLine("default:");
    countMethod.AppendLine("  UnexpectedOpcode();");
    countMethod.AppendLine("  continue;");
    countMethod.AppendLine("}");

    countMethod.AppendLine("}");

    return countMethod.ToString();
  }

  private void CountOpcodes(IEnumerable<OpCode> allOpcodes)
  {
    foreach (var groupByOperandType in allOpcodes.GroupBy(x => SkipOperandCode(x.OperandType)).OrderByDescending(x => string.IsNullOrEmpty(x.Key)))
    {
      foreach (var code in groupByOperandType.OrderBy(x => x.Value))
      {
        var value = code.Value;
        if (value == MultiByteOpCodePrefix) continue;

        if (IsTwoByteOpcode(code)) value = (short) (value & byte.MaxValue);

        myCountMethod.AppendFormat("case 0x{0:X2}: // {1}", value, code.Name).AppendLine();
      }

      myCountMethod.Append("  ");
      myCountMethod.Append(groupByOperandType.Key);
      myCountMethod.AppendLine();
      myCountMethod.AppendLine("  continue;");
    }
  }

  private static string SkipOperandCode(OperandType operandType)
  {
    switch (operandType)
    {
      case OperandType.InlineField:
      case OperandType.InlineI:
      case OperandType.InlineMethod:
      case OperandType.InlineSig:
      case OperandType.InlineString:
      case OperandType.InlineTok:
      case OperandType.InlineType:
      case OperandType.ShortInlineR:
      {
        return "reader.ReadInt32();";
      }

      case OperandType.InlineSwitch:
      {
        return "for (var cases = reader.ReadUInt32(); cases > 0; cases--) reader.ReadInt32();";
      }

      case OperandType.InlineI8:
      case OperandType.InlineR:
      {
        return "reader.ReadInt64();";
      }

      case OperandType.InlineVar:
      {
        return "reader.ReadInt16();";
      }

      case OperandType.ShortInlineVar:
      case OperandType.ShortInlineI:
      {
        return "reader.ReadByte();";
      }

      // todo: add decoding?
      case OperandType.InlineBrTarget:
      {
        return "reader.ReadInt32();";
      }

      case OperandType.ShortInlineBrTarget:
      {
        return "reader.ReadSByte();";
      }

      case OperandType.InlineNone:
      {
        return string.Empty;
      }

      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  private static void ReadOperand(OpCode code, StringBuilder builder)
  {
    var name = code.Name;
    var value = GetValue(code, ref name);

    builder.AppendFormat("  instructions.Add(new Instruction(offset");

    var caseName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(name).Replace(".", "");
    builder.AppendFormat(", Opcode.{0}", caseName);

    //myCases.Add(caseName);

    if (value != null)
    {
      builder.Append(", ").Append(value.Value);

      Debug.Assert(code.OperandType == OperandType.InlineNone);
    }
    else
    {
      switch (code.OperandType)
      {
        case OperandType.InlineField:
        case OperandType.InlineI:
        case OperandType.InlineMethod:
        case OperandType.InlineSig:
        case OperandType.InlineString:
        case OperandType.InlineTok:
        case OperandType.InlineType:
        case OperandType.ShortInlineR:
        {
          builder.Append(", reader.ReadInt32()");
          break;
        }

        case OperandType.InlineBrTarget:
        {
          builder.Append(", reader.ReadInt32() + reader.Offset");
          break;
        }

        case OperandType.InlineSwitch:
        {
          builder.Append(", ReadSwitch(ref reader)");
          break;
        }

        case OperandType.InlineI8:
        case OperandType.InlineR:
        {
          builder.Append(", reader.ReadInt64()");
          break;
        }

        case OperandType.InlineVar:
        {
          builder.Append(", reader.ReadInt16()");
          break;
        }

        case OperandType.ShortInlineVar:
        case OperandType.ShortInlineI:
        {
          builder.Append(", reader.ReadByte()");
          break;
        }

        case OperandType.ShortInlineBrTarget:
        {
          builder.Append(", reader.ReadSByte() + reader.Offset");
          break;
        }
      }
    }

    builder.Append("));");
    builder.AppendLine();
  }

  private static int? GetValue(OpCode code, ref string name)
  {
    if (code == OpCodes.Ldc_I4) return null;
    if (code == OpCodes.Ldc_I8) return null;
    if (code == OpCodes.Ldc_R4) return null;
    if (code == OpCodes.Ldc_R8) return null;

    var match = Regex.Match(name, @"^(.+)\.(m1|\d|s|[iu][1248]|r[48]|i|u|r)(\.un)?$");
    if (!match.Success) return null;

    var groups = match.Groups;

    name = groups[1].Value + groups[3].Value;

    switch (groups[2].Value)
    {
      case "m1": return -1;
      case "0": return 0;
      case "1": return 1;
      case "2": return 2;
      case "3": return 3;
      case "4": return 4;
      case "5": return 5;
      case "6": return 6;
      case "7": return 7;
      case "8": return 8;

      case "i": return 0;
      case "i1": return 1;
      case "i2": return 2;
      case "i4": return 3;
      case "i8": return 4;
      case "u1": return 5;
      case "u2": return 6;
      case "u4": return 7;
      case "u8": return 8;
      case "r4": return 9;
      case "r8": return 10;
      case "u": return 11;
      case "r": return 9;
    }

    return null;
  }
}