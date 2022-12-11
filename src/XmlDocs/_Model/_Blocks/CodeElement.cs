namespace Grynwald.XmlDocs;

/// <summary>
/// Represents a <c><![CDATA[<code>]]></c> text element in XML documentation comments.
/// </summary>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags">Recommended XML tags for C# documentation comments (Microsoft Learn)</seealso>
//TODO: Should content be allowed to be null? (e.g. for <code />)
public class CodeElement : BlockElement, IEquatable<CodeElement>
{
    /// <summary>
    /// Gets the content of the code element.
    /// </summary>
    public string Content { get; }

    /// <summary>
    /// Gets the language of the code sample.
    /// </summary>
    /// <value>
    /// The language of the code sample if it was specified or <c>null</c> is no language was specified.
    /// </value>
    public string? Language { get; }


    /// <summary>
    /// Initializes a new instance of <see cref="CodeElement"/>
    /// </summary>
    /// <param name="content">The content of the element.</param>
    /// <param name="language">The language of the code sample. Can be <c>null</c></param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="content"/> is <c>null</c></exception>
    public CodeElement(string content, string? language)
    {
        Content = content ?? throw new ArgumentNullException(nameof(content));
        Language = language;
    }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        unchecked
        {
            var hash = StringComparer.Ordinal.GetHashCode(Content) * 397;
            hash ^= Language == null ? 0 : StringComparer.Ordinal.GetHashCode(Language);
            return hash;
        }
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as CodeElement);

    /// <inheritdoc />
    public bool Equals(CodeElement? other)
    {
        if (other == null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return StringComparer.Ordinal.Equals(Content, other.Content) &&
               StringComparer.Ordinal.Equals(Language, other.Language);
    }


    /// <inheritdoc cref="FromXml(XElement)"/>
    public static CodeElement FromXml(string xml) => FromXml(XmlContentHelper.ParseXmlElement(xml));

    /// <summary>
    /// Creates a <see cref="CodeElement" /> from its XML representation.
    /// </summary>
    public static CodeElement FromXml(XElement xml)
    {
        xml.EnsureNameIs("code");

        // get the "language" attribute. If there is no "language" attribute,
        // use the "lang" attribute.
        // "lang" is legacy syntax according to SandCastle documentation
        // http://ewsoftware.github.io/XMLCommentsGuide/html/1abd1992-e3d0-45b4-b43d-91fcfc5e5574.htm
        var languageAttribute = xml.Attribute("language") ?? xml.Attribute("lang");

        var indent = XmlContentHelper.GetIndentation(xml.Value);
        return new CodeElement(XmlContentHelper.TrimCode(xml.Value, indent), languageAttribute?.Value);
    }
}
