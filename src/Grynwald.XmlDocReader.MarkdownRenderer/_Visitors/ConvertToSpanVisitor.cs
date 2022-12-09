namespace Grynwald.XmlDocReader.MarkdownRenderer;

/// <summary>
/// A visitor that converts a <see cref="TextElement"/> to Markdown (as <see cref="MdSpan"/>).
/// </summary>
/// <remarks>
/// Note that this class is intended to serve as the basis for a documentation generator by offering conversions to Markdown for the contents of an XML documentation file
/// but is not sufficient to generate the complete documentation for a .NET library.
/// <para>
/// The XML documentation file does not contain all members of an assembly but only the members for which the compiler found any XML documentation comments.
/// To generate the full documentation of an assembly requires building a semantic model of that assembly which can be achieved using libraries like Mono.Cecil or Roslyn (Microsoft.CodeAnalyis).
/// </para>
/// <para>
/// The semantic model is also required to resolve references between elements (e.g. <c><![CDATA[<see cref="SomeClass"/>]]></c>) which this implementation also will not be able to handle.
/// To resolve references, you can customize this visitor by overriding the corresponding <c>Visit()</c> methods.
/// </para>
/// </remarks>
/// <seealso href="https://en.wikipedia.org/wiki/Visitor_pattern">Visitor pattern (Wikipedia)</seealso>
public class ConvertToSpanVisitor : DocumentationVisitor
{
    private readonly Stack<MdCompositeSpan> m_Stack = new();


    public MdCompositeSpan Result => m_Stack.Single();

    private MdCompositeSpan CurrentSpan => m_Stack.Peek();


    public ConvertToSpanVisitor()
    {
        m_Stack.Push(new MdCompositeSpan());
    }


    /// <inheritdoc />
    public override void Visit(DocumentationFile documentationFile) => ThrowUnsupportedNode(documentationFile);

    /// <inheritdoc />
    public override void Visit(NamespaceMemberElement @namespace) => ThrowUnsupportedNode(@namespace);

    /// <inheritdoc />
    public override void Visit(TypeMemberElement type) => ThrowUnsupportedNode(type);

    /// <inheritdoc />
    public override void Visit(FieldMemberElement field) => ThrowUnsupportedNode(field);

    /// <inheritdoc />
    public override void Visit(PropertyMemberElement property) => ThrowUnsupportedNode(property);

    /// <inheritdoc />
    public override void Visit(MethodMemberElement method) => ThrowUnsupportedNode(method);

    /// <inheritdoc />
    public override void Visit(EventMemberElement @event) => ThrowUnsupportedNode(@event);

    /// <inheritdoc />
    public override void Visit(ParameterElement param) => ThrowUnsupportedNode(param);

    /// <inheritdoc />
    public override void Visit(TypeParameterElement typeParam) => ThrowUnsupportedNode(typeParam);

    /// <inheritdoc />
    public override void Visit(ExceptionElement exception) => ThrowUnsupportedNode(exception);

    /// <inheritdoc />
    public override void Visit(SeeAlsoUrlReferenceElement seeAlso) => ThrowUnsupportedNode(seeAlso);

    /// <inheritdoc />
    public override void Visit(SeeAlsoCodeReferenceElement seeAlso) => ThrowUnsupportedNode(seeAlso);

    /// <inheritdoc />
    public override void Visit(PlainTextElement plainText)
    {
        CurrentSpan.Add(new MdTextSpan(plainText.Content));
    }

    /// <inheritdoc />
    public override void Visit(ListElement list) => ThrowUnsupportedNode(list);

    /// <inheritdoc />
    public override void Visit(ListItemElement item) => ThrowUnsupportedNode(item);

    /// <inheritdoc />
    public override void Visit(CElement c)
    {
        if (!String.IsNullOrEmpty(c.Content))
            CurrentSpan.Add(new MdCodeSpan(c.Content));
    }

    /// <inheritdoc />
    public override void Visit(CodeElement code) => ThrowUnsupportedNode(code);

    /// <inheritdoc />
    public override void Visit(ParagraphElement para)
    {
        // a single span cannot contain multiple paragraphs, but we can at least add a line break
        CurrentSpan.Add(Environment.NewLine);

        // visit text block in paragraph        
        base.Visit(para);
    }

    /// <inheritdoc />
    public override void Visit(ParameterReferenceElement paramRef)
    {
        CurrentSpan.Add(new MdCodeSpan(paramRef.Name));
    }

    /// <inheritdoc />
    public override void Visit(TypeParameterReferenceElement typeParamRef)
    {
        CurrentSpan.Add(new MdCodeSpan(typeParamRef.Name));
    }

    /// <inheritdoc />
    public override void Visit(SeeCodeReferenceElement see)
    {
        if (see.Text is not null)
        {
            see.Text.Accept(this);
        }
        else
        {
            CurrentSpan.Add(new MdCodeSpan(see.Reference.Name));
        }
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

    /// <inheritdoc />
    public override void Visit(EmphasisElement emphasis)
    {
        CurrentSpan.Add(new MdEmphasisSpan(emphasis.Content));
    }

    public override void Visit(IdiomaticElement idiomatic)
    {
        CurrentSpan.Add(new MdEmphasisSpan(idiomatic.Content));
    }

    /// <inheritdoc />
    public override void Visit(UnrecognizedTextElement unrecognizedElement)
    {
        CurrentSpan.Add(unrecognizedElement.Xml.ToString());
    }

    protected virtual void BeginNestedSpan()
    {
        m_Stack.Push(new MdCompositeSpan());
    }

    protected virtual MdCompositeSpan EndNestedSpan()
    {
        return m_Stack.Pop();
    }


    private void ThrowUnsupportedNode(DocumentationElement element) => throw new InvalidOperationException($"Elments of type {element.GetType().Name} cannot be converted to inline Markdown text");
}
