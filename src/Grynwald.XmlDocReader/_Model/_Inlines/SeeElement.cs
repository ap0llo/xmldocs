namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a <c><![CDATA[<see />]]></c> element in XML documentation comments.
/// </summary>
/// <remarks>
/// <c>see</c> allows specifying links from within text.
/// <para>
/// The link can point to a code element (like methods or types) using the <c>cref</c> tag (represented by <see cref="SeeCodeReferenceElement"/> type)
/// or a URL using the <c>href</c> tag (represented by <see cref="SeeUrlReferenceElement"/> type).
/// </para>
/// <para>
/// Optionally, the element can display a text as body.
/// </para>
/// </remarks>
/// <example>
/// A <c><![CDATA[<see />]]></c> element without a description text referring to a code element:
/// <code language="xml"><![CDATA[
///     <see cref="MyClass.Method1" />
/// ]]>
/// </code>
/// </example>
/// <example>
/// A <c><![CDATA[<see />]]></c> element without a description text referring to a URL:
/// <code language="xml"><![CDATA[
///     <see href="https://example.com" />
/// ]]>
/// </code>
/// </example>
/// <example>
/// A <c><![CDATA[<see />]]></c> element with a description text referring to a code element:
/// <code language="xml"><![CDATA[
///     <see cref="MyClass.Method1">Some Description text</see>
/// ]]>
/// </code>
/// </example>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags">Recommended XML tags for C# documentation comments (Microsoft Learn)</seealso>
/// <seealso cref="SeeCodeReferenceElement" />
/// <seealso cref="SeeUrlReferenceElement" />
public abstract class SeeElement : InlineElement
{
    /// <summary>
    /// Gets the <c><![CDATA[<see />]]></c> element's text (optional).
    /// </summary>
    public TextBlock? Text { get; }


    /// <summary>
    /// Initializes a new instance of <see cref="SeeElement"/>.
    /// </summary>
    /// <param name="text">The element's text (optional).</param>
    public SeeElement(TextBlock? text)
    {
        Text = text;
    }


    /// <inheritdoc cref="FromXml(XElement)"/>
    public static SeeElement FromXml(string xml) => FromXml(XmlContentHelper.ParseXmlElement(xml));

    /// <summary>
    /// Initializes a new <see cref="SeeElement" /> from it's XML equivalent.
    /// </summary>
    /// <returns>
    /// Returns a <see cref="SeeCodeReferenceElement"/> if the <c><![CDATA[<see />]]></c> element refers to a code element using the <c>cref</c> tag or a
    /// <see cref="SeeUrlReferenceElement"/> if the element refers to a URl using the <c>href</c> tag.
    /// </returns>
    public static SeeElement FromXml(XElement xml)
    {
        xml.EnsureNameIs("see");

        if (xml.Attribute("cref") is not null)
        {
            return SeeCodeReferenceElement.FromXml(xml);
        }
        else if (xml.Attribute("href") is not null)
        {
            return SeeUrlReferenceElement.FromXml(xml);
        }
        else
        {
            throw new XmlDocReaderException($"Failed to parse <see /> element. Expected either a 'cref' or 'href' attribute to be present but found neither{xml.GetPositionString()}");
        }
    }
}
