namespace Grynwald.XmlDocs.Test;

/// <summary>
/// Tests for <see cref="LineBreakElement"/>
/// </summary>
public class LineBreakElementTest
{
    [Fact]
    public void FromXml_fails_on_invalid_xml()
    {
        // ARRANGE
        var input = "not xml";

        // ACT 
        var ex = Record.Exception(() => LineBreakElement.FromXml(input));

        // ASSERT
        Assert.IsType<XmlDocsException>(ex);
        Assert.IsType<XmlException>(ex.InnerException);
    }

    [Fact]
    public void FromXml_fails_on_invalid_root_element_name()
    {
        // ARRANGE
        var xml = """
                <not-br />
                """;

        // ACT
        var ex = Record.Exception(() => LineBreakElement.FromXml(xml));

        // ASSERT
        Assert.IsType<XmlDocsException>(ex);
    }



    [Fact]
    public void Two_instances_are_equal()
    {
        // ARRANGE
        var instance1 = new LineBreakElement();
        var instance2 = new LineBreakElement();

        // ACT / ASSERT
        Assert.Equal(instance1.GetHashCode(), instance2.GetHashCode());

        Assert.True(instance1.Equals((object)instance2));
        Assert.True(instance2.Equals((object)instance1));

        Assert.True(instance1.Equals(instance2));
        Assert.True(instance2.Equals(instance1));
    }


    [Fact]
    public void Equals_returns_false_when_comparing_to_null()
    {
        // ARRANGE
        var sut = new LineBreakElement();

        // ACT / ASSERT
        Assert.False(sut.Equals((object?)null));
        Assert.False(sut!.Equals(null));
    }
}
