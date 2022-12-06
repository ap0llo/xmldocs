namespace Grynwald.XmlDocReader;

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
            Summary = TryReadTextBlock(xml, "summary"),
            Remarks = TryReadTextBlock(xml, "remarks"),
            Example = TryReadTextBlock(xml, "example"),
            SeeAlso = xml.Elements("seealso").Select(SeeAlsoElement.FromXml).ToList(),
        };

        return member;
    }
}

