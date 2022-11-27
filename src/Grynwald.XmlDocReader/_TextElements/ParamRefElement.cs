using Grynwald.XmlDocReader.Internal;

namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a <c><![CDATA[<paramref>]]></c> element in XML documentation comments.
/// </summary>
/// <remarks>
/// The <c>paramref</c> tag allows referencing a method's parameter.
/// </remarks>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags">Recommended XML tags for C# documentation comments (Microsoft Learn)</seealso>
public class ParamRefElement : TextElement, IEquatable<ParamRefElement>
{

    public string Name { get; }



    public ParamRefElement(string name)
    {
        if (String.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Value must not be null or whitespace", nameof(name));

        Name = name;
    }


    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Name);

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as ParamRefElement);

    public bool Equals(ParamRefElement? other) =>
        other is not null &&
        StringComparer.Ordinal.Equals(Name, other.Name);


    public static ParamRefElement FromXml(XElement xml)
    {
        xml.EnsureNameIs("paramref");

        var name = xml
            .RequireAttribute("name")
            .RequireValue();

        return new ParamRefElement(name);
    }
}
