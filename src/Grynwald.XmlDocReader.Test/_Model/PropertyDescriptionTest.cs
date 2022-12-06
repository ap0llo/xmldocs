using Grynwald.XmlDocReader.Internal;

namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="PropertyDescription"/>
/// </summary>
public class PropertyDescriptionTest : MemberDescriptionCommonTest
{
    protected override MemberDescription CreateInstanceFromXml(string xml) =>
        PropertyDescription.FromXml(MemberId.Parse("P:MyNamespace.MyClass.Property"), XmlContentHelper.ParseXmlElement(xml));


    [Fact]
    public void FromXml_can_read_empty_element()
    {
        // ARRANGE
        var input = """
                <member name="P:Project.Class.Property">
                </member>
                """;

        // ACT 
        var sut = MemberDescription.FromXml(input);

        // ASSERT
        var propertyDescription = Assert.IsType<PropertyDescription>(sut);

        Assert.Equal(MemberId.Parse("P:Project.Class.Property"), sut.Id);

        Assert.Null(sut.Summary);

        Assert.Null(sut.Remarks);

        Assert.Null(propertyDescription.Value);

        Assert.Null(sut.Example);

        Assert.NotNull(propertyDescription.Parameters);
        Assert.Empty(propertyDescription.Parameters);

        Assert.NotNull(sut.SeeAlso);
        Assert.Empty(sut.SeeAlso);

        Assert.NotNull(propertyDescription.Exceptions);
        Assert.Empty(propertyDescription.Exceptions);
    }


    [Fact]
    public void FromXml_reads_value_element()
    {
        // ARRANGE
        var input = """
                <member name="P:Project.Class.Property">
                    <value>
                    The value means something
                    </value>
                </member>
            """;

        // ACT 
        var sut = MemberDescription.FromXml(input);

        // ASSERT
        var propertyDescription = Assert.IsType<PropertyDescription>(sut);
        Assert.NotNull(propertyDescription.Value);
        Assert.Equal(new TextBlock(new PlainTextElement("The value means something")), propertyDescription.Value);
    }

    [Fact]
    public void FromXml_reads_param_elements()
    {
        // ARRANGE
        var input = """
                <member name="P:Project.Class.Property[System.String,System.String]">
                    <param name="param1">Description of parameter 1.</param>
                    <param name="param2">Description of parameter 1.</param>
                </member>
                """;

        // ACT 
        var sut = MemberDescription.FromXml(input);

        // ASSERT
        var propertyDescription = Assert.IsType<PropertyDescription>(sut);

        Assert.NotNull(propertyDescription.Parameters);
        Assert.Collection(
            propertyDescription.Parameters,
            x => Assert.Equal("param1", x.Name),
            x => Assert.Equal("param2", x.Name)
        );
    }

    [Fact]
    public void FromXml_reads_exception_elements()
    {
        // ARRANGE
        var input = """
                <member name="P:Project.Class.Property">
                    <exception cref="T:Exception1">description</exception>
                    <exception cref="T:Exception2">description</exception>
                    <exception cref="T:Exception3">description</exception>
                </member>
                """;

        // ACT 
        var sut = MemberDescription.FromXml(input);

        // ASSERT
        var propertyDescription = Assert.IsType<PropertyDescription>(sut);

        Assert.NotNull(propertyDescription.Exceptions);
        Assert.Collection(
            propertyDescription.Exceptions,
            x => Assert.Equal(MemberId.Parse("T:Exception1"), x.Reference),
            x => Assert.Equal(MemberId.Parse("T:Exception2"), x.Reference),
            x => Assert.Equal(MemberId.Parse("T:Exception3"), x.Reference)
        );
    }

}
