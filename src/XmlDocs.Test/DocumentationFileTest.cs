namespace Grynwald.XmlDocs.Test;

/// <summary>
/// Tests for <see cref="DocumentationFile"/>
/// </summary>
public class DocumentationFileTest
{

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void AssemblyName_must_not_be_null_or_whitespace(string assemblyName)
    {
        // ARRANGE

        // ACT 
        var ex = Record.Exception(() => new DocumentationFile(assemblyName: assemblyName, Array.Empty<MemberElement>()));

        // ASSERT
        var argumentException = Assert.IsType<ArgumentException>(ex);
        Assert.Equal("assemblyName", argumentException.ParamName);
    }

    [Fact]
    public void Members_must_not_be_null()
    {
        // ARRANGE

        // ACT 
        var ex = Record.Exception(() => new DocumentationFile("AssemblyName", members: null!));

        // ASSERT
        var argumentNullException = Assert.IsType<ArgumentNullException>(ex);
        Assert.Equal("members", argumentNullException.ParamName);
    }


    [Fact]
    public void FromXml_fails_on_invald_xml_01()
    {
        // ARRANGE
        var input = "not xml";

        // ACT 
        var ex = Record.Exception(() => DocumentationFile.FromXml(input));

        // ASSERT
        Assert.IsType<XmlDocsException>(ex);
        Assert.IsType<XmlException>(ex.InnerException);
    }

    [Fact]
    public void FromXml_fails_on_invald_xml_2()
    {
        // ARRANGE
        var input = new XDocument();

        // ACT 
        var ex = Record.Exception(() => DocumentationFile.FromXml(input));

        // ASSERT
        Assert.IsType<XmlDocsException>(ex);
        Assert.Equal("XML document has not root element", ex.Message);
    }


    [Fact]
    public void FromXml_fails_on_invald_root_element_name()
    {
        // ARRANGE
        var input = """
                <not-a-doc-file />
                """;

        // ACT
        var ex = Record.Exception(() => DocumentationFile.FromXml(input));

        // ASSERT
        Assert.IsType<XmlDocsException>(ex);
    }

    [Fact]
    public void FromXml_correctly_loads_assembly_name()
    {
        // ARRANGE
        var input = """
                <?xml version="1.0"?>
                <doc>
                    <assembly>
                        <name>SomeAssemblyName</name>
                    </assembly>
                </doc>
                """;

        // ACT 
        var sut = DocumentationFile.FromXml(input);

        // ASSERT
        Assert.Equal("SomeAssemblyName", sut.AssemblyName);
        Assert.NotNull(sut.Members);
        Assert.Empty(sut.Members);
    }

    [Theory]
    [InlineData("T01", """
                <?xml version="1.0"?>
                <doc>
                </doc>
                """)]
    [InlineData("T02", """
                <?xml version="1.0"?>
                <doc>
                    <assembly>
                    </assembly>
                </doc>
                """)]
    [InlineData("T03", """
                <?xml version="1.0"?>
                <doc>
                    <assembly>
                        <name></name>
                    </assembly>
                </doc>
                """)]
    public void FromXml_fails_on_missing_or_empty_assembly_name(string id, string input)
    {
        // ARRANGE
        _ = id;

        // ACT 
        var ex = Record.Exception(() => DocumentationFile.FromXml(input));

        // ASSERT
        Assert.IsType<XmlDocsException>(ex);
    }

    [Fact]
    public void FromXml_reads_expected_members()
    {
        // ARRANGE
        var input = """
                <?xml version="1.0"?>
                <doc>
                    <assembly>
                        <name>SomeAssemblyName</name>
                    </assembly>
                    <members>
                        <member name="T:Project.Class">
                        </member>
                        <member name="F:Project.Class.Field">
                        </member>
                    </members>
                </doc>
                """;

        // ACT 
        var sut = DocumentationFile.FromXml(input);

        // ASSERT
        Assert.Equal("SomeAssemblyName", sut.AssemblyName);
        Assert.NotNull(sut.Members);
        Assert.Collection(
            sut.Members,
            x => Assert.Equal(MemberId.Parse("T:Project.Class"), x.Id),
            x => Assert.Equal(MemberId.Parse("F:Project.Class.Field"), x.Id)
        );
    }

    [Theory]
    [InlineData("T01", """"
            <?xml version="1.0"?>
            <doc>
                <assembly>
                    <name>SomeAssemblyName</name>
                </assembly>
                <members>
                    <member>
                    </member>
                </members>
            </doc>
            """")]
    [InlineData("T02", """"
            <?xml version="1.0"?>
            <doc>
                <assembly>
                    <name>SomeAssemblyName</name>
                </assembly>
                <members>
                    <member name="">
                    </member>
                </members>
            </doc>
            """")]
    [InlineData("T03", """"
            <?xml version="1.0"?>
            <doc>
                <assembly>
                    <name>SomeAssemblyName</name>
                </assembly>
                <members>
                    <member name="  ">
                    </member>
                </members>
            </doc>
            """")]
    public void FromXml_fails_on_missing_or_empty_member_name(string id, string input)
    {
        // ARRANGE
        _ = id;

        // ACT 
        var ex = Record.Exception(() => DocumentationFile.FromXml(input));

        // ASSERT
        Assert.IsType<XmlDocsException>(ex);
    }
}
