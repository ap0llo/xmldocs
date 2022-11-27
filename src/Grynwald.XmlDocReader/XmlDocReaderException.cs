namespace Grynwald.XmlDocReader;

public class XmlDocReaderException : Exception
{
    public XmlDocReaderException(string? message) : base(message)
    { }

    public XmlDocReaderException(string? message, Exception? innerException) : base(message, innerException)
    { }
}
