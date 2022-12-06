namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="ListItemElement"/>
/// </summary>
public class ListItemElementTest
{
    [Fact]
    public void Description_must_not_be_null()
    {
        // ARRANGE

        // ACT 
        var ex = Record.Exception(() => new ListItemElement(null, description: null!));

        // ASSERT
        var argumentNullException = Assert.IsType<ArgumentNullException>(ex);
        Assert.Equal("description", argumentNullException.ParamName);
    }

    [Fact]
    public void FromXml_fails_on_invalid_xml()
    {
        // ARRANGE
        var input = "not xml";

        // ACT 
        var ex = Record.Exception(() => ListItemElement.FromXml(input));

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
        var ex = Record.Exception(() => ListItemElement.FromXml(xml));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
    }

    [Fact]
    public void FromXml_parses_list_item_as_expected()
    {
        // ARRANGE
        var xml = """
            <item>
                <term>Assembly</term>
                <description>The library or executable built from a compilation.</description>
            </item>
            """;

        // ACT 
        var sut = ListItemElement.FromXml(xml);

        // ASSERT
        Assert.NotNull(sut.Term);
        Assert.Equal(new TextBlock(new PlainTextElement("Assembly")), sut.Term);

        Assert.NotNull(sut.Description);
        Assert.Equal(new TextBlock(new PlainTextElement("The library or executable built from a compilation.")), sut.Description);
    }

    [Fact]
    public void FromXml_can_parse_list_item_without_them()
    {
        // ARRANGE
        var xml = """
            <item>
                <description>Description Text</description>
            </item>
            """;

        // ACT 
        var sut = ListItemElement.FromXml(xml);

        // ASSERT
        Assert.Null(sut.Term);

        Assert.NotNull(sut.Description);
        Assert.Equal(new TextBlock(new PlainTextElement("Description Text")), sut.Description);
    }

    [Fact]
    public void FromXml_can_parse_list_item_without_description()
    {
        // ARRANGE
        var xml = """
            <item>                
            </item>
            """;

        // ACT 
        var sut = ListItemElement.FromXml(xml);

        // ASSERT
        Assert.Null(sut.Term);

        Assert.NotNull(sut.Description);
        Assert.Equal(new TextBlock(), sut.Description);
    }

    [Fact]
    public void Two_instances_are_equal_if_their_content_is_equal_01()
    {
        // ARRANGE
        var instance1 = new ListItemElement(null, new TextBlock(new PlainTextElement("Some Text")));
        var instance2 = new ListItemElement(null, new TextBlock(new PlainTextElement("Some Text")));

        // ACT / ASSERT
        Assert.Equal(instance1.GetHashCode(), instance2.GetHashCode());

        Assert.True(instance1.Equals((object)instance1));
        Assert.True(instance1.Equals(instance1));

        Assert.True(instance1.Equals((object)instance2));
        Assert.True(instance2.Equals((object)instance1));

        Assert.True(instance1.Equals(instance2));
        Assert.True(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_instances_are_equal_if_their_content_is_equal_02()
    {
        // ARRANGE
        var instance1 = new ListItemElement(new TextBlock(new PlainTextElement("Some Text")), new TextBlock(new PlainTextElement("Some Text")));
        var instance2 = new ListItemElement(new TextBlock(new PlainTextElement("Some Text")), new TextBlock(new PlainTextElement("Some Text")));

        // ACT / ASSERT
        Assert.Equal(instance1.GetHashCode(), instance2.GetHashCode());

        Assert.True(instance1.Equals((object)instance1));
        Assert.True(instance1.Equals(instance1));

        Assert.True(instance1.Equals((object)instance2));
        Assert.True(instance2.Equals((object)instance1));

        Assert.True(instance1.Equals(instance2));
        Assert.True(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_instances_are_not_equal_if_their_content_is_not_equal()
    {
        // ARRANGE
        var instance1 = new ListItemElement(null, new TextBlock(new PlainTextElement("Some Text")));
        var instance2 = new ListItemElement(new TextBlock(new PlainTextElement("Some Text")), new TextBlock(new PlainTextElement("Some Text")));
        var instance3 = new ListItemElement(new TextBlock(new PlainTextElement("Some Other Text")), new TextBlock(new PlainTextElement("Some Other Text")));

        // ACT / ASSERT
        Assert.False(instance1.Equals((object)instance2));
        Assert.False(instance2.Equals((object)instance1));

        Assert.False(instance1.Equals((object)instance3));
        Assert.False(instance3.Equals((object)instance1));

        Assert.False(instance2.Equals((object)instance3));
        Assert.False(instance3.Equals((object)instance2));


        Assert.False(instance1.Equals(instance2));
        Assert.False(instance2.Equals(instance1));

        Assert.False(instance1.Equals(instance3));
        Assert.False(instance3.Equals(instance1));


        Assert.False(instance2.Equals(instance3));
        Assert.False(instance3.Equals(instance2));
    }

    [Fact]
    public void Equals_returns_false_when_comparing_to_null()
    {
        // ARRANGE
        var sut = new ListItemElement(null, new TextBlock(new PlainTextElement("Some Text")));

        // ACT / ASSERT
        Assert.False(sut.Equals((object?)null));
        Assert.False(sut!.Equals(null));
    }
}
