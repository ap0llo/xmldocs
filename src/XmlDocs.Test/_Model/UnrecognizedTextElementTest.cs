namespace Grynwald.XmlDocs.Test;

public class UnrecognizedTextElementTest
{
    [Fact]
    public void Two_instances_are_equal_if_their_xml_content_is_equal()
    {
        // ARRANGE
        var instance1 = new UnrecognizedTextElement(new XElement("element"));
        var instance2 = new UnrecognizedTextElement(new XElement("element"));

        // ACT / ASSERT
        Assert.Equal(instance1.GetHashCode(), instance2.GetHashCode());

        Assert.True(instance1.Equals((object)instance2));
        Assert.True(instance2.Equals((object)instance1));

        Assert.True(instance1.Equals(instance2));
        Assert.True(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_instances_are_not_equal_if_their_xml_content_is_not_equal()
    {
        // ARRANGE
        var instance1 = new UnrecognizedTextElement(new XElement("element1"));
        var instance2 = new UnrecognizedTextElement(new XElement("element2"));

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
        var sut = new UnrecognizedTextElement(new XElement("element"));

        // ACT / ASSERT
        Assert.False(sut.Equals((object?)null));
        Assert.False(sut!.Equals(null));
    }
}
