using System.Diagnostics;
using JetBrains.Annotations;

namespace System.Reflection.Metadata.Extensions
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

    public string Name
    {
      get { return myMetadataReader.GetString(myParameter.Name); }
    }

    public int Index
    {
      get { return myParameter.SequenceNumber; }
    }

    public bool IsOptional
    {
      get { return (myParameter.Attributes & ParameterAttributes.Optional) != 0; }
    }

    public bool IsOut
    {
      get { return (myParameter.Attributes & ParameterAttributes.Out) != 0; }
    }

    private string DebugView
    {
      get { return string.Format("#{0}: {1}", Index, Name); }
    }
  }
}