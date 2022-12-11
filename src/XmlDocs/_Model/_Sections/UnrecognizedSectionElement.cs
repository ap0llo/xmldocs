namespace Grynwald.XmlDocs;

/// <summary>
/// Represents an unrecognized child element of a <see cref="MemberElement"/>.
/// </summary>
public class UnrecognizedSectionElement : DocumentationElement
{
    /// <summary>
    /// Gets the XML element which was not recognized.
    /// </summary>
    public XElement Xml { get; }


    /// <summary>
    /// Initialized a new instance of <see cref="UnrecognizedSectionElement"/>.
    /// </summary>
    /// <param name="xml">The XML element which was not recognized.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="xml"/> is <c>null</c>.</exception>
    public UnrecognizedSectionElement(XElement xml)
    {
        Xml = xml ?? throw new ArgumentNullException(nameof(xml));
    }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);
}
