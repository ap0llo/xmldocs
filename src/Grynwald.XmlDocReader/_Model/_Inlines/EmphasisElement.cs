namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a emphasis (<c><![CDATA[<em />]]></c>) text element in XML documentation comments.
/// </summary>
/// <remarks>
/// The <c><![CDATA[<em />]]></c> element emphasizes text. It is typicall rendered as italic text.
/// <para>
/// The implementation assumes that the content of the <c><![CDATA[<em />]]></c> tag is plain text only and does not support nested elements.
/// </para>
/// </remarks>
/// <example>
/// <code language="xml"><![CDATA[
/// <em>This text is emphazised.</em>
/// ]]>
/// </code>
/// </example>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags">Recommended XML tags for C# documentation comments (Microsoft Learn)</seealso>
public class EmphasisElement : InlineElement, IEquatable<EmphasisElement>
{
    public string Content { get; }


    /// <summary>
    /// Initializes a new instance of <see cref="EmphasisElement"/>.
    /// </summary>
    /// <param name="content">The emphasized text</param>
    public EmphasisElement(string content)
    {
        Content = content ?? throw new ArgumentNullException(nameof(content));
    }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as EmphasisElement);

    /// <inheritdoc />
    public bool Equals(EmphasisElement? other) =>
        other is not null &&
        StringComparer.Ordinal.Equals(Content, other.Content);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Content);


    /// <inheritdoc cref="FromXml(XElement)" />
    public static EmphasisElement FromXml(string xml) => FromXml(XmlContentHelper.ParseXmlElement(xml));

    /// <summary>
    /// Initializes a new <see cref="EmphasisElement" /> from it's XML equivalent.
    /// </summary>
    public static EmphasisElement FromXml(XElement xml)
    {
        xml.EnsureNameIs("em");

        return new EmphasisElement(xml.Value);
    }
}
