namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a <c><![CDATA[<seealso />]]></c> element in XML documentation comments.
/// </summary>
/// <remarks>
/// <c>seealso</c> allows specifying links that will show up in the "See Also" section.
/// <para>
/// The link can point to a code element (like methods or types) using the <c>cref</c> tag (represented by <see cref="SeeAlsoCodeReferenceDescription"/> type)
/// or a URL using the <c>href</c> tag (represented by <see cref="SeeAlsoUrlReferenceDescription"/> type).
/// </para>
/// <para>
/// Optionally, the element can display a text as body.
/// </para>
/// </remarks>
/// <example>
/// A <c><![CDATA[<seealso />]]></c> element without a description text referring to a code element:
/// <code language="xml"><![CDATA[
///     <seealso cref="MyClass.Method1" />
/// ]]>
/// </code>
/// </example>
/// <example>
/// A <c><![CDATA[<seealso />]]></c> element without a description text referring to a URL:
/// <code language="xml"><![CDATA[
///     <seealso href="https://example.com" />
/// ]]>
/// </code>
/// </example>
/// <example>
/// A <c><![CDATA[<seealso />]]></c> element with a description text referring to a code element:
/// <code language="xml"><![CDATA[
///     <seealso cref="MyClass.Method1">Some Description text</seealso>
/// ]]>
/// </code>
/// </example>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/">XML documentation comments (Microsoft Learn)</seealso>
/// <seealso cref="SeeAlsoCodeReferenceDescription" />
/// <seealso cref="SeeAlsoUrlReferenceDescription" />
public abstract class SeeAlsoDescription : IDocumentationNode
{
    /// <summary>
    /// Gets the <c><![CDATA[<seealso />]]></c> element's text (optional).
    /// </summary>
    public TextBlock? Text { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="SeeAlsoDescription"/>.
    /// </summary>
    /// <param name="text">The element's text (optional).</param>
    public SeeAlsoDescription(TextBlock? text)
    {
        Text = text;
    }



    /// <inheritdoc />
    public abstract void Accept(IDocumentationVisitor visitor);


    /// <inheritdoc cref="FromXml(XElement)"/>
    public static SeeAlsoDescription FromXml(string xml) => FromXml(XmlContentHelper.ParseXmlElement(xml));

    /// <summary>
    /// Creates a <see cref="SeeAlsoDescription" /> from its XML representation.
    /// </summary>
    /// <returns>
    /// Returns a <see cref="SeeAlsoCodeReferenceDescription"/> if the <c><![CDATA[<seealso />]]></c> element refers to a code element using the <c>cref</c> tag or a
    /// <see cref="SeeAlsoUrlReferenceDescription"/> if the element refers to a URl using the <c>href</c> tag.
    /// </returns>
    public static SeeAlsoDescription FromXml(XElement xml)
    {
        xml.EnsureNameIs("seealso");

        if (xml.Attribute("cref") is not null)
        {
            return SeeAlsoCodeReferenceDescription.FromXml(xml);
        }
        else if (xml.Attribute("href") is not null)
        {
            return SeeAlsoUrlReferenceDescription.FromXml(xml);
        }
        else
        {
            throw new XmlDocReaderException($"Failed to parse <seealso /> element. Expected either a 'cref' or 'href' attribute to be present but found neither{xml.GetPositionString()}");
        }
    }
}
