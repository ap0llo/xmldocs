namespace Grynwald.XmlDocs;

public class PropertyMemberElement : MemberElement
{
    /// <summary>
    /// Gets the content of the member's <![CDATA[<value />]]> text or <c>null</c> is no value text was found.
    /// </summary>
    public ValueElement? Value { get; init; }

    /// <summary>
    /// Gets all the property's <![CDATA[<param />]]> descriptions.
    /// </summary>
    /// <remarks>
    /// For regular properties, this will be empty, but there might be documentation of parameters for an indexer, which as modelled as properties with parameters.
    /// </remarks>
    public IReadOnlyList<ParameterElement> Parameters { get; init; } = Array.Empty<ParameterElement>();

    /// <summary>
    /// Gets all the member's <![CDATA[<exception />]]> descriptions.
    /// </summary>
    public IReadOnlyList<ExceptionElement> Exceptions { get; init; } = Array.Empty<ExceptionElement>();


    public PropertyMemberElement(MemberId id) : base(id)
    { }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);


    internal static PropertyMemberElement FromXml(MemberId id, XElement xml)
    {
        var member = new PropertyMemberElement(id)
        {
            Summary = TryReadElement(xml, "summary", SummaryElement.FromXml),
            Remarks = TryReadElement(xml, "remarks", RemarksElement.FromXml),
            Value = TryReadElement(xml, "value", ValueElement.FromXml),
            Example = TryReadElement(xml, "example", ExampleElement.FromXml),
            Parameters = xml.Elements("param").Select(ParameterElement.FromXml).ToList(),
            SeeAlso = xml.Elements("seealso").Select(SeeAlsoElement.FromXml).ToList(),
            Exceptions = xml.Elements("exception").Select(ExceptionElement.FromXml).ToList(),
            UnrecognizedElements = GetUnrecognizedElements(xml, "summary", "remarks", "value", "example", "param", "seealso", "exception")
        };

        return member;
    }
}

