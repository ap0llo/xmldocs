namespace Grynwald.XmlDocReader;

public abstract class SectionElement : DocumentationElement
{
    /// <summary>
    /// Gets the exception description's text.
    /// </summary>
    public TextBlock? Text { get; }


    public SectionElement(TextBlock? text)
    {
        Text = text;
    }
}
