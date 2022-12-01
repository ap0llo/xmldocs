namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a <c><![CDATA[<typeparamref />]]></c> text element in XML documentation comments.
/// </summary>
/// <remarks>
/// The <c>typeparamref</c> tag allows referencing a method's or type's generic type parameter.
/// </remarks>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags">Recommended XML tags for C# documentation comments (Microsoft Learn)</seealso>
public class TypeParameterReferenceElement : TextElement, IEquatable<TypeParameterReferenceElement>
{
    /// <summary>
    /// Gets the referenced type parameter's name.
    /// </summary>
    public string Name { get; }


    /// <summary>
    /// Initializes a new instance of <see cref="TypeParameterReferenceElement"/>.
    /// </summary>
    /// <param name="name">The name of the referenced type parameter.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is <c>null</c> or whitespace.</exception>
    public TypeParameterReferenceElement(string name)
    {
        if (String.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Value must not be null or whitespace", nameof(name));

        Name = name;
    }


    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Name);

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as TypeParameterReferenceElement);

    public bool Equals(TypeParameterReferenceElement? other) =>
        other is not null &&
        StringComparer.Ordinal.Equals(Name, other.Name);


    /// <inheritdoc cref="FromXml(XElement)"/>
    public static TypeParameterReferenceElement FromXml(string xml) => FromXml(XmlContentHelper.ParseXmlElement(xml));

    /// <summary>
    /// Initializes a new <see cref="TypeParameterReferenceElement" /> from it's XML equivalent.
    /// </summary>
    public static TypeParameterReferenceElement FromXml(XElement xml)
    {
        xml.EnsureNameIs("typeparamref");

        var name = xml
            .RequireAttribute("name")
            .RequireValue();

        return new TypeParameterReferenceElement(name);
    }
}
