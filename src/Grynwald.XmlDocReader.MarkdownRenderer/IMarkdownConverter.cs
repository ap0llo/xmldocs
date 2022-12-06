namespace Grynwald.XmlDocReader.MarkdownRenderer;

/// <summary>
/// Converts documentation to Markdown.
/// </summary>
public interface IMarkdownConverter
{
    /// <summary>
    /// Converts the specified documentation element to a Markdown block.
    /// </summary>
    MdBlock ConvertToBlock(DocumentationElement element);

    /// <summary>
    /// Converts the specified documentation node to a Markdown "span" (a inline text element).
    /// </summary>
    /// <remarks>
    /// Note that not all types of text elements can be converted to a span.
    /// </remarks>
    /// <param name="textElement">The <see cref="TextElement"/> to convert to a Markdown inline element.</param>
    MdSpan ConvertToSpan(TextElement textElement);
}
