namespace Grynwald.XmlDocs.Test;

/// <summary>
/// Tests for <see cref="TypeParameterElement"/> 
/// </summary>
public class TypeParameterElementTest
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Name_must_not_be_null_or_whitespace(string name)
    {
        // ARRANGE

        // ACT 
        var ex = Record.Exception(() => new TypeParameterElement(name, null));

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
        var ex = Record.Exception(() => TypeParameterElement.FromXml(xml));

        // ASSERT
        Assert.IsType<XmlDocsException>(ex);
        Assert.IsType<XmlException>(ex.InnerException);
    }

    [Fact]
    public void FromXml_fails_on_invald_root_element_name()
    {
        // ARRANGE
        var xml = """
                <not-typeparam />
                """;

        // ACT
        var ex = Record.Exception(() => TypeParameterElement.FromXml(xml));

        // ASSERT
        Assert.IsType<XmlDocsException>(ex);
    }


    [Theory]
    [InlineData(@"<typeparam>description</typeparam>", "Required attribute 'name' on element 'typeparam' (at 1:2) does not exist")]
    [InlineData(@"<typeparam name="""">description</typeparam>", "Value of attribute 'name' (at 1:2) is empty or whitespace")]
    [InlineData(@"<typeparam name="" "">description</typeparam>", "Value of attribute 'name' (at 1:2) is empty or whitespace")]
    public void FromXml_fails_if_the_name_attribute_is_missing_or_empty(string xml, string expectedErrorMessage)
    {
        // ARRANGE
        // 
        // ACT 
        var ex = Record.Exception(() => TypeParameterElement.FromXml(xml));

        // ASSERT
        Assert.IsType<XmlDocsException>(ex);
        Assert.Equal(expectedErrorMessage, ex.Message);
    }


    [Theory]
    [InlineData(@"<typeparam name=""parameterName""/>", "parameterName", null)]
    [InlineData(@"<typeparam name=""parameterName"">Description text</typeparam>", "parameterName", "Description text")]
    public void FromXml_returns_expected_value(string xml, string expectedName, string? expectedText)
    {
        // ARRANGE

        // ACT
        var sut = TypeParameterElement.FromXml(xml);

        // ASSERT
        Assert.Equal(expectedName, sut.Name);

        if (expectedText is null)
            Assert.Null(sut.Text);
        else
            Assert.Equal(new TextBlock(new PlainTextElement(expectedText)), sut.Text);
    }
}

