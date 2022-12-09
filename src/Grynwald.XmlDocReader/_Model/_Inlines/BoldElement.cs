namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a bold (<c><![CDATA[<b />]]></c>) text element in XML documentation comments.
/// </summary>
/// <remarks>
/// The <c><![CDATA[<c />]]></c> indicates the text should be rendered as bold text.
/// <para>
/// The implementation assumes that the content of the <c><![CDATA[<b />]]></c> tag is plain text only and does not support nested elements.
/// </para>
/// </remarks>
/// <example>
/// <code language="xml"><![CDATA[
/// The last word is <b>bold</b>.
/// ]]>
/// </code>
/// </example>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags">Recommended XML tags for C# documentation comments (Microsoft Learn)</seealso>
public class BoldElement : InlineElement, IEquatable<BoldElement>
{
    public string Content { get; }


    /// <summary>
    /// Initializes a new instance of <see cref="BoldElement"/>.
    /// </summary>
    /// <param name="content">The emphasized text</param>
    public BoldElement(string content)
    {
        Content = content ?? throw new ArgumentNullException(nameof(content));
    }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as BoldElement);

    /// <inheritdoc />
    public bool Equals(BoldElement? other) =>
        other is not null &&
        StringComparer.Ordinal.Equals(Content, other.Content);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Content);


    /// <inheritdoc cref="FromXml(XElement)" />
    public static BoldElement FromXml(string xml) => FromXml(XmlContentHelper.ParseXmlElement(xml));

    /// <summary>
    /// Initializes a new <see cref="BoldElement" /> from it's XML equivalent.
    /// </summary>
    public static BoldElement FromXml(XElement xml)
    {
        xml.EnsureNameIs("b");

        return new BoldElement(xml.Value);
    }
}
