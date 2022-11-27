using System.Xml.Linq;

namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="ListItemElement"/>
/// </summary>
public class ListItemElementTest
{
    [Fact]
    public void List_item_is_parsed_as_expected()
    {
        // ARRANGE
        var xml = XElement.Parse("""
            <item>
                <term>Assembly</term>
                <description>The library or executable built from a compilation.</description>
            </item>
            """);

        // ACT 
        var sut = ListItemElement.FromXml(xml);

        // ASSERT
        Assert.NotNull(sut.Term);
        Assert.Equal(new TextBlock(new PlainTextElement("Assembly")), sut.Term);

        Assert.NotNull(sut.Description);
        Assert.Equal(new TextBlock(new PlainTextElement("The library or executable built from a compilation.")), sut.Description);
    }


    [Fact]
    public void Term_is_optional()
    {
        // ARRANGE
        var xml = XElement.Parse("""
            <item>
                <description>Description Text</description>
            </item>
            """);

        // ACT 
        var sut = ListItemElement.FromXml(xml);

        // ASSERT
        Assert.Null(sut.Term);

        Assert.NotNull(sut.Description);
        Assert.Equal(new TextBlock(new PlainTextElement("Description Text")), sut.Description);
    }


}
