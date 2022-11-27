namespace Grynwald.XmlDocReader;

//TODO: Add access to underlying XML to each model object?
//TODO: Differentiate between different kinds of members (Types, field, properties, methods ..)
/// <summary>
/// Represents the documentation of a single member in the documentation file.
/// </summary>
/// <remarks>
/// A member can be a namespace, type, field, property, method or event
/// </remarks>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/">XML documentation comments (Microsoft Learn)</seealso>
public class MemberDescription
{
    /// <summary>
    /// Gets the name/id of the member.
    /// </summary>
    public string Name { get; }

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
    public IReadOnlyList<Parameter> Parameters { get; init; } = Array.Empty<Parameter>();

    /// <summary>
    /// Gets the all of the member's <![CDATA[<typeparam />]]> descriptions.
    /// </summary>
    public IReadOnlyList<TypeParameter> TypeParameters { get; init; } = Array.Empty<TypeParameter>();

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
    /// <param name="name">The name/id of the member</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="name"/> is null or whitespace</exception>
    public MemberDescription(string name)
    {
        if (String.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Value must not be null or whitespace", nameof(name));

        Name = name;
    }


    /// <summary>
    /// Creates a <see cref="MemberDescription" /> from its XML representation.
    /// </summary>
    public static MemberDescription FromXml(string xml)
    {
        XElement parsedXml;
        try
        {
            parsedXml = XElement.Parse(xml, LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo);
        }
        catch (XmlException ex)
        {
            throw new XmlDocReaderException("Failed to parse XML from string", ex);
        }

        return FromXml(parsedXml);
    }

    /// <summary>
    /// Creates a <see cref="MemberDescription" /> from its XML representation.
    /// </summary>
    public static MemberDescription FromXml(XElement xml)
    {
        xml.EnsureNameIs("member");

        var name = xml.RequireAttribute("name").RequireValue();
        var member = new MemberDescription(name)
        {
            Summary = TryReadTextBlock(xml, "summary"),
            Remarks = TryReadTextBlock(xml, "remarks"),
            Returns = TryReadTextBlock(xml, "returns"),
            Value = TryReadTextBlock(xml, "value"),
            Example = TryReadTextBlock(xml, "example"),
            Parameters = xml.Elements("param").Select(Parameter.FromXml).ToList(),
            TypeParameters = xml.Elements("typeparam").Select(TypeParameter.FromXml).ToList(),
            SeeAlso = xml.Elements("seealso").Select(SeeAlsoDescription.FromXml).ToList(),
            Exceptions = xml.Elements("exception").Select(ExceptionDescription.FromXml).ToList(),
        };

        //TODO: Handle unknown XML elements
        //TODO: Handle duplicate XML elements (e.g. multiple <summary /> elements)

        return member;
    }


    private static TextBlock? TryReadTextBlock(XElement parentElement, string elementName)
    {
        return parentElement.Element(elementName) is XElement element
            ? TextBlock.FromXml(element)
            : null;
    }
}
