namespace Grynwald.XmlDocs;

/// <summary>
/// Represents a <c><![CDATA[<list>]]></c>) element in XML documentation comments.
/// </summary>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags">Recommended XML tags for C# documentation comments (Microsoft Learn)</seealso>
/// <see cref="BulletedListElement"/>
/// <see cref="NumberedListElement"/>
/// <see cref="TableElement"/>
public abstract class ListElement : BlockElement
{

    /// <inheritdoc cref="FromXml(XElement)"/>
    public static ListElement FromXml(string xml) => FromXml(XmlContentHelper.ParseXmlElement(xml));

    /// <summary>
    /// Initializes a new <see cref="ListElement" /> from it's XML equivalent.
    /// </summary>
    public static ListElement FromXml(XElement xml)
    {
        xml.EnsureNameIs("list");

        var listType = xml.RequireAttribute("type").RequireValue();

        if (StringComparer.OrdinalIgnoreCase.Equals("bullet", listType))
        {
            return BulletedListElement.FromXml(xml);
        }
        else if (StringComparer.OrdinalIgnoreCase.Equals("number", listType))
        {
            return NumberedListElement.FromXml(xml);
        }
        else if (StringComparer.OrdinalIgnoreCase.Equals("table", listType))
        {
            return TableElement.FromXml(xml);
        }
        else
        {
            throw new XmlDocsException($"Failed to parse <list /> element. Attribute 'type' has unsupoorted value{xml.GetPositionString()}");
        }
    }
}
