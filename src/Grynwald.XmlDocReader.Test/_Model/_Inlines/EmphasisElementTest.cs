namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="EmphasisElement"/>.
/// </summary>
public class EmphasisElementTest
{
    [Fact]
    public void Content_must_not_be_null()
    {
        // ARRANGE

        // ACT 
        var ex = Record.Exception(() => new EmphasisElement(content: null!));

        // ASSERT
        var argumentNullException = Assert.IsType<ArgumentNullException>(ex);
        Assert.Equal("content", argumentNullException.ParamName);
    }

    [Fact]
    public void FromXml_fails_on_invalid_xml()
    {
        // ARRANGE
        var input = "not xml";

        // ACT 
        var ex = Record.Exception(() => EmphasisElement.FromXml(input));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
        Assert.IsType<XmlException>(ex.InnerException);
    }

    [Fact]
    public void FromXml_fails_on_invalid_root_element_name()
    {
        // ARRANGE
        var xml = """
                <not-em />
                """;

        // ACT
        var ex = Record.Exception(() => EmphasisElement.FromXml(xml));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
    }


    [Fact]
    public void Two_instances_are_equal_if_their_content_is_equal()
    {
        // ARRANGE
        var instance1 = new EmphasisElement("Some Text");
        var instance2 = new EmphasisElement("Some Text");

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
        var instance1 = new EmphasisElement("Content 1");
        var instance2 = new EmphasisElement("Content 2");

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
        var sut = new EmphasisElement("Some Text");

        // ACT / ASSERT
        Assert.False(sut.Equals((object?)null));
        Assert.False(sut!.Equals(null));
    }

}
