﻿using Grynwald.XmlDocs.Internal;

namespace Grynwald.XmlDocs.Test;

/// <summary>
/// Tests for <see cref="EventMemberElement"/>-
/// </summary>
public class EventMemberElementTest : CommonMemberElementTests
{
    protected override MemberElement CreateInstanceFromXml(string xml) =>
        EventMemberElement.FromXml(MemberId.Parse("E:MyNamespace.MyClass.Event"), XmlContentHelper.ParseXmlElement(xml));

    [Fact]
    public void FromXml_can_read_empty_element()
    {
        // ARRANGE
        var input = """
                <member name="E:Project.Class.Event">
                </member>
                """;

        // ACT 
        var sut = MemberElement.FromXml(input);

        // ASSERT
        var eventDescription = Assert.IsType<EventMemberElement>(sut);

        Assert.Equal(MemberId.Parse("E:Project.Class.Event"), sut.Id);

        Assert.Null(sut.Summary);

        Assert.Null(sut.Remarks);

        Assert.Null(sut.Example);

        Assert.NotNull(sut.SeeAlso);
        Assert.Empty(sut.SeeAlso);

        Assert.NotNull(eventDescription.Exceptions);
        Assert.Empty(eventDescription.Exceptions);
    }

    [Fact]
    public void FromXml_reads_exception_elements()
    {
        // ARRANGE
        var input = """
                <member name="E:Project.Class.Event">
                    <exception cref="T:Exception1">description</exception>
                    <exception cref="T:Exception2">description</exception>
                    <exception cref="T:Exception3">description</exception>
                </member>
                """;

        // ACT 
        var sut = MemberElement.FromXml(input);

        // ASSERT
        var eventDescription = Assert.IsType<EventMemberElement>(sut);

        Assert.NotNull(eventDescription.Exceptions);
        Assert.Collection(
            eventDescription.Exceptions,
            x => Assert.Equal(MemberId.Parse("T:Exception1"), x.Reference),
            x => Assert.Equal(MemberId.Parse("T:Exception2"), x.Reference),
            x => Assert.Equal(MemberId.Parse("T:Exception3"), x.Reference)
        );
    }
}
