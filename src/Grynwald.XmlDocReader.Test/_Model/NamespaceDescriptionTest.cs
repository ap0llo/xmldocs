using Grynwald.XmlDocReader.Internal;

namespace Grynwald.XmlDocReader.Test;

public class NamespaceDescriptionTest : MemberDesciptionCommonTest
{
    protected override MemberDescription CreateInstanceFromXml(string xml) =>
        NamespaceDescription.FromXml(MemberId.Parse("N:MyNamespace"), XmlContentHelper.ParseXmlElement(xml));


    [Fact]
    public void FromXml_can_read_empty_element()
    {
        // ARRANGE
        var input = """
                <member name="N:Project">
                </member>
                """;

        // ACT 
        var sut = MemberDescription.FromXml(input);

        // ASSERT
        Assert.IsType<NamespaceDescription>(sut);

        Assert.Equal(MemberId.Parse("N:Project"), sut.Id);

        Assert.Null(sut.Summary);

        Assert.Null(sut.Remarks);

        Assert.Null(sut.Example);

        Assert.NotNull(sut.SeeAlso);
        Assert.Empty(sut.SeeAlso);
    }
}
