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
            object[] TestCase(string id, IDocumentationNode input, string expectedMarkdown) =>
                new object[] { id, input, expectedMarkdown };


            yield return TestCase(
                "T01",
                new DocumentationFile("MyAssembly", Array.Empty<MemberDescription>()),
                $"# MyAssembly"
            );

            yield return TestCase(
                "T02",
                new DocumentationFile(
                    "MyAssembly",
                    new MemberDescription[]
                    {
                        new NamespaceDescription(MemberId.Parse("N:MyNamespace")),
                        new TypeDescription(MemberId.Parse("T:MyNamespace.MyClass")),
                        new FieldDescription(MemberId.Parse("F:MyNamespace.MyClass.Field")),
                        new PropertyDescription(MemberId.Parse("P:MyNamespace.MyClass.Property")),
                        new MethodDescription(MemberId.Parse("M:MyNamespace.MyClass.Method")),
                        new EventDescription(MemberId.Parse("E:MyNamespace.MyClass.Event")),
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
                new FieldDescription(MemberId.Parse("F:MyNamespace.MyClass.Field")),
                "## MyNamespace.MyClass.Field Field"
            );

            yield return TestCase(
                "T04",
                MemberDescription.FromXml("""
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
                new ParameterDescription("parameter1", null),
                "`parameter1`"
            );

            yield return TestCase(
                "T06",
                new ParameterDescription(
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
                new TypeParameterDescription("typeParameter1", null),
                "`typeParameter1`"
            );

            yield return TestCase(
                "T08",
                new TypeParameterDescription(
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
                MemberDescription.FromXml("""
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
                new ExceptionDescription(MemberId.Parse("T:MyNamespace.MyException"), null),
                "`MyNamespace.MyException`"
            );

            yield return TestCase(
                "T12",
                new ExceptionDescription(
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
                MemberDescription.FromXml("""
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
                new SeeAlsoUrlReferenceDescription("http://example.com", null),
                "[http://example.com](http://example.com/)"
            );

            yield return TestCase(
                "T15",
                new SeeAlsoUrlReferenceDescription("http://example.com", new TextBlock(new PlainTextElement("Link Text"))),
                "[Link Text](http://example.com/)"
            );

            yield return TestCase(
                "T16",
                MemberDescription.FromXml("""
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
                new SeeAlsoCodeReferenceDescription(MemberId.Parse("T:MyNamespace.MyClass"), null),
                "`MyNamespace.MyClass`"
            );

            yield return TestCase(
                "T18",
                new SeeAlsoCodeReferenceDescription(MemberId.Parse("T:MyNamespace.MyClass"), new TextBlock(new PlainTextElement("Link Text"))),
                "Link Text"
            );

            yield return TestCase(
                "T19",
                MemberDescription.FromXml("""
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
                MemberDescription.FromXml("""
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
                MemberDescription.FromXml("""
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
                MemberDescription.FromXml("""
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
                MemberDescription.FromXml("""
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
                "T27",
                new TypeParameterReferenceElement("parameter"),
                """
            `parameter`
            """
            );

            yield return TestCase(
                "T28",
                new SeeCodeReferenceElement(MemberId.Parse("M:MyClass.Method"), null),
                """
            `MyClass.Method`
            """
            );

            yield return TestCase(
                "T29",
                new SeeCodeReferenceElement(MemberId.Parse("M:MyClass.Method"), new TextBlock()),
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
                MemberDescription.FromXml("""
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
                "T31",
                new SeeUrlReferenceElement("https://example.com", null),
                """
            [https://example.com](https://example.com/)
            """
            );

            yield return TestCase(
                "T32",
                new SeeUrlReferenceElement("https://example.com", new TextBlock()),
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
                MemberDescription.FromXml("""
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
                MemberDescription.FromXml("""
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
                MemberDescription.FromXml("""
                <member name="M:MyClass.MyMethod">
                    <summary>
                        Summary for this method.
                    </summary>
                    <remarks>
                        Some remarks.
                    </remarks>
                    <value>
                        Description of value.
                    </value>
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

            ### Value

            Description of value.

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
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public void Documentation_is_converted_to_expected_Markdown_content(string id, IDocumentationNode input, string expectedMarkdown)
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
            object[] TestCase(string id, IDocumentationNode input) =>
                new object[] { id, input };

            yield return TestCase("T01", new DocumentationFile("MyAssembly", Array.Empty<MemberDescription>()));
            yield return TestCase("T02", new NamespaceDescription(MemberId.Parse("N:MyNamesapce")));
            yield return TestCase("T03", new TypeDescription(MemberId.Parse("T:MyClass")));
            yield return TestCase("T04", new FieldDescription(MemberId.Parse("F:MyClass.Field")));
            yield return TestCase("T05", new PropertyDescription(MemberId.Parse("P:MyClass.Property")));
            yield return TestCase("T06", new MethodDescription(MemberId.Parse("M:MyClass.Method")));
            yield return TestCase("T07", new EventDescription(MemberId.Parse("E:MyClass.Event")));
            yield return TestCase("T08", new ParameterDescription("parameter", null));
            yield return TestCase("T09", new TypeParameterDescription("typeParameter", null));
            yield return TestCase("T10", new ExceptionDescription(MemberId.Parse("T:MyException"), null));
            yield return TestCase("T11", new SeeAlsoUrlReferenceDescription("https://example.com", null));
            yield return TestCase("T12", new SeeAlsoCodeReferenceDescription(MemberId.Parse("M:MyClass.Method"), null));
            yield return TestCase("T13", new CodeElement("Some code", null));
            yield return TestCase("T14", new ListElement(ListType.Bullet, null, Array.Empty<ListItemElement>()));
            yield return TestCase("T15", new ListItemElement(null, new TextBlock()));

        }

        [Theory]
        [MemberData(nameof(UnsupportedDocumentationNodeTestCases))]
        public void Throws_InvalidOperationException_for_unsuppored_documentation_nodes(string id, IDocumentationNode input)
        {
            m_TestOutputHelper.WriteLine($"Test Id: {id}");

            // ARRANGE
            var sut = new MarkdownConverter();

            // ACT 
            var ex = Record.Exception(() => sut.ConvertToSpan(input));

            // ASSERT
            Assert.IsType<InvalidOperationException>(ex);
            Assert.Equal("ConvertToSpanVisitor can only convert text elements", ex.Message); //TODO: Add better error message
        }

        public static IEnumerable<object[]> TestCases()
        {
            object[] TestCase(string id, IDocumentationNode input, string expectedMarkdown) =>
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
                new SeeCodeReferenceElement(MemberId.Parse("M:MyClass.Method"), new TextBlock()),
                "`MyClass.Method`"
            );

            yield return TestCase(
                "T10",
                new SeeCodeReferenceElement(MemberId.Parse("M:MyClass.Method"), new TextBlock(new PlainTextElement("Link Text"))),
                "Link Text"
            );

            yield return TestCase(
                "T11",
                new SeeUrlReferenceElement("https://example.com", null),
                "[https://example.com](https://example.com/)"
            );

            yield return TestCase(
                "T12",
                new SeeUrlReferenceElement("https://example.com", new TextBlock()),
                "[https://example.com](https://example.com/)"
            );

            yield return TestCase(
                "T23",
                new SeeUrlReferenceElement("https://example.com", new TextBlock(new PlainTextElement("Link Text"))),
                "[Link Text](https://example.com/)"
            );
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public void Documentation_is_converted_to_expected_Markdown_content(string id, IDocumentationNode input, string expectedMarkdown)
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
