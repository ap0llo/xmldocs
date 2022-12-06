namespace Grynwald.XmlDocReader;

public class EventMemberElement : MemberElement
{
    /// <summary>
    /// Gets the all of the member's <![CDATA[<exception />]]> descriptions.
    /// </summary>
    public IReadOnlyList<ExceptionElement> Exceptions { get; init; } = Array.Empty<ExceptionElement>();


    public EventMemberElement(MemberId id) : base(id)
    { }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);


    internal static EventMemberElement FromXml(MemberId id, XElement xml)
    {
        var member = new EventMemberElement(id)
        {
            Summary = TryReadElement(xml, "summary", SummaryElement.FromXml),
            Remarks = TryReadElement(xml, "remarks", RemarksElement.FromXml),
            Example = TryReadElement(xml, "example", ExampleElement.FromXml),
            SeeAlso = xml.Elements("seealso").Select(SeeAlsoElement.FromXml).ToList(),
            Exceptions = xml.Elements("exception").Select(ExceptionElement.FromXml).ToList(),
        };

        return member;
    }
}

