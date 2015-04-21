namespace System.Reflection.Metadata.Extensions
{
  public enum MetadataVisibility : byte
  {
    Private = 1,
    ProtectedAndInternal = 1,
    Protected = 2,
    ProtectedOrInternal = 3,
    Internal = 4,
    Public = 5
  }
}