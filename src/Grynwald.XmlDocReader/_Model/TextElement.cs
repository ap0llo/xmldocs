namespace Grynwald.XmlDocReader;

public abstract class TextElement : DocumentationElement
{
    /// <inheritdoc />
    public abstract override int GetHashCode();

    /// <inheritdoc />
    public abstract override bool Equals(object? obj);
}
