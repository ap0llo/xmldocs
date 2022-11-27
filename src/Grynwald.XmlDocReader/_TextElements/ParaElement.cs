using Grynwald.XmlDocReader.Internal;

namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a <c><![CDATA[<para>]]></c> element in XML documentation comments.
/// </summary>
/// <remarks>
/// The <c>para</c> tag an be used to structure a text block into paragraphs.
/// </remarks>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags">Recommended XML tags for C# documentation comments (Microsoft Learn)</seealso>
public class ParaElement : TextElement, IEquatable<ParaElement>
{
    /// <summary>
    /// Gets the paragraphs's content.
    /// </summary>
    public TextBlock Text { get; }


    /// <summary>
    /// Initializes a new instance of <see cref="ParaElement"/>
    /// </summary>
    /// <param name="text">The paragraph's content.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="text"/> is <c>null</c>.</exception>
    public ParaElement(TextBlock text)
    {
        Text = text ?? throw new ArgumentNullException(nameof(text));
    }


    /// <inheritdoc />
    public override int GetHashCode() => Text.GetHashCode();

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as ParaElement);

    /// <inheritdoc />
    public bool Equals(ParaElement? other) =>
        other is not null &&
        Text.Equals(other.Text);

    /// <summary>
    /// Initializes a new <see cref="ParaElement" /> from it's XML equivalent
    /// </summary>
    /// <param name="xml"></param>
    /// <returns></returns>
    public static ParaElement FromXml(XElement xml)
    {
        xml.EnsureNameIs("para");

        var text = TextBlock.FromXml(xml);
        return new ParaElement(text);
    }
}
