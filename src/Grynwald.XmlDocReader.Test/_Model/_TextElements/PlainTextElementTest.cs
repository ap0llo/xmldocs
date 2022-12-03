namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="PlainTextElement"/>
/// </summary>
public class PlainTextElementTest
{
    [Fact]
    public void Content_must_not_be_null()
    {
        // ARRANGE

        // ACT 
        var ex = Record.Exception(() => new PlainTextElement(content: null!));

        // ASSERT
        var argumentNullException = Assert.IsType<ArgumentNullException>(ex);
        Assert.Equal("content", argumentNullException.ParamName);
    }

    [Fact]
    public void Two_instances_are_equal_if_their_content_is_equal()
    {
        // ARRANGE
        var instance1 = new PlainTextElement("Some Text");
        var instance2 = new PlainTextElement("Some Text");

        // ACT / ASSERT
        Assert.Equal(instance1.GetHashCode(), instance2.GetHashCode());

        Assert.True(instance1.Equals((object)instance2));
        Assert.True(instance2.Equals((object)instance1));

        Assert.True(instance1.Equals(instance2));
        Assert.True(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_instances_are_not_equal_if_their_content_is_not_equal()
    {
        // ARRANGE
        var instance1 = new PlainTextElement("Content 1");
        var instance2 = new PlainTextElement("Content 2");

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
        var sut = new PlainTextElement("parameter");

        // ACT / ASSERT
        Assert.False(sut.Equals((object?)null));
        Assert.False(sut!.Equals((PlainTextElement?)null));
    }
}
