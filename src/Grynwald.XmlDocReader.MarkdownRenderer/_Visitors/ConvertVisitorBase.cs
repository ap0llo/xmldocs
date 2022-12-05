using System.Diagnostics.CodeAnalysis;

namespace Grynwald.XmlDocReader.MarkdownRenderer;

public abstract class ConvertVisitorBase : DocumentationVisitor
{
    // TODO: Move this into the model
    protected bool TryParseMemberId(string id, [NotNullWhen(true)] out string? type, [NotNullWhen(true)] out string? name)
    {
        if (id is { Length: >= 2 } && id[1] == ':')
        {
            type = id[..1];
            name = id[2..];
            return true;
        }

        type = null;
        name = null;
        return false;
    }


    //TODO: Add test if this is really used
    protected virtual string? TryGetLinkForCodeReference(string reference)
    {
        // Extension point for derived classes.
        // Without a semantic model, references cannot be resovled
        return null;
    }
}

