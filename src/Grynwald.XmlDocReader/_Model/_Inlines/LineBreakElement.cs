namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a line-break (<c><![CDATA[<br />]]></c>) text element in XML documentation comments.
/// </summary>
/// <remarks>
/// The <c><![CDATA[<br />]]></c> element allows inserting line breaks into text.
/// </remarks>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags">Recommended XML tags for C# documentation comments (Microsoft Learn)</seealso>
public class LineBreakElement : InlineElement, IEquatable<LineBreakElement>
{
    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);

    /// <inheritdoc />
    // All LineBreak elements are considered equal, so GetHashCode() can return a constant value
    public override int GetHashCode() => 0;

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as LineBreakElement);

    /// <inheritdoc />
    // All LineBreak elements are considered equal, so Equals() only needs to check for null
    public bool Equals(LineBreakElement? other) => other is not null;


    /// <inheritdoc cref="FromXml(XElement)" />
    public static LineBreakElement FromXml(string xml) => FromXml(XmlContentHelper.ParseXmlElement(xml));

    /// <summary>
    /// Initializes a new <see cref="BoldElement" /> from it's XML equivalent.
    /// </summary>
    public static LineBreakElement FromXml(XElement xml)
    {
        xml.EnsureNameIs("br");
        return new LineBreakElement();
    }
}
