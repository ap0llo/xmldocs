namespace Grynwald.XmlDocs;

/// <summary>
/// Represents a idiomatic (<c><![CDATA[<i />]]></c>) text element in XML documentation comments.
/// </summary>
/// <remarks>
/// The <c><![CDATA[<i />]]></c> marks a range of text that is set off from normal text for some reason. It is typically rendered as italic text.
/// <para>
/// The implementation assumes that the content of the <c><![CDATA[<i />]]></c> tag is plain text only and does not support nested elements.
/// </para>
/// </remarks>
/// <example>
/// <code language="xml"><![CDATA[
/// The last word is set <i>off</i>.
/// ]]>
/// </code>
/// </example>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags">Recommended XML tags for C# documentation comments (Microsoft Learn)</seealso>
/// <seealso cref="EmphasisElement" />
public class IdiomaticElement : InlineElement, IEquatable<IdiomaticElement>
{
    public string Content { get; }


    /// <summary>
    /// Initializes a new instance of <see cref="IdiomaticElement"/>.
    /// </summary>
    /// <param name="content">The emphasized text</param>
    public IdiomaticElement(string content)
    {
        Content = content ?? throw new ArgumentNullException(nameof(content));
    }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as IdiomaticElement);

    /// <inheritdoc />
    public bool Equals(IdiomaticElement? other) =>
        other is not null &&
        StringComparer.Ordinal.Equals(Content, other.Content);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Content);


    /// <inheritdoc cref="FromXml(XElement)" />
    public static IdiomaticElement FromXml(string xml) => FromXml(XmlContentHelper.ParseXmlElement(xml));

    /// <summary>
    /// Initializes a new <see cref="IdiomaticElement" /> from it's XML equivalent.
    /// </summary>
    public static IdiomaticElement FromXml(XElement xml)
    {
        xml.EnsureNameIs("i");

        return new IdiomaticElement(xml.Value);
    }
}
