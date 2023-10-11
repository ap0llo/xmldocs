namespace Grynwald.XmlDocs;

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
public abstract class MemberElement : DocumentationElement
{
    /// <summary>
    /// Gets the id of the member.
    /// </summary>
    public MemberId Id { get; }

    /// <summary>
    /// Gets the content of the member's <![CDATA[<summary />]]> text or <c>null</c> is no summary was found.
    /// </summary>
    public SummaryElement? Summary { get; set; }

    /// <summary>
    /// Gets the content of the member's <![CDATA[<remarks />]]> text or <c>null</c> is no remarks were found.
    /// </summary>
    public RemarksElement? Remarks { get; set; }

    /// <summary>
    /// Gets the content of the member's <![CDATA[<example />]]> text or <c>null</c> is no example text was found.
    /// </summary>
    public ExampleElement? Example { get; set; }

    /// <summary>
    /// Gets all the member's <![CDATA[<seealso />]]> descriptions.
    /// </summary>
    public IReadOnlyList<SeeAlsoElement> SeeAlso { get; set; } = Array.Empty<SeeAlsoElement>();

    /// <summary>
    /// Gets all the sections that were found in the XML but are not recognized as any of the known sections.
    /// </summary>
    public IEnumerable<UnrecognizedSectionElement> UnrecognizedElements { get; set; } = Array.Empty<UnrecognizedSectionElement>();


    /// <summary>
    /// Initializes a new instance of <see cref="MemberElement" />.
    /// </summary>
    /// <param name="id">The name/id of the member</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="id"/> is null or whitespace</exception>
    public MemberElement(MemberId id)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
    }


    /// <inheritdoc  cref="FromXml(XElement)" />
    public static MemberElement FromXml(string xml) => FromXml(XmlContentHelper.ParseXmlElement(xml));

    /// <summary>
    /// Creates a <see cref="MemberElement" /> from its XML representation.
    /// </summary>
    public static MemberElement FromXml(XElement xml)
    {
        xml.EnsureNameIs("member");

        var name = xml.RequireAttribute("name").RequireValue();
        if (!MemberId.TryParse(name, out var id) || id == null)
        {
            throw new XmlDocsException($"Failed to parse member. Invalid member name '{name}'{xml.GetPositionString()}");
        }

        return id.Type switch
        {
            MemberType.Namespace => NamespaceMemberElement.FromXml(id, xml),
            MemberType.Type => TypeMemberElement.FromXml(id, xml),
            MemberType.Field => FieldMemberElement.FromXml(id, xml),
            MemberType.Property => PropertyMemberElement.FromXml(id, xml),
            MemberType.Method => MethodMemberElement.FromXml(id, xml),
            MemberType.Event => EventMemberElement.FromXml(id, xml),
            _ => throw new InvalidOperationException()
        };
    }


    protected static T? TryReadElement<T>(XElement parentElement, string elementName, Func<XElement, T> factory)
    {
        return parentElement.Element(elementName) is XElement element
            ? factory(element)
            : default;
    }

    protected static IReadOnlyList<UnrecognizedSectionElement> GetUnrecognizedElements(XElement parentElement, params XName[] knownElements)
    {
        return parentElement.Elements()
            .Where(x => !knownElements.Contains(x.Name))
            .Select(x => new UnrecognizedSectionElement(x))
            .ToList();
    }
}

