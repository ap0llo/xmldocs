namespace Grynwald.XmlDocReader;

public class TypeDescription : MemberDescription
{
    public TypeDescription(MemberId id) : base(id)
    { }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);


    internal static TypeDescription FromXml(MemberId id, XElement xml)
    {
        var member = new TypeDescription(id)
        {
            Summary = TryReadTextBlock(xml, "summary"),
            Remarks = TryReadTextBlock(xml, "remarks"),
            Returns = TryReadTextBlock(xml, "returns"),
            Value = TryReadTextBlock(xml, "value"),
            Example = TryReadTextBlock(xml, "example"),
            Parameters = xml.Elements("param").Select(ParameterDescription.FromXml).ToList(),
            TypeParameters = xml.Elements("typeparam").Select(TypeParameterDescription.FromXml).ToList(),
            SeeAlso = xml.Elements("seealso").Select(SeeAlsoDescription.FromXml).ToList(),
            Exceptions = xml.Elements("exception").Select(ExceptionDescription.FromXml).ToList(),
        };

        //TODO: Handle duplicate XML elements (e.g. multiple <summary /> elements)
        //TODO: Handle unknown XML elements

        return member;
    }
}
