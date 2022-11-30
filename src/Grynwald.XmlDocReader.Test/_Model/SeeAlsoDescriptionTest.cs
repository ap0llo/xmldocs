namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="SeeAlsoDescription"/>
/// </summary>
public class SeeAlsoDescriptionTest
{
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
                <not-seealso />
                """;

        // ACT
        var ex = Record.Exception(() => SeeAlsoDescription.FromXml(xml));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
    }

    [Theory]
    [InlineData(@"<seealso cref=""MyClass"" />", null)]
    [InlineData(@"<seealso cref=""MyClass"">Some description</seealso>", "Some description")]
    [InlineData(@"<seealso cref=""MyClass"" href=""https://Example.com"" />", null)]
    public void FromXml_returns_SeeAlsoCodeReferenceDescription_if_a_cref_attribute_is_present(string xml, string? expectedText)
    {
        // ARRANGE

        // ACT 
        var sut = SeeAlsoDescription.FromXml(xml);

        // ASSERT
        var seeAlsoCodeReference = Assert.IsType<SeeAlsoCodeReferenceDescription>(sut);
        Assert.Equal("MyClass", seeAlsoCodeReference.Reference);

        if (expectedText is null)
        {
            Assert.Null(sut.Text);
        }
        else
        {
            Assert.Equal(new TextBlock(new PlainTextElement(expectedText)), sut.Text);
        }
    }

    [Theory]
    [InlineData(@"<seealso href=""https://example.com"" />", null)]
    [InlineData(@"<seealso href=""https://example.com"">Some description</seealso>", "Some description")]
    public void FromXml_returns_SeeAlsoUrlReferenceDescriptionif_a_href_attribute_is_present(string xml, string? expectedText)
    {
        // ARRANGE

        // ACT 
        var sut = SeeAlsoDescription.FromXml(xml);

        // ASSERT
        var seeAlsoUrlReference = Assert.IsType<SeeAlsoUrlReferenceDescription>(sut);
        Assert.Equal("https://example.com", seeAlsoUrlReference.Link);

        if (expectedText is null)
        {
            Assert.Null(sut.Text);
        }
        else
        {
            Assert.Equal(new TextBlock(new PlainTextElement(expectedText)), sut.Text);
        }
    }

    [Theory]
    [InlineData("<seealso />")]
    [InlineData("<seealso>Some Description</seealso>")]
    public void FromXml_fails_is_neither_cref_or_href_attributes_exist(string xml)
    {
        // ARRANGE

        // ACT
        var ex = Record.Exception(() => SeeAlsoDescription.FromXml(xml));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
        Assert.Equal<object>("Failed to parse <seealso /> element. Expected either a 'cref' or 'href' attribute to be present but found neither (at 1:2)", ex.Message);
    }
}
