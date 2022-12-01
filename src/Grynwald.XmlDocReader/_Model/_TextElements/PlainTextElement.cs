namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a plain text element in XML documentation comments.
/// </summary>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags">Recommended XML tags for C# documentation comments (Microsoft Learn)</seealso>
public class PlainTextElement : TextElement, IEquatable<PlainTextElement>
{
    /// <summary>
    /// Gets the text element's content
    /// </summary>
    public string Content { get; }


    /// <summary>
    /// Initializes a new instance of <see cref="PlainTextElement"/>.
    /// </summary>
    /// <param name="content">The text element's content.</param>
    public PlainTextElement(string content)
    {
        Content = content ?? throw new ArgumentNullException(nameof(content));
    }


    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Content);

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as PlainTextElement);

    /// <inheritdoc />
    public bool Equals(PlainTextElement? other) =>
        other is not null &&
        StringComparer.Ordinal.Equals(Content, other.Content);
}
