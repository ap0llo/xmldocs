namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a <c><![CDATA[<item>]]></c>) element in XML documentation comments in a numbered or bulleted list.
/// </summary>
/// <seealso cref="ListElement"/>
/// <seealso cref="SimpleListItem"/>
/// <seealso cref="DefinitionListItem"/>
public abstract class ListItem : DocumentationElement
{
    /// <inheritdoc />
    public abstract override int GetHashCode();

    /// <inheritdoc />
    public abstract override bool Equals(object? obj);


    /// <inheritdoc cref="FromXml(XElement)"/>
    public static ListItem FromXml(string xml) => FromXml(XmlContentHelper.ParseXmlElement(xml));

    /// <summary>
    /// Initializes a new <see cref="ListItem" /> from it's XML equivalent.
    /// </summary>
    /// <returns>
    /// Returns a <see cref="DefinitionListItem"/> if the item has a <c>term</c> and/or <c>description</c> child element otherwise returns <see cref="SimpleListItem"/>
    /// </returns>
    public static ListItem FromXml(XElement xml)
    {
        xml.EnsureNameIs("item", "listheader");

        if (xml.Element("term") is not null || xml.Element("description") is not null)
        {
            return DefinitionListItem.FromXml(xml);
        }
        else
        {
            return SimpleListItem.FromXml(xml);
        }
    }
}
