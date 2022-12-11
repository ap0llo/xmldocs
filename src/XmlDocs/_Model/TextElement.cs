namespace Grynwald.XmlDocs;

public abstract class TextElement : DocumentationElement
{
    /// <inheritdoc />
    public abstract override int GetHashCode();

    /// <inheritdoc />
    public abstract override bool Equals(object? obj);
}
