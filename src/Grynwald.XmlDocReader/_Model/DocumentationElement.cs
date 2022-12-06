namespace Grynwald.XmlDocReader;

/// <summary>
/// Base interface for any element in the XML documentation file object structure.
/// </summary>
public abstract class DocumentationElement
{
    /// <summary>
    /// Calls the appropriate <c>Visit</c> method for this element on the specified <see cref="IDocumentationVisitor" />.
    /// </summary>
    public abstract void Accept(IDocumentationVisitor visitor);
}
