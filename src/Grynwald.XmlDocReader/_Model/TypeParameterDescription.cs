namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a <c><![CDATA[<typeparam />]]></c> element in XML documentation comments<em>emphasis</em><i>italis</i><b>bold</b>.
/// </summary>
/// <remarks>
/// <c>typeparam</c> provides information about a type's or method's type parameter.
/// </remarks>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/">XML documentation comments (Microsoft Learn)</seealso>
public class TypeParameterDescription
{
    /// <summary>
    /// Gets the type parameter's name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the parameter's desciption text.
    /// </summary>
    public TextBlock? Text { get; }


    /// <summary>
    /// Initializes a new instance of <see cref="TypeParameterDescription"/>.
    /// </summary>
    /// <param name="name">The type parameter's name.</param>
    /// <param name="text">The (optional) description of the parameter.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="name"/> is <c>null</c> or whitspace.</exception>
    public TypeParameterDescription(string name, TextBlock? text)
    {
        if (String.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Value must not be null or whitespace", nameof(name));

        Name = name;
        Text = text;
    }


    /// <summary>
    /// Calls the appropriate <c>Visit</c> method for this element on the specified visitor.
    /// </summary>
    public void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);


    /// <inheritdoc cref="FromXml(XElement)"/>
    public static TypeParameterDescription FromXml(string xml) => FromXml(XmlContentHelper.ParseXmlElement(xml));

    /// <summary>
    /// Creates a <see cref="ParameterDescription" /> from its XML representation.
    /// </summary>
    public static TypeParameterDescription FromXml(XElement xml)
    {
        xml.EnsureNameIs("typeparam");

        var name = xml
            .RequireAttribute("name")
            .RequireValue();

        var text = TextBlock.FromXmlOrNullIfEmpty(xml);

        return new TypeParameterDescription(name, text);
    }
}
