using Grynwald.XmlDocReader.Internal;
using Xunit;

namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="TextBlock"/>
/// </summary>
public class TextBlockTest
{
    [Fact]
    public void FromXml_fails_on_invalid_xml()
    {
        // ARRANGE
        var input = "not xml";

        // ACT 
        var ex = Record.Exception(() => TextBlock.FromXml(input));

        // ASSERT
        Assert.IsType<XmlDocReaderException>(ex);
        Assert.IsType<XmlException>(ex.InnerException);
    }

    public static IEnumerable<object[]> TestCases()
    {
        object[] TestCase(string id, string input, TextBlock expected)
        {
            return new object[] { id, input, expected };
        }

        yield return TestCase(
            "T01",
            "<summary></summary>",
            new TextBlock()
        );

        yield return TestCase(
            "T02",
            "<summary>Just some plain text</summary>",
            new TextBlock(
                new PlainTextElement("Just some plain text")
            )
        );

        yield return TestCase(
            "T03",
            """
                    <summary>
                    Just some plain text
                    </summary>
            """,
            new TextBlock(
                new PlainTextElement("Just some plain text")
            )
        );

        yield return TestCase(
            "T04",
            """
            <summary>
            Just some plain text
            </summary>
            """,
            new TextBlock(
                new PlainTextElement("Just some plain text")
            )
        );

        yield return TestCase(
            "T05",
            """
                    <summary>
                    Just some plain text
                    spread over multiple lines
                    </summary>
            """,
            new TextBlock(
                new PlainTextElement("Just some plain text spread over multiple lines")
            )
        );

        yield return TestCase(
            "T06",
            """
                    <summary>
                    Just some plain text
                    spread over multiple lines.
                    <para>
                        There's also a second paragraph
                    </para>
                    </summary>
            """,
            new TextBlock(
                new PlainTextElement("Just some plain text spread over multiple lines."),
                new ParagraphElement(
                    new TextBlock(
                        new PlainTextElement("There's also a second paragraph")
                ))
            )
        );

        yield return TestCase(
            "T08",
            """
            <summary><paramref name="parameter" /></summary>
            """,
            new TextBlock(
                new ParamRefElement("parameter")
            )
        );

        yield return TestCase(
            "T09",
            """
            <summary>
            <paramref name="parameter" />
            </summary>
            """,
            new TextBlock(
                new ParamRefElement("parameter")
            )
        );

        yield return TestCase(
            "T10",
            """
                    <summary>
                        <paramref name="parameter" />
                    </summary>
            """,
            new TextBlock(
                new ParamRefElement("parameter")
            )
        );

        yield return TestCase(
            "T10",
            """
                    <summary>
                        <typeparamref name="TKey" />
                    </summary>
            """,
            new TextBlock(
                new TypeParamRefElement("TKey")
            )
        );

        yield return TestCase(
            "T11",
            """
                <summary>
                    <code language="some-language">
                        Some source code
                            with some indentation
                    </code>
                </summary>
            """,
            new TextBlock(
                new CodeElement("""
                    Some source code
                        with some indentation
                    """,
                    "some-language")
            )
        );

        yield return TestCase(
            "T12",
            """
                <summary>
                    <c>inline code</c>
                </summary>
            """,
            new TextBlock(
              new CElement("inline code")
            )
        );

        yield return TestCase(
            "T13",
            """
                <summary>
                    See also <see cref="T:SomeType" /> 
                </summary>
            """,
            new TextBlock(
              new PlainTextElement("See also "),
              new SeeCRefElement("T:SomeType")
            )
        );
        yield return TestCase(
            "T14",
            """
                <summary>
                    See also <see href="https://example.com" /> 
                </summary>
            """,
            new TextBlock(
              new PlainTextElement("See also "),
              new SeeHRefElement("https://example.com")
            )
        );

        yield return TestCase(
            "T15",
            """
                <summary>
                    <list type="bullet">
                        <item><description>Item 1</description></item>
                    </list>
                </summary>
            """,
            new TextBlock(
              new ListElement(ListType.Bullet, null, new[]
              {
                  new ListItemElement(null, new TextBlock(new PlainTextElement("Item 1")))
              })
            )
        );

        // Unknown elements are ignored
        yield return TestCase(
            "T16",
            """
                <summary>
                    <unknown-element></unknown-element>
                </summary>
            """,
            new TextBlock()
        );
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void FromXml_returns_expected_elements(string id, string input, TextBlock expected)
    {
        // ARRANGE
        _ = id;

        var xml = XElement.Parse(input, LoadOptions.PreserveWhitespace);

        // ACT 
        var actual = TextBlock.FromXml(xml);

        // ASSERT
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FromXmlOrNullIfEmpty_returns_TextBlock_if_element_is_not_empty_01()
    {
        // ARRANGE

        // ACT 
        var textBlock = TextBlock.FromXmlOrNullIfEmpty(new XElement("parent", "Some Text"));

        // ASSERT
        Assert.NotNull(textBlock);
        Assert.Equal(new TextBlock(new PlainTextElement("Some Text")), textBlock);
    }

    [Fact]
    public void FromXmlOrNullIfEmpty_returns_TextBlock_if_element_is_not_empty_02()
    {
        // ARRANGE

        // ACT 
        var textBlock = TextBlock.FromXmlOrNullIfEmpty(XmlContentHelper.ParseXmlElement(
            """
            <summary>

            </summary>
            """
        ));

        // ASSERT
        Assert.NotNull(textBlock);
        Assert.Equal(new TextBlock(), textBlock);
    }

    [Fact]
    public void FromXmlOrNullIfEmpty_returns_null_if_element_is_empty()
    {
        // ARRANGE

        // ACT 
        var textBlock = TextBlock.FromXmlOrNullIfEmpty(new XElement("parent"));

        // ASSERT
        Assert.Null(textBlock);
    }

    [Fact]
    public void Two_TextBlock_instances_are_equal_if_all_their_elements_are_equal_01()
    {
        // ARRANGE
        var instance1 = new TextBlock();
        var instance2 = new TextBlock();

        // ACT / ASSERT
        Assert.Equal(instance1.GetHashCode(), instance2.GetHashCode());

        Assert.True(instance1.Equals((object)instance2));
        Assert.True(instance2.Equals((object)instance1));

        Assert.True(instance1.Equals(instance2));
        Assert.True(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_TextBlock_instances_are_equal_if_all_their_elements_are_equal_02()
    {
        // ARRANGE
        var instance1 = new TextBlock(
            new PlainTextElement("Some text"),
            new CElement("inline code")
        );

        var instance2 = new TextBlock(
            new PlainTextElement("Some text"),
            new CElement("inline code")
        );

        // ACT / ASSERT
        Assert.Equal(instance1.GetHashCode(), instance2.GetHashCode());

        Assert.True(instance1.Equals((object)instance2));
        Assert.True(instance2.Equals((object)instance1));

        Assert.True(instance1.Equals(instance2));
        Assert.True(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_TextBlock_instances_are_not_equal_if_their_elements_are_different_01()
    {
        // ARRANGE
        var instance1 = new TextBlock(
            new PlainTextElement("Some text"),
            new CElement("inline code 1")
        );

        var instance2 = new TextBlock(
            new PlainTextElement("Some text"),
            new CElement("inline code 2")
        );

        // ACT / ASSERT
        Assert.False(instance1.Equals((object)instance2));
        Assert.False(instance2.Equals((object)instance1));

        Assert.False(instance1.Equals(instance2));
        Assert.False(instance2.Equals(instance1));
    }

    [Fact]
    public void Two_TextBlock_instances_are_not_equal_if_their_elements_are_different_02()
    {
        // ARRANGE
        var instance1 = new TextBlock(
            new PlainTextElement("Some text")
        );

        var instance2 = new TextBlock(
            new PlainTextElement("Some text"),
            new CElement("inline code")
        );

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
        var sut = new TextBlock();

        // ACT / ASSERT
        Assert.False(sut.Equals((object?)null));
        Assert.False(sut!.Equals((TextBlock?)null));
    }
}
