namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a <c><![CDATA[<param />]]></c> element in XML documentation comments.
/// </summary>
/// <remarks>
/// <c>param</c> provides information about a method parameter.
/// </remarks>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/">XML documentation comments (Microsoft Learn)</seealso>
public class ParameterElement : SectionElement
{
    /// <summary>
    /// Gets the name of the parameter.
    /// </summary>
    public string Name { get; }


    /// <summary>
    /// Initializes a new instance of <see cref="ParameterElement" />.
    /// </summary>
    /// <param name="name">The name of the parameter.</param>
    /// <param name="text">The (optional) description of the parameter.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="name"/> is <c>null</c> or whitespace.</exception>
    public ParameterElement(string name, TextBlock? text) : base(text)
    {
        if (String.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Value must not be null or whitespace", nameof(name));

        Name = name;
    }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);


    /// <inheritdoc cref="FromXml(XElement)"/>
    public static ParameterElement FromXml(string xml) => FromXml(XmlContentHelper.ParseXmlElement(xml));

    /// <summary>
    /// Creates a <see cref="ParameterElement" /> from its XML representation.
    /// </summary>
    public static ParameterElement FromXml(XElement xml)
    {
        xml.EnsureNameIs("param");

        var name = xml
            .RequireAttribute("name")
            .RequireValue();

        var text = TextBlock.FromXmlOrNullIfEmpty(xml);

        return new ParameterElement(name, text);
    }
}

