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
    public string Id { get; }

    /// <summary>
    /// Gets the content of the member's <![CDATA[<summary />]]> text or <c>null</c> is no summary was found.
    /// </summary>
    public TextBlock? Summary { get; init; }

    /// <summary>
    /// Gets the content of the member's <![CDATA[<remarks />]]> text or <c>null</c> is no remarks were found.
    /// </summary>
    public TextBlock? Remarks { get; init; }

    /// <summary>
    /// Gets the content of the member's <![CDATA[<value />]]> text or <c>null</c> is no value text was found.
    /// </summary>
    public TextBlock? Value { get; init; }

    /// <summary>
    /// Gets the content of the member's <![CDATA[<returns />]]> text or <c>null</c> is no returns text was found.
    /// </summary>
    public TextBlock? Returns { get; init; }

    /// <summary>
    /// Gets the content of the member's <![CDATA[<example />]]> text or <c>null</c> is no example text was found.
    /// </summary>
    public TextBlock? Example { get; init; }

    /// <summary>
    /// Gets the all of the member's <![CDATA[<param />]]> descriptions.
    /// </summary>
    public IReadOnlyList<ParameterDescription> Parameters { get; init; } = Array.Empty<ParameterDescription>();

    /// <summary>
    /// Gets the all of the member's <![CDATA[<typeparam />]]> descriptions.
    /// </summary>
    public IReadOnlyList<TypeParameterDescription> TypeParameters { get; init; } = Array.Empty<TypeParameterDescription>();

    /// <summary>
    /// Gets the all of the member's <![CDATA[<seealso />]]> descriptions.
    /// </summary>
    public IReadOnlyList<SeeAlsoDescription> SeeAlso { get; init; } = Array.Empty<SeeAlsoDescription>();

    /// <summary>
    /// Gets the all of the member's <![CDATA[<exception />]]> descriptions.
    /// </summary>
    public IReadOnlyList<ExceptionDescription> Exceptions { get; init; } = Array.Empty<ExceptionDescription>();


    /// <summary>
    /// Initializes a new instance of <see cref="MemberDescription" />.
    /// </summary>
    /// <param name="id">The name/id of the member</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="id"/> is null or whitespace</exception>
    public MemberDescription(string id)
    {
        if (String.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Value must not be null or whitespace", nameof(id));

        Id = id;
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

        var id = xml.RequireAttribute("name").RequireValue();

        var type = id is { Length: >= 2 } && id[1] == ':'
            ? id[0].ToString()
            : null;

        return type switch
        {
            "N" => NamespaceDescription.FromXml(xml),
            "T" => TypeDescription.FromXml(xml),
            "F" => FieldDescription.FromXml(xml),
            "P" => PropertyDescription.FromXml(xml),
            "M" => MethodDescription.FromXml(xml),
            "E" => EventDescription.FromXml(xml),
            _ => throw new NotImplementedException() //TODO: Handle unknown Id
        };

        //TODO: Handle unknown XML elements
    }


    protected static TextBlock? TryReadTextBlock(XElement parentElement, string elementName)
    {
        return parentElement.Element(elementName) is XElement element
            ? TextBlock.FromXml(element)
            : null;
    }
}
