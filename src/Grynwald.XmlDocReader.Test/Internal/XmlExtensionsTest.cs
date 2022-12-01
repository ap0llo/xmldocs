using Grynwald.XmlDocReader.Internal;

namespace Grynwald.XmlDocReader.Test.Internal;

/// <summary>
/// Tests for <see cref="XmlExtensions"/>
/// </summary>
public class XmlExtensionsTest
{
    [Theory]
    [InlineData("<node />", LoadOptions.None, "")]
    [InlineData(""""
            <node>
                <innerNode />
            </node>
            """",
        LoadOptions.SetLineInfo | LoadOptions.PreserveWhitespace,
        " (at 1:2)")]
    public void GetPositionString_returns_expected_value(string xml, LoadOptions loadOptions, string expected)
    {
        // ARRANGE
        var node = XElement.Parse(xml, loadOptions);

        // ACT 
        var actual = node.GetPositionString();

        // ASSERT
        Assert.Equal(expected, actual);
    }
}
