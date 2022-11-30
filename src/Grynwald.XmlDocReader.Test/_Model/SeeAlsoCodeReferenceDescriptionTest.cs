namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="SeeAlsoCodeReferenceDescription"/>
/// </summary>
public class SeeAlsoCodeReferenceDescriptionTest
{
    [Fact]
    public void Reference_must_not_be_null()
    {
        // ARRANGE

        // ACT 
        var ex = Record.Exception(() => new SeeAlsoCodeReferenceDescription(reference: null!, text: null));

        // ASSERT
        var argumentNullException = Assert.IsType<ArgumentNullException>(ex);
        Assert.Equal("reference", argumentNullException.ParamName);
    }
}
