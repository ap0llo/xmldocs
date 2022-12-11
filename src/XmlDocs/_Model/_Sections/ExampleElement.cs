namespace Grynwald.XmlDocs;

/// <summary>
/// Represents a <c><![CDATA[<example />]]></c> element in XML documentation comments.
/// </summary>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/">XML documentation comments (Microsoft Learn)</seealso>
public class ExampleElement : SectionElement
{
    public ExampleElement(TextBlock? text) : base(text)
    { }

    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);


    internal static ExampleElement FromXml(XElement xml)
    {
        xml.EnsureNameIs("example");

        return new ExampleElement(
            TextBlock.FromXmlOrNullIfEmpty(xml)
        );
    }
}
