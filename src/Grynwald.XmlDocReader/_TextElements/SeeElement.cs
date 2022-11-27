using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Grynwald.XmlDocReader.Internal;

namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a <c><![CDATA[<see>]]></c> element in XML documentation comments.
/// </summary>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags">Recommended XML tags for C# documentation comments (Microsoft Learn)</seealso>
public abstract class SeeElement : TextElement
{

    public static SeeElement FromXml(XElement xml)
    {
        xml.EnsureNameIs("see");

        // <see /> allows adding links to the documentation
        //
        //   - using  <see cref="..." /> a link to other assembly members
        //     can be inserted (supported by Visual Studio)
        //   - using <see href="..." /> a link to an external resource,
        //     typically a website can be specified (unofficial extension, not supported by VS)
        //
        //   If both cref and href attributes are present, href is ignored
        //
        if (xml.Attribute("cref") is XAttribute crefAttribute)
        {
            return new SeeCRefElement(crefAttribute.Value);
        }
        else if (xml.Attribute("href") is XAttribute hrefAttribute)
        {
            return new SeeHRefElement(hrefAttribute.Value);
        }
        else
        {
            throw new XmlDocReaderException("Cannot load 'see' element that has neither a 'cref' nor a 'href' attribute");
        }

        //TODO: Read TextBlock

    }

}


public class SeeCRefElement : SeeElement, IEquatable<SeeCRefElement>
{

    public string CRef { get; }

    public SeeCRefElement(string cref)
    {

        if (String.IsNullOrWhiteSpace(cref))
            throw new ArgumentException("Value must not be null or whitespace", nameof(cref));

        CRef = cref;
    }

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(CRef);

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as SeeCRefElement);

    /// <inheritdoc />
    public bool Equals(SeeCRefElement? other) =>
        other is not null &&
        StringComparer.Ordinal.Equals(CRef, other.CRef);
}

public class SeeHRefElement : SeeElement, IEquatable<SeeHRefElement>
{
    // TODO: Use URI
    public string HRef { get; }

    public SeeHRefElement(string href)
    {

        if (String.IsNullOrWhiteSpace(href))
            throw new ArgumentException("Value must not be null or whitespace", nameof(href));

        HRef = href;
    }

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(HRef);

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as SeeHRefElement);

    /// <inheritdoc />
    public bool Equals(SeeHRefElement? other) =>
        other is not null &&
        StringComparer.Ordinal.Equals(HRef, other.HRef);
}
