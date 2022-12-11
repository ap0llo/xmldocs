using Grynwald.XmlDocs.Internal;

namespace Grynwald.XmlDocs.Test;

/// <summary>
/// Tests for <see cref="ListElement"/>
/// </summary>
public class ListElementTest
{
    [Fact]
    public void FromXml_fails_on_invalid_xml()
    {
        // ARRANGE
        var input = "not xml";

        // ACT 
        var ex = Record.Exception(() => ListElement.FromXml(input));

        // ASSERT
        Assert.IsType<XmlDocsException>(ex);
        Assert.IsType<XmlException>(ex.InnerException);
    }

    [Fact]
    public void FromXml_fails_on_invalid_root_element_name()
    {
        // ARRANGE
        var xml = """
                <not-list />
                """;

        // ACT
        var ex = Record.Exception(() => ListElement.FromXml(xml));

        // ASSERT
        Assert.IsType<XmlDocsException>(ex);
    }

    [Theory]
    [InlineData(null, "Required attribute 'type' on element 'list' (at 1:2) does not exist")]
    [InlineData("not a list type", "Failed to parse <list /> element. Attribute 'type' has unsupoorted value (at 1:2)")]
    public void FromXml_fails_if_list_type_is_invalid(string listType, string expectedErrorMessage)
    {
        // ARRANGE
        var xml = XElement.Parse("""
                <list>
                </list>
            """);

        xml.SetAttributeValue("type", listType);

        // ACT 
        var ex = Record.Exception(() => ListElement.FromXml(xml.ToString()));

        // ASSERT
        Assert.IsType<XmlDocsException>(ex);
        Assert.Equal(expectedErrorMessage, ex.Message);
    }

    [Fact]
    public void FromXml_returns_BulletedListElement_instance_for_bulleted_list()
    {
        // ARRANGE
        var xml = XElement.Parse("""
                <list type="bullet">
                    <item>Item 1</item>
                    <item>Item 2</item>
                </list>
            """);

        // ACT
        var list = ListElement.FromXml(xml);

        // ASSERT
        var bulletedList = Assert.IsType<BulletedListElement>(list);
        Assert.Collection(
            bulletedList.Items,
            x =>
            {
                var expected = new SimpleListItem(
                    new TextBlock(
                        new PlainTextElement("Item 1")));

                Assert.Equal(expected, x);
            },
            x =>
            {
                var expected = new SimpleListItem(
                    new TextBlock(
                        new PlainTextElement("Item 2")));

                Assert.Equal(expected, x);
            });
    }

    [Fact]
    public void FromXml_NumberedListElement_instance_for_numbered_list()
    {
        // ARRANGE
        var xml = XElement.Parse("""
                <list type="number">
                    <item>Item 1</item>
                    <item>Item 2</item>
                </list>
            """);

        // ACT
        var list = ListElement.FromXml(xml);

        // ASSERT
        var numberedList = Assert.IsType<NumberedListElement>(list);
        Assert.Collection(
            numberedList.Items,
            x =>
            {
                var expected = new SimpleListItem(
                    new TextBlock(
                        new PlainTextElement("Item 1")));

                Assert.Equal(expected, x);
            },
            x =>
            {
                var expected = new SimpleListItem(
                    new TextBlock(
                        new PlainTextElement("Item 2")));

                Assert.Equal(expected, x);
            });
    }

    [Fact]
    public void FromXml_returns_TableElement_instance_for_table_01()
    {
        // ARRANGE
        var xml = XmlContentHelper.ParseXmlElement("""
                <list type="table">
                    <listheader>
                        <term>Header 1</term>
                        <term>Header 2</term>
                    </listheader>
                    <item>
                        <term>Row 1, Column 1</term>
                        <term>Row 1, Column 2</term>
                    </item>
                    <item>
                        <term>Row 2, Column 1</term>
                        <term>Row 2, Column 2</term>
                    </item>
                </list>
            """);

        // ACT
        var list = ListElement.FromXml(xml);

        // ASSERT
        var table = Assert.IsType<TableElement>(list);
        Assert.NotNull(table.Header);
        Assert.Equal(
            new TableRow(new[]
            {
                new TextBlock(new PlainTextElement("Header 1")),
                new TextBlock(new PlainTextElement("Header 2")),
            }),
            table.Header
        );

        Assert.Collection(
            table.Rows,
            actualRow =>
            {
                var expectedRow = new TableRow(new[]
                {
                    new TextBlock(new PlainTextElement("Row 1, Column 1")),
                    new TextBlock(new PlainTextElement("Row 1, Column 2")),
                });

                Assert.Equal(expectedRow, actualRow);
            },
            actualRow =>
            {
                var expectedRow = new TableRow(new[]
                {
                    new TextBlock(new PlainTextElement("Row 2, Column 1")),
                    new TextBlock(new PlainTextElement("Row 2, Column 2")),
                });

                Assert.Equal(expectedRow, actualRow);
            }
        );

    }

    [Fact]
    public void FromXml_returns_TableElement_instance_for_table_02()
    {
        // ARRANGE
        var xml = XmlContentHelper.ParseXmlElement("""
                <list type="table">
                    <item>
                        <term>Row 1, Column 1</term>
                        <term>Row 1, Column 2</term>
                    </item>
                    <item>
                        <term>Row 2, Column 1</term>
                        <term>Row 2, Column 2</term>
                    </item>
                </list>
            """);

        // ACT
        var list = ListElement.FromXml(xml);

        // ASSERT
        var table = Assert.IsType<TableElement>(list);
        Assert.Null(table.Header);

        Assert.Collection(
            table.Rows,
            actualRow =>
            {
                var expectedRow = new TableRow(new[]
                {
                    new TextBlock(new PlainTextElement("Row 1, Column 1")),
                    new TextBlock(new PlainTextElement("Row 1, Column 2")),
                });

                Assert.Equal(expectedRow, actualRow);
            },
            actualRow =>
            {
                var expectedRow = new TableRow(new[]
                {
                    new TextBlock(new PlainTextElement("Row 2, Column 1")),
                    new TextBlock(new PlainTextElement("Row 2, Column 2")),
                });

                Assert.Equal(expectedRow, actualRow);
            }
        );

    }
}
