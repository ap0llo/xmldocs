namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="ParagraphElement"/>
/// </summary>
public class ParagraphElementTest
{
    [Fact]
    public void FromXml_fails_on_invalid_xml()
    {
        // ARRANGE
        var input = "not xml";

        // ACT 
        var ex = Record.Exception(() => ParagraphElement.FromXml(input));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
        Assert.IsType<XmlException>(ex.InnerException);
    }

    [Fact]
    public void FromXml_fails_on_invalid_root_element_name()
    {
        // ARRANGE
        var xml = """
                <not-para />
                """;

        // ACT
        var ex = Record.Exception(() => ParagraphElement.FromXml(xml));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
    }

    [Theory]
    [InlineData("<para />", null)]
    [InlineData("<para>Some Content</para>", "Some Content")]
    public void FromXml_retuns_expected_result(string xml, string? expectedContent)
    {
        // ARRANGE

        // ACT 
        var sut = ParagraphElement.FromXml(xml);

        // ASSERT
        if (expectedContent is null)
        {
            Assert.Null(sut.Content);
        }
        else
        {
            Assert.Equal(new TextBlock(new PlainTextElement(expectedContent)), sut.Content);
        }
    }

    [Fact]
    public void Two_instances_are_equal_if_their_content_is_equal_01()
    {
        // ARRANGE
        var instance1 = new ParagraphElement(new TextBlock(new PlainTextElement("Content")));
        var instance2 = new ParagraphElement(new TextBlock(new PlainTextElement("Content")));

        // ACT / ASSERT
        Assert.Equal(instance1.GetHashCode(), instance2.GetHashCode());

        Assert.True(instance1.Equals((object)instance2));
        Assert.True(instance2.Equals((object)instance1));

        Assert.True(instance1.Equals(instance2));
        Assert.True(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_instances_are_equal_if_their_content_is_equal_02()
    {
        // ARRANGE
        var instance1 = new ParagraphElement(null);
        var instance2 = new ParagraphElement(null);

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
        var instance1 = new ParagraphElement(new TextBlock(new PlainTextElement("Some text")));
        var instance2 = new ParagraphElement(new TextBlock(new PlainTextElement("Some other text")));

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
        var sut = new ParagraphElement(null);

        // ACT / ASSERT
        Assert.False(sut.Equals((object?)null));
        Assert.False(sut!.Equals((ParagraphElement?)null));
    }
}
