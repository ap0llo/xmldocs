namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a blcok of formatted text in XML documentation comments.
/// </summary>
public class TextBlock : IEquatable<TextBlock>, IDocumentationNode
{
    /// <summary>
    /// Gets the text block's text elements.
    /// </summary>
    public IReadOnlyList<TextElement> Elements { get; init; } = Array.Empty<TextElement>();


    /// <summary>
    /// Initializes a new instance of <see cref="TextBlock"/>.
    /// </summary>
    /// <param name="elements">The <see cref="TextElement"/> object the text block consists of.</param>
    public TextBlock(params TextElement[] elements)
    {
        Elements = elements;
    }


    /// <inheritdoc />
    public void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        if (Elements.Count == 0)
            return 0;

        unchecked
        {
            var hash = Elements[0].GetHashCode() * 397;
            for (int i = 1; i < Elements.Count; i++)
            {
                hash ^= Elements[i].GetHashCode();
            }
            return hash;
        }
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as TextBlock);

    /// <inheritdoc />
    public bool Equals(TextBlock? other) =>
        other is not null &&
        Elements.SequenceEqual(other.Elements);


    /// <inheritdoc  cref="FromXml(XElement)" />
    public static TextBlock FromXml(string xml) => FromXml(XmlContentHelper.ParseXmlElement(xml));

    /// <summary>
    /// Creates a <see cref="TextBlock" /> from its XML representation.
    /// </summary>
    public static TextBlock FromXml(XElement xml)
    {
        return new TextBlock()
        {
            Elements = ReadElements(xml).ToList()
        };
    }


    internal static TextBlock? FromXmlOrNullIfEmpty(XElement xml) => xml.Nodes().Any() ? FromXml(xml) : null;


    private static IEnumerable<TextElement> ReadElements(XElement xml)
    {
        var indent = 0;
        if (xml.Nodes().OfType<XText>().FirstOrDefault() is XText textElement)
        {
            indent = XmlContentHelper.GetIndentation(textElement.Value);
        }

        foreach (var node in xml.Nodes())
        {
            if (node is XText textNode)
            {
                var text = XmlContentHelper.TrimText(textNode.Value, indent);
                if (!String.IsNullOrEmpty(text))
                    yield return new PlainTextElement(text);
            }
            else if (node is XElement element)
            {
                if (element.Name.LocalName switch
                {
                    "para" => ParagraphElement.FromXml(element),
                    "paramref" => ParameterReferenceElement.FromXml(element),
                    "typeparamref" => TypeParameterReferenceElement.FromXml(element),
                    "code" => CodeElement.FromXml(element),
                    "c" => CElement.FromXml(element),
                    "see" => SeeElement.FromXml(element),
                    "list" => ListElement.FromXml(element),

                    //TODO: <em> <i> <b>
                    //TODO: Handle unknown elements

                    _ => default(TextElement)

                } is TextElement parsed)
                    yield return parsed;
            }
        }
    }
}
