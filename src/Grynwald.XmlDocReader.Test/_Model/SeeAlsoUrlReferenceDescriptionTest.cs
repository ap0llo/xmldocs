namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="SeeAlsoUrlReferenceDescription"/>
/// </summary>
public class SeeAlsoUrlReferenceDescriptionTest
{
    [Fact]
    public void Link_must_not_be_null()
    {
        // ARRANGE

        // ACT 
        var ex = Record.Exception(() => new SeeAlsoUrlReferenceDescription(link: null!, text: null));

        // ASSERT
        var argumentNullException = Assert.IsType<ArgumentNullException>(ex);
        Assert.Equal("link", argumentNullException.ParamName);
    }
}
