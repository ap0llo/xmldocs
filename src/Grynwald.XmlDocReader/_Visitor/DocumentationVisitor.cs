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
        @namespace.Summary?.Accept(this);
        @namespace.Remarks?.Accept(this);
        @namespace.Example?.Accept(this);
        VisitSeeAlso(@namespace);
        VisitUnrecgonizedElements(@namespace);
    }

    /// <inheritdoc />
    public virtual void Visit(TypeMemberElement type)
    {
        type.Summary?.Accept(this);
        type.Remarks?.Accept(this);
        type.Example?.Accept(this);
        VisitTypeParameters(type, type.TypeParameters);
        VisitSeeAlso(type);
        VisitUnrecgonizedElements(type);
    }

    /// <inheritdoc />
    public virtual void Visit(FieldMemberElement field)
    {
        field.Summary?.Accept(this);
        field.Remarks?.Accept(this);
        field.Value?.Accept(this);
        field.Example?.Accept(this);
        VisitSeeAlso(field);
        VisitUnrecgonizedElements(field);
    }

    /// <inheritdoc />
    public virtual void Visit(PropertyMemberElement property)
    {
        property.Summary?.Accept(this);
        property.Remarks?.Accept(this);
        property.Value?.Accept(this);
        property.Example?.Accept(this);
        VisitParameters(property, property.Parameters);
        VisitExceptions(property, property.Exceptions);
        VisitSeeAlso(property);
        VisitUnrecgonizedElements(property);
    }

    /// <inheritdoc />
    public virtual void Visit(MethodMemberElement method)
    {
        method.Summary?.Accept(this);
        method.Remarks?.Accept(this);
        method.Returns?.Accept(this);
        method.Example?.Accept(this);
        VisitParameters(method, method.Parameters);
        VisitTypeParameters(method, method.TypeParameters);
        VisitExceptions(method, method.Exceptions);
        VisitSeeAlso(method);
        VisitUnrecgonizedElements(method);
    }

    /// <inheritdoc />
    public virtual void Visit(EventMemberElement @event)
    {
        @event.Summary?.Accept(this);
        @event.Remarks?.Accept(this);
        @event.Example?.Accept(this);
        VisitExceptions(@event, @event.Exceptions);
        VisitSeeAlso(@event);
        VisitUnrecgonizedElements(@event);
    }

    /// <inheritdoc />
    public virtual void Visit(SummaryElement summary)
    {
        summary.Text?.Accept(this);
    }

    /// <inheritdoc />
    public virtual void Visit(RemarksElement remarks)
    {
        remarks.Text?.Accept(this);
    }

    /// <inheritdoc />
    public virtual void Visit(ExampleElement example)
    {
        example.Text?.Accept(this);
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
    public virtual void Visit(UnrecognizedSectionElement unrecognizedElement)
    { }

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
    { }

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

    /// <inheritdoc />
    public virtual void Visit(ValueElement value)
    {
        value.Text?.Accept(this);
    }

    /// <inheritdoc />
    public virtual void Visit(ReturnsElement returns)
    {
        returns.Text?.Accept(this);
    }

    /// <inheritdoc />
    public virtual void Visit(EmphasisElement emphasis)
    { }

    /// <inheritdoc />
    public virtual void Visit(IdiomaticElement idiomatic)
    { }

    /// <inheritdoc />
    public virtual void Visit(BoldElement bold)
    { }

    /// <inheritdoc />
    public virtual void Visit(StrongElement strong)
    { }


    /// <inheritdoc />
    public virtual void Visit(UnrecognizedTextElement unrecognizedElement)
    { }


    /// <summary>
    /// Visits a PropertyDescription's or MethodDescription's parameters.
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
    /// Visits a TypeDescription's of MethodDescription's type parameters.
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
    /// Visits a MemberDescription's exceptions.
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
    /// Visits a MemberDescription's "See Also" references.
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

    /// <summary>
    /// Visits a MemberDescription's unrecognized elements
    /// </summary>
    /// <remarks>
    /// This method is called by the <c>Visit()</c> methods for types derived from <see cref="MemberElement" /> instead of visiting the items in <see cref="MemberElement.UnrecognizedElements" /> directly
    /// to allow derived visitors to perform actions specifically before or after these items are processed without having to re-implement the <c>Visit()</c> method.
    /// <para>
    /// Note that this method will also be called if <see cref="MemberElement.UnrecognizedElements"/> is empty.
    /// </para>
    /// </remarks>
    /// <seealso cref="MemberElement.UnrecognizedElements"/>
    public virtual void VisitUnrecgonizedElements(MemberElement member)
    {
        foreach (var seeAlso in member.UnrecognizedElements)
        {
            seeAlso.Accept(this);
        }
    }
}

