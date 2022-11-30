namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a <c><![CDATA[<paramref />]]></c> element in XML documentation comments.
/// </summary>
/// <remarks>
/// The <c>paramref</c> tag allows referencing a method's parameter.
/// </remarks>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags">Recommended XML tags for C# documentation comments (Microsoft Learn)</seealso>
public class ParameterReferenceElement : TextElement, IEquatable<ParameterReferenceElement>
{
    /// <summary>
    /// Gets the referenced parameter's name.
    /// </summary>
    public string Name { get; }


    /// <summary>
    /// Initializes a new instance of <see cref="ParameterReferenceElement"/>.
    /// </summary>
    /// <param name="name">The name of the referenced parameter.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is <c>null</c> or whitespace.</exception>
    public ParameterReferenceElement(string name)
    {
        if (String.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Value must not be null or whitespace", nameof(name));

        Name = name;
    }


    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Name);

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as ParameterReferenceElement);

    public bool Equals(ParameterReferenceElement? other) =>
        other is not null &&
        StringComparer.Ordinal.Equals(Name, other.Name);


    /// <inheritdoc cref="FromXml(XElement)"/>
    public static ParameterReferenceElement FromXml(string xml) => FromXml(XmlContentHelper.ParseXmlElement(xml));

    /// <summary>
    /// Initializes a new <see cref="ParameterReferenceElement" /> from it's XML equivalent.
    /// </summary>
    public static ParameterReferenceElement FromXml(XElement xml)
    {
        xml.EnsureNameIs("paramref");

        var name = xml
            .RequireAttribute("name")
            .RequireValue();

        return new ParameterReferenceElement(name);
    }
}
