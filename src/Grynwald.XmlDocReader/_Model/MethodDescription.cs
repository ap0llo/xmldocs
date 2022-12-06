namespace Grynwald.XmlDocReader;

public class MethodDescription : MemberDescription
{
    /// <summary>
    /// Gets the content of the method's <![CDATA[<returns />]]> text or <c>null</c> is no returns text was found.
    /// </summary>
    public TextBlock? Returns { get; init; }

    /// <summary>
    /// Gets the all of the methods's <![CDATA[<param />]]> descriptions.
    /// </summary>
    public IReadOnlyList<ParameterDescription> Parameters { get; init; } = Array.Empty<ParameterDescription>();

    /// <summary>
    /// Gets the all of the method's <![CDATA[<typeparam />]]> descriptions.
    /// </summary>
    public IReadOnlyList<TypeParameterDescription> TypeParameters { get; init; } = Array.Empty<TypeParameterDescription>();

    /// <summary>
    /// Gets the all of the member's <![CDATA[<exception />]]> descriptions.
    /// </summary>
    public IReadOnlyList<ExceptionDescription> Exceptions { get; init; } = Array.Empty<ExceptionDescription>();


    public MethodDescription(MemberId id) : base(id)
    { }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);


    internal static MethodDescription FromXml(MemberId id, XElement xml)
    {
        var member = new MethodDescription(id)
        {
            Summary = TryReadTextBlock(xml, "summary"),
            Remarks = TryReadTextBlock(xml, "remarks"),
            Returns = TryReadTextBlock(xml, "returns"),
            Example = TryReadTextBlock(xml, "example"),
            Parameters = xml.Elements("param").Select(ParameterDescription.FromXml).ToList(),
            TypeParameters = xml.Elements("typeparam").Select(TypeParameterDescription.FromXml).ToList(),
            SeeAlso = xml.Elements("seealso").Select(SeeAlsoDescription.FromXml).ToList(),
            Exceptions = xml.Elements("exception").Select(ExceptionDescription.FromXml).ToList(),
        };

        return member;
    }
}
