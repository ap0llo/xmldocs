namespace Grynwald.XmlDocReader;

public class EventDescription : MemberDescription
{
    /// <summary>
    /// Gets the all of the member's <![CDATA[<exception />]]> descriptions.
    /// </summary>
    public IReadOnlyList<ExceptionDescription> Exceptions { get; init; } = Array.Empty<ExceptionDescription>();


    public EventDescription(MemberId id) : base(id)
    { }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);


    internal static EventDescription FromXml(MemberId id, XElement xml)
    {
        var member = new EventDescription(id)
        {
            Summary = TryReadTextBlock(xml, "summary"),
            Remarks = TryReadTextBlock(xml, "remarks"),
            Example = TryReadTextBlock(xml, "example"),
            SeeAlso = xml.Elements("seealso").Select(SeeAlsoDescription.FromXml).ToList(),
            Exceptions = xml.Elements("exception").Select(ExceptionDescription.FromXml).ToList(),
        };

        //TODO: Handle duplicate XML elements (e.g. multiple <summary /> elements)
        //TODO: Handle unknown XML elements

        return member;
    }
}
