namespace Grynwald.XmlDocReader;

public class FieldMemberElement : MemberElement
{
    /// <summary>
    /// Gets the content of the field's <![CDATA[<value />]]> text or <c>null</c> is no value text was found.
    /// </summary>
    public TextBlock? Value { get; init; }


    public FieldMemberElement(MemberId id) : base(id)
    { }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);


    internal static FieldMemberElement FromXml(MemberId id, XElement xml)
    {
        var member = new FieldMemberElement(id)
        {
            Summary = TryReadTextBlock(xml, "summary"),
            Remarks = TryReadTextBlock(xml, "remarks"),
            Value = TryReadTextBlock(xml, "value"),
            Example = TryReadTextBlock(xml, "example"),
            SeeAlso = xml.Elements("seealso").Select(SeeAlsoElement.FromXml).ToList(),
        };

        return member;
    }
}

