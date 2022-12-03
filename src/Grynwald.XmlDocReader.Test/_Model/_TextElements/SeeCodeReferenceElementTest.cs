namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="SeeCodeReferenceElement"/>
/// </summary>
public class SeeCodeReferenceElementTest
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Reference_must_not_be_null_or_whitespace(string reference)
    {
        // ARRANGE

        // ACT
        var ex = Record.Exception(() => new SeeCodeReferenceElement(reference: reference, null));

        // ASSERT
        var argumentException = Assert.IsType<ArgumentException>(ex);
        Assert.Equal("reference", argumentException.ParamName);
    }

    [Fact]
    public void Two_instances_are_equal_if_reference_and_text_are_equal_01()
    {
        // ARRANGE
        var instance1 = new SeeCodeReferenceElement("T:MyClass", null);
        var instance2 = new SeeCodeReferenceElement("T:MyClass", null);

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
        var instance1 = new SeeCodeReferenceElement("T:MyClass", new TextBlock(new PlainTextElement("Text")));
        var instance2 = new SeeCodeReferenceElement("T:MyClass", new TextBlock(new PlainTextElement("Text")));

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
        var instance1 = new SeeCodeReferenceElement("T:MyClass1", null);
        var instance2 = new SeeCodeReferenceElement("T:MyClass2", null);

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
        var instance1 = new SeeCodeReferenceElement("T:MyClass", new TextBlock(new PlainTextElement("Some Text")));
        var instance2 = new SeeCodeReferenceElement("T:MyClass", new TextBlock(new PlainTextElement("Some other Text")));

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
        var sut = new SeeCodeReferenceElement("T:MyClass", null);

        // ACT / ASSERT
        Assert.False(sut.Equals((object?)null));
        Assert.False(sut!.Equals((SeeCodeReferenceElement?)null));
    }
}
