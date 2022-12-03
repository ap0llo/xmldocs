namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a <c><![CDATA[<seealso />]]></c> element referencing a external URL
/// </summary>
/// <seealso cref="SeeAlsoDescription"/>
public class SeeAlsoUrlReferenceDescription : SeeAlsoDescription
{
    /// <summary>
    /// Gets the content of the <c><![CDATA[<seealso />]]></c> element's <c>href</c> attribute.
    /// </summary>
    public string Link { get; }


    /// <summary>
    /// Initializes a new instance of <see cref="SeeAlsoUrlReferenceDescription"/>.
    /// </summary>
    /// <param name="link">The content of the element's <c>href</c> attribute.</param>
    /// <param name="text">The element's text (optional).</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="link"/> is <c>null</c>.</exception>
    public SeeAlsoUrlReferenceDescription(string link, TextBlock? text) : base(text)
    {
        Link = link ?? throw new ArgumentNullException(nameof(link));
    }



    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);


    internal static new SeeAlsoUrlReferenceDescription FromXml(XElement xml)
    {
        var href = xml
            .RequireAttribute("href")
            .RequireValue();

        var text = TextBlock.FromXmlOrNullIfEmpty(xml);

        return new SeeAlsoUrlReferenceDescription(href, text);
    }
}
