using System.Xml.Linq;

namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="TypeParameter"/>
/// </summary>
public class TypeParameterTest
{
    [Theory]
    [InlineData("T01", @"<typeparam>description</typeparam>")]
    [InlineData("T02", @"<typeparam name="""">description</typeparam>")]
    [InlineData("T02", @"<typeparam name="" "">description</typeparam>")]
    public void Fails_on_missing_name(string id, string input)
    {
        // ARRANGE
        _ = id;
        var xml = XElement.Parse(input);

        // ACT 
        var ex = Record.Exception(() => TypeParameter.FromXml(xml));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
    }
}
