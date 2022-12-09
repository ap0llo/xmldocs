namespace Grynwald.XmlDocReader.MarkdownRenderer;

//TODO: Allow setting the heading levels
/// <summary>
/// A visitor that traverses a <see cref="DocumentationFile"/> and generates Markdown from it (as <see cref="MdBlock"/>).
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
public class ConvertToBlockVisitor : DocumentationVisitor
{
    private readonly IMarkdownConverter m_MarkdownConverter;
    private readonly Stack<MdContainerBlockBase> m_Stack = new(new[] { new MdContainerBlock() });
    private MdParagraph? m_CurrentParagraph;


    /// <summary>
    /// Gets the root block of the generated Markdown
    /// </summary>
    public MdBlock Result => m_Stack.Single();

    /// <summary>
    /// Gets the markdown block currently being appended to 
    /// </summary>
    private MdContainerBlockBase CurrentBlock => m_Stack.Peek();


    /// <summary>
    /// Initializes a new instance of <see cref="ConvertToBlockVisitor"/>
    /// </summary>
    /// <param name="markdownConverter">The <see cref="IMarkdownConverter"/> to use for creating inline elements.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="markdownConverter"/> is <c>null</c>.</exception>
    public ConvertToBlockVisitor(IMarkdownConverter markdownConverter)
    {
        m_MarkdownConverter = markdownConverter ?? throw new ArgumentNullException(nameof(markdownConverter));
    }


    /// <inheritdoc />
    public override void Visit(DocumentationFile documentationFile)
    {
        AppendBlock(new MdHeading(1, documentationFile.AssemblyName));
        base.Visit(documentationFile);
    }

    /// <inheritdoc />
    public override void Visit(NamespaceMemberElement member)
    {
        AppendBlock(new MdHeading(2, $"{member.Id.Name} Namespace"));
        base.Visit(member);
    }

    /// <inheritdoc />
    public override void Visit(TypeMemberElement member)
    {
        AppendBlock(new MdHeading(2, member.Id.Name));
        base.Visit(member);
    }

    /// <inheritdoc />
    public override void Visit(FieldMemberElement member)
    {
        AppendBlock(new MdHeading(2, $"{member.Id.Name} Field"));
        base.Visit(member);
    }

    /// <inheritdoc />
    public override void Visit(PropertyMemberElement member)
    {
        AppendBlock(new MdHeading(2, $"{member.Id.Name} Property"));
        base.Visit(member);
    }

    /// <inheritdoc />
    public override void Visit(MethodMemberElement member)
    {
        AppendBlock(new MdHeading(2, $"{member.Id.Name} Method"));
        base.Visit(member);
    }

    /// <inheritdoc />
    public override void Visit(EventMemberElement member)
    {
        AppendBlock(new MdHeading(2, $"{member.Id.Name} Event"));
        base.Visit(member);
    }

    /// <inheritdoc />
    public override void Visit(ParameterElement parameter)
    {
        AppendBlock(new MdParagraph(new MdCodeSpan(parameter.Name)));
        base.Visit(parameter);
    }

    /// <inheritdoc />
    public override void Visit(TypeParameterElement typeParameter)
    {
        AppendBlock(new MdParagraph(new MdCodeSpan(typeParameter.Name)));
        base.Visit(typeParameter);
    }

    /// <inheritdoc />
    public override void Visit(ExceptionElement exception)
    {
        AppendBlock(new MdParagraph(new MdCodeSpan(exception.Reference.Name)));
        base.Visit(exception);
    }

    /// <inheritdoc />
    public override void Visit(SeeAlsoUrlReferenceElement seeAlso)
    {
        MdSpan linkText;

        if (seeAlso.Text is not null)
        {
            linkText = m_MarkdownConverter.ConvertToSpan(seeAlso.Text);
        }
        else
        {
            linkText = new MdRawMarkdownSpan(seeAlso.Link);
        }

        AddToCurrentParagraph(new MdLinkSpan(linkText, seeAlso.Link));
        AddToCurrentParagraph(new MdRawMarkdownSpan(Environment.NewLine));
    }

    /// <inheritdoc />
    public override void Visit(SeeAlsoCodeReferenceElement seeAlso)
    {
        var span = seeAlso.Text is not null
            ? m_MarkdownConverter.ConvertToSpan(seeAlso.Text)
            : new MdCodeSpan(seeAlso.Reference.Name);

        AddToCurrentParagraph(span);
        AddToCurrentParagraph(new MdRawMarkdownSpan(Environment.NewLine));
    }

    /// <inheritdoc />
    public override void Visit(TextBlock textBlock)
    {
        EndCurrentParagraph();
        base.Visit(textBlock);
    }

    /// <inheritdoc />
    public override void Visit(PlainTextElement plainText)
    {
        AddToCurrentParagraph(plainText.Content);
        base.Visit(plainText);
    }

    /// <inheritdoc />
    public override void Visit(ListElement listElement)
    {
        //TODO: Implement full list support, use Sandcastle list example as guideline: http://ewsoftware.github.io/XMLCommentsGuide/html/4fe0d5e6-7a33-a185-f424-7ea249f15596.htm
        if (listElement.Type == ListType.Bullet || listElement.Type == ListType.Number)
        {

            var outputList = listElement.Type == ListType.Number ? new MdOrderedList() : (MdList)new MdBulletList();

            AppendBlock(outputList);

            foreach (var inputItem in listElement.Items)
            {
                // create a new list item and add it to the list
                var outputItem = new MdListItem();
                outputList.Add(outputItem);

                // make the list item the new current block
                m_Stack.Push(outputItem);

                // visit list item
                inputItem.Accept(this);

                // end the current paragraph and restore previous current block
                EndCurrentParagraph();
                m_Stack.Pop();
            }
        }
        else if (listElement.Type == ListType.Table)
        {

            MdTableRow CreateRow(ListItemElement? itemElement)
            {
                if (itemElement == null)
                {
                    return new MdTableRow("", "");
                }

                var term = itemElement.Term is null || itemElement.Term.Elements.Count == 0
                    ? MdEmptySpan.Instance
                    : m_MarkdownConverter.ConvertToSpan(itemElement.Term);

                var description = itemElement.Description is null || itemElement.Description.Elements.Count == 0
                    ? MdEmptySpan.Instance
                    : m_MarkdownConverter.ConvertToSpan(itemElement.Description);

                return new MdTableRow(term, description);
            }

            var table = new MdTable(
                CreateRow(listElement.ListHeader),
                listElement.Items.Select(CreateRow)
            );

            AppendBlock(table);
        }
    }

    /// <inheritdoc />
    public override void Visit(ListItemElement item)
    {
        if (item.Term is not null && item.Term.Elements.Count > 0)
        {
            var term = m_MarkdownConverter.ConvertToSpan(item.Term);

            AddToCurrentParagraph(new MdStrongEmphasisSpan(term, ":"));
        }

        item.Description.Accept(this);
    }

    /// <inheritdoc />
    public override void Visit(CElement element)
    {
        if (!String.IsNullOrEmpty(element.Content))
        {
            AddToCurrentParagraph(new MdCodeSpan(element.Content));
        }
    }

    /// <inheritdoc />
    public override void Visit(CodeElement code)
    {
        AppendBlock(new MdCodeBlock(code.Content, code.Language));
    }

    /// <inheritdoc />
    public override void Visit(ParameterReferenceElement paramRef)
    {
        AddToCurrentParagraph(new MdCodeSpan(paramRef.Name));
    }

    /// <inheritdoc />
    public override void Visit(TypeParameterReferenceElement typeParamRef)
    {
        AddToCurrentParagraph(new MdCodeSpan(typeParamRef.Name));
    }

    /// <inheritdoc />
    public override void Visit(SeeCodeReferenceElement see)
    {
        var span = see.Text is not null
            ? m_MarkdownConverter.ConvertToSpan(see.Text)
            : new MdCodeSpan(see.Reference.Name);

        AddToCurrentParagraph(span);
    }

    /// <inheritdoc />
    public override void Visit(SeeUrlReferenceElement see)
    {
        MdSpan linkText;

        if (see.Text is not null)
        {
            linkText = m_MarkdownConverter.ConvertToSpan(see.Text);
        }
        else
        {
            linkText = new MdRawMarkdownSpan(see.Link);
        }

        AddToCurrentParagraph(new MdLinkSpan(linkText, see.Link));
    }

    /// <inheritdoc />
    public override void Visit(SummaryElement summary)
    {
        if (summary.Text is not null)
        {
            AppendBlock(new MdHeading(3, "Summary"));
            base.Visit(summary);
        }
    }

    /// <inheritdoc />
    public override void Visit(RemarksElement remarks)
    {
        if (remarks.Text is not null)
        {
            AppendBlock(new MdHeading(3, "Remarks"));
            base.Visit(remarks);
        }
    }

    /// <inheritdoc />
    public override void Visit(ValueElement value)
    {
        if (value is not null)
        {
            AppendBlock(new MdHeading(3, "Value"));
            base.Visit(value);
        }
    }

    /// <inheritdoc />
    public override void Visit(ReturnsElement returns)
    {
        if (returns.Text is not null)
        {
            AppendBlock(new MdHeading(3, "Returns"));
            base.Visit(returns);
        }
    }

    /// <inheritdoc />
    public override void Visit(ExampleElement example)
    {
        if (example.Text is not null)
        {
            AppendBlock(new MdHeading(3, "Example"));
            base.Visit(example);
        }
    }

    /// <inheritdoc />
    public override void Visit(EmphasisElement emphasis)
    {
        if (!String.IsNullOrEmpty(emphasis.Content))
        {
            AddToCurrentParagraph(new MdEmphasisSpan(emphasis.Content));
        }
    }

    /// <inheritdoc />
    public override void Visit(IdiomaticElement idiomatic)
    {
        if (!String.IsNullOrEmpty(idiomatic.Content))
        {
            AddToCurrentParagraph(new MdEmphasisSpan(idiomatic.Content));
        }
    }

    /// <inheritdoc />
    public override void Visit(BoldElement bold)
    {
        if (!String.IsNullOrEmpty(bold.Content))
        {
            AddToCurrentParagraph(new MdStrongEmphasisSpan(bold.Content));
        }
    }

    /// <inheritdoc />
    public override void Visit(UnrecognizedTextElement unrecognizedElement)
    {
        AddToCurrentParagraph(unrecognizedElement.Xml.ToString());
        base.Visit(unrecognizedElement);
    }

    /// <inheritdoc />
    public override void VisitParameters(MemberElement methodOrProperty, IReadOnlyList<ParameterElement> parameters)
    {
        if (parameters.Count > 0)
        {
            AppendBlock(new MdHeading(3, "Parameters"));
            base.VisitParameters(methodOrProperty, parameters);
        }
    }

    /// <inheritdoc />
    public override void VisitTypeParameters(MemberElement typeOrMethod, IReadOnlyCollection<TypeParameterElement> typeParameters)
    {
        if (typeParameters.Count > 0)
        {
            AppendBlock(new MdHeading(3, "Type Parameters"));
            base.VisitTypeParameters(typeOrMethod, typeParameters);
        }
    }

    /// <inheritdoc />
    public override void VisitExceptions(MemberElement methodOrPropertyOrEvent, IReadOnlyList<ExceptionElement> exceptions)
    {
        if (exceptions.Count > 0)
        {
            AppendBlock(new MdHeading(3, "Exceptions"));
            base.VisitExceptions(methodOrPropertyOrEvent, exceptions);
        }
    }

    /// <inheritdoc />
    public override void VisitSeeAlso(MemberElement member)
    {
        if (member.SeeAlso.Count > 0)
        {
            AppendBlock(new MdHeading(3, "See Also"));
            base.VisitSeeAlso(member);
        }
    }


    /// <summary>
    /// Appends the specified block to the output
    /// </summary>
    protected virtual void AppendBlock(MdBlock block)
    {
        EndCurrentParagraph();
        CurrentBlock.Add(block);
    }

    /// <summary>
    /// Adds the specified span to the current paragraph.
    /// A new paragraph is created implicitly when there is no current paragraph.
    /// </summary>
    protected virtual void AddToCurrentParagraph(MdSpan span)
    {
        // create paragraph and add it to the current block
        if (m_CurrentParagraph == null)
        {
            m_CurrentParagraph = new MdParagraph();
            CurrentBlock.Add(m_CurrentParagraph);
        }

        m_CurrentParagraph.Add(span);
    }

    /// <summary>
    /// Ends the current paragraph.
    /// Any further content via <see cref="AddToCurrentParagraph(MdSpan)"/> will be added to a new paragraph.
    /// </summary>
    protected virtual void EndCurrentParagraph()
    {
        m_CurrentParagraph = null;
    }
}
