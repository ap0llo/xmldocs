namespace Grynwald.XmlDocs.Test;

/// <summary>
/// Tests for <see cref="SeeUrlReferenceElement"/>
/// </summary>
public class SeeUrlReferenceElementTest
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Reference_must_not_be_null_or_whitespace(string link)
    {
        // ARRANGE

        // ACT
        var ex = Record.Exception(() => new SeeUrlReferenceElement(link: link, null));

        // ASSERT
        var argumentException = Assert.IsType<ArgumentException>(ex);
        Assert.Equal("link", argumentException.ParamName);
    }

    [Fact]
    public void Two_instances_are_equal_if_link_and_text_are_equal_01()
    {
        // ARRANGE
        var instance1 = new SeeUrlReferenceElement("https://example.com", null);
        var instance2 = new SeeUrlReferenceElement("https://example.com", null);

        // ACT / ASSERT
        Assert.Equal(instance1.GetHashCode(), instance2.GetHashCode());

        Assert.True(instance1.Equals((object)instance2));
        Assert.True(instance2.Equals((object)instance1));

        Assert.True(instance1.Equals(instance2));
        Assert.True(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_instances_are_equal_if_link_and_text_are_equal_02()
    {
        // ARRANGE
        var instance1 = new SeeUrlReferenceElement("https://example.com", new TextBlock(new PlainTextElement("Text")));
        var instance2 = new SeeUrlReferenceElement("https://example.com", new TextBlock(new PlainTextElement("Text")));

        // ACT / ASSERT
        Assert.Equal(instance1.GetHashCode(), instance2.GetHashCode());

        Assert.True(instance1.Equals((object)instance2));
        Assert.True(instance2.Equals((object)instance1));

        Assert.True(instance1.Equals(instance2));
        Assert.True(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_instances_are_not_equal_if_link_or_text_are_not_equal_01()
    {
        // ARRANGE
        var instance1 = new SeeUrlReferenceElement("https://example1.com", null);
        var instance2 = new SeeUrlReferenceElement("https://example2.com", null);

        // ACT / ASSERT
        Assert.False(instance1.Equals((object)instance2));
        Assert.False(instance2.Equals((object)instance1));

        Assert.False(instance1.Equals(instance2));
        Assert.False(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_instances_are_not_equal_if_link_or_text_are_not_equal_02()
    {
        // ARRANGE
        var instance1 = new SeeUrlReferenceElement("https://example.com", new TextBlock(new PlainTextElement("Some Text")));
        var instance2 = new SeeUrlReferenceElement("https://example.com", new TextBlock(new PlainTextElement("Some other Text")));

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
        var sut = new SeeUrlReferenceElement("https://example.com", null);

        // ACT / ASSERT
        Assert.False(sut.Equals((object?)null));
        Assert.False(sut!.Equals(null));
    }
}
