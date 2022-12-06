﻿
using Grynwald.XmlDocReader.Internal;

namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="TypeDescription"/>
/// </summary>
public class TypeDescriptionTest : MemberDescriptionCommonTest
{
    protected override MemberDescription CreateInstanceFromXml(string xml) =>
        TypeDescription.FromXml(MemberId.Parse("T:MyNamespace.MyClass"), XmlContentHelper.ParseXmlElement(xml));


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
        var typeDescription = Assert.IsType<TypeDescription>(sut);

        Assert.Equal(MemberId.Parse("T:Project.Class"), sut.Id);

        Assert.Null(sut.Summary);

        Assert.Null(sut.Remarks);

        Assert.Null(sut.Example);

        Assert.NotNull(typeDescription.TypeParameters);
        Assert.Empty(typeDescription.TypeParameters);

        Assert.NotNull(sut.SeeAlso);
        Assert.Empty(sut.SeeAlso);
    }

    [Fact]
    public void FromXml_reads_typeparam_elements()
    {
        // ARRANGE
        var input = """
                <member name="T:Project.Class">
                    <typeparam name="TKey">Description of type parameter</typeparam>
                    <typeparam name="TValue">Description of type parameter</typeparam>
                </member>
                """;

        // ACT 
        var sut = MemberDescription.FromXml(input);

        // ASSERT
        var typeDescription = Assert.IsType<TypeDescription>(sut);

        Assert.NotNull(typeDescription.TypeParameters);
        Assert.Collection(
            typeDescription.TypeParameters,
            x => Assert.Equal("TKey", x.Name),
            x => Assert.Equal("TValue", x.Name)
        );
    }

}
