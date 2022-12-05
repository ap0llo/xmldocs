using System.Diagnostics.CodeAnalysis;

namespace Grynwald.XmlDocReader.MarkdownRenderer;

public abstract class ConvertVisitorBase : DocumentationVisitor
{
    //TODO: Add test if this is really used
    protected virtual string? TryGetLinkForCodeReference(MemberId reference)
    {
        // Extension point for derived classes.
        // Without a semantic model, references cannot be resovled
        return null;
    }
}

