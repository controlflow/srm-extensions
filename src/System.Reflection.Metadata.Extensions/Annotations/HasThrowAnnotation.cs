using JetBrains.Annotations;

namespace System.Reflection.Metadata.Annotations
{
  public class HasThrowAnnotation : Annotation
  {
    
  }

  public class Annotation
  {

  }

  public class Walker
  {
    public void Infer([NotNull] MetadataReader reader)
    {
      foreach (var definitionHandle in reader.MethodDefinitions)
      {
        var methodDefinition = reader.GetMethodDefinition(definitionHandle);

        var virtualAddress = methodDefinition.RelativeVirtualAddress;
        if (virtualAddress == 0) continue;


      }
    }
  }

  public abstract class AnnotationsCollector
  {
    public void PutAnnotation(MethodDefinition methodDefinition, [NotNull] Annotation annotation)
    {
      
    }
  }

  public class XmlAnnotationsCollector : AnnotationsCollector
  {

  }

  public class CompiledCollector : AnnotationsCollector
  {
    
  }
}