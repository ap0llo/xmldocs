namespace Grynwald.XmlDocs;

/// <summary>
/// Represents a block of formatted text in XML documentation comments.
/// </summary>
public class TextBlock : TextElement, IEquatable<TextBlock>
{
    /// <summary>
    /// Gets the text block's text elements.
    /// </summary>
    public IReadOnlyList<TextElement> Elements { get; set; } = Array.Empty<TextElement>();


    /// <summary>
    /// Initializes a new instance of <see cref="TextBlock"/>.
    /// </summary>
    /// <param name="elements">The <see cref="InlineElement"/> object the text block consists of.</param>
    public TextBlock(params TextElement[] elements)
    {
        Elements = elements;
    }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);

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
            TextElement? returnElement = null;

            try
            {
                if (node is XText textNode)
                {
                    var text = XmlContentHelper.TrimText(textNode.Value, indent);
                    if (!String.IsNullOrEmpty(text))
                        returnElement = new PlainTextElement(text);
                }
                else if (node is XElement element)
                {
                    //TODO: <a> tag (supported by Visual Studio)
                    returnElement = element.Name.LocalName switch
                    {
                        "para" => ParagraphElement.FromXml(element),
                        "paramref" => ParameterReferenceElement.FromXml(element),
                        "typeparamref" => TypeParameterReferenceElement.FromXml(element),
                        "code" => CodeElement.FromXml(element),
                        "c" => CElement.FromXml(element),
                        "see" => SeeElement.FromXml(element),
                        "list" => ListElement.FromXml(element),
                        "em" => EmphasisElement.FromXml(element),
                        "i" => IdiomaticElement.FromXml(element),
                        "b" => BoldElement.FromXml(element),
                        "strong" => StrongElement.FromXml(element),
                        "br" => LineBreakElement.FromXml(element),
                        _ => new UnrecognizedTextElement(element)
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
            if (returnElement != null)
            {
                yield return returnElement;
            }
        }
    }
}
