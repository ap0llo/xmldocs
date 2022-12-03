namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a paragraph (<c><![CDATA[<para />]]></c>) text element in XML documentation comments.
/// </summary>
/// <remarks>
/// The <c>para</c> tag an be used to structure a text block into paragraphs.
/// </remarks>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags">Recommended XML tags for C# documentation comments (Microsoft Learn)</seealso>
public class ParagraphElement : TextElement, IEquatable<ParagraphElement>
{
    /// <summary>
    /// Gets the paragraphs's content or <c>null</c> if the paragraph does not have any content.
    /// </summary>
    public TextBlock? Content { get; }


    /// <summary>
    /// Initializes a new instance of <see cref="ParagraphElement"/>.
    /// </summary>
    /// <param name="content">The paragraph's content.</param>
    public ParagraphElement(TextBlock? content)
    {
        Content = content;
    }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);

    /// <inheritdoc />
    public override int GetHashCode() => Content?.GetHashCode() ?? 0;

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as ParagraphElement);

    /// <inheritdoc />
    public bool Equals(ParagraphElement? other)
    {
        if (other is null)
            return false;

        if (Content is null)
            return other.Content is null;

        return Content.Equals(other.Content);
    }


    /// <inheritdoc cref="FromXml(XElement)"/>
    public static ParagraphElement FromXml(string xml) => FromXml(XmlContentHelper.ParseXmlElement(xml));

    /// <summary>
    /// Initializes a new <see cref="ParagraphElement" /> from it's XML equivalent.
    /// </summary>
    public static ParagraphElement FromXml(XElement xml)
    {
        xml.EnsureNameIs("para");

        var text = TextBlock.FromXmlOrNullIfEmpty(xml);
        return new ParagraphElement(text);
    }
}
