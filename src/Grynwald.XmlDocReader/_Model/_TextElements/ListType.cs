namespace Grynwald.XmlDocReader;

/// <summary>
/// Enumeration of the supported list types for use with the <c><![CDATA[<list />]]></c> element.
/// </summary>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags">Recommended XML tags for C# documentation comments (Microsoft Learn)</seealso>
public enum ListType
{
    Bullet,
    Number,
    Table
}
