namespace Grynwald.XmlDocReader;

public interface IDocumentationVisitor
{
    void Visit(DocumentationFile documentationFile);

    void Visit(MemberDescription member);

    void Visit(TextBlock textBlock);

    void Visit(ParameterDescription param);

    void Visit(TypeParameterDescription typeParam);

    void Visit(ExceptionDescription exception);

    void Visit(ListElement list);

    void Visit(CElement c);

    void Visit(CodeElement code);

    void Visit(ListItemElement item);

    void Visit(ParagraphElement para);

    void Visit(ParameterReferenceElement paramRef);

    void Visit(PlainTextElement plainText);

    void Visit(SeeCodeReferenceElement see);

    void Visit(SeeUrlReferenceElement see);

    void Visit(TypeParameterReferenceElement typeParamRef);

    void Visit(SeeAlsoUrlReferenceDescription seeAlso);

    void Visit(SeeAlsoCodeReferenceDescription seeAlso);
}
