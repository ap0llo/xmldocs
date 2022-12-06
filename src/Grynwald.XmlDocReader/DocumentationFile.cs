namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a XML documentation file.
/// </summary>
public class DocumentationFile : IDocumentationNode
{
    /// <summary>
    /// Gets the name of the assembly the documetnation applies to.
    /// </summary>
    public string AssemblyName { get; }

    /// <summary>
    /// Gets the documented members in the documentation file.
    /// </summary>
    public IReadOnlyList<MemberElement> Members { get; }


    /// <summary>
    /// Initialzes a new instance of <see cref="DocumentationFile"/>
    /// </summary>
    /// <param name="assemblyName">The name of the assembly the documentation applies to.</param>
    /// <param name="members">The documented members in the documentation file.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="assemblyName"/> is <c>null</c> or whitespace.</exception>
    /// <exception cref="ArgumentException">Thrown is <paramref name="members"/> is <c>null</c>.</exception>
    public DocumentationFile(string assemblyName, IReadOnlyList<MemberElement> members)
    {
        if (String.IsNullOrWhiteSpace(assemblyName))
            throw new ArgumentException("Value must not be null or whitespace", nameof(assemblyName));

        AssemblyName = assemblyName;
        Members = members ?? throw new ArgumentNullException(nameof(members));
    }


    /// <inheritdoc />
    public void Accept(IDocumentationVisitor vistor) => vistor.Visit(this);

    /// <summary>
    /// Creates a <see cref="DocumentationFile" /> by parsing a XML string.
    /// </summary>
    /// <returns>
    /// Returns a <see cref="DocumentationFile"/> representing the documentation parsed from the specified XML string.
    /// </returns>
    public static DocumentationFile FromXml(string xml) => FromXml(XmlContentHelper.ParseXmlDocument(xml));

    /// <summary>
    /// Creates a <see cref="DocumentationFile" /> by parsing a XML document.
    /// </summary>
    /// <returns>
    /// Returns a <see cref="DocumentationFile"/> representing the documentation parsed from the specified XML document.
    /// </returns>
    public static DocumentationFile FromXml(XDocument xml)
    {
        var root = xml.RequireRootElement();

        root.EnsureNameIs("doc");

        var assemblyName = root
            .RequireElement("assembly")
            .RequireElement("name")
            .RequireValue();

        var members = (root.Element("members") is XElement membersElement)
                 ? membersElement.Elements("member").Select(MemberElement.FromXml).ToArray()
                 : Array.Empty<MemberElement>();

        return new DocumentationFile(assemblyName, members);
    }
}
