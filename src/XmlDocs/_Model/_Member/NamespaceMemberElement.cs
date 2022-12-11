namespace Grynwald.XmlDocs;

public class NamespaceMemberElement : MemberElement
{
    public NamespaceMemberElement(MemberId id) : base(id)
    { }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);


    internal static NamespaceMemberElement FromXml(MemberId id, XElement xml)
    {
        var member = new NamespaceMemberElement(id)
        {
            Summary = TryReadElement(xml, "summary", SummaryElement.FromXml),
            Remarks = TryReadElement(xml, "remarks", RemarksElement.FromXml),
            Example = TryReadElement(xml, "example", ExampleElement.FromXml),
            SeeAlso = xml.Elements("seealso").Select(SeeAlsoElement.FromXml).ToList(),
            UnrecognizedElements = GetUnrecognizedElements(xml, "summary", "remarks", "example", "seealso")
        };

        return member;
    }
}

