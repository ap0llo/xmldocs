namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a <c><![CDATA[<remarks />]]></c> element in XML documentation comments.
/// </summary>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/">XML documentation comments (Microsoft Learn)</seealso>
public class RemarksElement : SectionElement
{
    public RemarksElement(TextBlock? text) : base(text)
    { }

    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);


    internal static RemarksElement FromXml(XElement xml)
    {
        xml.EnsureNameIs("remarks");

        return new RemarksElement(
            TextBlock.FromXmlOrNullIfEmpty(xml)
        );
    }
}
