using System.Collections.Immutable;
using System.Reflection.Metadata.Decoding;
using System.Reflection.Metadata.Model;
using JetBrains.Annotations;

namespace System.Reflection.Metadata.ILReader
{
  public class MetadataModelTypeProvider : ISignatureTypeProvider<MetadataTypeSpecification>
  {
    [NotNull] private readonly MetadataReader myReader;

    [NotNull] public MetadataReader Reader { get { return myReader; } }

    public MetadataModelTypeProvider([NotNull] MetadataReader reader)
    {
      myReader = reader;
    }

    public MetadataTypeSpecification GetGenericInstance(
      MetadataTypeSpecification genericType, ImmutableArray<MetadataTypeSpecification> typeArguments)
    {
      throw new NotImplementedException();
    }

    public MetadataTypeSpecification GetArrayType(MetadataTypeSpecification elementType, ArrayShape shape)
    {
      throw new NotImplementedException();
    }

    public MetadataTypeSpecification GetByReferenceType(MetadataTypeSpecification elementType)
    {
      throw new NotImplementedException();
    }

    public MetadataTypeSpecification GetSZArrayType(MetadataTypeSpecification elementType)
    {
      throw new NotImplementedException();
    }

    public MetadataTypeSpecification GetPointerType(MetadataTypeSpecification elementType)
    {
      throw new NotImplementedException();
    }

    public MetadataTypeSpecification GetFunctionPointerType(MethodSignature<MetadataTypeSpecification> signature)
    {
      throw new NotImplementedException();
    }

    public MetadataTypeSpecification GetGenericMethodParameter(int index)
    {
      throw new NotImplementedException();
    }

    public MetadataTypeSpecification GetGenericTypeParameter(int index)
    {
      throw new NotImplementedException();
    }

    public MetadataTypeSpecification GetModifiedType(
      MetadataTypeSpecification unmodifiedType, ImmutableArray<CustomModifier<MetadataTypeSpecification>> customModifiers)
    {
      throw new NotImplementedException();
    }

    public MetadataTypeSpecification GetPinnedType(MetadataTypeSpecification elementType)
    {
      throw new NotImplementedException();
    }

    public MetadataTypeSpecification GetPrimitiveType(PrimitiveTypeCode typeCode)
    {
      throw new NotImplementedException();
    }

    public MetadataTypeSpecification GetTypeFromDefinition(TypeDefinitionHandle handle)
    {
      throw new NotImplementedException();
    }

    public MetadataTypeSpecification GetTypeFromReference(TypeReferenceHandle handle)
    {
      throw new NotImplementedException();
    }
  }
}