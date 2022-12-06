namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="SeeCodeReferenceElement"/>
/// </summary>
public class SeeCodeReferenceElementTest
{
    [Fact]
    public void Reference_must_not_be_null()
    {
        // ARRANGE

        // ACT
        var ex = Record.Exception(() => new SeeCodeReferenceElement(reference: null!, null));

        // ASSERT
        var argumentNullException = Assert.IsType<ArgumentNullException>(ex);
        Assert.Equal("reference", argumentNullException.ParamName);
    }

    [Theory]
    [InlineData(@"<see cref="""">description</see>", "Value of attribute 'cref' (at 1:2) is empty or whitespace")]
    [InlineData(@"<see cref="" "">description</see>", "Value of attribute 'cref' (at 1:2) is empty or whitespace")]
    public void FromXml_fails_if_cref_attribute_has_empty_vale(string xml, string expectedErrorMessage)
    {
        // ARRANGE

        // ACT 
        var ex = Record.Exception(() => SeeElement.FromXml(xml));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
        Assert.Equal(expectedErrorMessage, ex.Message);
    }

    [Fact]
    public void FromXml_fails_if_cref_cannot_be_parsed()
    {
        // ARRANGE
        var input = """
            <see cref="not-a-member-id" />
            """;

        // ACT 
        var ex = Record.Exception(() => SeeElement.FromXml(input));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
        Assert.Equal("Failed to parse code reference in <see /> element. Invalid reference 'not-a-member-id' (at 1:2)", ex.Message);
    }

    [Fact]
    public void Two_instances_are_equal_if_reference_and_text_are_equal_01()
    {
        // ARRANGE
        var instance1 = new SeeCodeReferenceElement(MemberId.Parse("T:MyClass"), null);
        var instance2 = new SeeCodeReferenceElement(MemberId.Parse("T:MyClass"), null);

        // ACT / ASSERT
        Assert.Equal(instance1.GetHashCode(), instance2.GetHashCode());

        Assert.True(instance1.Equals((object)instance2));
        Assert.True(instance2.Equals((object)instance1));

        Assert.True(instance1.Equals(instance2));
        Assert.True(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_instances_are_equal_if_reference_and_text_are_equal_02()
    {
        // ARRANGE
        var instance1 = new SeeCodeReferenceElement(MemberId.Parse("T:MyClass"), new TextBlock(new PlainTextElement("Text")));
        var instance2 = new SeeCodeReferenceElement(MemberId.Parse("T:MyClass"), new TextBlock(new PlainTextElement("Text")));

        // ACT / ASSERT
        Assert.Equal(instance1.GetHashCode(), instance2.GetHashCode());

        Assert.True(instance1.Equals((object)instance2));
        Assert.True(instance2.Equals((object)instance1));

        Assert.True(instance1.Equals(instance2));
        Assert.True(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_instances_are_not_equal_if_reference_or_text_are_not_equal_01()
    {
        // ARRANGE
        var instance1 = new SeeCodeReferenceElement(MemberId.Parse("T:MyClass1"), null);
        var instance2 = new SeeCodeReferenceElement(MemberId.Parse("T:MyClass2"), null);

        // ACT / ASSERT
        Assert.False(instance1.Equals((object)instance2));
        Assert.False(instance2.Equals((object)instance1));

        Assert.False(instance1.Equals(instance2));
        Assert.False(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_instances_are_not_equal_if_reference_or_text_are_not_equal_02()
    {
        // ARRANGE
        var instance1 = new SeeCodeReferenceElement(MemberId.Parse("T:MyClass"), new TextBlock(new PlainTextElement("Some Text")));
        var instance2 = new SeeCodeReferenceElement(MemberId.Parse("T:MyClass"), new TextBlock(new PlainTextElement("Some other Text")));

        // ACT / ASSERT
        Assert.False(instance1.Equals((object)instance2));
        Assert.False(instance2.Equals((object)instance1));

        Assert.False(instance1.Equals(instance2));
        Assert.False(instance2.Equals(instance1));
    }

    [Fact]
    public void Equals_returns_false_when_comparing_to_null()
    {
        // ARRANGE
        var sut = new SeeCodeReferenceElement(MemberId.Parse("T:MyClass"), null);

        // ACT / ASSERT
        Assert.False(sut.Equals((object?)null));
        Assert.False(sut!.Equals(null));
    }
}
