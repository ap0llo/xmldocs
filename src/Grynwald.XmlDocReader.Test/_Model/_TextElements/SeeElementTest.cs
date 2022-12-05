namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="SeeElement"/>
/// </summary>
public class SeeElementTest
{
    [Fact]
    public void FromXml_fails_on_invalid_xml()
    {
        // ARRANGE
        var input = "not xml";

        // ACT 
        var ex = Record.Exception(() => SeeElement.FromXml(input));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
        Assert.IsType<XmlException>(ex.InnerException);
    }

    [Fact]
    public void FromXml_fails_on_invalid_root_element_name()
    {
        // ARRANGE
        var xml = """
                <not-see />
                """;

        // ACT
        var ex = Record.Exception(() => SeeElement.FromXml(xml));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
    }

    [Theory]
    [InlineData(@"<see cref=""T:SomeType"" />", null)]
    [InlineData(@"<see cref=""T:SomeType"">Some Text</see>", "Some Text")]
    public void FromXml_returns_SeeCodeReferenceElement_if_a_cref_attribute_is_present(string xml, string? expectedText)
    {
        // ARRANGE

        // ACT 
        var result = SeeElement.FromXml(xml);

        // ASSERT
        var seeCodeReferenceElement = Assert.IsType<SeeCodeReferenceElement>(result);
        Assert.Equal(MemberId.Parse("T:SomeType"), seeCodeReferenceElement.Reference);

        if (expectedText is null)
        {
            Assert.Null(seeCodeReferenceElement.Text);
        }
        else
        {
            Assert.Equal(new TextBlock(new PlainTextElement(expectedText)), seeCodeReferenceElement.Text);
        }

    }

    [Theory]
    [InlineData(@"<see href=""http://example.com"" />", null)]
    [InlineData(@"<see href=""http://example.com"">Some text</see>", "Some text")]
    public void FromXml_returns_SeeCodeReferenceElement_if_a_href_attribute_is_present(string xml, string? expectedText)
    {
        // ARRANGE

        // ACT 
        var result = SeeElement.FromXml(xml);

        // ASSERT
        var seeUrlReferenceElement = Assert.IsType<SeeUrlReferenceElement>(result);
        Assert.Equal("http://example.com", seeUrlReferenceElement.Link);

        if (expectedText is null)
        {
            Assert.Null(seeUrlReferenceElement.Text);
        }
        else
        {
            Assert.Equal(new TextBlock(new PlainTextElement(expectedText)), seeUrlReferenceElement.Text);
        }
    }

    [Theory]
    [InlineData("<see />")]
    [InlineData("<see>Some Description</see>")]
    public void FromXml_fails_is_neither_cref_or_href_attributes_exist(string xml)
    {
        // ARRANGE

        // ACT
        var ex = Record.Exception(() => SeeElement.FromXml(xml));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
        Assert.Equal<object>("Failed to parse <see /> element. Expected either a 'cref' or 'href' attribute to be present but found neither (at 1:2)", ex.Message);
    }
}
