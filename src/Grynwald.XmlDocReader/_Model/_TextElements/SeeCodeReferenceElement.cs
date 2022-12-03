namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a <c><![CDATA[<see />]]></c> element referencing a code element
/// </summary>
/// <seealso cref="SeeElement"/>
public class SeeCodeReferenceElement : SeeElement, IEquatable<SeeCodeReferenceElement>
{
    /// <summary>
    /// Gets the content of the <c><![CDATA[<see />]]></c> element's <c>cref</c> attribute.
    /// </summary>
    public string Reference { get; }


    /// <summary>
    /// Initializes a new instance of <see cref="SeeCodeReferenceElement"/>.
    /// </summary>
    /// <param name="reference">The content of the elements <c>cref</c> attribute.</param>
    /// <param name="text">The element's text (optional).</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="reference"/> is <c>null</c>.</exception>
    public SeeCodeReferenceElement(string reference, TextBlock? text) : base(text)
    {
        if (String.IsNullOrWhiteSpace(reference))
            throw new ArgumentException("Value must not be null or whitespace", nameof(reference));

        Reference = reference;
    }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Reference);

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as SeeCodeReferenceElement);

    /// <inheritdoc />
    public bool Equals(SeeCodeReferenceElement? other)
    {
        if (other is null)
            return false;

        if (!StringComparer.Ordinal.Equals(Reference, other.Reference))
            return false;

        if (Text is null)
            return other.Text is null;

        return Text.Equals(other.Text);
    }


    internal static new SeeCodeReferenceElement FromXml(XElement xml)
    {
        var cref = xml
            .RequireAttribute("cref")
            .RequireValue();

        var text = TextBlock.FromXmlOrNullIfEmpty(xml);

        return new SeeCodeReferenceElement(cref, text);
    }
}
