namespace Grynwald.XmlDocs;

/// <summary>
/// Represents a bulleted list (<c><![CDATA[<list type="number">]]></c>) element in XML documentation comments.
/// </summary>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags">Recommended XML tags for C# documentation comments (Microsoft Learn)</seealso>
/// <see cref="ListElement"/>
public class NumberedListElement : ListElement, IEquatable<NumberedListElement>
{
    /// <summary>
    /// Gets the list's items.
    /// </summary>
    public IReadOnlyList<ListItem> Items { get; }


    /// <summary>
    /// Initializes a new instance of <see cref="NumberedListElement"/>.
    /// </summary>
    /// <param name="items">The list's element.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> is <c>null</c>.</exception>
    public NumberedListElement(IReadOnlyList<ListItem> items)
    {
        Items = items ?? throw new ArgumentNullException(nameof(items));
    }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        if (Items.Count == 0)
            return 0;

        unchecked
        {
            var hashCode = Items[0].GetHashCode() * 397;
            for (var i = 1; i < Items.Count; i++)
            {
                hashCode ^= Items[i].GetHashCode();
            }
            return hashCode;
        }
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as NumberedListElement);

    /// <inheritdoc />
    public bool Equals(NumberedListElement? other) =>
        other is not null &&
        Items.SequenceEqual(other.Items);


    /// <summary>
    /// Initializes a new <see cref="NumberedListElement" /> from it's XML equivalent.
    /// </summary>
    internal static new NumberedListElement FromXml(XElement xml)
    {
        var listItems = xml
            .Elements("item")
            .Select(ListItem.FromXml)
            .ToList();

        //TODO: Support start attribute?
        //TODO: warn if there are unrecognized elements

        return new NumberedListElement(listItems);

    }
}
