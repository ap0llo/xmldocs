namespace Grynwald.XmlDocReader.MarkdownRenderer;

public interface IMarkdownConverter
{
    /// <summary>
    /// Converts the specified documentation node to a Markdown block.
    /// </summary>
    MdBlock ConvertToBlock(IDocumentationNode documentationNode);

    /// <summary>
    /// Converts the specified documentation node to a Markdown span (inline text element).
    /// </summary>
    /// <remarks>
    /// Note that not all documentation nodes can be converted to span.
    /// </remarks>
    /// <param name="documentationNode"></param>
    // TODO: Introduce separate interface for inline elements?
    MdSpan ConvertToSpan(IDocumentationNode documentationNode);
}
