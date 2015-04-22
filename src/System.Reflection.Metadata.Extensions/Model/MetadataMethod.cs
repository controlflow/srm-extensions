using System.Collections.Immutable;
using System.Diagnostics;
using JetBrains.Annotations;

namespace System.Reflection.Metadata.Model
{
  [DebuggerDisplay("{Name}")]
  public struct MetadataMethod
  {
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [NotNull] private readonly MetadataReader myMetadataReader;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private MethodDefinition myMethodDefinition;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private ImmutableArray<MetadataParameter> myParameters;

    public MetadataMethod([NotNull] MetadataReader metadataReader, MethodDefinition methodDefinition)
    {
      myMethodDefinition = methodDefinition;
      myMetadataReader = metadataReader;
      myParameters = default(ImmutableArray<MetadataParameter>);
    }

    public MethodDefinition Definition
    {
      get { return myMethodDefinition; }
    }

    [NotNull] public string Name
    {
      get { return myMetadataReader.GetString(myMethodDefinition.Name); }
    }

    public ImmutableArray<MetadataParameter> Parameters
    {
      get
      {
        if (myParameters.IsDefault)
        {
          var handleCollection = myMethodDefinition.GetParameters();
          var builder = ImmutableArray.CreateBuilder<MetadataParameter>(handleCollection.Count);

          foreach (var parameterHandle in handleCollection)
          {
            var parameter = myMetadataReader.GetParameter(parameterHandle);
            builder.Add(new MetadataParameter(myMetadataReader, parameter));
          }

          myParameters = builder.MoveToImmutable();
        }

        return myParameters;
      }
    }
  }
}