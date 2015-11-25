using System.Collections.Immutable;
using System.Diagnostics;
using JetBrains.Annotations;

namespace System.Reflection.Metadata.Model
{
  [DebuggerDisplay("{ToString()}")]
  public struct MetadataMethodDefinition
  {
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [NotNull] private readonly MetadataReader myMetadataReader;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private MethodDefinition myMethodDefinition;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private ImmutableArray<MetadataParameter> myParameters;

    public MetadataMethodDefinition([NotNull] MetadataReader metadataReader, MethodDefinition methodDefinition)
    {
      myMethodDefinition = methodDefinition;
      myMetadataReader = metadataReader;
      myParameters = default(ImmutableArray<MetadataParameter>);
    }

    public MethodDefinition Definition => myMethodDefinition;

    [NotNull] public string Name => myMetadataReader.GetString(myMethodDefinition.Name);

    public MetadataTypeDefinition? ContainingType
    {
      get
      {
        var declaringType = myMethodDefinition.GetDeclaringType();
        if (declaringType.IsNil) return null;

        var typeDefinition = myMetadataReader.GetTypeDefinition(declaringType);
        return new MetadataTypeDefinition(myMetadataReader, typeDefinition);
      }
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

    public override string ToString()
    {
      if (!ContainingType.HasValue) return Name;

      return ContainingType.Value.FullName + "." + Name;
    }
  }
}