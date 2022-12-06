namespace Grynwald.XmlDocReader;

//TODO: Add access to underlying XML to each model object?
//TODO: Implement equality members for all types
//TODO: Support multiple examples
/// <summary>
/// Represents the documentation of a single member in the documentation file.
/// </summary>
/// <remarks>
/// A member can be a namespace, type, field, property, method or event
/// </remarks>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/">XML documentation comments (Microsoft Learn)</seealso>
public abstract class MemberDescription : IDocumentationNode
{
    /// <summary>
    /// Gets the id of the member.
    /// </summary>
    public MemberId Id { get; }

    /// <summary>
    /// Gets the content of the member's <![CDATA[<summary />]]> text or <c>null</c> is no summary was found.
    /// </summary>
    public TextBlock? Summary { get; init; }

    /// <summary>
    /// Gets the content of the member's <![CDATA[<remarks />]]> text or <c>null</c> is no remarks were found.
    /// </summary>
    public TextBlock? Remarks { get; init; }

    /// <summary>
    /// Gets the content of the member's <![CDATA[<example />]]> text or <c>null</c> is no example text was found.
    /// </summary>
    public TextBlock? Example { get; init; }

    /// <summary>
    /// Gets the all of the member's <![CDATA[<seealso />]]> descriptions.
    /// </summary>
    public IReadOnlyList<SeeAlsoDescription> SeeAlso { get; init; } = Array.Empty<SeeAlsoDescription>();


    /// <summary>
    /// Initializes a new instance of <see cref="MemberDescription" />.
    /// </summary>
    /// <param name="id">The name/id of the member</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="id"/> is null or whitespace</exception>
    public MemberDescription(MemberId id)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
    }


    /// <inheritdoc />
    public abstract void Accept(IDocumentationVisitor visitor);

    /// <inheritdoc  cref="FromXml(XElement)" />
    public static MemberDescription FromXml(string xml) => FromXml(XmlContentHelper.ParseXmlElement(xml));

    /// <summary>
    /// Creates a <see cref="MemberDescription" /> from its XML representation.
    /// </summary>
    public static MemberDescription FromXml(XElement xml)
    {
        xml.EnsureNameIs("member");

        var name = xml.RequireAttribute("name").RequireValue();
        if (!MemberId.TryParse(name, out var id))
        {
            throw new XmlDocReaderException($"Failed to parse member. Invalid member name '{name}'{xml.GetPositionString()}");
        }

        return id.Type switch
        {
            MemberType.Namespace => NamespaceDescription.FromXml(id, xml),
            MemberType.Type => TypeDescription.FromXml(id, xml),
            MemberType.Field => FieldDescription.FromXml(id, xml),
            MemberType.Property => PropertyDescription.FromXml(id, xml),
            MemberType.Method => MethodDescription.FromXml(id, xml),
            MemberType.Event => EventDescription.FromXml(id, xml),
            _ => throw new InvalidOperationException()
        };
    }


    protected static TextBlock? TryReadTextBlock(XElement parentElement, string elementName)
    {
        return parentElement.Element(elementName) is XElement element
            ? TextBlock.FromXml(element)
            : null;
    }
}
