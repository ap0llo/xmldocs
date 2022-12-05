namespace Grynwald.XmlDocReader;

public class TypeDescription : MemberDescription
{
    /// <summary>
    /// Gets the all of the type's <![CDATA[<typeparam />]]> descriptions.
    /// </summary>
    public IReadOnlyList<TypeParameterDescription> TypeParameters { get; init; } = Array.Empty<TypeParameterDescription>();


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
            Example = TryReadTextBlock(xml, "example"),
            TypeParameters = xml.Elements("typeparam").Select(TypeParameterDescription.FromXml).ToList(),
            SeeAlso = xml.Elements("seealso").Select(SeeAlsoDescription.FromXml).ToList(),
        };

        //TODO: Handle duplicate XML elements (e.g. multiple <summary /> elements)
        //TODO: Handle unknown XML elements

        return member;
    }
}
