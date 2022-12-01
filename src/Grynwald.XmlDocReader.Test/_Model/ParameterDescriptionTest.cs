namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="ParameterDescription"/>
/// </summary>
public class ParameterDescriptionTest
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Name_must_not_be_null_or_whitespace(string name)
    {
        // ARRANGE

        // ACT 
        var ex = Record.Exception(() => new ParameterDescription(name, null));

        // ASSERT
        var argumentException = Assert.IsType<ArgumentException>(ex);
        Assert.Equal("name", argumentException.ParamName);
    }

    [Fact]
    public void FromXml_fails_on_invalid_xml()
    {
        // ARRANGE
        var xml = "not xml";

        // ACT 
        var ex = Record.Exception(() => ParameterDescription.FromXml(xml));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
        Assert.IsType<XmlException>(ex.InnerException);
    }

    [Fact]
    public void FromXml_fails_on_invald_root_element_name()
    {
        // ARRANGE
        var xml = """
                <not-seealso />
                """;

        // ACT
        var ex = Record.Exception(() => ParameterDescription.FromXml(xml));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
    }

    [Theory]
    [InlineData(@"<param>description</param>", "Required attribute 'name' on element 'param' (at 1:2) does not exist")]
    [InlineData(@"<param name="""">description</param>", "Value of attribute 'name' (at 1:2) is empty or whitespace")]
    [InlineData(@"<param name="" "">description</param>", "Value of attribute 'name' (at 1:2) is empty or whitespace")]
    public void FromXml_fails_if_the_name_attribute_is_missing_or_empty(string xml, string expectedErrorMessage)
    {
        // ARRANGE

        // ACT 
        var ex = Record.Exception(() => ParameterDescription.FromXml(xml));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
        Assert.Equal(expectedErrorMessage, ex.Message);
    }

    [Theory]
    [InlineData(@"<param name=""parameterName""/>", "parameterName", null)]
    [InlineData(@"<param name=""parameterName"">Description text</param>", "parameterName", "Description text")]
    public void FromXml_returns_expected_value(string xml, string expectedName, string? expectedText)
    {
        // ARRANGE

        // ACT
        var sut = ParameterDescription.FromXml(xml);

        // ASSERT
        Assert.Equal(expectedName, sut.Name);

        if (expectedText is null)
        {
            Assert.Null(sut.Text);
        }
        else
        {
            Assert.Equal(new TextBlock(new PlainTextElement(expectedText)), sut.Text);
        }
    }
}
