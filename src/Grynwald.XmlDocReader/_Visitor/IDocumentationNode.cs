namespace Grynwald.XmlDocReader;

public interface IDocumentationNode
{
    /// <summary>
    /// Calls the appropriate <c>Visit</c> method for this element on the specified <see cref="IDocumentationVisitor" />.
    /// </summary>
    void Accept(IDocumentationVisitor visitor);
}
