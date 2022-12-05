namespace Grynwald.XmlDocReader;

/// <summary>
/// Default implementation of <see cref="IDocumentationVisitor"/>
/// </summary>
/// <remarks>
/// Use this as base class for custom visitors processing the XML documentation file instead of 
/// implementing <see cref="IDocumentationVisitor"/> directly to avoid having to implement the traversal logic yourself.
/// </remarks>
public class DocumentationVisitor : IDocumentationVisitor
{
    /// <inheritdoc />
    public virtual void Visit(DocumentationFile documentationFile)
    {
        foreach (var member in documentationFile.Members)
        {
            member.Accept(this);
        }
    }

    /// <inheritdoc />
    public virtual void Visit(NamespaceDescription member)
    {
        VisitSummary(member);
        VisitRemarks(member);
        VisitValue(member);
        VisitReturns(member);
        VisitExample(member);

        VisitParameters(member);

        VisitTypeParameters(member);

        VisitExceptions(member);

        VisitSeeAlso(member);
    }

    /// <inheritdoc />
    public virtual void Visit(TypeDescription member)
    {
        VisitSummary(member);
        VisitRemarks(member);
        VisitValue(member);
        VisitReturns(member);
        VisitExample(member);

        VisitParameters(member);

        VisitTypeParameters(member);

        VisitExceptions(member);

        VisitSeeAlso(member);
    }

    /// <inheritdoc />
    public virtual void Visit(FieldDescription member)
    {
        VisitSummary(member);
        VisitRemarks(member);
        VisitValue(member);
        VisitReturns(member);
        VisitExample(member);

        VisitParameters(member);

        VisitTypeParameters(member);

        VisitExceptions(member);

        VisitSeeAlso(member);
    }

    /// <inheritdoc />
    public virtual void Visit(PropertyDescription member)
    {
        VisitSummary(member);
        VisitRemarks(member);
        VisitValue(member);
        VisitReturns(member);
        VisitExample(member);

        VisitParameters(member);

        VisitTypeParameters(member);

        VisitExceptions(member);

        VisitSeeAlso(member);
    }

    /// <inheritdoc />
    public virtual void Visit(MethodDescription member)
    {
        VisitSummary(member);
        VisitRemarks(member);
        VisitValue(member);
        VisitReturns(member);
        VisitExample(member);

        VisitParameters(member);

        VisitTypeParameters(member);

        VisitExceptions(member);

        VisitSeeAlso(member);
    }

    /// <inheritdoc />
    public virtual void Visit(EventDescription member)
    {
        VisitSummary(member);
        VisitRemarks(member);
        VisitValue(member);
        VisitReturns(member);
        VisitExample(member);

        VisitParameters(member);

        VisitTypeParameters(member);

        VisitExceptions(member);

        VisitSeeAlso(member);
    }

    /// <inheritdoc />
    public virtual void Visit(ParameterDescription param)
    {
        param.Text?.Accept(this);
    }

    /// <inheritdoc />
    public virtual void Visit(TypeParameterDescription typeParam)
    {
        typeParam.Text?.Accept(this);
    }

    /// <inheritdoc />
    public virtual void Visit(ExceptionDescription exception)
    {
        exception.Text?.Accept(this);
    }

    /// <inheritdoc />
    public virtual void Visit(SeeAlsoUrlReferenceDescription seeAlso)
    {
        seeAlso.Text?.Accept(this);
    }

    /// <inheritdoc />
    public virtual void Visit(SeeAlsoCodeReferenceDescription seeAlso)
    {
        seeAlso.Text?.Accept(this);
    }

    /// <inheritdoc />
    public virtual void Visit(TextBlock textBlock)
    {
        foreach (var element in textBlock.Elements)
        {
            element.Accept(this);
        }
    }

    /// <inheritdoc />
    public virtual void Visit(PlainTextElement plainText)
    {
    }

    /// <inheritdoc />
    public virtual void Visit(ListElement list)
    {
        list.ListHeader?.Accept(this);
        foreach (var item in list.Items)
        {
            item.Accept(this);
        }
    }

    /// <inheritdoc />
    public virtual void Visit(ListItemElement item)
    {
        item.Term?.Accept(this);
        item.Description.Accept(this);
    }

    /// <inheritdoc />
    public virtual void Visit(CElement c)
    {
    }

    /// <inheritdoc />
    public virtual void Visit(CodeElement code)
    {
    }

    /// <inheritdoc />
    public virtual void Visit(ParagraphElement para)
    {
        para.Content?.Accept(this);
    }

    /// <inheritdoc />
    public virtual void Visit(ParameterReferenceElement paramRef)
    {
    }

    /// <inheritdoc />
    public virtual void Visit(TypeParameterReferenceElement typeParamRef)
    {
    }

    /// <inheritdoc />
    public virtual void Visit(SeeCodeReferenceElement see)
    {
        see.Text?.Accept(this);
    }

    /// <inheritdoc />
    public virtual void Visit(SeeUrlReferenceElement see)
    {
        see.Text?.Accept(this);
    }

    /// <summary>
    /// Visits a a MemberDescription's <c>Summary</c>.
    /// </summary>
    /// <remarks>
    /// This method is called by the <c>Visit()</c> methods for types derived from <see cref="MemberDescription" /> instead of visiting the <see cref="MemberDescription.Summary" /> text block directly
    /// to allow derived visitors to perform actions specifically before or after the Summary is processed without having to re-implement the <c>Visit()</c> method.
    /// <para>
    /// Note that this method will ablso be called if <see cref="MemberDescription.Summary"/> is <c>null</c>.
    /// </para>
    /// </remarks>
    /// <seealso cref="MemberDescription.Summary"/>
    public virtual void VisitSummary(MemberDescription member)
    {
        member.Summary?.Accept(this);
    }

    /// <summary>
    /// Visits a a MemberDescription's <c>Remarks</c>.
    /// </summary>
    /// <remarks>
    /// This method is called by the <c>Visit()</c> methods for types derived from <see cref="MemberDescription" /> instead of visiting the <see cref="MemberDescription.Remarks" /> text block directly
    /// to allow derived visitors to perform actions specifically before or after the <c>Remarks</c> is processed without having to re-implement the <c>Visit()</c> method.
    /// <para>
    /// Note that this method will ablso be called if <see cref="MemberDescription.Remarks"/> is <c>null</c>.
    /// </para>
    /// </remarks>
    /// <seealso cref="MemberDescription.Remarks"/>
    public virtual void VisitRemarks(MemberDescription member)
    {
        member.Remarks?.Accept(this);
    }

    /// <summary>
    /// Visits a a MemberDescription's <c>Value</c>.
    /// </summary>
    /// <remarks>
    /// This method is called by the <c>Visit()</c> methods for types derived from <see cref="MemberDescription" /> instead of visiting the <see cref="MemberDescription.Value" /> text block directly
    /// to allow derived visitors to perform actions specifically before or after <c>Value</c> is processed without having to re-implement the <c>Visit()</c> method.
    /// <para>
    /// Note that this method will ablso be called if <see cref="MemberDescription.Value"/> is <c>null</c>.
    /// </para>
    /// </remarks>
    /// <seealso cref="MemberDescription.Value"/>
    public virtual void VisitValue(MemberDescription member)
    {
        member.Value?.Accept(this);
    }

    /// <summary>
    /// Visits a a MemberDescription's <c>Returns</c>.
    /// </summary>
    /// <remarks>
    /// This method is called by the <c>Visit()</c> methods for types derived from <see cref="MemberDescription" /> instead of visiting the <see cref="MemberDescription.Returns" /> text block directly
    /// to allow derived visitors to perform actions specifically before or after <c>Returns</c> is processed without having to re-implement the <c>Visit()</c> method.
    /// <para>
    /// Note that this method will ablso be called if <see cref="MemberDescription.Returns"/> is <c>null</c>.
    /// </para>
    /// </remarks>
    /// <seealso cref="MemberDescription.Returns"/>
    public virtual void VisitReturns(MemberDescription member)
    {
        member.Returns?.Accept(this);
    }

    /// <summary>
    /// Visits a a MemberDescription's <c>Example</c>.
    /// </summary>
    /// <remarks>
    /// This method is called by the <c>Visit()</c> methods for types derived from <see cref="MemberDescription" /> instead of visiting the <see cref="MemberDescription.Example" /> text block directly
    /// to allow derived visitors to perform actions specifically before or after <c>Example</c> is processed without having to re-implement the <c>Visit()</c> method.
    /// <para>
    /// Note that this method will ablso be called if <see cref="MemberDescription.Example"/> is <c>null</c>.
    /// </para>
    /// </remarks>
    /// <seealso cref="MemberDescription.Example"/>
    public virtual void VisitExample(MemberDescription member)
    {
        member.Example?.Accept(this);
    }

    /// <summary>
    /// Visits a a MemberDescription's parameters.
    /// </summary>
    /// <remarks>
    /// This method is called by the <c>Visit()</c> methods for types derived from <see cref="MemberDescription" /> instead of visiting the items in <see cref="MemberDescription.Parameters" /> directly
    /// to allow derived visitors to perform actions specifically before or after <c>Parameters</c> is processed without having to re-implement the <c>Visit()</c> method.
    /// <para>
    /// Note that this method will ablso be called if <see cref="MemberDescription.Parameters"/> is empty.
    /// </para>
    /// </remarks>
    /// <seealso cref="MemberDescription.Parameters"/>
    public virtual void VisitParameters(MemberDescription member)
    {
        foreach (var parameter in member.Parameters)
        {
            parameter.Accept(this);
        }
    }

    /// <summary>
    /// Visits a a MemberDescription's type parameters.
    /// </summary>
    /// <remarks>
    /// This method is called by the <c>Visit()</c> methods for types derived from <see cref="MemberDescription" /> instead of visiting the items in <see cref="MemberDescription.TypeParameters" /> directly
    /// to allow derived visitors to perform actions specifically before or after <c>TypeParameters</c> is processed without having to re-implement the <c>Visit()</c> method.
    /// <para>
    /// Note that this method will ablso be called if <see cref="MemberDescription.TypeParameters"/> is empty.
    /// </para>
    /// </remarks>
    /// <seealso cref="MemberDescription.TypeParameters"/>
    public virtual void VisitTypeParameters(MemberDescription member)
    {
        foreach (var typeParameter in member.TypeParameters)
        {
            typeParameter.Accept(this);
        }
    }

    /// <summary>
    /// Visits a a MemberDescription's exceptions.
    /// </summary>
    /// <remarks>
    /// This method is called by the <c>Visit()</c> methods for types derived from <see cref="MemberDescription" /> instead of visiting the items in <see cref="MemberDescription.Exceptions" /> directly
    /// to allow derived visitors to perform actions specifically before or after <c>Exceptions</c> is processed without having to re-implement the <c>Visit()</c> method.
    /// <para>
    /// Note that this method will ablso be called if <see cref="MemberDescription.Exceptions"/> is empty.
    /// </para>
    /// </remarks>
    /// <seealso cref="MemberDescription.Exceptions"/>
    public virtual void VisitExceptions(MemberDescription member)
    {
        foreach (var exception in member.Exceptions)
        {
            exception.Accept(this);
        }
    }

    /// <summary>
    /// Visits a a MemberDescription's "See Also" references.
    /// </summary>
    /// <remarks>
    /// This method is called by the <c>Visit()</c> methods for types derived from <see cref="MemberDescription" /> instead of visiting the items in <see cref="MemberDescription.SeeAlso" /> directly
    /// to allow derived visitors to perform actions specifically before or after <c>SeeAlso</c> is processed without having to re-implement the <c>Visit()</c> method.
    /// <para>
    /// Note that this method will ablso be called if <see cref="MemberDescription.SeeAlso"/> is empty.
    /// </para>
    /// </remarks>
    /// <seealso cref="MemberDescription.SeeAlso"/>
    public virtual void VisitSeeAlso(MemberDescription member)
    {
        foreach (var seeAlso in member.SeeAlso)
        {
            seeAlso.Accept(this);
        }
    }
}

