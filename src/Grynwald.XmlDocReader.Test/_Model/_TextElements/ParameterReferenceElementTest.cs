namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="ParameterReferenceElement"/>
/// </summary>
public class ParameterReferenceElementTest
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Name_must_not_be_null_or_whitespace(string name)
    {
        // ARRANGE

        // ACT 
        var ex = Record.Exception(() => new ParameterReferenceElement(name: name));

        // ASSERT
        var argumentException = Assert.IsType<ArgumentException>(ex);
        Assert.Equal("name", argumentException.ParamName);
    }

    [Fact]
    public void FromXml_fails_on_invalid_xml()
    {
        // ARRANGE
        var input = "not xml";

        // ACT 
        var ex = Record.Exception(() => ParameterReferenceElement.FromXml(input));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
        Assert.IsType<XmlException>(ex.InnerException);
    }

    [Fact]
    public void FromXml_fails_on_invalid_root_element_name()
    {
        // ARRANGE
        var xml = """
                <not-paramnref />
                """;

        // ACT
        var ex = Record.Exception(() => ParameterReferenceElement.FromXml(xml));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
    }

    [Theory]
    [InlineData(@"<paramref />", "Required attribute 'name' on element 'paramref' (at 1:2) does not exist")]
    [InlineData(@"<paramref name="""" />", "Value of attribute 'name' (at 1:2) is empty or whitespace")]
    [InlineData(@"<paramref name="" "" />", "Value of attribute 'name' (at 1:2) is empty or whitespace")]
    public void FromXml_fails_if_name_attribute_is_missing_or_empty(string xml, string expectedErrorMessage)
    {
        // ARRANGE

        // ACT 
        var ex = Record.Exception(() => ParameterReferenceElement.FromXml(xml));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
        Assert.Equal(expectedErrorMessage, ex.Message);
    }

    [Theory]
    [InlineData(@"<paramref name=""parameterName"" />", "parameterName")]
    public void FromXml_returns_expected_value(string xml, string expectedParameterName)
    {
        // ARRANGE

        // ACT
        var sut = ParameterReferenceElement.FromXml(xml);

        // ASSERT
        Assert.Equal(expectedParameterName, sut.Name);
    }

    [Fact]
    public void Two_instances_are_equal_if_their_parameter_name_is_equal()
    {
        // ARRANGE
        var instance1 = new ParameterReferenceElement("parameter");
        var instance2 = new ParameterReferenceElement("parameter");

        // ACT / ASSERT
        Assert.Equal(instance1.GetHashCode(), instance2.GetHashCode());

        Assert.True(instance1.Equals((object)instance2));
        Assert.True(instance2.Equals((object)instance1));

        Assert.True(instance1.Equals(instance2));
        Assert.True(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_instances_are_not_equal_if_their_parameter_name_is_not_equal_01()
    {
        // ARRANGE
        var instance1 = new ParameterReferenceElement("parameter1");
        var instance2 = new ParameterReferenceElement("parameter2");

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
        var sut = new ParameterReferenceElement("parameter");

        // ACT / ASSERT
        Assert.False(sut.Equals((object?)null));
        Assert.False(sut!.Equals((TextBlock?)null));
    }
}
