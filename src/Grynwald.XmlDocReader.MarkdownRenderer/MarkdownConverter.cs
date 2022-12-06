namespace Grynwald.XmlDocReader.MarkdownRenderer;

public class MarkdownConverter : IMarkdownConverter
{
    /// <inheritdoc />
    public virtual MdBlock ConvertToBlock(DocumentationElement documentationNode)
    {
        if (documentationNode is null)
            throw new ArgumentNullException(nameof(documentationNode));

        var visitor = CreateConvertToBlockVisitor();
        documentationNode.Accept(visitor);

        return visitor.Result;
    }

    /// <inheritdoc />
    public virtual MdSpan ConvertToSpan(DocumentationElement documentationNode)
    {
        if (documentationNode is null)
            throw new ArgumentNullException(nameof(documentationNode));

        var visitor = CreateConvertToSpanVisitor();
        documentationNode.Accept(visitor);

        return visitor.Result;
    }



    //TODO: Find better names for this method
    protected virtual ConvertToBlockVisitor CreateConvertToBlockVisitor()
    {
        return new ConvertToBlockVisitor(this);
    }

    //TODO: Find better names for this method
    protected virtual ConvertToSpanVisitor CreateConvertToSpanVisitor()
    {
        return new ConvertToSpanVisitor();
    }

}
