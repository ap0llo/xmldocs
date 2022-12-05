using Grynwald.XmlDocReader.Internal;

namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="FieldDescription"/>
/// </summary>
public class FieldDescriptionTest : MemberDesciptionCommonTest
{
    protected override MemberDescription CreateInstanceFromXml(string xml) =>
        FieldDescription.FromXml(MemberId.Parse("F:MyNamespace.MyClass.Field"), XmlContentHelper.ParseXmlElement(xml));


    [Fact]
    public void FromXml_can_read_empty_element()
    {
        // ARRANGE
        var input = """
                <member name="F:Project.Class.Field">
                </member>
                """;

        // ACT 
        var sut = MemberDescription.FromXml(input);

        // ASSERT
        var fieldDescription = Assert.IsType<FieldDescription>(sut);
        Assert.Equal(MemberId.Parse("F:Project.Class.Field"), sut.Id);

        Assert.Null(sut.Summary);

        Assert.Null(sut.Remarks);

        Assert.Null(fieldDescription.Value);

        Assert.Null(sut.Example);

        Assert.NotNull(sut.SeeAlso);
        Assert.Empty(sut.SeeAlso);
    }

    [Fact]
    public void FromXml_reads_value_element()
    {
        // ARRANGE
        var input = """
                <member name="F:Project.Class.Field">
                    <value>
                    The value means something
                    </value>
                </member>
            """;

        // ACT 
        var sut = MemberDescription.FromXml(input);

        // ASSERT
        var fieldDescription = Assert.IsType<FieldDescription>(sut);
        Assert.NotNull(fieldDescription.Value);
        Assert.Equal(new TextBlock(new PlainTextElement("The value means something")), fieldDescription.Value);
    }
}
