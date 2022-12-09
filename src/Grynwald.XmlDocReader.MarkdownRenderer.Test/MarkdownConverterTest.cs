using System.Xml.Linq;

namespace Grynwald.XmlDocReader.MarkdownRenderer.Test;

/// <summary>
/// Tests for <see cref="MarkdownConverter"/>
/// </summary>
public class MarkdownConverterTest
{
    public class ConvertToBlock
    {
        private readonly ITestOutputHelper m_TestOutputHelper;

        public ConvertToBlock(ITestOutputHelper testOutputHelper)
        {
            m_TestOutputHelper = testOutputHelper;
        }

        public static IEnumerable<object[]> TestCases()
        {
            object[] TestCase(string id, DocumentationElement input, string expectedMarkdown) =>
                new object[] { id, input, expectedMarkdown };


            yield return TestCase(
                "T01",
                new DocumentationFile("MyAssembly", Array.Empty<MemberElement>()),
                $"# MyAssembly"
            );

            yield return TestCase(
                "T02",
                new DocumentationFile(
                    "MyAssembly",
                    new MemberElement[]
                    {
                        new NamespaceMemberElement(MemberId.Parse("N:MyNamespace")),
                        new TypeMemberElement(MemberId.Parse("T:MyNamespace.MyClass")),
                        new FieldMemberElement(MemberId.Parse("F:MyNamespace.MyClass.Field")),
                        new PropertyMemberElement(MemberId.Parse("P:MyNamespace.MyClass.Property")),
                        new MethodMemberElement(MemberId.Parse("M:MyNamespace.MyClass.Method")),
                        new EventMemberElement(MemberId.Parse("E:MyNamespace.MyClass.Event")),
                    }),
                """
                # MyAssembly
           
                ## MyNamespace Namespace
           
                ## MyNamespace.MyClass
           
                ## MyNamespace.MyClass.Field Field
           
                ## MyNamespace.MyClass.Property Property
           
                ## MyNamespace.MyClass.Method Method
           
                ## MyNamespace.MyClass.Event Event
                """
            );


            yield return TestCase(
                "T03",
                new FieldMemberElement(MemberId.Parse("F:MyNamespace.MyClass.Field")),
                "## MyNamespace.MyClass.Field Field"
            );

            yield return TestCase(
                "T04",
                MemberElement.FromXml("""
                <member name="M:MyClass.MyMethod">
                    <param name="someParameter" />                    
                    <param name="someOtherParameter">Some Description</param>
                </member>
                """),
                """
                ## MyClass.MyMethod Method

                ### Parameters

                `someParameter`

                `someOtherParameter`

                Some Description
                """
            );

            yield return TestCase(
                "T05",
                new ParameterElement("parameter1", null),
                "`parameter1`"
            );

            yield return TestCase(
                "T06",
                new ParameterElement(
                    "parameter1",
                    new TextBlock(new PlainTextElement("Description of parameter1"))
                ),
                """
                `parameter1`
  
                Description of parameter1
                """
            );

            yield return TestCase(
                "T07",
                new TypeParameterElement("typeParameter1", null),
                "`typeParameter1`"
            );

            yield return TestCase(
                "T08",
                new TypeParameterElement(
                    "typeParameter1",
                    new TextBlock(new PlainTextElement("Description of typeParameter1"))
                ),
                """
                `typeParameter1`
  
                Description of typeParameter1
                """
            );


            yield return TestCase(
                "T09",
                MemberElement.FromXml("""
                <member name="M:MyClass.MyMethod">
                    <param name="someParameter" />                    
                    <typeparam name="T1">Some Description</typeparam>
                </member>
                """),
                """
                ## MyClass.MyMethod Method

                ### Parameters

                `someParameter`

                ### Type Parameters

                `T1`

                Some Description
                """
            );


            yield return TestCase(
                "T10",
                new TextBlock(new PlainTextElement("Some Text")),
                "Some Text"
            );

            yield return TestCase(
                "T11",
                new ExceptionElement(MemberId.Parse("T:MyNamespace.MyException"), null),
                "`MyNamespace.MyException`"
            );

            yield return TestCase(
                "T12",
                new ExceptionElement(
                    MemberId.Parse("T:MyNamespace.MyException"),
                    new TextBlock(new PlainTextElement("Description of the exception"))
                ),
                """
                `MyNamespace.MyException`

                Description of the exception
                """
            );

            yield return TestCase(
                "T13",
                MemberElement.FromXml("""
                <member name="M:MyClass.MyMethod">
                    <exception cref="T:MyNamespace.MyException1" />
                    <exception cref="T:MyNamespace.MyException2">Thrown in some cases</exception>
                </member>
                """),
                """
                ## MyClass.MyMethod Method

                ### Exceptions

                `MyNamespace.MyException1`

                `MyNamespace.MyException2`

                Thrown in some cases
                """
            );

            yield return TestCase(
                "T14",
                new SeeAlsoUrlReferenceElement("http://example.com", null),
                "[http://example.com](http://example.com/)"
            );

            yield return TestCase(
                "T15",
                new SeeAlsoUrlReferenceElement("http://example.com", new TextBlock(new PlainTextElement("Link Text"))),
                "[Link Text](http://example.com/)"
            );

            yield return TestCase(
                "T16",
                MemberElement.FromXml("""
                <member name="M:MyClass.MyMethod">
                    <seealso href="http://example.com" />
                    <seealso href="http://example.com">Link Text</seealso>
                </member>
                """),
                """
                ## MyClass.MyMethod Method

                ### See Also

                [http://example.com](http://example.com/)  
                [Link Text](http://example.com/)
                """
            );

            yield return TestCase(
                "T17",
                new SeeAlsoCodeReferenceElement(MemberId.Parse("T:MyNamespace.MyClass"), null),
                "`MyNamespace.MyClass`"
            );

            yield return TestCase(
                "T18",
                new SeeAlsoCodeReferenceElement(MemberId.Parse("T:MyNamespace.MyClass"), new TextBlock(new PlainTextElement("Link Text"))),
                "Link Text"
            );

            yield return TestCase(
                "T19",
                MemberElement.FromXml("""
                <member name="M:MyClass.MyMethod">
                    <seealso cref="T:MyNamespace.MyClass" />
                    <seealso cref="T:MyNamespace.MyClass">Link Text</seealso>
                </member>
                """),
                """
                ## MyClass.MyMethod Method

                ### See Also

                `MyNamespace.MyClass`  
                Link Text
                """
            );


            yield return TestCase(
                "T20",
                CElement.FromXml(@"<c>Some code</c>"),
                "`Some code`"
            );

            yield return TestCase(
                "T21",
                MemberElement.FromXml("""
                <member name="M:MyClass.MyMethod">
                    <summary>
                        This is some text with <c>inline code</c>.
                    </summary>
                </member>
                """),
                """
                ## MyClass.MyMethod Method

                ### Summary

                This is some text with `inline code`.
                """
            );

            yield return TestCase(
                "T22",
                new CodeElement(
                    """
                    Some Code content
                    """,
                    language: null),
                """
                ```
                Some Code content
                ```
                """
            );

            yield return TestCase(
                "T23",
                new CodeElement(
                    """
                    Some Code content
                    """,
                    language: "lang"),
                """
                ```lang
                Some Code content
                ```
                """
            );

            yield return TestCase(
                "T24",
                MemberElement.FromXml("""
                <member name="M:MyClass.MyMethod">
                    <summary>
                        This is some text with a code block.
                        <code language="lang">
                            Some Code
                        </code>
                    </summary>
                </member>
                """),
                """
                ## MyClass.MyMethod Method

                ### Summary

                This is some text with a code block.

                ```lang
                Some Code
                ```
                """
            );

            yield return TestCase(
                "T25",
                new ParagraphElement(
                    new TextBlock(new PlainTextElement("Some Text"))
                ),
                """
                Some Text
                """
            );

            yield return TestCase(
                "T26",
                MemberElement.FromXml("""
                <member name="M:MyClass.MyMethod">
                    <summary>
                        This is the first paragraph.
                        <para>
                        This is the second paragraph.
                        </para>
                    </summary>
                </member>
                """),
                """
                ## MyClass.MyMethod Method

                ### Summary

                This is the first paragraph.

                This is the second paragraph.
                """
            );

            yield return TestCase(
                "T27",
                new ParameterReferenceElement("parameter"),
                """
                `parameter`
                """
            );

            yield return TestCase(
                "T26",
                MemberElement.FromXml("""
                <member name="M:MyClass.MyMethod">
                    <summary>
                        Text referencing <paramref name="parameter1" />.
                    </summary>
                </member>
                """),
                """
            ## MyClass.MyMethod Method

            ### Summary

            Text referencing `parameter1`.
            """
            );

            yield return TestCase(
                "T28",
                new TypeParameterReferenceElement("parameter"),
                """
                `parameter`
                """
            );

            yield return TestCase(
                "T29",
                new SeeCodeReferenceElement(MemberId.Parse("M:MyClass.Method"), null),
                """
                `MyClass.Method`
                """
            );

            yield return TestCase(
                "T30",
                new SeeCodeReferenceElement(MemberId.Parse("M:MyClass.Method"), new TextBlock(new PlainTextElement("Link Text"))),
                """
                Link Text
                """
            );

            yield return TestCase(
                "T31",
                MemberElement.FromXml("""
                <member name="M:MyClass.MyMethod">
                    <summary>
                        Text with a inline code reference to <see cref="T:MyClass">definition of MyClass</see>.
                    </summary>
                </member>
                """),
                """
                ## MyClass.MyMethod Method

                ### Summary

                Text with a inline code reference to definition of MyClass.
                """
            );

            yield return TestCase(
                "T32",
                new SeeUrlReferenceElement("https://example.com", null),
                """
                [https://example.com](https://example.com/)
                """
            );

            yield return TestCase(
                "T33",
                new SeeUrlReferenceElement("https://example.com", new TextBlock(new PlainTextElement("Link Text"))),
                """
                [Link Text](https://example.com/)
                """
            );


            yield return TestCase(
                "T34",
                MemberElement.FromXml("""
                <member name="M:MyClass.MyMethod">
                    <summary>
                        Text with a inline link to <see href="https://example.com">example.com</see>.
                    </summary>
                </member>
                """),
                """
                ## MyClass.MyMethod Method

                ### Summary

                Text with a inline link to [example.com](https://example.com/).
                """
            );


            yield return TestCase(
                "T35",
                MemberElement.FromXml("""
                <member name="M:MyClass.MyMethod">
                    <summary>
                        Text referencing type parameter <typeparamref name="parameter1" />.
                    </summary>
                </member>
                """),
                """
                ## MyClass.MyMethod Method

                ### Summary

                Text referencing type parameter `parameter1`.
                """
            );


            yield return TestCase(
                "T36",
                new ListElement(
                    ListType.Bullet,
                    null,
                    new[]
                    {
                        new ListItemElement(null, new TextBlock(new PlainTextElement("Item 1"))),
                        new ListItemElement(null, new TextBlock(new PlainTextElement("Item 2"))),
                    }),
                """
                - Item 1
                - Item 2
                """
            );

            yield return TestCase(
                "T37",
                new ListElement(
                    ListType.Number,
                    null,
                    new[]
                    {
                        new ListItemElement(null, new TextBlock(new PlainTextElement("Item 1"))),
                        new ListItemElement(null, new TextBlock(new PlainTextElement("Item 2"))),
                    }),
                """
                1. Item 1
                2. Item 2
                """
            );

            yield return TestCase(
                "T38",
                new ListElement(
                    ListType.Number,
                    null,
                    new[]
                    {
                        new ListItemElement(null, new TextBlock(new PlainTextElement("Item 1"))),
                        new ListItemElement(
                            null,
                            new TextBlock(
                                new PlainTextElement("Item 2"),
                                new ListElement(
                                    ListType.Bullet,
                                    null,
                                    new []
                                    {
                                        new ListItemElement(null, new TextBlock(new PlainTextElement("Item 2.1"))),
                                        new ListItemElement(null, new TextBlock(new PlainTextElement("Item 2.2"))),
                                    })
                        )),
                    }),
                """
                1. Item 1
                2. Item 2
                   - Item 2.1
                   - Item 2.2
                """
            );

            yield return TestCase(
                "T39",
                new ListElement(
                    ListType.Bullet,
                    null,
                    new[]
                    {
                        new ListItemElement(
                            new TextBlock(new PlainTextElement("Term 1")),
                            new TextBlock(new PlainTextElement("Description 1"))
                        ),
                        new ListItemElement(
                            new TextBlock(new PlainTextElement("Term 2")),
                            new TextBlock(new PlainTextElement("Description 2"))
                        ),
                    }),
                """
                - **Term 1:**

                  Description 1
                - **Term 2:**

                  Description 2
                """
            );

            yield return TestCase(
                "T40",
                new ListElement(
                    ListType.Table,
                    new ListItemElement(
                        new TextBlock(new PlainTextElement("Column 1")),
                        new TextBlock(new PlainTextElement("Column 2"))
                    ),
                    new[]
                    {
                        new ListItemElement(
                            new TextBlock(new PlainTextElement("Row 1, Column 1")),
                            new TextBlock(new PlainTextElement("Row 1, Column 2"))
                        ),
                        new ListItemElement(
                            new TextBlock(new PlainTextElement("Row 2, Column 1")),
                            new TextBlock(new PlainTextElement("Row 2, Column 2"))
                        ),
                    }),
                """
                | Column 1        | Column 2        |
                | --------------- | --------------- |
                | Row 1, Column 1 | Row 1, Column 2 |
                | Row 2, Column 1 | Row 2, Column 2 |
                """
            );

            yield return TestCase(
                "T41",
                MemberElement.FromXml("""
                <member name="M:MyClass.MyMethod">
                    <summary>
                        Summary for this method.
                    </summary>
                    <remarks>
                        Some remarks.
                    </remarks>
                    <returns>
                        Documentation of return value.
                    </returns>
                    <example>
                        Some example
                    </example>
                    <param name="someParameter" />                    
                    <param name="someOtherParameter">Description of <paramref name="someOtherParameter" />.</param>                   
                    <typeparam name="T1">Some Description</typeparam>
                    <exception cref="T:InvalidOperationException">Thrown if operation is invalid.</exception>
                    <exception cref="T:NotSupportedException" />
                    <seealso href="http://example.com" />
                    <seealso href="http://example.com">Link Text</seealso>
                    <seealso cref="T:MyNamespace.MyClass" />
                </member>
                """),
                """
                ## MyClass.MyMethod Method

                ### Summary

                Summary for this method.

                ### Remarks

                Some remarks.

                ### Returns

                Documentation of return value.

                ### Example

                Some example

                ### Parameters

                `someParameter`

                `someOtherParameter`

                Description of `someOtherParameter`.

                ### Type Parameters

                `T1`

                Some Description

                ### Exceptions

                `InvalidOperationException`

                Thrown if operation is invalid.

                `NotSupportedException`

                ### See Also
            
                [http://example.com](http://example.com/)  
                [Link Text](http://example.com/)  
                `MyNamespace.MyClass`
                """
            );

            yield return TestCase(
                "T42",
                MemberElement.FromXml("""
                <member name="P:MyClass.MyProperty">
                    <summary>
                        Summary for this method.
                    </summary>
                    <remarks>
                        Some remarks.
                    </remarks>
                    <value>
                        Description of value.
                    </value>
                    <example>
                        Some example
                    </example>
                    <param name="someParameter" />                    
                    <param name="someOtherParameter">Description of <paramref name="someOtherParameter" />.</param>                   
                    <exception cref="T:InvalidOperationException">Thrown if operation is invalid.</exception>
                    <exception cref="T:NotSupportedException" />
                    <seealso href="http://example.com" />
                    <seealso href="http://example.com">Link Text</seealso>
                    <seealso cref="T:MyNamespace.MyClass" />
                </member>
                """),
                """
                ## MyClass.MyProperty Property

                ### Summary

                Summary for this method.

                ### Remarks

                Some remarks.

                ### Value

                Description of value.

                ### Example

                Some example

                ### Parameters

                `someParameter`

                `someOtherParameter`

                Description of `someOtherParameter`.

                ### Exceptions

                `InvalidOperationException`

                Thrown if operation is invalid.

                `NotSupportedException`

                ### See Also
            
                [http://example.com](http://example.com/)  
                [Link Text](http://example.com/)  
                `MyNamespace.MyClass`
                """
            );

            // Unknown section elements are ignored
            yield return TestCase(
                "T42",
                MemberElement.FromXml("""
                <member name="P:MyClass.MyProperty">
                    <summary>
                        Summary for this method.
                    </summary>
                    <some-unknown-elements />
                </member>
                """),
                """
                ## MyClass.MyProperty Property

                ### Summary

                Summary for this method.
                """
            );

            // Unknown text elements are serialized to string
            yield return TestCase(
                "T42",
                MemberElement.FromXml("""
                <member name="P:MyClass.MyProperty">
                    <summary>
                        Summary for this method with unknown element <element />.
                    </summary>                    
                </member>
                """),
                """
                ## MyClass.MyProperty Property

                ### Summary

                Summary for this method with unknown element \<element \/\>.
                """
            );

            yield return TestCase(
                "T43",
                new EmphasisElement("Some Text"),
                "*Some Text*"
            );

            yield return TestCase(
                "T44",
                MemberElement.FromXml("""
                <member name="P:MyClass.MyProperty">
                    <summary>
                        Summary with <em>emphasized</em> text.
                    </summary>                    
                </member>
                """),
                """
                ## MyClass.MyProperty Property

                ### Summary

                Summary with *emphasized* text.
                """
            );

            yield return TestCase(
                "T45",
                new IdiomaticElement("Some Text"),
                "*Some Text*"
            );

            yield return TestCase(
                "T44",
                MemberElement.FromXml("""
                <member name="P:MyClass.MyProperty">
                    <summary>
                        Summary with <i>italic</i> text.
                    </summary>                    
                </member>
                """),
                """
                ## MyClass.MyProperty Property

                ### Summary

                Summary with *italic* text.
                """
            );
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public void Documentation_is_converted_to_expected_Markdown_content(string id, DocumentationElement input, string expectedMarkdown)
        {
            m_TestOutputHelper.WriteLine($"Test Id: {id}");

            // ARRANGE
            expectedMarkdown += Environment.NewLine;
            var sut = new MarkdownConverter();

            // ACT 
            var result = sut.ConvertToBlock(input);
            var actualMarkdown = result.ToString();

            // ASSERT
            Assert.Equal(expectedMarkdown, actualMarkdown);
        }
    }

    public class ConvertToSpan
    {
        private readonly ITestOutputHelper m_TestOutputHelper;

        public ConvertToSpan(ITestOutputHelper testOutputHelper)
        {
            m_TestOutputHelper = testOutputHelper;
        }

        public static IEnumerable<object[]> UnsupportedDocumentationNodeTestCases()
        {
            object[] TestCase(string id, TextElement input) =>
                new object[] { id, input };

            yield return TestCase("T01", new CodeElement("Some code", null));
            yield return TestCase("T02", new ListElement(ListType.Bullet, null, Array.Empty<ListItemElement>()));
            yield return TestCase("T03", new ListItemElement(null, new TextBlock()));

        }

        [Theory]
        [MemberData(nameof(UnsupportedDocumentationNodeTestCases))]
        public void Throws_InvalidOperationException_for_unsuppored_documentation_nodes(string id, TextElement input)
        {
            m_TestOutputHelper.WriteLine($"Test Id: {id}");

            // ARRANGE
            var sut = new MarkdownConverter();

            // ACT 
            var ex = Record.Exception(() => sut.ConvertToSpan(input));

            // ASSERT
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal($"Elments of type {input.GetType().Name} cannot be converted to inline Markdown text", ex.Message);
        }

        public static IEnumerable<object[]> TestCases()
        {
            object[] TestCase(string id, TextElement input, string expectedMarkdown) =>
                new object[] { id, input, expectedMarkdown };

            yield return TestCase(
                "T01",
                new TextBlock(
                    new PlainTextElement("Some Text "),
                    new PlainTextElement("spread over multiple elements")
                ),
                "Some Text spread over multiple elements"
            );

            yield return TestCase(
                "T02",
                new PlainTextElement("Some Text"),
                "Some Text"
            );

            yield return TestCase(
                "T03",
                new CElement("Some Code"),
                "`Some Code`"
            );

            yield return TestCase(
                "T04",
                new CElement(""),
                ""
            );

            yield return TestCase(
                "T05",
                new TextBlock(
                    new PlainTextElement("Paragraph 1"),
                    new ParagraphElement(
                        new TextBlock(
                            new PlainTextElement("Paragraph 2")
                ))),
                """
            Paragraph 1
            Paragraph 2
            """
            );

            yield return TestCase(
                "T06",
                new ParameterReferenceElement("parameter"),
                "`parameter`"
            );

            yield return TestCase(
                "T07",
                new TypeParameterReferenceElement("T1"),
                "`T1`"
            );

            yield return TestCase(
                "T08",
                new SeeCodeReferenceElement(MemberId.Parse("M:MyClass.Method"), null),
                "`MyClass.Method`"
            );

            yield return TestCase(
                "T09",
                new SeeCodeReferenceElement(MemberId.Parse("M:MyClass.Method"), new TextBlock(new PlainTextElement("Link Text"))),
                "Link Text"
            );

            yield return TestCase(
                "T10",
                new SeeUrlReferenceElement("https://example.com", null),
                "[https://example.com](https://example.com/)"
            );


            yield return TestCase(
                "T11",
                new SeeUrlReferenceElement("https://example.com", new TextBlock(new PlainTextElement("Link Text"))),
                "[Link Text](https://example.com/)"
            );

            // Unknown text elements are serialized to string
            yield return TestCase(
                "T12",
                new TextBlock(
                    new PlainTextElement("Plain text "),
                    new UnrecognizedTextElement(XElement.Parse("<element />"))
                ),
                "Plain text \\<element \\/\\>"
            );

            yield return TestCase(
                "T13",
                new EmphasisElement("Some Text"),
                "*Some Text*"
            );

            yield return TestCase(
                "T14",
                new IdiomaticElement("Some Text"),
                "*Some Text*"
            );

        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public void Documentation_is_converted_to_expected_Markdown_content(string id, TextElement input, string expectedMarkdown)
        {
            m_TestOutputHelper.WriteLine($"Test Id: {id}");

            // ARRANGE
            var sut = new MarkdownConverter();

            // ACT 
            var result = sut.ConvertToSpan(input);
            var actualMarkdown = result.ToString();

            // ASSERT
            Assert.Equal(expectedMarkdown, actualMarkdown);
        }
    }
}
