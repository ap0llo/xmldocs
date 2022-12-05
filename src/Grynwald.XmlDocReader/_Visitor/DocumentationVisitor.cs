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
    public virtual void Visit(NamespaceDescription @namespace)
    {
        VisitSummary(@namespace);
        VisitRemarks(@namespace);
        VisitExample(@namespace);
        VisitSeeAlso(@namespace);
    }

    /// <inheritdoc />
    public virtual void Visit(TypeDescription type)
    {
        VisitSummary(type);
        VisitRemarks(type);
        VisitExample(type);
        VisitTypeParameters(type, type.TypeParameters);
        VisitSeeAlso(type);
    }

    /// <inheritdoc />
    public virtual void Visit(FieldDescription field)
    {
        VisitSummary(field);
        VisitRemarks(field);
        VisitValue(field, field.Value);
        VisitExample(field);
        VisitSeeAlso(field);
    }

    /// <inheritdoc />
    public virtual void Visit(PropertyDescription property)
    {
        VisitSummary(property);
        VisitRemarks(property);
        VisitValue(property, property.Value);
        VisitExample(property);
        VisitParameters(property, property.Parameters);
        VisitExceptions(property, property.Exceptions);
        VisitSeeAlso(property);
    }

    /// <inheritdoc />
    public virtual void Visit(MethodDescription method)
    {
        VisitSummary(method);
        VisitRemarks(method);
        VisitReturns(method);
        VisitExample(method);
        VisitParameters(method, method.Parameters);
        VisitTypeParameters(method, method.TypeParameters);
        VisitExceptions(method, method.Exceptions);
        VisitSeeAlso(method);
    }

    /// <inheritdoc />
    public virtual void Visit(EventDescription @event)
    {
        VisitSummary(@event);
        VisitRemarks(@event);
        VisitExample(@event);
        VisitExceptions(@event, @event.Exceptions);
        VisitSeeAlso(@event);
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
    /// Note that this method will also be called if <see cref="MemberDescription.Summary"/> is <c>null</c>.
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
    /// Note that this method will also be called if <see cref="MemberDescription.Remarks"/> is <c>null</c>.
    /// </para>
    /// </remarks>
    /// <seealso cref="MemberDescription.Remarks"/>
    public virtual void VisitRemarks(MemberDescription member)
    {
        member.Remarks?.Accept(this);
    }

    /// <summary>
    /// Visits a a FieldDescription's of PropertyDescription's <c>Value</c>.
    /// </summary>
    /// <remarks>
    /// This method is called by <see cref="Visit(FieldDescription)"/> and <see cref="Visit(PropertyDescription)"/> instead of visiting the <c>Value</c> text block directly
    /// to allow derived visitors to perform actions specifically before or after <c>Value</c> is processed without having to re-implement the <c>Visit()</c> method.
    /// <para>
    /// Note that this method will also be called if <see cref="FieldDescription.Value"/>/<see cref="PropertyDescription.Value"/> is <c>null</c>.
    /// </para>
    /// </remarks>
    /// <seealso cref="FieldDescription.Value"/>
    /// <seealso cref="PropertyDescription.Value"/>
    public virtual void VisitValue(MemberDescription fieldOrProperty, TextBlock? value)
    {
        value?.Accept(this);
    }

    /// <summary>
    /// Visits a a MethodDescription's <c>Returns</c>.
    /// </summary>
    /// <remarks>
    /// This method is called by <see cref="Visit(MethodDescription)"/> instead of visiting the <see cref="MethodDescription.Returns" /> text block directly
    /// to allow derived visitors to perform actions specifically before or after <c>Returns</c> is processed without having to re-implement the <c>Visit()</c> method.
    /// <para>
    /// Note that this method will also be called if <see cref="MethodDescription.Returns"/> is <c>null</c>.
    /// </para>
    /// </remarks>
    /// <seealso cref="MethodDescription.Returns"/>
    public virtual void VisitReturns(MethodDescription member)
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
    /// Note that this method will also be called if <see cref="MemberDescription.Example"/> is <c>null</c>.
    /// </para>
    /// </remarks>
    /// <seealso cref="MemberDescription.Example"/>
    public virtual void VisitExample(MemberDescription member)
    {
        member.Example?.Accept(this);
    }

    /// <summary>
    /// Visits a a PropertyDescription's or MethodDescription's parameters.
    /// </summary>
    /// <remarks>
    /// This method is called by <see cref="Visit(MethodDescription)"/> and <see cref="Visit(PropertyDescription)"/> instead of visiting the parameters directly
    /// to allow derived visitors to perform actions specifically before or after <c>Parameters</c> are processed without having to re-implement the <c>Visit()</c> method.
    /// <para>
    /// Note that this method will also be called if <see cref="MethodDescription.Parameters"/> or <see cref="PropertyDescription.Parameters"/> is empty.
    /// </para>
    /// </remarks>
    /// <seealso cref="MethodDescription.Parameters"/>
    /// <seealso cref="PropertyDescription.Parameters"/>
    public virtual void VisitParameters(MemberDescription methodOrProperty, IReadOnlyList<ParameterDescription> parameters)
    {
        foreach (var parameter in parameters)
        {
            parameter.Accept(this);
        }
    }

    /// <summary>
    /// Visits a a TypeDescription's of MethodDescription's type parameters.
    /// </summary>
    /// <remarks>
    /// This method is called by <see cref="Visit(TypeDescription)"/> and <see cref="Visit(MethodDescription)"/> instead of visiting the items in <c>TypeParameters</c> directly
    /// to allow derived visitors to perform actions specifically before or after <c>TypeParameters</c> is processed without having to re-implement the <c>Visit()</c> method.
    /// <para>
    /// Note that this method will also be called if <see cref="TypeDescription.TypeParameters"/> of <see cref="MethodDescription.TypeParameters"/> is empty.
    /// </para>
    /// </remarks>
    /// <seealso cref="TypeDescription.TypeParameters"/>
    /// <seealso cref="MethodDescription.TypeParameters"/>
    public virtual void VisitTypeParameters(MemberDescription typeOrMethod, IReadOnlyCollection<TypeParameterDescription> typeParameters)
    {
        foreach (var typeParameter in typeParameters)
        {
            typeParameter.Accept(this);
        }
    }

    /// <summary>
    /// Visits a a MemberDescription's exceptions.
    /// </summary>
    /// <remarks>
    /// This method is called by <see cref="Visit(MethodDescription)"/>, <see cref="Visit(PropertyDescription)"/> and <see cref="Visit(EventDescription)"/> instead of visiting the items in <c>Exceptions</c> directly
    /// to allow derived visitors to perform actions specifically before or after <c>Exceptions</c> is processed without having to re-implement the <c>Visit()</c> method.
    /// <para>
    /// Note that this method will also be called if <see cref="MethodDescription.Exceptions"/>, <see cref="PropertyDescription.Exceptions" /> or <see cref="EventDescription.Exceptions"/> is empty.
    /// </para>
    /// </remarks>
    /// <seealso cref="MethodDescription.Exceptions"/>
    /// <seealso cref="PropertyDescription.Exceptions"/>
    /// <seealso cref="EventDescription.Exceptions"/>
    public virtual void VisitExceptions(MemberDescription methodOrPropertyOrEvent, IReadOnlyList<ExceptionDescription> exceptions)
    {
        foreach (var exception in exceptions)
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
    /// Note that this method will also be called if <see cref="MemberDescription.SeeAlso"/> is empty.
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

