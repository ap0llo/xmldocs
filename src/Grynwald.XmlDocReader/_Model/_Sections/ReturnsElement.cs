namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a <c><![CDATA[<returns />]]></c> element in XML documentation comments.
/// </summary>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/">XML documentation comments (Microsoft Learn)</seealso>
public class ReturnsElement : SectionElement
{
    public ReturnsElement(TextBlock? text) : base(text)
    { }

    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);


    internal static ReturnsElement FromXml(XElement xml)
    {
        xml.EnsureNameIs("returns");

        return new ReturnsElement(
            TextBlock.FromXmlOrNullIfEmpty(xml)
        );
    }
}
