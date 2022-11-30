﻿namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="MemberDescription"/>
/// </summary>
public class MemberDescriptionTest
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Name_must_not_be_null_or_whitespace(string name)
    {
        // ARRANGE

        // ACT 
        var ex = Record.Exception(() => new MemberDescription(name));

        // ASSERT
        var argumentException = Assert.IsType<ArgumentException>(ex);
        Assert.Equal("name", argumentException.ParamName);
    }

    [Fact]
    public void FromXml_fails_on_invalid_xml()
    {
        // ARRANGE
        var input = "not xml";

        // ACT 
        var ex = Record.Exception(() => MemberDescription.FromXml(input));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
        Assert.IsType<XmlException>(ex.InnerException);
    }

    [Fact]
    public void FromXml_can_read_empty_element()
    {
        // ARRANGE
        var input = """
                <member name="T:Project.Class">
                </member>
                """;

        // ACT 
        var sut = MemberDescription.FromXml(input);

        // ASSERT
        Assert.Equal("T:Project.Class", sut.Name);

        Assert.Null(sut.Summary);

        Assert.Null(sut.Remarks);

        Assert.Null(sut.Returns);

        Assert.Null(sut.Value);

        Assert.Null(sut.Example);

        Assert.NotNull(sut.Parameters);
        Assert.Empty(sut.Parameters);

        Assert.NotNull(sut.TypeParameters);
        Assert.Empty(sut.TypeParameters);

        Assert.NotNull(sut.SeeAlso);
        Assert.Empty(sut.SeeAlso);

        Assert.NotNull(sut.Exceptions);
        Assert.Empty(sut.Exceptions);
    }

    [Fact]
    public void FromXml_reads_summary_element()
    {
        // ARRANGE
        var input = """
                <member name="T:Project.Class">
                    <summary>
                    Example summary
                    </summary>
                </member>
            """;

        // ACT 
        var sut = MemberDescription.FromXml(input);

        // ASSERT            
        Assert.NotNull(sut.Summary);
        Assert.Equal(new TextBlock(new PlainTextElement("Example summary")), sut.Summary);
    }

    [Fact]
    public void FromXml_reads_remarks_element()
    {
        // ARRANGE
        var input = """
                <member name="T:Project.Class">
                    <summary>
                    Example summary
                    </summary>
                    <remarks>
                    Example remarks
                    </remarks>
                </member>
            """;

        // ACT 
        var sut = MemberDescription.FromXml(input);

        // ASSERT            
        Assert.NotNull(sut.Remarks);
        Assert.Equal(new TextBlock(new PlainTextElement("Example remarks")), sut.Remarks);
    }

    [Fact]
    public void FromXml_reads_returns_element()
    {
        // ARRANGE
        var input = """
                <member name="M:Project.Class.Method">
                    <returns>
                    Returns some value
                    </returns>
                </member>
            """;

        // ACT 
        var sut = MemberDescription.FromXml(input);

        // ASSERT            
        Assert.NotNull(sut.Returns);
        Assert.Equal(new TextBlock(new PlainTextElement("Returns some value")), sut.Returns);
    }

    [Fact]
    public void FromXml_reads_value_element()
    {
        // ARRANGE
        var input = """
                <member name="M:Project.Class.Method">
                    <value>
                    The value means something
                    </value>
                </member>
            """;

        // ACT 
        var sut = MemberDescription.FromXml(input);

        // ASSERT            
        Assert.NotNull(sut.Value);
        Assert.Equal(new TextBlock(new PlainTextElement("The value means something")), sut.Value);
    }

    [Fact]
    public void FromXml_reads_example_element()
    {
        // ARRANGE
        var input = """
                <member name="M:Project.Class.Method">
                    <example>
                    Some example
                    </example>
                </member>
            """;

        // ACT 
        var sut = MemberDescription.FromXml(input);

        // ASSERT            
        Assert.NotNull(sut.Example);
        Assert.Equal(new TextBlock(new PlainTextElement("Some example")), sut.Example);
    }

    [Fact]
    public void FromXml_reads_param_elements()
    {
        // ARRANGE
        var input = """
                <member name="M:Project.Class.Method">
                    <param name="param1">Description of parameter 1.</param>
                    <param name="param2">Description of parameter 1.</param>
                </member>
                """;

        // ACT 
        var sut = MemberDescription.FromXml(input);

        // ASSERT            
        Assert.NotNull(sut.Parameters);
        Assert.Collection(
            sut.Parameters,
            x => Assert.Equal("param1", x.Name),
            x => Assert.Equal("param2", x.Name)
        );
    }

    [Fact]
    public void FromXml_reads_typeparam_elements()
    {
        // ARRANGE
        var input = """
                <member name="M:Project.Class.Method">
                    <typeparam name="TKey">Description of type parameter</typeparam>
                    <typeparam name="TValue">Description of type parameter</typeparam>
                </member>
                """;

        // ACT 
        var sut = MemberDescription.FromXml(input);

        // ASSERT            
        Assert.NotNull(sut.TypeParameters);
        Assert.Collection(
            sut.TypeParameters,
            x => Assert.Equal("TKey", x.Name),
            x => Assert.Equal("TValue", x.Name)
        );
    }

    [Fact]
    public void FromXml_reads_seealso_elements()
    {
        // ARRANGE
        var input = """
                <member name="M:Project.Class.Method">
                    <seealso cref="member"/>
                    <seealso href="link">Link Text</seealso>
                </member>
                """;

        // ACT 
        var sut = MemberDescription.FromXml(input);

        // ASSERT            
        Assert.NotNull(sut.SeeAlso);
        Assert.Collection(
            sut.SeeAlso,
            x => Assert.IsType<SeeAlsoCodeReferenceDescription>(x),
            x => Assert.IsType<SeeAlsoUrlReferenceDescription>(x)
        );
    }

    [Fact]
    public void FromXml_reads_exception_elements()
    {
        // ARRANGE
        var input = """
                <member name="M:Project.Class.Method">
                    <exception cref="T:Exception1">description</exception>
                    <exception cref="T:Exception2">description</exception>
                    <exception cref="T:Exception3">description</exception>
                </member>
                """;

        // ACT 
        var sut = MemberDescription.FromXml(input);

        // ASSERT            
        Assert.NotNull(sut.Exceptions);
        Assert.Collection(
            sut.Exceptions,
            x => Assert.Equal("T:Exception1", x.Cref),
            x => Assert.Equal("T:Exception2", x.Cref),
            x => Assert.Equal("T:Exception3", x.Cref)
        );
    }

    [Fact]
    public void FromXml_fails_on_invald_root_element_name()
    {
        // ARRANGE
        var xml = """
                <not-member />
                """;

        // ACT
        var ex = Record.Exception(() => MemberDescription.FromXml(xml));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void FromXml_fails_on_missing_or_empty_name_attribute(string name)
    {
        // ARRANGE
        var xml = XElement.Parse("""
                <member />
                """);

        xml.SetAttributeValue("name", name);

        // ACT
        var ex = Record.Exception(() => MemberDescription.FromXml(xml));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
    }
}