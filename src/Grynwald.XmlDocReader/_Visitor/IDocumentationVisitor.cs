namespace Grynwald.XmlDocReader;

/// <summary>
/// The interface for a visitor that traverses the object structure of an XML documentation file.
/// </summary>
/// <seealso href="https://en.wikipedia.org/wiki/Visitor_pattern">Visitor pattern (Wikipedia)</seealso>
public interface IDocumentationVisitor
{
    void Visit(DocumentationFile documentationFile);

    void Visit(NamespaceMemberElement member);

    void Visit(TypeMemberElement member);

    void Visit(FieldMemberElement member);

    void Visit(PropertyMemberElement member);

    void Visit(MethodMemberElement member);

    void Visit(EventMemberElement member);

    void Visit(ParameterElement param);

    void Visit(TypeParameterElement typeParam);

    void Visit(ExceptionElement exception);

    void Visit(SeeAlsoUrlReferenceElement seeAlso);

    void Visit(SeeAlsoCodeReferenceElement seeAlso);

    void Visit(SummaryElement summary);

    void Visit(RemarksElement summary);

    void Visit(ExampleElement summary);

    void Visit(ValueElement summary);

    void Visit(ReturnsElement summary);

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
