namespace Grynwald.XmlDocReader;

public abstract class TextElement : IDocumentationNode
{
    /// <inheritdoc />
    public abstract void Accept(IDocumentationVisitor visitor);

    /// <inheritdoc />
    public abstract override int GetHashCode();

    /// <inheritdoc />
    public abstract override bool Equals(object? obj);
}
