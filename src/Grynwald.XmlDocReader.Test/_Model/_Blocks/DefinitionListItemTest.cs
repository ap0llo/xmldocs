namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="DefinitionListItem"/>
/// </summary>
public class DefinitionListItemTest
{
    [Fact]
    public void Two_instances_are_equal_if_their_content_is_equal_01()
    {
        // ARRANGE
        var instance1 = new DefinitionListItem(null, null);
        var instance2 = new DefinitionListItem(null, null);

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
        var instance1 = new DefinitionListItem(null, new TextBlock(new PlainTextElement("Some Text")));
        var instance2 = new DefinitionListItem(null, new TextBlock(new PlainTextElement("Some Text")));

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
    public void Two_instances_are_equal_if_their_content_is_equal_03()
    {
        // ARRANGE
        var instance1 = new DefinitionListItem(new TextBlock(new PlainTextElement("Some Text")), new TextBlock(new PlainTextElement("Some Text")));
        var instance2 = new DefinitionListItem(new TextBlock(new PlainTextElement("Some Text")), new TextBlock(new PlainTextElement("Some Text")));

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
        var instance1 = new DefinitionListItem(null, new TextBlock(new PlainTextElement("Some Text")));
        var instance2 = new DefinitionListItem(new TextBlock(new PlainTextElement("Some Text")), new TextBlock(new PlainTextElement("Some Text")));
        var instance3 = new DefinitionListItem(new TextBlock(new PlainTextElement("Some Other Text")), new TextBlock(new PlainTextElement("Some Other Text")));

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
        var sut = new DefinitionListItem(null, new TextBlock(new PlainTextElement("Some Text")));

        // ACT / ASSERT
        Assert.False(sut.Equals((object?)null));
        Assert.False(sut!.Equals(null));
    }
}
