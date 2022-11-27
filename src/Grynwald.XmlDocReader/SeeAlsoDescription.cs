using Grynwald.XmlDocReader.Internal;

namespace Grynwald.XmlDocReader;

public class SeeAlsoDescription
{

    //TODO: cref, href, text

    public static SeeAlsoDescription FromXml(XElement xml)
    {
        xml.EnsureNameIs("seealso");

        return new();
    }
}
