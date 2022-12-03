namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a <c><![CDATA[<list>]]></c> element in XML documentation comments.
/// </summary>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags">Recommended XML tags for C# documentation comments (Microsoft Learn)</seealso>
public class ListElement : TextElement
{
    /// <summary>
    /// Gets the type of the list.
    /// </summary>
    public ListType Type { get; }

    /// <summary>
    /// Gets the list header.
    /// </summary>
    /// <value>
    /// The list header of <c>null</c> is no list header was specified.
    /// </value>
    public ListItemElement? ListHeader { get; }

    /// <summary>
    /// Gets the list's items.
    /// </summary>
    public IReadOnlyList<ListItemElement> Items { get; }


    /// <summary>
    /// Initializes a new instance of <see cref="ListElement"/>
    /// </summary>
    /// <param name="type">The list's type.</param>
    /// <param name="listHeader">The list's header. Can be <c>null</c></param>
    /// <param name="items">The list's element.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> is <c>null</c>.</exception>
    public ListElement(ListType type, ListItemElement? listHeader, IReadOnlyList<ListItemElement> items)
    {
        Type = type;
        ListHeader = listHeader;
        Items = items ?? throw new ArgumentNullException(nameof(items));
    }


    /// <inheritdoc />
    public override int GetHashCode() =>
        ListHeader is null
            ? Type.GetHashCode()
            : HashCode.Combine(Type, ListHeader);

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as ListElement);

    /// <inheritdoc />
    public bool Equals(ListElement? other)
    {
        if (other is null)
            return false;

        if (Type != other.Type)
            return false;

        if (ListHeader is not null)
        {
            if (!ListHeader.Equals(other.ListHeader))
                return false;
        }
        else
        {
            if (other.ListHeader is not null)
                return false;
        }

        return Items.SequenceEqual(other.Items);
    }


    /// <inheritdoc cref="FromXml(XElement)"/>
    public static ListElement FromXml(string xml) => FromXml(XmlContentHelper.ParseXmlElement(xml));

    /// <summary>
    /// Initializes a new <see cref="ListElement" /> from it's XML equivalent.
    /// </summary>
    public static ListElement FromXml(XElement xml)
    {
        xml.EnsureNameIs("list");

        // parse list type
        if (!Enum.TryParse<ListType>(xml.RequireAttribute("type").RequireValue(), ignoreCase: true, out var listType))
        {
            throw new XmlDocReaderException($"Failed to parse <list /> element. Attribute 'type' has unsupoorted value{xml.GetPositionString()}");
        }

        var listHeader = xml.Element("listheader") is XElement listHeaderElement
            ? ListItemElement.FromXml(listHeaderElement)
            : null;

        var listItems = xml
            .Elements("item")
            .Select(ListItemElement.FromXml)
            .ToList();

        //TODO: Warn if there are multiple list headers
        //TODO: warn if there are unrecognized elements


        return new ListElement(listType, listHeader, listItems);

    }
}
