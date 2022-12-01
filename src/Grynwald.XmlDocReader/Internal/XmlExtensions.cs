using System.Xml;

namespace Grynwald.XmlDocReader.Internal
{
    internal static class XmlExtensions
    {
        public static XElement RequireElement(this XElement parent, XName name)
        {
            var element = parent.Element(name);

            if (element is null)
                throw new XmlDocReaderException($"Required child element '{name}' in element '{parent.Name}'{parent.GetPositionString()} does not exist");

            return element;
        }

        public static string RequireValue(this XElement element)
        {
            var value = element.Value;

            if (String.IsNullOrWhiteSpace(value))
                throw new XmlDocReaderException($"Value of element '{element.Name}'{element.GetPositionString()} is empty or whitespace");

            return value;

        }

        public static XAttribute RequireAttribute(this XElement parent, string name)
        {
            var attribute = parent.Attribute(name);

            if (attribute is null)
                throw new XmlDocReaderException($"Required attribute '{name}' on element '{parent.Name}'{parent.GetPositionString()} does not exist");

            return attribute;
        }

        public static string RequireValue(this XAttribute attribute)
        {
            var value = attribute.Value;

            if (String.IsNullOrWhiteSpace(value))
                throw new XmlDocReaderException($"Value of attribute '{attribute.Name}'{attribute.Parent.GetPositionString()} is empty or whitespace");

            return value;
        }

        public static void EnsureNameIs(this XElement element, XName name)
        {
            if (element.Name != name)
            {
                throw new XmlDocReaderException($"Unexpected element name. Expected '{name}', actual '{element.Name}'{element.GetPositionString()}");
            }
        }

        public static void EnsureNameIs(this XElement element, params XName[] validNames)
        {
            if (!validNames.Contains(element.Name))
                throw new InvalidOperationException($"Unexpected element name. Expected: {String.Join<XName>(" or ", validNames)}, actual {element.Name}");
        }

        public static string GetPositionString(this IXmlLineInfo? node)
        {
            if (node is not null && node.HasLineInfo())
            {
                return $" (at {node.LineNumber}:{node.LinePosition})";
            }
            return "";
        }
    }
}
