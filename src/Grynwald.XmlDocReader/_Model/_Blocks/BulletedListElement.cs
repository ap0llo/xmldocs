namespace Grynwald.XmlDocReader;


/// <summary>
/// Represents a bulleted list (<c><![CDATA[<list type="bullet">]]></c>) element in XML documentation comments.
/// </summary>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags">Recommended XML tags for C# documentation comments (Microsoft Learn)</seealso>
/// <see cref="ListElement"/>
public class BulletedListElement : ListElement, IEquatable<BulletedListElement>
{
    /// <summary>
    /// Gets the list's items.
    /// </summary>
    public IReadOnlyList<ListItem> Items { get; }


    /// <summary>
    /// Initializes a new instance of <see cref="BulletedListElement"/>
    /// </summary>
    /// <param name="items">The list's element.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> is <c>null</c>.</exception>
    public BulletedListElement(IReadOnlyList<ListItem> items)
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
    public override bool Equals(object? obj) => Equals(obj as BulletedListElement);

    /// <inheritdoc />
    public bool Equals(BulletedListElement? other) =>
        other is not null &&
         Items.SequenceEqual(other.Items);


    /// <summary>
    /// Initializes a new <see cref="BulletedListElement" /> from it's XML equivalent.
    /// </summary>
    internal static new BulletedListElement FromXml(XElement xml)
    {
        var listItems = xml
            .Elements("item")
            .Select(ListItem.FromXml)
            .ToList();

        //TODO: warn if there are unrecognized elements

        return new BulletedListElement(listItems);

    }
}
