namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a <c><![CDATA[<item>]]></c> in a definition list in XML documentation comments.
/// </summary>
/// <remarks>
/// A <see cref="DefinitionListItem"/> is used to represent list items which have a <c>term</c> and/or <c>description</c> node.
/// </remarks>
/// <example>
/// <code language="xml"><![CDATA[
///     <item>
///         <term>Term 1</term>
///         <description>Description Content</description>
///     </item>
/// ]]>
/// </code>
/// </example>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags">Recommended XML tags for C# documentation comments (Microsoft Learn)</seealso>
/// <seealso cref="ListElement"/>
/// <seealso cref="ListItem"/>
public class DefinitionListItem : ListItem, IEquatable<DefinitionListItem>
{
    /// <summary>
    /// The term described by the list item.
    /// </summary>
    /// <value>
    /// The content of the <c>term></c> element if it was specified or <c>null</c>
    /// </value>
    public TextBlock? Term { get; }

    /// <summary>
    /// Gets the list item's content.
    /// </summary>
    public TextBlock? Description { get; }


    /// <summary>
    /// Initializes a new instance of <see cref="DefinitionListItem"/>
    /// </summary>
    /// <param name="term">The content of the list items <c>term</c> element. Can be <c>null</c></param>
    /// <param name="description">The content of the list items <c>description</c> element.</param>
    public DefinitionListItem(TextBlock? term, TextBlock? description)
    {
        Term = term;
        Description = description;
    }


    /// <inheritdoc />
    public override void Accept(IDocumentationVisitor visitor) => visitor.Visit(this);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Description, Term);

    /// <inheritdoc />                                                                  
    public override bool Equals(object? obj) => Equals(obj as DefinitionListItem);

    /// <inheritdoc />
    public bool Equals(DefinitionListItem? other)
    {
        if (other is null)
            return false;

        if (Term is null)
        {
            if (other.Term is not null)
                return false;
        }
        else
        {
            if (!Term.Equals(other.Term))
                return false;
        }

        if (Description is null)
        {
            return other.Description is null;
        }
        else
        {
            return Description.Equals(other.Description);
        }
    }


    internal static new DefinitionListItem FromXml(XElement xml)
    {
        var term = xml.Element("term") is XElement termElement
            ? TextBlock.FromXml(termElement)
            : null;

        var description = xml.Element("description") is XElement descriptionElement
            ? TextBlock.FromXml(descriptionElement)
            : null;

        //TODO: Warn on unrecognized elements
        //TODO: Warn if there are multiple term/description elements

        return new DefinitionListItem(term, description);
    }
}
