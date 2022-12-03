namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a <c><![CDATA[<c>]]></c> text element in XML documentation comments.
/// </summary>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags">Recommended XML tags for C# documentation comments (Microsoft Learn)</seealso>
public class CElement : TextElement, IEquatable<CElement>
{
    /// <summary>
    /// Gets the content of the element.
    /// </summary>
    public string Content { get; }


    /// <summary>
    /// Initializes a new instance of <see cref="CElement"/>.
    /// </summary>
    /// <param name="content">The content of the element.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="content"/> is <c>null</c></exception>
    public CElement(string content)
    {
        Content = content ?? throw new ArgumentNullException(nameof(content));
    }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Content);

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as CElement);

    /// <inheritdoc />
    public bool Equals(CElement? other) => other is not null && StringComparer.Ordinal.Equals(Content, other.Content);

    /// <inheritdoc cref="FromXml(XElement)"/>
    public static CElement FromXml(string xml) => FromXml(XmlContentHelper.ParseXmlElement(xml));

    /// <summary>
    /// Creates a <see cref="CElement" /> from its XML representation.
    /// </summary>
    public static CElement FromXml(XElement xml)
    {
        xml.EnsureNameIs("c");
        return new CElement(xml.Value);
    }
}
