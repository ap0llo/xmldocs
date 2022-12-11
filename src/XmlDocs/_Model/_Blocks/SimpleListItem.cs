namespace Grynwald.XmlDocs;

/// <summary>
/// Represents a <c><![CDATA[<item>]]></c> in a list in XML documentation comments.
/// </summary>
/// <remarks>
/// A <see cref="SimpleListItem"/> is used to represent list items which's content is specified directly in the item, not as a <c>term</c> and <c>description</c> (this is handled by <see cref="DefinitionListItem"/>).
/// </remarks>
/// <example>
/// <code language="xml"><![CDATA[
///     <item>Item 1</item>
/// ]]>
/// </code>
/// </example>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags">Recommended XML tags for C# documentation comments (Microsoft Learn)</seealso>
/// <seealso cref="ListElement"/>
/// <seealso cref="ListItem"/>
public class SimpleListItem : ListItem, IEquatable<SimpleListItem>
{
    /// <summary>
    /// Gets the list item's content.
    /// </summary>
    public TextBlock Text { get; }


    /// <summary>
    /// Initializes a new instance of <see cref="SimpleListItem" />.
    /// </summary>
    /// <param name="text">The list item's content.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="text"/> is <c>null</c>.</exception>
    public SimpleListItem(TextBlock text)
    {
        Text = text ?? throw new ArgumentNullException(nameof(text));
    }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);

    /// <inheritdoc />
    public override int GetHashCode() => Text.GetHashCode();

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as SimpleListItem);

    /// <inheritdoc />
    public bool Equals(SimpleListItem? other) =>
        other is not null &&
        Text.Equals(other.Text);


    internal static new SimpleListItem FromXml(XElement xml)
    {
        var text = TextBlock.FromXml(xml);
        return new SimpleListItem(text);
    }
}
