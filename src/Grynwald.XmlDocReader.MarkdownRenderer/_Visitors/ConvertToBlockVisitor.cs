namespace Grynwald.XmlDocReader.MarkdownRenderer;

//TODO: Allow setting the heading levels
public class ConvertToBlockVisitor : ConvertVisitorBase
{
    private readonly IMarkdownConverter m_MarkdownConverter;
    private readonly Stack<MdContainerBlockBase> m_Stack = new(new[] { new MdContainerBlock() });
    private MdParagraph? m_CurrentParagraph;

    /// <summary>
    /// Gets the root block of the generated markdown
    /// </summary>
    public MdBlock Result => m_Stack.Single();

    /// <summary>
    /// Gets the markdown block currently being appended to 
    /// </summary>
    private MdContainerBlockBase CurrentBlock => m_Stack.Peek();


    public ConvertToBlockVisitor(IMarkdownConverter markdownConverter)
    {
        m_MarkdownConverter = markdownConverter ?? throw new ArgumentNullException(nameof(markdownConverter));
    }

    /// <inheritdoc />
    public override void Visit(DocumentationFile documentationFile)
    {
        EndParagraph();
        CurrentBlock.Add(new MdHeading(1, documentationFile.AssemblyName));

        base.Visit(documentationFile);
    }

    /// <inheritdoc />
    public override void Visit(NamespaceDescription member)
    {
        EndParagraph();

        CurrentBlock.Add(new MdHeading(2, $"{member.Id.Name} Namespace"));

        base.Visit(member);
    }


    /// <inheritdoc />
    public override void Visit(TypeDescription member)
    {
        EndParagraph();

        CurrentBlock.Add(new MdHeading(2, member.Id.Name));

        base.Visit(member);
    }


    /// <inheritdoc />
    public override void Visit(FieldDescription member)
    {
        EndParagraph();

        CurrentBlock.Add(new MdHeading(2, $"{member.Id.Name} Field"));

        base.Visit(member);
    }


    /// <inheritdoc />
    public override void Visit(PropertyDescription member)
    {
        EndParagraph();

        CurrentBlock.Add(new MdHeading(2, $"{member.Id.Name} Property"));

        base.Visit(member);
    }



    /// <inheritdoc />
    public override void Visit(MethodDescription member)
    {
        EndParagraph();

        CurrentBlock.Add(new MdHeading(2, $"{member.Id.Name} Method"));

        base.Visit(member);
    }


    /// <inheritdoc />
    public override void Visit(EventDescription member)
    {
        EndParagraph();

        CurrentBlock.Add(new MdHeading(2, $"{member.Id.Name} Event"));

        base.Visit(member);
    }

    /// <inheritdoc />
    public override void Visit(ParameterDescription parameter)
    {
        EndParagraph();

        AddToCurrentParagraph(new MdCodeSpan(parameter.Name));

        EndParagraph();

        base.Visit(parameter);
    }

    /// <inheritdoc />
    public override void Visit(TypeParameterDescription typeParameter)
    {
        EndParagraph();

        AddToCurrentParagraph(new MdCodeSpan(typeParameter.Name));

        EndParagraph();

        base.Visit(typeParameter);
    }

    /// <inheritdoc />
    public override void Visit(ExceptionDescription exception)
    {
        EndParagraph();

        AddToCurrentParagraph(new MdCodeSpan(exception.Reference.Name));

        EndParagraph();

        //TODO: Use TryGetLinkForCodeReference

        base.Visit(exception);
    }

    /// <inheritdoc />
    public override void Visit(SeeAlsoUrlReferenceDescription seeAlso)
    {
        MdSpan linkText;

        if (seeAlso.Text is not null && seeAlso.Text.Elements.Count > 0)
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
    public override void Visit(SeeAlsoCodeReferenceDescription seeAlso)
    {
        MdSpan textSpan;

        if (seeAlso.Text is not null && seeAlso.Text.Elements.Count > 0)
        {
            textSpan = m_MarkdownConverter.ConvertToSpan(seeAlso.Text);
        }
        else
        {
            textSpan = new MdCodeSpan(seeAlso.Reference.Name);
        }

        // Default implementation cannot resolve "cref" values because that would require a semantic model of assembly
        var linkTarget = TryGetLinkForCodeReference(seeAlso.Reference);
        if (linkTarget is not null)
        {
            textSpan = new MdLinkSpan(textSpan, linkTarget);
        }

        AddToCurrentParagraph(textSpan);
        AddToCurrentParagraph(new MdRawMarkdownSpan(Environment.NewLine));
    }

    /// <inheritdoc />
    public override void Visit(TextBlock textBlock)
    {
        EndParagraph();

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
            EndParagraph();

            var outputList = listElement.Type == ListType.Number ? new MdOrderedList() : (MdList)new MdBulletList();

            CurrentBlock.Add(outputList);

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
                EndParagraph();
                m_Stack.Pop();
            }
        }
        else if (listElement.Type == ListType.Table)
        {
            EndParagraph();

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

            CurrentBlock.Add(table);
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
        EndParagraph();
        CurrentBlock.Add(new MdCodeBlock(code.Content, code.Language));
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
        MdSpan textSpan;

        if (see.Text is not null && see.Text.Elements.Count > 0)
        {
            textSpan = m_MarkdownConverter.ConvertToSpan(see.Text);
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

        AddToCurrentParagraph(textSpan);
    }

    /// <inheritdoc />
    public override void Visit(SeeUrlReferenceElement see)
    {
        MdSpan linkText;

        if (see.Text is not null && see.Text.Elements.Count > 0)
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
    public override void VisitSummary(MemberDescription member)
    {
        if (member.Summary is not null)
        {
            EndParagraph();
            CurrentBlock.Add(new MdHeading(3, "Summary"));
            base.VisitSummary(member);
        }
    }

    /// <inheritdoc />
    public override void VisitRemarks(MemberDescription member)
    {
        if (member.Remarks is not null)
        {
            EndParagraph();
            CurrentBlock.Add(new MdHeading(3, "Remarks"));
            base.VisitRemarks(member);
        }
    }

    /// <inheritdoc />
    public override void VisitValue(MemberDescription propertyOrValue, TextBlock? value)
    {
        if (value is not null)
        {
            EndParagraph();
            CurrentBlock.Add(new MdHeading(3, "Value"));
            base.VisitValue(propertyOrValue, value);
        }
    }

    /// <inheritdoc />
    public override void VisitReturns(MethodDescription member)
    {
        if (member.Returns is not null)
        {
            EndParagraph();
            CurrentBlock.Add(new MdHeading(3, "Returns"));
            base.VisitReturns(member);
        }
    }

    /// <inheritdoc />
    public override void VisitExample(MemberDescription member)
    {
        if (member.Example is not null)
        {
            EndParagraph();
            CurrentBlock.Add(new MdHeading(3, "Example"));
            base.VisitExample(member);
        }
    }

    /// <inheritdoc />
    public override void VisitParameters(MemberDescription methodOrProperty, IReadOnlyList<ParameterDescription> parameters)
    {
        if (parameters.Count > 0)
        {
            EndParagraph();
            CurrentBlock.Add(new MdHeading(3, "Parameters"));
            base.VisitParameters(methodOrProperty, parameters);
        }
    }

    /// <inheritdoc />
    public override void VisitTypeParameters(MemberDescription typeOrMethod, IReadOnlyCollection<TypeParameterDescription> typeParameters)
    {
        if (typeParameters.Count > 0)
        {
            EndParagraph();
            CurrentBlock.Add(new MdHeading(3, "Type Parameters"));
            base.VisitTypeParameters(typeOrMethod, typeParameters);
        }
    }

    /// <inheritdoc />
    public override void VisitExceptions(MemberDescription methodOrPropertyOrEvent, IReadOnlyList<ExceptionDescription> exceptions)
    {
        if (exceptions.Count > 0)
        {
            EndParagraph();
            CurrentBlock.Add(new MdHeading(3, "Exceptions"));
            base.VisitExceptions(methodOrPropertyOrEvent, exceptions);
        }
    }

    /// <inheritdoc />
    public override void VisitSeeAlso(MemberDescription member)
    {
        if (member.SeeAlso.Count > 0)
        {
            EndParagraph();
            CurrentBlock.Add(new MdHeading(3, "See Also"));
            base.VisitSeeAlso(member);
        }
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
    /// Ends the current paragraph
    /// </summary>
    protected virtual void EndParagraph()
    {
        m_CurrentParagraph = null;
    }
}
