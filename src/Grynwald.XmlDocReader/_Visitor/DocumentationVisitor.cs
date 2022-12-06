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
    public virtual void Visit(NamespaceMemberElement @namespace)
    {
        VisitSummary(@namespace);
        VisitRemarks(@namespace);
        VisitExample(@namespace);
        VisitSeeAlso(@namespace);
    }

    /// <inheritdoc />
    public virtual void Visit(TypeMemberElement type)
    {
        VisitSummary(type);
        VisitRemarks(type);
        VisitExample(type);
        VisitTypeParameters(type, type.TypeParameters);
        VisitSeeAlso(type);
    }

    /// <inheritdoc />
    public virtual void Visit(FieldMemberElement field)
    {
        VisitSummary(field);
        VisitRemarks(field);
        VisitValue(field, field.Value);
        VisitExample(field);
        VisitSeeAlso(field);
    }

    /// <inheritdoc />
    public virtual void Visit(PropertyMemberElement property)
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
    public virtual void Visit(MethodMemberElement method)
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
    public virtual void Visit(EventMemberElement @event)
    {
        VisitSummary(@event);
        VisitRemarks(@event);
        VisitExample(@event);
        VisitExceptions(@event, @event.Exceptions);
        VisitSeeAlso(@event);
    }

    /// <inheritdoc />
    public virtual void Visit(ParameterElement param)
    {
        param.Text?.Accept(this);
    }

    /// <inheritdoc />
    public virtual void Visit(TypeParameterElement typeParam)
    {
        typeParam.Text?.Accept(this);
    }

    /// <inheritdoc />
    public virtual void Visit(ExceptionElement exception)
    {
        exception.Text?.Accept(this);
    }

    /// <inheritdoc />
    public virtual void Visit(SeeAlsoUrlReferenceElement seeAlso)
    {
        seeAlso.Text?.Accept(this);
    }

    /// <inheritdoc />
    public virtual void Visit(SeeAlsoCodeReferenceElement seeAlso)
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
    /// This method is called by the <c>Visit()</c> methods for types derived from <see cref="MemberElement" /> instead of visiting the <see cref="MemberElement.Summary" /> text block directly
    /// to allow derived visitors to perform actions specifically before or after the Summary is processed without having to re-implement the <c>Visit()</c> method.
    /// <para>
    /// Note that this method will also be called if <see cref="MemberElement.Summary"/> is <c>null</c>.
    /// </para>
    /// </remarks>
    /// <seealso cref="MemberElement.Summary"/>
    public virtual void VisitSummary(MemberElement member)
    {
        member.Summary?.Accept(this);
    }

    /// <summary>
    /// Visits a a MemberDescription's <c>Remarks</c>.
    /// </summary>
    /// <remarks>
    /// This method is called by the <c>Visit()</c> methods for types derived from <see cref="MemberElement" /> instead of visiting the <see cref="MemberElement.Remarks" /> text block directly
    /// to allow derived visitors to perform actions specifically before or after the <c>Remarks</c> is processed without having to re-implement the <c>Visit()</c> method.
    /// <para>
    /// Note that this method will also be called if <see cref="MemberElement.Remarks"/> is <c>null</c>.
    /// </para>
    /// </remarks>
    /// <seealso cref="MemberElement.Remarks"/>
    public virtual void VisitRemarks(MemberElement member)
    {
        member.Remarks?.Accept(this);
    }

    /// <summary>
    /// Visits a a FieldDescription's of PropertyDescription's <c>Value</c>.
    /// </summary>
    /// <remarks>
    /// This method is called by <see cref="Visit(FieldMemberElement)"/> and <see cref="Visit(PropertyMemberElement)"/> instead of visiting the <c>Value</c> text block directly
    /// to allow derived visitors to perform actions specifically before or after <c>Value</c> is processed without having to re-implement the <c>Visit()</c> method.
    /// <para>
    /// Note that this method will also be called if <see cref="FieldMemberElement.Value"/>/<see cref="PropertyMemberElement.Value"/> is <c>null</c>.
    /// </para>
    /// </remarks>
    /// <seealso cref="FieldMemberElement.Value"/>
    /// <seealso cref="PropertyMemberElement.Value"/>
    public virtual void VisitValue(MemberElement fieldOrProperty, TextBlock? value)
    {
        value?.Accept(this);
    }

    /// <summary>
    /// Visits a a MethodDescription's <c>Returns</c>.
    /// </summary>
    /// <remarks>
    /// This method is called by <see cref="Visit(MethodMemberElement)"/> instead of visiting the <see cref="MethodMemberElement.Returns" /> text block directly
    /// to allow derived visitors to perform actions specifically before or after <c>Returns</c> is processed without having to re-implement the <c>Visit()</c> method.
    /// <para>
    /// Note that this method will also be called if <see cref="MethodMemberElement.Returns"/> is <c>null</c>.
    /// </para>
    /// </remarks>
    /// <seealso cref="MethodMemberElement.Returns"/>
    public virtual void VisitReturns(MethodMemberElement member)
    {
        member.Returns?.Accept(this);
    }

    /// <summary>
    /// Visits a a MemberDescription's <c>Example</c>.
    /// </summary>
    /// <remarks>
    /// This method is called by the <c>Visit()</c> methods for types derived from <see cref="MemberElement" /> instead of visiting the <see cref="MemberElement.Example" /> text block directly
    /// to allow derived visitors to perform actions specifically before or after <c>Example</c> is processed without having to re-implement the <c>Visit()</c> method.
    /// <para>
    /// Note that this method will also be called if <see cref="MemberElement.Example"/> is <c>null</c>.
    /// </para>
    /// </remarks>
    /// <seealso cref="MemberElement.Example"/>
    public virtual void VisitExample(MemberElement member)
    {
        member.Example?.Accept(this);
    }

    /// <summary>
    /// Visits a a PropertyDescription's or MethodDescription's parameters.
    /// </summary>
    /// <remarks>
    /// This method is called by <see cref="Visit(MethodMemberElement)"/> and <see cref="Visit(PropertyMemberElement)"/> instead of visiting the parameters directly
    /// to allow derived visitors to perform actions specifically before or after <c>Parameters</c> are processed without having to re-implement the <c>Visit()</c> method.
    /// <para>
    /// Note that this method will also be called if <see cref="MethodMemberElement.Parameters"/> or <see cref="PropertyMemberElement.Parameters"/> is empty.
    /// </para>
    /// </remarks>
    /// <seealso cref="MethodMemberElement.Parameters"/>
    /// <seealso cref="PropertyMemberElement.Parameters"/>
    public virtual void VisitParameters(MemberElement methodOrProperty, IReadOnlyList<ParameterElement> parameters)
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
    /// This method is called by <see cref="Visit(TypeMemberElement)"/> and <see cref="Visit(MethodMemberElement)"/> instead of visiting the items in <c>TypeParameters</c> directly
    /// to allow derived visitors to perform actions specifically before or after <c>TypeParameters</c> is processed without having to re-implement the <c>Visit()</c> method.
    /// <para>
    /// Note that this method will also be called if <see cref="TypeMemberElement.TypeParameters"/> of <see cref="MethodMemberElement.TypeParameters"/> is empty.
    /// </para>
    /// </remarks>
    /// <seealso cref="TypeMemberElement.TypeParameters"/>
    /// <seealso cref="MethodMemberElement.TypeParameters"/>
    public virtual void VisitTypeParameters(MemberElement typeOrMethod, IReadOnlyCollection<TypeParameterElement> typeParameters)
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
    /// This method is called by <see cref="Visit(MethodMemberElement)"/>, <see cref="Visit(PropertyMemberElement)"/> and <see cref="Visit(EventMemberElement)"/> instead of visiting the items in <c>Exceptions</c> directly
    /// to allow derived visitors to perform actions specifically before or after <c>Exceptions</c> is processed without having to re-implement the <c>Visit()</c> method.
    /// <para>
    /// Note that this method will also be called if <see cref="MethodMemberElement.Exceptions"/>, <see cref="PropertyMemberElement.Exceptions" /> or <see cref="EventMemberElement.Exceptions"/> is empty.
    /// </para>
    /// </remarks>
    /// <seealso cref="MethodMemberElement.Exceptions"/>
    /// <seealso cref="PropertyMemberElement.Exceptions"/>
    /// <seealso cref="EventMemberElement.Exceptions"/>
    public virtual void VisitExceptions(MemberElement methodOrPropertyOrEvent, IReadOnlyList<ExceptionElement> exceptions)
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
    /// This method is called by the <c>Visit()</c> methods for types derived from <see cref="MemberElement" /> instead of visiting the items in <see cref="MemberElement.SeeAlso" /> directly
    /// to allow derived visitors to perform actions specifically before or after <c>SeeAlso</c> is processed without having to re-implement the <c>Visit()</c> method.
    /// <para>
    /// Note that this method will also be called if <see cref="MemberElement.SeeAlso"/> is empty.
    /// </para>
    /// </remarks>
    /// <seealso cref="MemberElement.SeeAlso"/>
    public virtual void VisitSeeAlso(MemberElement member)
    {
        foreach (var seeAlso in member.SeeAlso)
        {
            seeAlso.Accept(this);
        }
    }
}

