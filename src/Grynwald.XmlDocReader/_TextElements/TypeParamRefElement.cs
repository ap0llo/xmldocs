using Grynwald.XmlDocReader.Internal;

namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a <c><![CDATA[<typeparamref>]]></c> element in XML documentation comments.
/// </summary>
/// <remarks>
/// The <c>para</c> tag can be used to reference a method's or type's generic type parameter.
/// </remarks>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags">Recommended XML tags for C# documentation comments (Microsoft Learn)</seealso>
public class TypeParamRefElement : TextElement, IEquatable<TypeParamRefElement>
{
    /// <summary>
    /// Gets the name of the type parameter being referenced.
    /// </summary>
    public string Name { get; }

    

    /// <summary>
    /// Initializes a new instance of <see cref="TypeParamRefElement"/>.
    /// </summary>
    /// <param name="name">The name of the type parameter being referenced.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is <c>null</c> or whitespace.</exception>
    public TypeParamRefElement(string name)
    {
        if (String.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Value must not be null or whitespace", nameof(name));

        Name = name;
    }


    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Name);

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as TypeParamRefElement);

    public bool Equals(TypeParamRefElement? other) =>
        other is not null &&
        StringComparer.Ordinal.Equals(Name, other.Name);


    public static TypeParamRefElement FromXml(XElement xml)
    {
        xml.EnsureNameIs("typeparamref");

        var name = xml
            .RequireAttribute("name")
            .RequireValue();

        return new TypeParamRefElement(name);
    }
}
