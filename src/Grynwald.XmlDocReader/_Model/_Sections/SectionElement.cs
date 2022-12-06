namespace Grynwald.XmlDocReader;

public abstract class SectionElement : IDocumentationNode
{
    /// <inheritdoc />
    public abstract void Accept(IDocumentationVisitor visitor);
}
