namespace Grynwald.XmlDocReader;

public class NamespaceDescription : MemberDescription
{
    public NamespaceDescription(MemberId id) : base(id)
    { }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);


    internal static NamespaceDescription FromXml(MemberId id, XElement xml)
    {
        var member = new NamespaceDescription(id)
        {
            Summary = TryReadTextBlock(xml, "summary"),
            Remarks = TryReadTextBlock(xml, "remarks"),
            Example = TryReadTextBlock(xml, "example"),
            SeeAlso = xml.Elements("seealso").Select(SeeAlsoDescription.FromXml).ToList(),
        };

        //TODO: Handle duplicate XML elements (e.g. multiple <summary /> elements)
        //TODO: Handle unknown XML elements

        return member;
    }
}
