namespace Grynwald.XmlDocReader;

/// <summary>
/// Represents a XML documentation file.
/// </summary>
public class DocumentationFile
{
    /// <summary>
    /// Gets the name of the assembly the documetnation applies to.
    /// </summary>
    public string AssemblyName { get; }

    /// <summary>
    /// Gets the documented members in the documentation file.
    /// </summary>
    public IReadOnlyList<MemberDescription> Members { get; }


    /// <summary>
    /// Initialzes a new instance of <see cref="DocumentationFile"/>
    /// </summary>
    /// <param name="assemblyName">The name of the assembly the documentation applies to.</param>
    /// <param name="members">The documented members in the documentation file.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="assemblyName"/> is <c>null</c> or whitespace.</exception>
    /// <exception cref="ArgumentException">Thrown is <paramref name="members"/> is <c>null</c>.</exception>
    public DocumentationFile(string assemblyName, IReadOnlyList<MemberDescription> members)
    {
        if (String.IsNullOrWhiteSpace(assemblyName))
            throw new ArgumentException("Value must not be null or whitespace", nameof(assemblyName));

        AssemblyName = assemblyName;
        Members = members ?? throw new ArgumentNullException(nameof(members));
    }


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
        if (xml is null)
            throw new ArgumentNullException(nameof(xml));

        xml.Root!.EnsureNameIs("doc");

        var assemblyName = xml.Root!
            .RequireElement("assembly")
            .RequireElement("name")
            .RequireValue();

        var members = xml.Root!
            .Element("members")
            ?.Elements("member")
            ?.Select(MemberDescription.FromXml)
            ?.ToArray() ?? Array.Empty<MemberDescription>();

        return new DocumentationFile(assemblyName, members);
    }
}
