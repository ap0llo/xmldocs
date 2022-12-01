using System;
using System.Diagnostics.CodeAnalysis;

namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a <c><![CDATA[<see />]]></c> element referencing an external URL
/// </summary>
/// <seealso cref="SeeElement"/>
public class SeeUrlReferenceElement : SeeElement, IEquatable<SeeUrlReferenceElement>
{
    /// <summary>
    /// Gets the content of the <c><![CDATA[<see />]]></c> element's <c>href</c> attribute.
    /// </summary>
    public string Link { get; }


    /// <summary>
    /// Initializes a new instance of <see cref="SeeUrlReferenceElement"/>.
    /// </summary>
    /// <param name="link">The content of the element's <c>href</c> attribute.</param>
    /// <param name="text">The element's text (optional).</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="link"/> is <c>null</c>.</exception>
    public SeeUrlReferenceElement(string link, TextBlock? text) : base(text)
    {
        if (String.IsNullOrWhiteSpace(link))
            throw new ArgumentException("Value must not be null or whitespace", nameof(link));

        Link = link;
    }


    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Link);

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as SeeUrlReferenceElement);

    /// <inheritdoc />
    public bool Equals(SeeUrlReferenceElement? other)
    {
        if (other is null)
            return false;

        if (!StringComparer.Ordinal.Equals(Link, other.Link))
            return false;


        if (Text is null)
            return other.Text is null;

        return Text.Equals(other.Text);
    }


    internal static new SeeUrlReferenceElement FromXml(XElement xml)
    {
        var href = xml
            .RequireAttribute("href")
            .RequireValue();

        var text = TextBlock.FromXmlOrNullIfEmpty(xml);

        return new SeeUrlReferenceElement(href, text);
    }
}
