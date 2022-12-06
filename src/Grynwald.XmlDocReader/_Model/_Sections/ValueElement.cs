namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a <c><![CDATA[<value />]]></c> element in XML documentation comments.
/// </summary>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/">XML documentation comments (Microsoft Learn)</seealso>
public class ValueElement : SectionElement
{
    public ValueElement(TextBlock? text) : base(text)
    { }

    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);


    internal static ValueElement FromXml(XElement xml)
    {
        xml.EnsureNameIs("value");

        return new ValueElement(
            TextBlock.FromXmlOrNullIfEmpty(xml)
        );
    }
}
