using System.Diagnostics;
using JetBrains.Annotations;

namespace System.Reflection.Metadata.Model
{
  [DebuggerDisplay("{DebugView,nq}")]
  public struct MetadataTypeSpecification
  {
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [NotNull] private readonly MetadataReader myMetadataReader;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private TypeSpecification myTypeSpecification;

    public MetadataTypeSpecification([NotNull] MetadataReader metadataReader, TypeSpecification typeSpecification)
    {
      myMetadataReader = metadataReader;
      myTypeSpecification = typeSpecification;

      //metadataReader.

      //metadataReader.GetGenericParameterConstraint().

      var signature = metadataReader.GetBlobReader(typeSpecification.Signature);
      var signatureTypeCode = signature.ReadSignatureTypeCode();

      


      //metadataReader.GetTypeSpecification().
    }

    [NotNull]
    public string FullName
    {
      get { return string.Empty; }
    }

    [NotNull, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebugView
    {
      get { return "[typespec] " + FullName; }
    }

    public override string ToString()
    {
      return FullName;
    }
  }
}