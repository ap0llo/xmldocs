namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="ExceptionDescription"/>
/// </summary>
public class ExceptionDescriptionTest
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Reference_must_not_be_null_or_whitespace(string reference)
    {
        // ARRANGE

        // ACT 
        var ex = Record.Exception(() => new ExceptionDescription(reference, null!));

        // ASSERT
        var argumentException = Assert.IsType<ArgumentException>(ex);
        Assert.Equal("reference", argumentException.ParamName);
    }

    [Fact]
    public void FromXml_fails_on_invalid_xml()
    {
        // ARRANGE
        var input = "not xml";

        // ACT 
        var ex = Record.Exception(() => SeeAlsoDescription.FromXml(input));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
        Assert.IsType<XmlException>(ex.InnerException);
    }

    [Fact]
    public void FromXml_fails_on_invald_root_element_name()
    {
        // ARRANGE
        var xml = """
                <not-exception />
                """;

        // ACT
        var ex = Record.Exception(() => SeeAlsoDescription.FromXml(xml));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
    }

    [Theory]
    [InlineData(@"<exception>description</exception>", "Required attribute 'cref' on element 'exception' (at 1:2) does not exist")]
    [InlineData(@"<exception cref="""">description</exception>", "Value of attribute 'cref' (at 1:2) is empty or whitespace")]
    [InlineData(@"<exception cref="" "">description</exception>", "Value of attribute 'cref' (at 1:2) is empty or whitespace")]
    public void FromXml_fails_if_cref_attribute_is_missing_or_empty(string xml, string expectedErrorMessage)
    {
        // ARRANGE

        // ACT 
        var ex = Record.Exception(() => ExceptionDescription.FromXml(xml));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
        Assert.Equal(expectedErrorMessage, ex.Message);
    }

    [Theory]
    [InlineData(@"<exception cref=""T:MyException""/>", "T:MyException", null)]
    [InlineData(@"<exception cref=""T:MyException"">Description text</exception>", "T:MyException", "Description text")]
    public void FromXml_returns_expected_value(string xml, string expectedReference, string? expectedText)
    {
        // ARRANGE

        // ACT
        var sut = ExceptionDescription.FromXml(xml);

        // ASSERT
        Assert.Equal(expectedReference, sut.Reference);

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
