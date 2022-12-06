namespace Grynwald.XmlDocReader.MarkdownRenderer;

public class ConvertToSpanVisitor : ConvertVisitorBase
{
    private readonly Stack<MdCompositeSpan> m_Stack = new();


    public MdCompositeSpan Result => m_Stack.Single();

    private MdCompositeSpan CurrentSpan => m_Stack.Peek();


    public ConvertToSpanVisitor()
    {
        m_Stack.Push(new MdCompositeSpan());
    }


    public override void Visit(DocumentationFile documentationFile) => ThrowUnsupportedNode();

    public override void Visit(NamespaceMemberElement member) => ThrowUnsupportedNode();

    public override void Visit(TypeMemberElement member) => ThrowUnsupportedNode();

    public override void Visit(FieldMemberElement member) => ThrowUnsupportedNode();

    public override void Visit(PropertyMemberElement member) => ThrowUnsupportedNode();

    public override void Visit(MethodMemberElement member) => ThrowUnsupportedNode();

    public override void Visit(EventMemberElement member) => ThrowUnsupportedNode();

    public override void Visit(ParameterElement param) => ThrowUnsupportedNode();

    public override void Visit(TypeParameterElement typeParam) => ThrowUnsupportedNode();

    public override void Visit(ExceptionElement exception) => ThrowUnsupportedNode();

    public override void Visit(SeeAlsoUrlReferenceElement seeAlso) => ThrowUnsupportedNode();

    public override void Visit(SeeAlsoCodeReferenceElement seeAlso) => ThrowUnsupportedNode();

    public override void Visit(PlainTextElement plainText)
    {
        CurrentSpan.Add(new MdTextSpan(plainText.Content));
    }

    public override void Visit(ListElement list) => ThrowUnsupportedNode();

    public override void Visit(ListItemElement item) => ThrowUnsupportedNode();

    public override void Visit(CElement c)
    {
        if (!String.IsNullOrEmpty(c.Content))
            CurrentSpan.Add(new MdCodeSpan(c.Content));
    }

    public override void Visit(CodeElement code) => ThrowUnsupportedNode();

    public override void Visit(ParagraphElement para)
    {
        // a single span cannot contain multiple paragraphs, but we can at least add a line break
        CurrentSpan.Add(Environment.NewLine);

        // visit text block in paragraph        
        base.Visit(para);
    }

    public override void Visit(ParameterReferenceElement paramRef)
    {
        CurrentSpan.Add(new MdCodeSpan(paramRef.Name));
    }

    public override void Visit(TypeParameterReferenceElement typeParamRef)
    {
        CurrentSpan.Add(new MdCodeSpan(typeParamRef.Name));
    }

    /// <inheritdoc />
    public override void Visit(SeeCodeReferenceElement see)
    {
        MdSpan textSpan;

        if (see.Text is not null)
        {
            BeginNestedSpan();
            see.Text.Accept(this);
            textSpan = EndNestedSpan();
        }
        else
        {
            textSpan = new MdCodeSpan(see.Reference.Name);
        }

        // Default implementation cannot resolve "cref" values because that would require a semantic model of assembly
        var linkTarget = TryGetLinkForCodeReference(see.Reference);
        if (linkTarget is not null)
        {
            textSpan = new MdLinkSpan(textSpan, linkTarget);
        }

        CurrentSpan.Add(textSpan);
    }

    /// <inheritdoc />
    public override void Visit(SeeUrlReferenceElement see)
    {
        MdSpan linkText;

        if (see.Text is not null)
        {
            BeginNestedSpan();
            see.Text.Accept(this);
            linkText = EndNestedSpan();
        }
        else
        {
            linkText = new MdRawMarkdownSpan(see.Link);
        }

        CurrentSpan.Add(new MdLinkSpan(linkText, see.Link));
    }


    protected virtual void BeginNestedSpan()
    {
        m_Stack.Push(new MdCompositeSpan());
    }

    protected virtual MdCompositeSpan EndNestedSpan()
    {
        return m_Stack.Pop();
    }


    private void ThrowUnsupportedNode() => throw new InvalidOperationException($"{nameof(ConvertToSpanVisitor)} can only convert text elements");
}
