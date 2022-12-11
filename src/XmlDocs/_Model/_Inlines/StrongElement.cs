namespace Grynwald.XmlDocs;

/// <summary>
/// Represents a bold (<c><![CDATA[<strong />]]></c>) text element in XML documentation comments.
/// </summary>
/// <remarks>
/// The <c><![CDATA[<strong />]]></c> element indicates text that is strongly emphasized. It is typically rendered as bold type.
/// <para>
/// The implementation assumes that the content of the <c><![CDATA[<strong />]]></c> tag is plain text only and does not support nested elements.
/// </para>
/// </remarks>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags">Recommended XML tags for C# documentation comments (Microsoft Learn)</seealso>
/// <seealso cref="BoldElement"/>
public class StrongElement : InlineElement, IEquatable<StrongElement>
{
    public string Content { get; }


    /// <summary>
    /// Initializes a new instance of <see cref="BoldElement"/>.
    /// </summary>
    /// <param name="content">The emphasized text</param>
    public StrongElement(string content)
    {
        Content = content ?? throw new ArgumentNullException(nameof(content));
    }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as StrongElement);

    /// <inheritdoc />
    public bool Equals(StrongElement? other) =>
        other is not null &&
        StringComparer.Ordinal.Equals(Content, other.Content);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Content);


    /// <inheritdoc cref="FromXml(XElement)" />
    public static StrongElement FromXml(string xml) => FromXml(XmlContentHelper.ParseXmlElement(xml));

    /// <summary>
    /// Initializes a new <see cref="BoldElement" /> from it's XML equivalent.
    /// </summary>
    public static StrongElement FromXml(XElement xml)
    {
        xml.EnsureNameIs("strong");

        return new StrongElement(xml.Value);
    }
}
