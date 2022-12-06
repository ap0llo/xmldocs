using Grynwald.XmlDocReader.Internal;

namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="MethodDescription"/>
/// </summary>
public class MethodDescriptionTest : MemberDescriptionCommonTest
{
    protected override MemberDescription CreateInstanceFromXml(string xml) =>
        MethodDescription.FromXml(MemberId.Parse("M:MyNamespace.MyClass.Method"), XmlContentHelper.ParseXmlElement(xml));


    [Fact]
    public void FromXml_can_read_empty_element()
    {
        // ARRANGE
        var input = """
                <member name="M:Project.Class.Method">
                </member>
                """;

        // ACT
        var sut = MemberDescription.FromXml(input);

        // ASSERT
        var methodDescription = Assert.IsType<MethodDescription>(sut);

        Assert.Equal(MemberId.Parse("M:Project.Class.Method"), sut.Id);

        Assert.Null(sut.Summary);

        Assert.Null(sut.Remarks);

        Assert.Null(methodDescription.Returns);

        Assert.Null(sut.Example);

        Assert.NotNull(methodDescription.Parameters);
        Assert.Empty(methodDescription.Parameters);

        Assert.NotNull(methodDescription.TypeParameters);
        Assert.Empty(methodDescription.TypeParameters);

        Assert.NotNull(sut.SeeAlso);
        Assert.Empty(sut.SeeAlso);

        Assert.NotNull(methodDescription.Exceptions);
        Assert.Empty(methodDescription.Exceptions);
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
        var methodDescription = Assert.IsType<MethodDescription>(sut);

        Assert.NotNull(methodDescription.Returns);
        Assert.Equal(new TextBlock(new PlainTextElement("Returns some value")), methodDescription.Returns);
    }

    [Fact]
    public void FromXml_reads_param_elements()
    {
        // ARRANGE
        var input = """
                <member name="M:Project.Class.Method(System.String,System.String)">
                    <param name="param1">Description of parameter 1.</param>
                    <param name="param2">Description of parameter 1.</param>
                </member>
                """;

        // ACT 
        var sut = MemberDescription.FromXml(input);

        // ASSERT
        var methodDescription = Assert.IsType<MethodDescription>(sut);

        Assert.NotNull(methodDescription.Parameters);
        Assert.Collection(
            methodDescription.Parameters,
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
        var methodDescription = Assert.IsType<MethodDescription>(sut);

        Assert.NotNull(methodDescription.TypeParameters);
        Assert.Collection(
            methodDescription.TypeParameters,
            x => Assert.Equal("TKey", x.Name),
            x => Assert.Equal("TValue", x.Name)
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
        var methodDescription = Assert.IsType<MethodDescription>(sut);

        Assert.NotNull(methodDescription.Exceptions);
        Assert.Collection(
            methodDescription.Exceptions,
            x => Assert.Equal(MemberId.Parse("T:Exception1"), x.Reference),
            x => Assert.Equal(MemberId.Parse("T:Exception2"), x.Reference),
            x => Assert.Equal(MemberId.Parse("T:Exception3"), x.Reference)
        );
    }
}
