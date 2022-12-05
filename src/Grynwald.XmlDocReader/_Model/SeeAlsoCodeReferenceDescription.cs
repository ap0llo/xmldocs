namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a <c><![CDATA[<seealso />]]></c> element referencing a code element
/// </summary>
/// <seealso cref="SeeAlsoDescription"/>
public class SeeAlsoCodeReferenceDescription : SeeAlsoDescription
{
    /// <summary>
    /// Gets the content of the <c><![CDATA[<seealso />]]></c> element's <c>cref</c> attribute as <see cref="MemberId"/>.
    /// </summary>
    public MemberId Reference { get; }


    /// <summary>
    /// Initializes a new instance of <see cref="SeeAlsoCodeReferenceDescription"/>.
    /// </summary>
    /// <param name="reference">The content of the elements <c>cref</c> attribute.</param>
    /// <param name="text">The element's text (optional).</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="reference"/> is <c>null</c>.</exception>
    public SeeAlsoCodeReferenceDescription(MemberId reference, TextBlock? text) : base(text)
    {
        Reference = reference ?? throw new ArgumentNullException(nameof(reference));
    }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);


    internal static new SeeAlsoCodeReferenceDescription FromXml(XElement xml)
    {
        var cref = xml
            .RequireAttribute("cref")
            .RequireValue();

        if (!MemberId.TryParse(cref, out var reference))
        {
            //TODO: Handle unparsable member id
            throw new NotImplementedException();
        }


        var text = TextBlock.FromXmlOrNullIfEmpty(xml);

        return new SeeAlsoCodeReferenceDescription(reference, text);
    }
}
