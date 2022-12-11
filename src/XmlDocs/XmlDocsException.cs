namespace Grynwald.XmlDocs;

public class XmlDocsException : Exception
{
    public XmlDocsException(string? message) : base(message)
    { }

    public XmlDocsException(string? message, Exception? innerException) : base(message, innerException)
    { }
}

