namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a <c><![CDATA[<exception />]]></c> element in XML documentation comments.
/// </summary>
/// <remarks>
/// <c>exception</c> provides information about exception that might be thrown by a method.
/// </remarks>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/">XML documentation comments (Microsoft Learn)</seealso>
public class ExceptionDescription : IDocumentationNode
{
    /// <summary>
    /// Gets the reference to the exception's type.
    /// </summary>
    public string Reference { get; }

    /// <summary>
    /// Gets the exception description's text.
    /// </summary>
    public TextBlock? Text { get; }


    /// <summary>
    /// Initializes a new instance of <see cref="ExceptionDescription"/>.
    /// </summary>
    /// <param name="reference">The reference to the exception#s type.</param>
    /// <param name="text">The exception's description.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="reference"/> is <c>null</c> or whitespace.</exception>
    public ExceptionDescription(string reference, TextBlock? text)
    {
        if (String.IsNullOrWhiteSpace(reference))
            throw new ArgumentException("Value must not be null or whitespace", nameof(reference));

        Reference = reference;
        Text = text;
    }


    /// <inheritdoc />
    public void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);


    /// <inheritdoc cref="FromXml(XElement)"/>
    public static ExceptionDescription FromXml(string xml) => FromXml(XmlContentHelper.ParseXmlElement(xml));

    /// <summary>
    /// Creates a <see cref="ParameterDescription" /> from its XML representation.
    /// </summary>
    public static ExceptionDescription FromXml(XElement xml)
    {
        xml.EnsureNameIs("exception");

        var cref = xml
            .RequireAttribute("cref")
            .RequireValue();

        var text = TextBlock.FromXmlOrNullIfEmpty(xml);

        return new ExceptionDescription(cref, text);
    }
}
