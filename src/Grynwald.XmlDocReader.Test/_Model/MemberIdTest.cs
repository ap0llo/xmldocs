namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="MemberId"/>
/// </summary>
public class MemberIdTest
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("M:")]
    [InlineData("I:MyClass.MyMember")]
    public void TryParse_returns_false_if_id_cannot_be_parsed(string id)
    {
        // ARRANGE

        // ACT
        var success = MemberId.TryParse(id, out var parsed);

        // ASSERT
        Assert.False(success);
        Assert.Null(parsed);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("M:")]
    [InlineData("I:MyClass.MyMember")]
    public void TryParse_failes_if_id_cannot_be_parsed(string id)
    {
        // ARRANGE

        // ACT
        var ex = Record.Exception(() => MemberId.Parse(id));

        // ASSERT
        Assert.IsType<FormatException>(ex);
        Assert.Equal($"'{id}' is not a valid member id", ex.Message);
    }


    [Theory]
    [InlineData("N:MyNamespace", MemberType.Namespace, "MyNamespace")]
    [InlineData("T:MyNamespace.MyClass", MemberType.Type, "MyNamespace.MyClass")]
    [InlineData("F:MyNamespace.MyClass.Field", MemberType.Field, "MyNamespace.MyClass.Field")]
    [InlineData("P:MyNamespace.MyClass.Property", MemberType.Property, "MyNamespace.MyClass.Property")]
    [InlineData("M:MyNamespace.MyClass.Method", MemberType.Method, "MyNamespace.MyClass.Method")]
    [InlineData("E:MyNamespace.MyClass.Event", MemberType.Event, "MyNamespace.MyClass.Event")]
    public void TryParse_succeeds_for_valid_member_id(string id, MemberType expectedType, string expectedName)
    {
        // ARRANGE

        // ACT
        var success = MemberId.TryParse(id, out var parsed);

        // ASSERT
        Assert.True(success);
        Assert.NotNull(parsed);

        Assert.Equal(id, parsed!.Id);
        Assert.Equal(expectedType, parsed.Type);
        Assert.Equal(expectedName, parsed.Name);
    }

    [Theory]
    [InlineData("N:MyNamespace", MemberType.Namespace, "MyNamespace")]
    [InlineData("T:MyNamespace.MyClass", MemberType.Type, "MyNamespace.MyClass")]
    [InlineData("F:MyNamespace.MyClass.Field", MemberType.Field, "MyNamespace.MyClass.Field")]
    [InlineData("P:MyNamespace.MyClass.Property", MemberType.Property, "MyNamespace.MyClass.Property")]
    [InlineData("M:MyNamespace.MyClass.Method", MemberType.Method, "MyNamespace.MyClass.Method")]
    [InlineData("E:MyNamespace.MyClass.Event", MemberType.Event, "MyNamespace.MyClass.Event")]
    public void Parse_succeeds_for_valid_member_id(string id, MemberType expectedType, string expectedName)
    {
        // ARRANGE

        // ACT
        var parsed = MemberId.Parse(id);

        // ASSERT
        Assert.NotNull(parsed);
        Assert.Equal(id, parsed!.Id);
        Assert.Equal(expectedType, parsed.Type);
        Assert.Equal(expectedName, parsed.Name);
    }


    [Theory]
    [InlineData("N:MyNamespace")]
    [InlineData("T:MyNamespace.MyClass")]
    [InlineData("F:MyNamespace.MyClass.Field")]
    [InlineData("P:MyNamespace.MyClass.Property")]
    [InlineData("M:MyNamespace.MyClass.Method")]
    [InlineData("E:MyNamespace.MyClass.Event")]
    public void ToString_returns_original_id_string(string id)
    {
        // ARRANGE

        // ACT
        var success = MemberId.TryParse(id, out var parsed);

        // ASSERT
        Assert.True(success);
        Assert.NotNull(parsed);

        Assert.Equal(id, parsed!.ToString());
    }

    [Fact]
    public void Two_instances_are_equal_if_their_id_is_equal()
    {
        // ARRANGE
        var instance1 = MemberId.Parse("T:MyNamespace.MyClass");
        var instance2 = MemberId.Parse("T:MyNamespace.MyClass");

        // ACT / ASSERT
        Assert.Equal(instance1.GetHashCode(), instance2.GetHashCode());

        Assert.True(instance1.Equals((object)instance2));
        Assert.True(instance2.Equals((object)instance1));

        Assert.True(instance1.Equals(instance2));
        Assert.True(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_instances_are_not_equal_if_link_or_text_are_not_equal_01()
    {
        // ARRANGE
        var instance1 = MemberId.Parse("T:MyNamespace.Class1");
        var instance2 = MemberId.Parse("T:MyNamespace.Class2");

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
        var sut = MemberId.Parse("T:MyNamespace.MyClass");

        // ACT / ASSERT
        Assert.False(sut.Equals((object?)null));
        Assert.False(sut!.Equals((MemberId?)null));
    }
}
