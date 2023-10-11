namespace Grynwald.XmlDocs;

/// <summary>
/// Represents a <c><![CDATA[<exception />]]></c> element in XML documentation comments.
/// </summary>
/// <remarks>
/// <c>exception</c> provides information about exception that might be thrown by a method.
/// </remarks>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/">XML documentation comments (Microsoft Learn)</seealso>
public class ExceptionElement : SectionElement
{
    /// <summary>
    /// Gets the reference to the exception's type as <see cref="MemberId"/>.
    /// </summary>
    public MemberId Reference { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="ExceptionElement"/>.
    /// </summary>
    /// <param name="reference">The reference to the exception's type.</param>
    /// <param name="text">The exception's description.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="reference"/> is <c>null</c> or whitespace.</exception>
    public ExceptionElement(MemberId reference, TextBlock? text) : base(text)
    {
        Reference = reference ?? throw new ArgumentNullException(nameof(reference));
    }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);


    /// <inheritdoc cref="FromXml(XElement)"/>
    public static ExceptionElement FromXml(string xml) => FromXml(XmlContentHelper.ParseXmlElement(xml));

    /// <summary>
    /// Creates a <see cref="ParameterElement" /> from its XML representation.
    /// </summary>
    public static ExceptionElement FromXml(XElement xml)
    {
        xml.EnsureNameIs("exception");

        var cref = xml
            .RequireAttribute("cref")
            .RequireValue();

        if (!MemberId.TryParse(cref, out var reference) || reference == null)
        {
            throw new XmlDocsException($"Failed to parse code reference in <exception /> element. Invalid reference '{cref}'{xml.GetPositionString()}");
        }

        var text = TextBlock.FromXmlOrNullIfEmpty(xml);

        return new ExceptionElement(reference, text);
    }
}
