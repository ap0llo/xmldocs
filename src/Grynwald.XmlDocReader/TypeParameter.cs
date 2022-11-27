using Grynwald.XmlDocReader.Internal;

namespace Grynwald.XmlDocReader;

public class TypeParameter
{
    public string Name { get; }

    //TODO: TextBlock Description

    public TypeParameter(string name)
    {
        if (String.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Value must not be null or whitespace", nameof(name));

        Name = name;
    }


    public static TypeParameter FromXml(XElement xml)
    {
        xml.EnsureNameIs("typeparam");

        var name = xml
            .RequireAttribute("name")
            .RequireValue();

        return new TypeParameter(name);
    }
}
