namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a <c><![CDATA[<summary />]]></c> element in XML documentation comments.
/// </summary>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/">XML documentation comments (Microsoft Learn)</seealso>
public class SummaryElement : SectionElement
{
    public SummaryElement(TextBlock? text) : base(text)
    { }

    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);


    internal static SummaryElement FromXml(XElement xml)
    {
        xml.EnsureNameIs("summary");

        return new SummaryElement(
            TextBlock.FromXmlOrNullIfEmpty(xml)
        );
    }
}
