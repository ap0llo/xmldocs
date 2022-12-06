namespace Grynwald.XmlDocReader;

public class PropertyDescription : MemberDescription
{
    /// <summary>
    /// Gets the content of the member's <![CDATA[<value />]]> text or <c>null</c> is no value text was found.
    /// </summary>
    public TextBlock? Value { get; init; }

    /// <summary>
    /// Gets the all of the properties's <![CDATA[<param />]]> descriptions.
    /// </summary>
    /// <remarks>
    /// For regular properties, this will be empty, but there might be documentation of parameters for an indexer, which as modeled as properties with parameters.
    /// </remarks>
    public IReadOnlyList<ParameterDescription> Parameters { get; init; } = Array.Empty<ParameterDescription>();

    /// <summary>
    /// Gets the all of the member's <![CDATA[<exception />]]> descriptions.
    /// </summary>
    public IReadOnlyList<ExceptionDescription> Exceptions { get; init; } = Array.Empty<ExceptionDescription>();


    public PropertyDescription(MemberId id) : base(id)
    { }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);


    internal static PropertyDescription FromXml(MemberId id, XElement xml)
    {
        var member = new PropertyDescription(id)
        {
            Summary = TryReadTextBlock(xml, "summary"),
            Remarks = TryReadTextBlock(xml, "remarks"),
            Value = TryReadTextBlock(xml, "value"),
            Example = TryReadTextBlock(xml, "example"),
            Parameters = xml.Elements("param").Select(ParameterDescription.FromXml).ToList(),
            SeeAlso = xml.Elements("seealso").Select(SeeAlsoDescription.FromXml).ToList(),
            Exceptions = xml.Elements("exception").Select(ExceptionDescription.FromXml).ToList(),
        };

        return member;
    }
}
