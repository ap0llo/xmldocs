namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a table list (<c><![CDATA[<list type="table">]]></c>) element in XML documentation comments.
/// </summary>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags">Recommended XML tags for C# documentation comments (Microsoft Learn)</seealso>
/// <see cref="ListElement"/>
public class TableElement : ListElement, IEquatable<TableElement>
{
    /// <summary>
    /// Gets the list's (optional) header row
    /// </summary>
    public TableRow? Header { get; }

    /// <summary>
    /// Gets the list's rows
    /// </summary>
    public IReadOnlyList<TableRow> Rows { get; }


    /// <summary>
    /// Initializes a new instance of <see cref="TableElement"/>.
    /// </summary>
    /// <param name="header">The table's (optional) header row.</param>
    /// <param name="rows">The table's rows.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="rows"/> is <c>null</c>.</exception>
    public TableElement(TableRow? header, IReadOnlyList<TableRow> rows)
    {
        Header = header;
        Rows = rows ?? throw new ArgumentNullException(nameof(rows));
    }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        if (Rows.Count == 0)
            return 0;

        unchecked
        {
            var hashCode = (Header?.GetHashCode() ?? 0) * 397;
            for (var i = 0; i < Rows.Count; i++)
            {
                hashCode ^= Rows[i].GetHashCode();
            }
            return hashCode;
        }
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as TableElement);

    /// <inheritdoc />
    public bool Equals(TableElement? other)
    {
        if (other is null)
            return false;

        if (Header is null)
        {
            if (other.Header is not null)
                return false;
        }
        else
        {
            if (!Header.Equals(other.Header))
                return false;
        }

        return Rows.SequenceEqual(other.Rows);
    }


    /// <inheritdoc />
    internal static new TableElement FromXml(XElement xml)
    {
        var header = xml.Element("listheader") is XElement listHeaderElement
            ? TableRow.FromXml(listHeaderElement)
            : null;

        var rows = xml.Elements("item")
            .Select(TableRow.FromXml)
            .ToArray();

        return new TableElement(header, rows);
    }
}
