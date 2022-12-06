namespace Grynwald.XmlDocReader;

public class FieldMemberElement : MemberElement
{
    /// <summary>
    /// Gets the content of the field's <![CDATA[<value />]]> text or <c>null</c> is no value text was found.
    /// </summary>
    public ValueElement? Value { get; init; }


    public FieldMemberElement(MemberId id) : base(id)
    { }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);


    internal static FieldMemberElement FromXml(MemberId id, XElement xml)
    {
        var member = new FieldMemberElement(id)
        {
            Summary = TryReadElement(xml, "summary", SummaryElement.FromXml),
            Remarks = TryReadElement(xml, "remarks", RemarksElement.FromXml),
            Value = TryReadElement(xml, "value", ValueElement.FromXml),
            Example = TryReadElement(xml, "example", ExampleElement.FromXml),
            SeeAlso = xml.Elements("seealso").Select(SeeAlsoElement.FromXml).ToList(),
        };

        return member;
    }
}

