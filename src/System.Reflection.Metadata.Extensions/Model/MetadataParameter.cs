using System.Diagnostics;
using JetBrains.Annotations;

namespace System.Reflection.Metadata.Model
{
  [DebuggerDisplay("{DebugView}")]
  public struct MetadataParameter
  {
    [NotNull] private readonly MetadataReader myMetadataReader;
    private Parameter myParameter;

    public MetadataParameter([NotNull] MetadataReader metadataReader, Parameter parameter)
    {
      myMetadataReader = metadataReader;
      myParameter = parameter;
    }

    public string Name => myMetadataReader.GetString(myParameter.Name);

    public int Index => myParameter.SequenceNumber;

    public bool IsOptional => (myParameter.Attributes & ParameterAttributes.Optional) != 0;

    public bool IsOut => (myParameter.Attributes & ParameterAttributes.Out) != 0;

    private string DebugView => $"#{Index}: {Name}";
  }
}