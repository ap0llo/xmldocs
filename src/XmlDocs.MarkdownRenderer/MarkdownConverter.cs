namespace Grynwald.XmlDocs.MarkdownRenderer;

/// <summary>
/// Default implementation of <see cref="IMarkdownConverter"/>.
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
/// </para>
/// <para>
/// To customize <see cref="MarkdownConverter"/>, you can override the factory methods thar are used to create the visitors (<see cref="CreateConvertToBlockVisitor"/> and <see cref="CreateConvertToSpanVisitor"/>)
/// that perform the conversion to Markdown and provide a customized implementation.
/// </para>
/// </remarks>
public class MarkdownConverter : IMarkdownConverter
{
    /// <inheritdoc />
    public virtual MdBlock ConvertToBlock(DocumentationElement element)
    {
        if (element is null)
            throw new ArgumentNullException(nameof(element));

        var visitor = CreateConvertToBlockVisitor();
        element.Accept(visitor);

        return visitor.Result;
    }

    /// <inheritdoc />
    public virtual MdSpan ConvertToSpan(TextElement textElement)
    {
        if (textElement is null)
            throw new ArgumentNullException(nameof(textElement));

        var visitor = CreateConvertToSpanVisitor();
        textElement.Accept(visitor);

        return visitor.Result;
    }



    /// <summary>
    /// Creates an instance of <see cref="ConvertToBlockVisitor" /> which is used in <see cref="ConvertToBlock(DocumentationElement)"/>.
    /// </summary>
    protected virtual ConvertToBlockVisitor CreateConvertToBlockVisitor()
    {
        return new ConvertToBlockVisitor(this);
    }

    /// <summary>
    /// Creates an instance of <see cref="ConvertToSpanVisitor" /> which is used in <see cref="ConvertToSpanVisitor"/>.
    /// </summary>
    protected virtual ConvertToSpanVisitor CreateConvertToSpanVisitor()
    {
        return new ConvertToSpanVisitor();
    }
}
