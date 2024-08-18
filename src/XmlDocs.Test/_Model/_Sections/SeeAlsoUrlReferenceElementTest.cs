namespace Grynwald.XmlDocs.Test;

/// <summary>
/// Tests for <see cref="SeeAlsoUrlReferenceElement"/>
/// </summary>
public class SeeAlsoUrlReferenceElementTest
{

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Link_must_not_be_null_or_whitespace(string? link)
    {
        // ARRANGE

        // ACT 
        var ex = Record.Exception(() => new SeeAlsoUrlReferenceElement(link: link!, text: null));

        // ASSERT
        var argumentException = Assert.IsType<ArgumentException>(ex);
        Assert.Equal("link", argumentException.ParamName);
    }
}
