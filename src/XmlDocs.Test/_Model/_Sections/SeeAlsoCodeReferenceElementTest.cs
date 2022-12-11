namespace Grynwald.XmlDocs.Test;

/// <summary>
/// Tests for <see cref="SeeAlsoCodeReferenceElement"/>
/// </summary>
public class SeeAlsoCodeReferenceElementTest
{

    [Fact]
    public void Reference_must_not_be_null()
    {
        // ARRANGE

        // ACT 
        var ex = Record.Exception(() => new SeeAlsoCodeReferenceElement(reference: null!, text: null));

        // ASSERT
        var argumentNullException = Assert.IsType<ArgumentNullException>(ex);
        Assert.Equal("reference", argumentNullException.ParamName);
    }

    [Theory]
    [InlineData(@"<seealso cref="""">description</seealso>", "Value of attribute 'cref' (at 1:2) is empty or whitespace")]
    [InlineData(@"<seealso cref="" "">description</seealso>", "Value of attribute 'cref' (at 1:2) is empty or whitespace")]
    public void FromXml_fails_if_cref_attribute_has_empty_vale(string xml, string expectedErrorMessage)
    {
        // ARRANGE

        // ACT 
        var ex = Record.Exception(() => SeeAlsoElement.FromXml(xml));

        // ASSERT
        Assert.IsType<XmlDocsException>(ex);
        Assert.Equal(expectedErrorMessage, ex.Message);
    }

    [Fact]
    public void FromXml_fails_if_cref_cannot_be_parsed()
    {
        // ARRANGE
        var input = """
            <seealso cref="not-a-member-id" />
            """;

        // ACT 
        var ex = Record.Exception(() => SeeAlsoElement.FromXml(input));

        // ASSERT
        Assert.IsType<XmlDocsException>(ex);
        Assert.Equal("Failed to parse code reference in <seealso /> element. Invalid reference 'not-a-member-id' (at 1:2)", ex.Message);
    }
}
