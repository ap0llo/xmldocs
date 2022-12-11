namespace Grynwald.XmlDocs;

/// <summary>
/// Represents a <c><![CDATA[<item>]]></c> in a table in XML documentation comments (which is specifed as a <![CDATA[<list type="table" />]]> element).
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
/// <seealso cref="TableElement"/>
/// <seealso cref="ListItem"/>
public class TableRow : DocumentationElement, IEquatable<TableRow>
{
    /// <summary>
    /// Gets the table row's columns.
    /// </summary>
    public IReadOnlyList<TextBlock> Columns { get; }


    /// <summary>
    /// Initializes a new insatnce of <see cref="TableRow"/>.
    /// </summary>
    /// <param name="columns">The row's columns.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="columns"/> is <c>null</c>.</exception>
    public TableRow(IReadOnlyList<TextBlock> columns)
    {
        Columns = columns ?? throw new ArgumentNullException(nameof(columns));
    }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        if (Columns.Count == 0)
            return 0;

        unchecked
        {
            var hashCode = Columns[0].GetHashCode() * 397;
            for (var i = 1; i < Columns.Count; i++)
            {
                hashCode ^= Columns[i].GetHashCode();
            }
            return hashCode;
        }
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as TableRow);

    /// <inheritdoc />
    public bool Equals(TableRow? other) =>
        other is not null &&
        Columns.SequenceEqual(other.Columns);


    /// <summary>
    /// Initializes a new <see cref="TableRow" /> from it's XML equivalent.
    /// </summary>
    internal static TableRow FromXml(XElement xml)
    {
        //TODO: Warn on unrecognized elements      
        var columns = xml.Elements()
            .Where(x => x.Name == "term" || x.Name == "description")
            .Select(TextBlock.FromXml)
            .ToArray();

        return new TableRow(columns);
    }
}
