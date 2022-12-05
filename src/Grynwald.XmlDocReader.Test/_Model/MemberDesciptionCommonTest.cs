namespace Grynwald.XmlDocReader.Test;

/// <summary>
/// Tests shared between all derived classes of <see cref="MemberDescription"/>.
/// </summary>
public abstract class MemberDesciptionCommonTest
{
    protected abstract MemberDescription CreateInstanceFromXml(string xml);

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
        Assert.Equal(new TextBlock(new PlainTextElement("Example summary")), sut.Summary);
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
        Assert.Equal(new TextBlock(new PlainTextElement("Example remarks")), sut.Remarks);
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
        Assert.Equal(new TextBlock(new PlainTextElement("Some example")), sut.Example);
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
            x => Assert.IsType<SeeAlsoCodeReferenceDescription>(x),
            x => Assert.IsType<SeeAlsoUrlReferenceDescription>(x)
        );
    }
}
