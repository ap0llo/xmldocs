using Grynwald.XmlDocReader.Internal;

namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="NamespaceMemberElement"/>
/// </summary>
public class NamespaceMemberElementTest : CommonMemberElementTests
{
    protected override MemberElement CreateInstanceFromXml(string xml) =>
        NamespaceMemberElement.FromXml(MemberId.Parse("N:MyNamespace"), XmlContentHelper.ParseXmlElement(xml));


    [Fact]
    public void FromXml_can_read_empty_element()
    {
        // ARRANGE
        var input = """
                <member name="N:Project">
                </member>
                """;

        // ACT 
        var sut = MemberElement.FromXml(input);

        // ASSERT
        Assert.IsType<NamespaceMemberElement>(sut);

        Assert.Equal(MemberId.Parse("N:Project"), sut.Id);

        Assert.Null(sut.Summary);

        Assert.Null(sut.Remarks);

        Assert.Null(sut.Example);

        Assert.NotNull(sut.SeeAlso);
        Assert.Empty(sut.SeeAlso);
    }
}
