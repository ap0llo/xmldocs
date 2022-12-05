namespace Grynwald.XmlDocReader;

/// <summary>
/// The interface for a visitor that traverses the object structure of a XML documentation file.
/// </summary>
public interface IDocumentationVisitor
{
    void Visit(DocumentationFile documentationFile);

    void Visit(NamespaceDescription member);

    void Visit(TypeDescription member);

    void Visit(FieldDescription member);

    void Visit(PropertyDescription member);
    void Visit(MethodDescription member);

    void Visit(EventDescription member);

    void Visit(ParameterDescription param);

    void Visit(TypeParameterDescription typeParam);

    void Visit(ExceptionDescription exception);

    void Visit(SeeAlsoUrlReferenceDescription seeAlso);

    void Visit(SeeAlsoCodeReferenceDescription seeAlso);

    void Visit(TextBlock textBlock);

    void Visit(PlainTextElement plainText);

    void Visit(ListElement list);

    void Visit(ListItemElement item);

    void Visit(CElement c);

    void Visit(CodeElement code);

    void Visit(ParagraphElement para);

    void Visit(ParameterReferenceElement paramRef);

    void Visit(TypeParameterReferenceElement typeParamRef);

    void Visit(SeeCodeReferenceElement see);

    void Visit(SeeUrlReferenceElement see);
}
