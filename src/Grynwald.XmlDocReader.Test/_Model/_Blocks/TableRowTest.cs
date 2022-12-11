using Grynwald.XmlDocReader.Internal;

namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="TableRow"/>
/// </summary>
public class TableRowTest
{
    [Fact]
    public void Columns_must_not_be_null()
    {
        // ARRANGE

        // ACT 
        var ex = Record.Exception(() => new TableRow(columns: null!));

        // ASSERT
        var argumentNullException = Assert.IsType<ArgumentNullException>(ex);
        Assert.Equal("columns", argumentNullException.ParamName);
    }

    [Fact]
    public void GetHashCode_returns_0_for_empty_row()
    {
        // ARRANGE
        var sut = new TableRow(columns: Array.Empty<TextBlock>());

        // ACT 
        var hashCode = sut.GetHashCode();

        // ASSERT
        Assert.Equal(0, hashCode);
    }


    [Fact]
    public void Two_instances_are_equal_if_their_columns_are_equal()
    {
        // ARRANGE
        var instance1 = new TableRow(
            new[]
            {
                new TextBlock(new PlainTextElement("Column 1")),
                new TextBlock(new PlainTextElement("Column 2")),
            }
        );
        var instance2 = new TableRow(
            new[]
            {
                new TextBlock(new PlainTextElement("Column 1")),
                new TextBlock(new PlainTextElement("Column 2")),
            }
        );

        // ACT / ASSERT
        Assert.Equal(instance1.GetHashCode(), instance2.GetHashCode());

        Assert.True(instance1.Equals((object)instance2));
        Assert.True(instance2.Equals((object)instance1));

        Assert.True(instance1.Equals(instance2));
        Assert.True(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_instances_are_not_equal_if_their_columns_are_not_equal_01()
    {
        // ARRANGE
        var instance1 = new TableRow(
            new[]
            {
                new TextBlock(new PlainTextElement("Column 1")),
                new TextBlock(new PlainTextElement("Column 2")),
            }
        );
        var instance2 = new TableRow(
            new[]
            {
                new TextBlock(new PlainTextElement("Column 1")),
            }
        );

        // ACT / ASSERT
        Assert.False(instance1.Equals((object)instance2));
        Assert.False(instance2.Equals((object)instance1));

        Assert.False(instance1.Equals(instance2));
        Assert.False(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_instances_are_not_equal_if_their_columns_are_not_equal_02()
    {
        // ARRANGE
        var instance1 = new TableRow(
            new[]
            {
                new TextBlock(new PlainTextElement("Column 1")),
                new TextBlock(new PlainTextElement("Column 2"))
            }
        );
        var instance2 = new TableRow(
            new[]
            {
                new TextBlock(new PlainTextElement("Column 3")),
                new TextBlock(new PlainTextElement("Column 4")),
            }
        );

        // ACT / ASSERT
        Assert.False(instance1.Equals((object)instance2));
        Assert.False(instance2.Equals((object)instance1));

        Assert.False(instance1.Equals(instance2));
        Assert.False(instance2.Equals(instance1));
    }

    [Fact]
    public void Equals_returns_false_when_comparing_to_null()
    {
        // ARRANGE
        var sut = new TableRow(
            new[]
            {
                new TextBlock(new PlainTextElement("Column 1")),
                new TextBlock(new PlainTextElement("Column 2"))
            }
        );

        // ACT / ASSERT
        Assert.False(sut.Equals((object?)null));
        Assert.False(sut!.Equals(null));
    }

    [Theory]
    [InlineData(
        """
        <listheader>
            <term>Assembly</term>
            <description>The library or executable built from a compilation.</description>
        </listheader>
        """,
       new string[] { "Assembly", "The library or executable built from a compilation." })]
    [InlineData(
        """
        <item>
            <term>Assembly</term>
            <description>The library or executable built from a compilation.</description>
        </item>
        """,
       new string[] { "Assembly", "The library or executable built from a compilation." })]
    [InlineData(
        """
        <item>
            <term>Term 1</term>
            <term>Term 2</term>
        </item>
        """,
       new string[] { "Term 1", "Term 2" })]
    [InlineData(
        """
        <item>
            <description>Description 1</description>
            <description>Description 2</description>
        </item>
        """,
       new string[] { "Description 1", "Description 2" })]
    [InlineData(
        """
        <item>
            <description>Column 1</description>
            <term>Column 2</term>
            <description>Column 3</description>
        </item>
        """,
       new string[] { "Column 1", "Column 2", "Column 3" })]
    public void FromXml_parses_list_item_as_expected(string xml, string[] expectedColumns)
    {
        // ARRANGE
        var elementInspectors = expectedColumns
            .Select<string, Action<TextBlock>>(expected => actual =>
            {
                var expectedTextBlock = new TextBlock(new PlainTextElement(expected));
                Assert.Equal(expectedTextBlock, actual);
            })
            .ToArray();

        // ACT 
        var sut = TableRow.FromXml(XmlContentHelper.ParseXmlElement(xml));

        // ASSERT
        Assert.Collection(sut.Columns, elementInspectors);
    }
}
