using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Grynwald.XmlDocReader.Internal;

namespace Grynwald.XmlDocReader;

public class Parameter
{
    public string Name { get; }

    //TODO: TextBlock Description

    public Parameter(string name)
    {
        if (String.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Value must not be null or whitespace", nameof(name));

        Name = name;
    }


    public static Parameter FromXml(XElement xml)
    {
        xml.EnsureNameIs("param");

        var name = xml
            .RequireAttribute("name")
            .RequireValue();

        return new Parameter(name);
    }
}
