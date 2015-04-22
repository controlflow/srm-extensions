using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using JetBrains.Annotations;

namespace System.Reflection.Metadata.Extensions
{
  [DebuggerDisplay("{FullName,nq}")]
  public struct MetadataAssemblyReference : IEquatable<MetadataAssemblyReference>
  {
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [NotNull] private readonly MetadataReader myMetadataReader;
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private AssemblyReference myAssemblyReference;

    public MetadataAssemblyReference([NotNull] MetadataReader metadataReader, AssemblyReference assemblyReference)
    {
      myMetadataReader = metadataReader;
      myAssemblyReference = assemblyReference;
    }

    [NotNull] public string Name
    {
      get { return myMetadataReader.GetString(myAssemblyReference.Name); }
    }

    [NotNull] public string FullName
    {
      get
      {
        var builder = new StringBuilder();
        var assemblyReference = myAssemblyReference;

        var shortName = myMetadataReader.GetString(assemblyReference.Name);
        builder.Append(shortName);

        var version = assemblyReference.Version;
        builder.Append(", Version=").Append(version);

        var culture = assemblyReference.Culture;
        builder.Append(", Culture=");
        builder.Append(culture.IsNil ? "neutral" : myMetadataReader.GetString(culture));

        var blobReader = myMetadataReader.GetBlobReader(assemblyReference.PublicKeyOrToken);
        builder.Append(", PublicKeyToken=");

        while (blobReader.RemainingBytes > 0)
        {
          var tokenByte = blobReader.ReadByte();
          builder.Append(tokenByte.ToString("x"));
        }

        return builder.ToString();
      }
    }

    [NotNull] public Version Version
    {
      get { return myAssemblyReference.Version; }
    }

    public bool Equals(MetadataAssemblyReference other)
    {
      if (myMetadataReader != other.myMetadataReader) return false;

      var reference = myAssemblyReference;
      var otherReference = other.myAssemblyReference;
      if (reference.Name != otherReference.Name) return false;
      if (reference.Culture != otherReference.Culture) return false;
      if (reference.Flags != otherReference.Flags) return false;
      if (reference.PublicKeyOrToken != otherReference.PublicKeyOrToken) return false;
      if (reference.HashValue != otherReference.HashValue) return false;
      if (reference.Version != otherReference.Version) return false;

      return true;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      return obj is MetadataAssemblyReference && Equals((MetadataAssemblyReference) obj);
    }

    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public override int GetHashCode()
    {
      unchecked
      {
        var reference = myAssemblyReference;
        var hashCode = myMetadataReader.GetHashCode();

        hashCode = (hashCode * 397) ^ reference.Name.GetHashCode();
        hashCode = (hashCode * 397) ^ reference.Culture.GetHashCode();
        hashCode = (hashCode * 397) ^ (int) reference.Flags;
        hashCode = (hashCode * 397) ^ reference.PublicKeyOrToken.GetHashCode();
        hashCode = (hashCode * 397) ^ reference.HashValue.GetHashCode();
        //hashCode = (hashCode * 397) ^ reference.Version.GetHashCode();

        return hashCode;
      }
    }
  }
}