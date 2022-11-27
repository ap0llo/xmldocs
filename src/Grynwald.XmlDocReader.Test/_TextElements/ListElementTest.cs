using System.Xml.Linq;

namespace Grynwald.XmlDocReader.Test
{
    public class ListElementTest
    {

        [Theory]
        [InlineData(null, ListType.Unknown)]
        [InlineData("not a list type", ListType.Unknown)]
        [InlineData("bullet", ListType.Bullet)]
        [InlineData("number", ListType.Number)]
        [InlineData("table", ListType.Table)]
        public void List_type_is_parsed_as_expected(string listType, ListType expected)
        {
            // ARRANGE
            var xml = XElement.Parse("""
                <list>
                </list>
            """);

            xml.SetAttributeValue("type", listType);

            // ACT 
            var sut = ListElement.FromXml(xml);

            // ASSERT
            Assert.Equal(expected, sut.Type);
        }



        [Fact]
        public void List_header_is_parsed_as_expected()
        {
            // ARRANGE
            var xml = XElement.Parse("""
            <list>
                <listheader>
                    <term>term</term>
                    <description>description</description>
                </listheader>
                <item>
                    <term>Assembly</term>
                    <description>The library or executable built from a compilation.</description>
                </item>
            </list>            
            """);

            // ACT 
            var sut = ListElement.FromXml(xml);

            // ASSERT
            Assert.NotNull(sut.ListHeader);
            Assert.Equal(new TextBlock(new PlainTextElement("term")), sut.ListHeader!.Term);
            Assert.Equal(new TextBlock(new PlainTextElement("description")), sut.ListHeader!.Description);
        }

        [Fact]
        public void List_header_is_optional()
        {
            // ARRANGE
            var xml = XElement.Parse("""
            <list>
                <item>
                    <term>Assembly</term>
                    <description>The library or executable built from a compilation.</description>
                </item>
            </list>            
            """);

            // ACT 
            var sut = ListElement.FromXml(xml);

            // ASSERT
            Assert.Null(sut.ListHeader);
        }


        [Fact]
        public void List_items_are_parsed_as_expected()
        {
            // ARRANGE
            var xml = XElement.Parse("""
            <list type="bullet">
                <item>
                    <term>Term 1</term>
                    <description>Definition 1</description>
                </item>
                <item>
                    <term>Term 2</term>
                    <description>Definition 2</description>
                </item>
            </list>            
            """);

            // ACT 
            var sut = ListElement.FromXml(xml);

            // ASSERT
            Assert.Collection(
                sut.Items,
                x =>
                {
                    Assert.Equal(new TextBlock(new PlainTextElement("Term 1")), x.Term);
                    Assert.Equal(new TextBlock(new PlainTextElement("Definition 1")), x.Description);
                },
                x =>
                {
                    Assert.Equal(new TextBlock(new PlainTextElement("Term 2")), x.Term);
                    Assert.Equal(new TextBlock(new PlainTextElement("Definition 2")), x.Description);
                }
            );
        }


    }
}
