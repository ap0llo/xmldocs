namespace Grynwald.XmlDocs.Test;

/// <summary>
/// Tests for <see cref="BoldElement"/>.
/// </summary>
public class BoldElementTest
{
    [Fact]
    public void Content_must_not_be_null()
    {
        // ARRANGE

        // ACT 
        var ex = Record.Exception(() => new BoldElement(content: null!));

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
        var ex = Record.Exception(() => BoldElement.FromXml(input));

        // ASSERT
        Assert.IsType<XmlDocsException>(ex);
        Assert.IsType<XmlException>(ex.InnerException);
    }

    [Fact]
    public void FromXml_fails_on_invalid_root_element_name()
    {
        // ARRANGE
        var xml = """
                <not-b />
                """;

        // ACT
        var ex = Record.Exception(() => BoldElement.FromXml(xml));

        // ASSERT
        Assert.IsType<XmlDocsException>(ex);
    }

    [Fact]
    public void FromXml_returns_expected_element()
    {
        // ARRANGE
        var xml = """
            <b>Text</b>
            """;

        var expected = new BoldElement("Text");

        // ACT 
        var actual = BoldElement.FromXml(xml);

        // ASSERT
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Two_instances_are_equal_if_their_content_is_equal()
    {
        // ARRANGE
        var instance1 = new BoldElement("Some Text");
        var instance2 = new BoldElement("Some Text");

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
        var instance1 = new BoldElement("Content 1");
        var instance2 = new BoldElement("Content 2");

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
        var sut = new BoldElement("Some Text");

        // ACT / ASSERT
        Assert.False(sut.Equals((object?)null));
        Assert.False(sut!.Equals(null));
    }

}
