namespace Grynwald.XmlDocReader;

public class FieldDescription : MemberDescription
{
    /// <summary>
    /// Gets the content of the field's <![CDATA[<value />]]> text or <c>null</c> is no value text was found.
    /// </summary>
    public TextBlock? Value { get; init; }


    public FieldDescription(MemberId id) : base(id)
    { }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);


    internal static FieldDescription FromXml(MemberId id, XElement xml)
    {
        var member = new FieldDescription(id)
        {
            Summary = TryReadTextBlock(xml, "summary"),
            Remarks = TryReadTextBlock(xml, "remarks"),
            Value = TryReadTextBlock(xml, "value"),
            Example = TryReadTextBlock(xml, "example"),
            SeeAlso = xml.Elements("seealso").Select(SeeAlsoDescription.FromXml).ToList(),
        };

        //TODO: Handle duplicate XML elements (e.g. multiple <summary /> elements)
        //TODO: Handle unknown XML elements

        return member;
    }
}
