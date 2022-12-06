namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="CodeElement"/>
/// </summary>
public class CodeElementTest
{
    [Fact]
    public void Content_must_not_be_null()
    {
        // ARRANGE

        // ACT 
        var ex = Record.Exception(() => new CodeElement(content: null!, language: null));

        // ASSERT
        var argumentNullException = Assert.IsType<ArgumentNullException>(ex);
        Assert.Equal("content", argumentNullException.ParamName);
    }

    [Fact]
    public void FromXml_fails_on_invalid_xml()
    {
        // ARRANGE
        var input = "not xml";

        // ACT 
        var ex = Record.Exception(() => CodeElement.FromXml(input));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
        Assert.IsType<XmlException>(ex.InnerException);
    }

    [Fact]
    public void FromXml_fails_on_invalid_root_element_name()
    {
        // ARRANGE
        var xml = """
                <not-c />
                """;

        // ACT
        var ex = Record.Exception(() => CodeElement.FromXml(xml));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
    }

    [Theory]
    [InlineData(null, null, null)]
    [InlineData("lang", null, "lang")]
    [InlineData(null, "lang", "lang")]
    [InlineData("lang1", "lang2", "lang1")]
    public void FromXml_reads_language(string? languageAttribute, string? langAttribute, string? expectedLanguage)
    {
        // ARRANGE
        var xml = XElement.Parse("""
                <code>
                Some code block
                </code>
                """);

        xml.SetAttributeValue("language", languageAttribute);
        xml.SetAttributeValue("lang", langAttribute);

        // ACT 
        var sut = CodeElement.FromXml(xml);

        // ASSERT
        Assert.Equal(expectedLanguage, sut.Language);
    }

    [Fact]
    public void Two_CodeElement_instances_are_equal_if_their_content_is_equal_01()
    {
        // ARRANGE
        var instance1 = new CodeElement("some content", null);
        var instance2 = new CodeElement("some content", null);

        // ACT / ASSERT
        Assert.Equal(instance1.GetHashCode(), instance2.GetHashCode());

        Assert.True(instance1.Equals((object)instance1));
        Assert.True(instance1.Equals(instance1));

        Assert.True(instance1.Equals((object)instance2));
        Assert.True(instance2.Equals((object)instance1));

        Assert.True(instance1.Equals(instance2));
        Assert.True(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_CodeElement_instances_are_equal_if_their_content_is_equal_02()
    {
        // ARRANGE
        var instance1 = new CodeElement("some content", "language");
        var instance2 = new CodeElement("some content", "language");

        // ACT / ASSERT
        Assert.Equal(instance1.GetHashCode(), instance2.GetHashCode());

        Assert.True(instance1.Equals((object)instance2));
        Assert.True(instance2.Equals((object)instance1));

        Assert.True(instance1.Equals(instance2));
        Assert.True(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_CodeElement_instances_are_not_equal_if_their_content_is_not_equal()
    {
        // ARRANGE
        var instance1 = new CodeElement("Code 1", "language1");
        var instance2 = new CodeElement("Code 1", "language2");
        var instance3 = new CodeElement("Code 2", "language1");
        var instance4 = new CodeElement("Code 2", "language2");

        // ACT / ASSERT
        Assert.False(instance1.Equals((object)instance2));
        Assert.False(instance2.Equals((object)instance1));

        Assert.False(instance1.Equals((object)instance3));
        Assert.False(instance3.Equals((object)instance1));

        Assert.False(instance1.Equals((object)instance4));
        Assert.False(instance4.Equals((object)instance1));

        Assert.False(instance2.Equals((object)instance3));
        Assert.False(instance3.Equals((object)instance2));

        Assert.False(instance2.Equals((object)instance4));
        Assert.False(instance4.Equals((object)instance2));

        Assert.False(instance3.Equals((object)instance4));
        Assert.False(instance4.Equals((object)instance3));


        Assert.False(instance1.Equals(instance2));
        Assert.False(instance2.Equals(instance1));

        Assert.False(instance1.Equals(instance3));
        Assert.False(instance3.Equals(instance1));

        Assert.False(instance1.Equals(instance4));
        Assert.False(instance4.Equals(instance1));

        Assert.False(instance2.Equals(instance3));
        Assert.False(instance3.Equals(instance2));

        Assert.False(instance2.Equals(instance4));
        Assert.False(instance4.Equals(instance2));

        Assert.False(instance3.Equals(instance4));
        Assert.False(instance4.Equals(instance3));
    }

    [Fact]
    public void Equals_returns_false_when_comparing_to_null()
    {
        // ARRANGE
        var sut = new CodeElement("code", "language");

        // ACT / ASSERT
        Assert.False(sut.Equals((object?)null));
        Assert.False(sut!.Equals(null));
    }
}
