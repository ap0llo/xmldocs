namespace Grynwald.XmlDocs;

/// <summary>
/// Represents a <c><![CDATA[<seealso />]]></c> element referencing an external URL
/// </summary>
/// <seealso cref="SeeAlsoElement"/>
public class SeeAlsoUrlReferenceElement : SeeAlsoElement
{
    /// <summary>
    /// Gets the content of the <c><![CDATA[<seealso />]]></c> element's <c>href</c> attribute.
    /// </summary>
    public string Link { get; }


    /// <summary>
    /// Initializes a new instance of <see cref="SeeAlsoUrlReferenceElement"/>.
    /// </summary>
    /// <param name="link">The content of the element's <c>href</c> attribute.</param>
    /// <param name="text">The element's text (optional).</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="link"/> is <c>null</c>.</exception>
    public SeeAlsoUrlReferenceElement(string link, TextBlock? text) : base(text)
    {
        if (String.IsNullOrWhiteSpace(link))
            throw new ArgumentException("Value must not be null or whitespace", nameof(link));

        Link = link;
    }



    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);


    internal static new SeeAlsoUrlReferenceElement FromXml(XElement xml)
    {
        var href = xml
            .RequireAttribute("href")
            .RequireValue();

        var text = TextBlock.FromXmlOrNullIfEmpty(xml);

        return new SeeAlsoUrlReferenceElement(href, text);
    }
}
