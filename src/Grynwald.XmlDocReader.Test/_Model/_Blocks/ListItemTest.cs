namespace Grynwald.XmlDocReader.Test;


/// <summary>
/// Tests for <see cref="ListItem"/>
/// </summary>
public class ListItemTest
{
    [Fact]
    public void FromXml_fails_on_invalid_xml()
    {
        // ARRANGE
        var input = "not xml";

        // ACT 
        var ex = Record.Exception(() => ListItem.FromXml(input));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
        Assert.IsType<XmlException>(ex.InnerException);
    }

    [Fact]
    public void FromXml_fails_on_invalid_root_element_name()
    {
        // ARRANGE
        var xml = """
                <not-item />
                """;

        // ACT
        var ex = Record.Exception(() => ListItem.FromXml(xml));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
    }


    [Fact]
    public void FromXml_returns_SimpleListItem_if_item_does_not_have_a_description_or_term_element()
    {
        // ARRANGE
        var xml = """
            <item>Some content</item>
            """;

        // ACT 
        var listItem = ListItem.FromXml(xml);

        // ASSERT
        var simpleListItem = Assert.IsType<SimpleListItem>(listItem);
    }


    [Theory]
    [InlineData("""
        <item>
            <description>Some description</description>
        </item>
        """,
        null,
        "Some description")]
    [InlineData("""
        <item>
            <term>Some Term</term>            
        </item>
        """,
        "Some Term",
        null)]
    [InlineData("""
        <item>
            <term>Some Term</term>
            <description>Some description</description>
        </item>
        """,
        "Some Term",
        "Some description")]
    public void FromXml_returns_DefinitionListItem_if_item_has_a_description_or_a_term_element(string xml, string? expectedTerm, string? expectedDescription)
    {
        // ARRANGE 

        // ACT 
        var listItem = ListItem.FromXml(xml);

        // ASSERT
        var definitionListItem = Assert.IsType<DefinitionListItem>(listItem);

        if (expectedTerm is null)
        {
            Assert.Null(definitionListItem.Term);
        }
        else
        {
            Assert.Equal(
                new TextBlock(new PlainTextElement(expectedTerm)),
                definitionListItem.Term
            );
        }

        if (expectedDescription is null)
        {
            Assert.Null(definitionListItem.Description);
        }
        else
        {
            Assert.Equal(
                new TextBlock(new PlainTextElement(expectedDescription)),
                definitionListItem.Description
            );
        }

    }

}
