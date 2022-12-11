namespace Grynwald.XmlDocs;

/// <summary>
/// Represents a <c><![CDATA[<seealso />]]></c> element referencing a code element
/// </summary>
/// <seealso cref="SeeAlsoElement"/>
public class SeeAlsoCodeReferenceElement : SeeAlsoElement
{

    /// <summary>
    /// Gets the content of the <c><![CDATA[<seealso />]]></c> element's <c>cref</c> attribute as <see cref="MemberId"/>.
    /// </summary>
    public MemberId Reference { get; }


    /// <summary>
    /// Initializes a new instance of <see cref="SeeAlsoCodeReferenceElement"/>.
    /// </summary>
    /// <param name="reference">The content of the elements <c>cref</c> attribute.</param>
    /// <param name="text">The element's text (optional).</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="reference"/> is <c>null</c>.</exception>
    public SeeAlsoCodeReferenceElement(MemberId reference, TextBlock? text) : base(text)
    {
        Reference = reference ?? throw new ArgumentNullException(nameof(reference));
    }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);


    internal static new SeeAlsoCodeReferenceElement FromXml(XElement xml)
    {
        var cref = xml
            .RequireAttribute("cref")
            .RequireValue();

        if (!MemberId.TryParse(cref, out var reference))
        {
            throw new XmlDocsException($"Failed to parse code reference in <seealso /> element. Invalid reference '{cref}'{xml.GetPositionString()}");
        }


        var text = TextBlock.FromXmlOrNullIfEmpty(xml);

        return new SeeAlsoCodeReferenceElement(reference, text);
    }
}

