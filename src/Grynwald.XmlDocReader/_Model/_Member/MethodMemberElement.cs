namespace Grynwald.XmlDocReader;

public class MethodMemberElement : MemberElement
{
    /// <summary>
    /// Gets the content of the method's <![CDATA[<returns />]]> text or <c>null</c> is no returns text was found.
    /// </summary>
    public ReturnsElement? Returns { get; init; }

    /// <summary>
    /// Gets the all of the methods's <![CDATA[<param />]]> descriptions.
    /// </summary>
    public IReadOnlyList<ParameterElement> Parameters { get; init; } = Array.Empty<ParameterElement>();

    /// <summary>
    /// Gets the all of the method's <![CDATA[<typeparam />]]> descriptions.
    /// </summary>
    public IReadOnlyList<TypeParameterElement> TypeParameters { get; init; } = Array.Empty<TypeParameterElement>();

    /// <summary>
    /// Gets the all of the member's <![CDATA[<exception />]]> descriptions.
    /// </summary>
    public IReadOnlyList<ExceptionElement> Exceptions { get; init; } = Array.Empty<ExceptionElement>();


    public MethodMemberElement(MemberId id) : base(id)
    { }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);


    internal static MethodMemberElement FromXml(MemberId id, XElement xml)
    {
        var member = new MethodMemberElement(id)
        {
            Summary = TryReadElement(xml, "summary", SummaryElement.FromXml),
            Remarks = TryReadElement(xml, "remarks", RemarksElement.FromXml),
            Returns = TryReadElement(xml, "returns", ReturnsElement.FromXml),
            Example = TryReadElement(xml, "example", ExampleElement.FromXml),
            Parameters = xml.Elements("param").Select(ParameterElement.FromXml).ToList(),
            TypeParameters = xml.Elements("typeparam").Select(TypeParameterElement.FromXml).ToList(),
            SeeAlso = xml.Elements("seealso").Select(SeeAlsoElement.FromXml).ToList(),
            Exceptions = xml.Elements("exception").Select(ExceptionElement.FromXml).ToList(),
        };

        return member;
    }
}

