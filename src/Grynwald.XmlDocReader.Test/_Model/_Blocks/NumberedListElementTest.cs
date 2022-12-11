namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="NumberedListElement"/>
/// </summary>
public class NumberedListElementTest
{
    [Fact]
    public void Items_must_not_be_null()
    {
        // ARRANGE

        // ACT 
        var ex = Record.Exception(() => new NumberedListElement(items: null!));

        // ASSERT
        var argumentNullException = Assert.IsType<ArgumentNullException>(ex);
        Assert.Equal("items", argumentNullException.ParamName);
    }

    [Fact]
    public void Two_instances_are_equal_if_their_items_are_equal_01()
    {
        // ARRANGE
        var instance1 = new NumberedListElement(Array.Empty<ListItem>());
        var instance2 = new NumberedListElement(Array.Empty<ListItem>());

        // ACT / ASSERT
        Assert.Equal(instance1.GetHashCode(), instance2.GetHashCode());

        Assert.True(instance1.Equals((object)instance2));
        Assert.True(instance2.Equals((object)instance1));

        Assert.True(instance1.Equals(instance2));
        Assert.True(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_instances_are_equal_if_their_items_are_equal_02()
    {
        // ARRANGE
        var instance1 = new NumberedListElement(
            new[]
            {
                new SimpleListItem(new TextBlock(new PlainTextElement("item 1")))
            });
        var instance2 = new NumberedListElement(
            new[]
            {
                new SimpleListItem(new TextBlock(new PlainTextElement("item 1")))
            });

        // ACT / ASSERT
        Assert.Equal(instance1.GetHashCode(), instance2.GetHashCode());

        Assert.True(instance1.Equals((object)instance2));
        Assert.True(instance2.Equals((object)instance1));

        Assert.True(instance1.Equals(instance2));
        Assert.True(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_instances_are_not_equal_if_their_items_are_not_equal_01()
    {
        // ARRANGE
        var instance1 = new NumberedListElement(
            new[]
            {
                new SimpleListItem(new TextBlock(new PlainTextElement("Item 1"))),
            }
        );
        var instance2 = new NumberedListElement(
            Array.Empty<ListItem>()
        );

        // ACT / ASSERT
        Assert.False(instance1.Equals((object)instance2));
        Assert.False(instance2.Equals((object)instance1));

        Assert.False(instance1.Equals(instance2));
        Assert.False(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_instances_are_not_equal_if_their_items_are_not_equal_02()
    {
        // ARRANGE
        var instance1 = new NumberedListElement(
            new[]
            {
                new SimpleListItem (new TextBlock(new PlainTextElement("Item 1"))),
            }
        );
        var instance2 = new NumberedListElement(
            new[]
            {
                new SimpleListItem (new TextBlock(new PlainTextElement("Item"))),
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
        var sut = new NumberedListElement(Array.Empty<ListItem>());

        // ACT / ASSERT
        Assert.False(sut.Equals((object?)null));
        Assert.False(sut!.Equals(null));
    }
}
