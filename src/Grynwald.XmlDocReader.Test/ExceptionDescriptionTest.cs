using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="ExceptionDescription"/>
/// </summary>
public class ExceptionDescriptionTest
{
    [Theory]
    [InlineData("T01", @"<exception>description</exception>")]
    [InlineData("T02", @"<exception cref="""">description</exception>")]
    [InlineData("T02", @"<exception cref="" "">description</exception>")]
    public void Fails_on_missing_cref(string id, string input)
    {
        // ARRANGE
        _ = id;

        var xml = XElement.Parse(input);

        // ACT 
        var ex = Record.Exception(() => ExceptionDescription.FromXml(xml));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
    }

}
