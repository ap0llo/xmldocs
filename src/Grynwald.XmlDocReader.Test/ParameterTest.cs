using System.Xml.Linq;

namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="Parameter"/>
/// </summary>
public class ParameterTest
{
    [Theory]
    [InlineData("T01", @"<param>description</param>")]
    [InlineData("T02", @"<param name="""">description</param>")]
    [InlineData("T02", @"<param name="" "">description</param>")]
    public void Fails_on_missing_name(string id, string input)
    {
        // ARRANGE
        _ = id;
        var xml = XElement.Parse(input);

        // ACT 
        var ex = Record.Exception(() => Parameter.FromXml(xml));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
    }
}
