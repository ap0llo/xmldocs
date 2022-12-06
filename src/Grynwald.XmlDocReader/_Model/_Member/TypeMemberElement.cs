namespace Grynwald.XmlDocReader;

public class TypeMemberElement : MemberElement
{
    /// <summary>
    /// Gets the all of the type's <![CDATA[<typeparam />]]> descriptions.
    /// </summary>
    public IReadOnlyList<TypeParameterElement> TypeParameters { get; init; } = Array.Empty<TypeParameterElement>();


    public TypeMemberElement(MemberId id) : base(id)
    { }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);


    internal static TypeMemberElement FromXml(MemberId id, XElement xml)
    {
        var member = new TypeMemberElement(id)
        {
            Summary = TryReadTextBlock(xml, "summary"),
            Remarks = TryReadTextBlock(xml, "remarks"),
            Example = TryReadTextBlock(xml, "example"),
            TypeParameters = xml.Elements("typeparam").Select(TypeParameterElement.FromXml).ToList(),
            SeeAlso = xml.Elements("seealso").Select(SeeAlsoElement.FromXml).ToList(),
        };

        return member;
    }
}

