using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grynwald.XmlDocReader.Internal;

namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a <c><![CDATA[<c>]]></c> element in XML documentation comments.
/// </summary>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags">Recommended XML tags for C# documentation comments (Microsoft Learn)</seealso>
public class CElement : TextElement, IEquatable<CElement>
{

    /// <summary>
    /// Gets the content of the element
    /// </summary>
    public string Content { get; }


    /// <summary>
    /// Initializes a new instance of <see cref="CElement"/>
    /// </summary>
    /// <param name="content">The content of the element.</param>
    public CElement(string content)
    {
        Content = content ?? throw new ArgumentNullException(nameof(content));
    }


    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Content);

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as CElement);

    /// <inheritdoc />
    public bool Equals(CElement? other) => other is not null && StringComparer.Ordinal.Equals(Content, other.Content);


    public static CElement FromXml(XElement xml)
    {
        xml.EnsureNameIs("c");

        return new CElement(xml.Value);
    }
}
