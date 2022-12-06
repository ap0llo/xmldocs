namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="ListElement"/>
/// </summary>
public class ListElementTest
{
    [Fact]
    public void Items_must_not_be_null()
    {
        // ARRANGE

        // ACT 
        var ex = Record.Exception(() => new ListElement(ListType.Bullet, null, items: null!));

        // ASSERT
        var argumentNullException = Assert.IsType<ArgumentNullException>(ex);
        Assert.Equal("items", argumentNullException.ParamName);
    }

    [Fact]
    public void FromXml_fails_on_invalid_xml()
    {
        // ARRANGE
        var input = "not xml";

        // ACT 
        var ex = Record.Exception(() => ListElement.FromXml(input));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
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
        Assert.IsType<XmlDocReaderException>(ex);
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
        Assert.IsType<XmlDocReaderException>(ex);
        Assert.Equal(expectedErrorMessage, ex.Message);
    }

    [Theory]
    [InlineData("bullet", ListType.Bullet)]
    [InlineData("number", ListType.Number)]
    [InlineData("table", ListType.Table)]
    public void FromXml_parses_list_type_as_expected(string listType, ListType expected)
    {
        // ARRANGE
        var xml = XElement.Parse("""
                <list>
                </list>
            """);

        xml.SetAttributeValue("type", listType);

        // ACT 
        var sut = ListElement.FromXml(xml);

        // ASSERT
        Assert.Equal(expected, sut.Type);
    }

    [Fact]
    public void FromXml_parses_list_header_if_it_is_present()
    {
        // ARRANGE
        var xml = """
            <list type="bullet">
                <listheader>
                    <term>term</term>
                    <description>description</description>
                </listheader>
                <item>
                    <term>Assembly</term>
                    <description>The library or executable built from a compilation.</description>
                </item>
            </list>            
            """;

        // ACT 
        var sut = ListElement.FromXml(xml);

        // ASSERT
        Assert.NotNull(sut.ListHeader);
        Assert.Equal(new TextBlock(new PlainTextElement("term")), sut.ListHeader!.Term);
        Assert.Equal(new TextBlock(new PlainTextElement("description")), sut.ListHeader!.Description);
    }

    [Fact]
    public void FromXml_can_parses_list_without_list_header()
    {
        // ARRANGE
        var xml = """
            <list type="bullet" >
                <item>
                    <term>Assembly</term>
                    <description>The library or executable built from a compilation.</description>
                </item>
            </list>            
            """;

        // ACT 
        var sut = ListElement.FromXml(xml);

        // ASSERT
        Assert.Null(sut.ListHeader);
    }


    [Fact]
    public void FromXml_parses_list_items_as_expected()
    {
        // ARRANGE
        var xml = """
            <list type="bullet">
                <item>
                    <term>Term 1</term>
                    <description>Definition 1</description>
                </item>
                <item>
                    <term>Term 2</term>
                    <description>Definition 2</description>
                </item>
            </list>            
            """;

        // ACT 
        var sut = ListElement.FromXml(xml);

        // ASSERT
        Assert.Collection(
            sut.Items,
            x =>
            {
                Assert.Equal(new TextBlock(new PlainTextElement("Term 1")), x.Term);
                Assert.Equal(new TextBlock(new PlainTextElement("Definition 1")), x.Description);
            },
            x =>
            {
                Assert.Equal(new TextBlock(new PlainTextElement("Term 2")), x.Term);
                Assert.Equal(new TextBlock(new PlainTextElement("Definition 2")), x.Description);
            }
        );
    }

    [Theory]
    [InlineData(ListType.Bullet)]
    [InlineData(ListType.Number)]
    [InlineData(ListType.Table)]
    public void Two_instances_are_equal_if_their_content_is_equal_01(ListType listType)
    {
        // ARRANGE
        var instance1 = new ListElement(listType, null, Array.Empty<ListItemElement>());
        var instance2 = new ListElement(listType, null, Array.Empty<ListItemElement>());

        // ACT / ASSERT
        Assert.Equal(instance1.GetHashCode(), instance2.GetHashCode());

        Assert.True(instance1.Equals((object)instance2));
        Assert.True(instance2.Equals((object)instance1));

        Assert.True(instance1.Equals(instance2));
        Assert.True(instance2.Equals(instance1));
    }

    [Theory]
    [InlineData(ListType.Bullet)]
    [InlineData(ListType.Number)]
    [InlineData(ListType.Table)]
    public void Two_instances_are_equal_if_their_content_is_equal_02(ListType listType)
    {
        // ARRANGE
        var instance1 = new ListElement(listType, new ListItemElement(null, new TextBlock(new PlainTextElement("Text"))), Array.Empty<ListItemElement>());
        var instance2 = new ListElement(listType, new ListItemElement(null, new TextBlock(new PlainTextElement("Text"))), Array.Empty<ListItemElement>());

        // ACT / ASSERT
        Assert.Equal(instance1.GetHashCode(), instance2.GetHashCode());

        Assert.True(instance1.Equals((object)instance2));
        Assert.True(instance2.Equals((object)instance1));

        Assert.True(instance1.Equals(instance2));
        Assert.True(instance2.Equals(instance1));
    }

    [Theory]
    [InlineData(ListType.Bullet)]
    [InlineData(ListType.Number)]
    [InlineData(ListType.Table)]
    public void Two_instances_are_equal_if_their_content_is_equal_03(ListType listType)
    {
        // ARRANGE
        var instance1 = new ListElement(
            listType,
            new ListItemElement(null, new TextBlock(new PlainTextElement("Text"))),
            new[]
            {
                new ListItemElement(null, new TextBlock(new PlainTextElement("item 1")))
            });
        var instance2 = new ListElement(
            listType,
            new ListItemElement(null, new TextBlock(new PlainTextElement("Text"))),
            new[]
            {
                new ListItemElement(null, new TextBlock(new PlainTextElement("item 1")))
            });

        // ACT / ASSERT
        Assert.Equal(instance1.GetHashCode(), instance2.GetHashCode());

        Assert.True(instance1.Equals((object)instance2));
        Assert.True(instance2.Equals((object)instance1));

        Assert.True(instance1.Equals(instance2));
        Assert.True(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_instances_are_not_equal_if_their_content_is_not_equal_01()
    {
        // ARRANGE
        var instance1 = new ListElement(ListType.Bullet, null, Array.Empty<ListItemElement>());
        var instance2 = new ListElement(ListType.Number, null, Array.Empty<ListItemElement>());

        // ACT / ASSERT
        Assert.False(instance1.Equals((object)instance2));
        Assert.False(instance2.Equals((object)instance1));

        Assert.False(instance1.Equals(instance2));
        Assert.False(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_instances_are_not_equal_if_their_content_is_not_equal_02()
    {
        // ARRANGE
        var instance1 = new ListElement(
            ListType.Bullet,
            new ListItemElement(null, new TextBlock(new PlainTextElement("Text 1"))),
            Array.Empty<ListItemElement>()
        );
        var instance2 = new ListElement(
            ListType.Bullet,
            new ListItemElement(null, new TextBlock(new PlainTextElement("Text 2"))),
            Array.Empty<ListItemElement>()
        );

        // ACT / ASSERT
        Assert.False(instance1.Equals((object)instance2));
        Assert.False(instance2.Equals((object)instance1));

        Assert.False(instance1.Equals(instance2));
        Assert.False(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_instances_are_not_equal_if_their_content_is_not_equal_03()
    {
        // ARRANGE
        var instance1 = new ListElement(
            ListType.Bullet,
            new ListItemElement(null, new TextBlock(new PlainTextElement("text"))),
            Array.Empty<ListItemElement>()
        );
        var instance2 = new ListElement(
            ListType.Bullet,
            null,
            Array.Empty<ListItemElement>()
        );

        // ACT / ASSERT
        Assert.False(instance1.Equals((object)instance2));
        Assert.False(instance2.Equals((object)instance1));

        Assert.False(instance1.Equals(instance2));
        Assert.False(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_instances_are_not_equal_if_their_content_is_not_equal_04()
    {
        // ARRANGE
        var instance1 = new ListElement(
            ListType.Bullet,
            new ListItemElement(null, new TextBlock(new PlainTextElement("text"))),
            new[]
            {
                new ListItemElement(null, new TextBlock(new PlainTextElement("Item 1"))),
            }
        );
        var instance2 = new ListElement(
            ListType.Bullet,
            new ListItemElement(null, new TextBlock(new PlainTextElement("text"))),
            Array.Empty<ListItemElement>()
        );

        // ACT / ASSERT
        Assert.False(instance1.Equals((object)instance2));
        Assert.False(instance2.Equals((object)instance1));

        Assert.False(instance1.Equals(instance2));
        Assert.False(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_instances_are_not_equal_if_their_content_is_not_equal_05()
    {
        // ARRANGE
        var instance1 = new ListElement(
            ListType.Bullet,
            new ListItemElement(null, new TextBlock(new PlainTextElement("text"))),
            new[]
            {
                new ListItemElement(null, new TextBlock(new PlainTextElement("Item 1"))),
            }
        );
        var instance2 = new ListElement(
            ListType.Bullet,
            new ListItemElement(null, new TextBlock(new PlainTextElement("text"))),
            new[]
            {
                new ListItemElement(null, new TextBlock(new PlainTextElement("Item"))),
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
        var sut = new ListElement(ListType.Bullet, null, Array.Empty<ListItemElement>());

        // ACT / ASSERT
        Assert.False(sut.Equals((object?)null));
        Assert.False(sut!.Equals(null));
    }
}
