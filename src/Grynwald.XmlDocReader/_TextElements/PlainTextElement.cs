namespace Grynwald.XmlDocReader;

public class PlainTextElement : TextElement, IEquatable<PlainTextElement>
{
    public string Text { get; }


    public PlainTextElement(string text)
    {
        Text = text;
    }


    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Text);

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as PlainTextElement);

    /// <inheritdoc />
    public bool Equals(PlainTextElement? other) =>
        other is not null &&
        StringComparer.Ordinal.Equals(Text, other.Text);
}
