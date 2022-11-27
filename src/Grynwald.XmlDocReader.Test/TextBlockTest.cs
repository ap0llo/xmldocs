using System.Xml.Linq;

namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests for <see cref="TextBlock"/>
/// </summary>
public class TextBlockTest
{

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
                new ParaElement(
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
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void FromXml_returns_expected_element(string id, string input, TextBlock expected)
    {
        // ARRANGE
        _ = id;

        var xml = XElement.Parse(input, LoadOptions.PreserveWhitespace);

        // ACT 
        var actual = TextBlock.FromXml(xml);

        // ASSERT
        Assert.Equal(expected, actual);
    }
}
