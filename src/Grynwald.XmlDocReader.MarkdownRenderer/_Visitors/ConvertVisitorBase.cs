namespace Grynwald.XmlDocReader.MarkdownRenderer;

public abstract class ConvertVisitorBase : DocumentationVisitor
{
    //TODO: Remove this
    protected virtual string? TryGetLinkForCodeReference(MemberId reference)
    {
        // Extension point for derived classes.
        // Without a semantic model, references cannot be resovled
        return null;
    }
}

