namespace Grynwald.XmlDocs;

/// <summary>
/// Represents an unrecognized xml element within a text block.
/// </summary>
public class UnrecognizedTextElement : TextElement, IEquatable<UnrecognizedTextElement>
{
    /// <summary>
    /// Gets the XML element which was not recognized.
    /// </summary>
    public XElement Xml { get; }


    /// <summary>
    /// Initialized a new instance of <see cref="UnrecognizedTextElement"/>.
    /// </summary>
    /// <param name="xml">The XML element which was not recognized.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="xml"/> is <c>null</c>.</exception>
    public UnrecognizedTextElement(XElement xml)
    {
        Xml = xml ?? throw new ArgumentNullException(nameof(xml));
    }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as UnrecognizedTextElement);

    /// <inheritdoc />
    public bool Equals(UnrecognizedTextElement? other) =>
        other is not null &&
        XNode.DeepEquals(Xml, other.Xml);

    /// <inheritdoc />
    public override int GetHashCode() => Xml.Name.GetHashCode();
}
