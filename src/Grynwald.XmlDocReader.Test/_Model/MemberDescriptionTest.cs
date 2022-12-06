﻿namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="MemberDescription"/>
/// </summary>
public class MemberDescriptionTest
{
    [Fact]
    public void Id_must_not_be_null()
    {
        // ARRANGE

        // ACT 
        var ex = Record.Exception(() => new NamespaceDescription(null!));

        // ASSERT
        var argumentNullException = Assert.IsType<ArgumentNullException>(ex);
        Assert.Equal("id", argumentNullException.ParamName);
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

    [Theory]
    [InlineData("N:MyNamespace", typeof(NamespaceDescription))]
    [InlineData("T:MyNamespace.MyClass", typeof(TypeDescription))]
    [InlineData("F:MyNamespace.MyClass.FieldName", typeof(FieldDescription))]
    [InlineData("P:MyNamespace.MyClass.PropertyName", typeof(PropertyDescription))]
    [InlineData("M:MyNamespace.MyClass.Method(System.String)", typeof(MethodDescription))]
    [InlineData("E:MyNamespace.MyClass.Event", typeof(EventDescription))]
    public void FromXml_returns_expected_type(string id, Type expectedType)
    {
        // ARRANGE
        var input = $"""
                <member name="{id}">
                </member>
                """;
        // ACT 
        var member = MemberDescription.FromXml(input);

        // ASSERT
        Assert.IsType(expectedType, member);
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

    [Fact]
    public void FromXml_fails_if_name_cannot_be_parsed()
    {
        // ARRANGE
        var input = """
            <member name="not-a-member-id">
            </member>
            """;

        // ACT 
        var ex = Record.Exception(() => MemberDescription.FromXml(input));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
        Assert.Equal("Failed to parse member. Invalid member name 'not-a-member-id' (at 1:2)", ex.Message);
    }
}
