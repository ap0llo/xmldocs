namespace Grynwald.XmlDocReader;

/// <summary>
/// Base interface for any element in the XML documentation file obejct structure that can be processed by a <see cref="IDocumentationVisitor"/>.
/// </summary>
public interface IDocumentationNode
{
    /// <summary>
    /// Calls the appropriate <c>Visit</c> method for this element on the specified <see cref="IDocumentationVisitor" />.
    /// </summary>
    void Accept(IDocumentationVisitor visitor);
}
