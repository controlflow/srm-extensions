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
  private readonly StringBuilder myBuilder = new StringBuilder();
  private readonly HashSet<string> myCases = new HashSet<string>(StringComparer.Ordinal);

  const byte MultiByteOpCodePrefix = 0xFE;

  public string Build()
  {
    var allOpcodes = typeof(OpCodes).GetFields()
      .Where(x => x.FieldType == typeof(OpCode))
      .Select(x => (OpCode)x.GetValue(null))
      .ToList();

    myBuilder.AppendLine("switch (reader.ReadByte()) {");

    // single-byte instructions
    foreach (var code in allOpcodes)
    {
      var upperByte = code.Value >> 8;
      if ((upperByte & MultiByteOpCodePrefix) != 0) continue;

      myBuilder.AppendFormat("case 0x{0:X2}: // {1}", code.Value, code.Name).AppendLine();

      AppendOpcode(code);
      myBuilder.AppendLine(" break;");
    }

    myBuilder.AppendFormat("case {0}:", MultiByteOpCodePrefix).AppendLine();
    myBuilder.AppendLine("  switch (reader.ReadByte()) {");

    // two-byte instructions
    foreach (var code in allOpcodes)
    {
      var upperByte = code.Value >> 8;
      if ((upperByte & MultiByteOpCodePrefix) == 0) continue;

      var value = code.Value & byte.MaxValue;
      myBuilder.AppendFormat("  case 0x{0:X2}: // {1}", value, code.Name).AppendLine();
      myBuilder.Append("  ");

      AppendOpcode(code);
      myBuilder.AppendLine(" break;");
    }

    myBuilder.AppendLine("  default:");
    myBuilder.AppendLine("    throw new ArgumentException(\"Unexpected opcode\")");
    myBuilder.AppendLine("  }");
    myBuilder.AppendLine("  break;");

    myBuilder.AppendLine("default:");
    myBuilder.AppendLine("  throw new ArgumentException(\"Unexpected opcode\")");

    myBuilder.AppendLine("}");

    var cases = string.Join(", ", myCases.OrderBy(x => x).Select((x, i) => string.Format("{0} = {1}", x, i)));

    myBuilder.AppendFormat("enum OpCode {{ {0} }}", cases);

    return myBuilder.ToString();
  }

  private void AppendOpcode(OpCode code)
  {
    var name = code.Name;
    var value = GetValue(code, ref name);

    myBuilder.AppendFormat("  instructions.Add(new Instruction(offset");

    var caseName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(name).Replace(".", "");
    myBuilder.AppendFormat(", OpCode.{0}", caseName);

    myCases.Add(caseName);

    if (value != null)
    {
      myBuilder.Append(", ").Append(value.Value);

      Debug.Assert(code.OperandType == OperandType.InlineNone);
    }
    else
    {
      switch (code.OperandType)
      {
        case OperandType.InlineBrTarget:
        case OperandType.InlineField:
        case OperandType.InlineI:
        case OperandType.InlineMethod:
        case OperandType.InlineSig:
        case OperandType.InlineString:
        case OperandType.InlineTok:
        case OperandType.InlineType:
        case OperandType.ShortInlineR:
        {
          myBuilder.Append(", reader.ReadInt32()");
          break;
        }

        case OperandType.InlineSwitch:
        {
          myBuilder.Append(", ReadSwitch(reader)");
          break;
        }

        case OperandType.InlineI8:
        case OperandType.InlineR:
        {
          myBuilder.Append(", reader.ReadInt64()");
          break;
        }

        case OperandType.InlineVar:
        {
          myBuilder.Append(", reader.ReadInt16()");
          break;
        }

        case OperandType.ShortInlineVar:
        case OperandType.ShortInlineI:
        case OperandType.ShortInlineBrTarget:
        {
          myBuilder.Append(", reader.ReadByte()");
          break;
        }
      }
    }

    myBuilder.Append("));");
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