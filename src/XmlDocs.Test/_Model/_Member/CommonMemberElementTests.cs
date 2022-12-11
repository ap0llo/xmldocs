namespace Grynwald.XmlDocs.Test;

/// <summary>
/// Tests shared between all derived classes of <see cref="MemberElement"/>.
/// </summary>
public abstract class CommonMemberElementTests
{
    protected abstract MemberElement CreateInstanceFromXml(string xml);

    [Fact]
    public void FromXml_reads_summary_element()
    {
        // ARRANGE
        var input = """
                <member name="T:Project.Class">
                    <summary>
                    Example summary
                    </summary>
                </member>
            """;

        // ACT 
        var sut = CreateInstanceFromXml(input);

        // ASSERT            
        Assert.NotNull(sut.Summary);
        Assert.NotNull(sut!.Summary!.Text);
        Assert.Equal(new TextBlock(new PlainTextElement("Example summary")), sut.Summary.Text);
    }

    [Fact]
    public void FromXml_reads_remarks_element()
    {
        // ARRANGE
        var input = """
                <member name="T:Project.Class">
                    <summary>
                    Example summary
                    </summary>
                    <remarks>
                    Example remarks
                    </remarks>
                </member>
            """;

        // ACT 
        var sut = CreateInstanceFromXml(input);

        // ASSERT            
        Assert.NotNull(sut.Remarks);
        Assert.NotNull(sut!.Remarks!.Text);
        Assert.Equal(new TextBlock(new PlainTextElement("Example remarks")), sut.Remarks.Text);
    }

    [Fact]
    public void FromXml_reads_example_element()
    {
        // ARRANGE
        var input = """
                <member name="M:Project.Class.Method">
                    <example>
                    Some example
                    </example>
                </member>
            """;

        // ACT 
        var sut = CreateInstanceFromXml(input);

        // ASSERT            
        Assert.NotNull(sut.Example);
        Assert.NotNull(sut!.Example!.Text);
        Assert.Equal(new TextBlock(new PlainTextElement("Some example")), sut.Example.Text);
    }

    [Fact]
    public void FromXml_reads_seealso_elements()
    {
        // ARRANGE
        var input = """
                <member name="M:Project.Class.Method">
                    <seealso cref="P:MyClass.Property"/>
                    <seealso href="link">Link Text</seealso>
                </member>
                """;

        // ACT 
        var sut = CreateInstanceFromXml(input);

        // ASSERT            
        Assert.NotNull(sut.SeeAlso);
        Assert.Collection(
            sut.SeeAlso,
            x => Assert.IsType<SeeAlsoCodeReferenceElement>(x),
            x => Assert.IsType<SeeAlsoUrlReferenceElement>(x)
        );
    }

    [Fact]
    public void FromXml_stores_unrecognized_elements_as_unrecognized_elements()
    {
        // ARRANGE
        var input = """
                <member name="M:Project.Class.Method">
                    <unknownElement1 />                    
                    <unknownElement2 />                    
                </member>
                """;

        // ACT 
        var sut = CreateInstanceFromXml(input);

        // ASSERT            
        Assert.NotNull(sut.UnrecognizedElements);
        Assert.Collection(
            sut.UnrecognizedElements,
            x => Assert.Equal("unknownElement1", x.Xml.Name),
            x => Assert.Equal("unknownElement2", x.Xml.Name)
        );
    }

}
