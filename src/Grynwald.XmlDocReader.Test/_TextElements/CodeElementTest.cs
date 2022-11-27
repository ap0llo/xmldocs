using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace Grynwald.XmlDocReader.Test
{
    /// <summary>
    /// Tests for <see cref="CodeElement"/>
    /// </summary>
    public class CodeElementTest
    {

        [Theory]
        [InlineData(null, null, null)]
        [InlineData("lang", null, "lang")]
        [InlineData(null, "lang", "lang")]
        [InlineData("lang1", "lang2", "lang1")]
        public void FromXml_reads_lanuage(string? languageAttribute, string? langAttribute, string? expectedLanguage)
        {
            // ARRANGE
            var xml = XElement.Parse("""
                <code>
                Some code block
                </code>
                """);

            xml.SetAttributeValue("language", languageAttribute);
            xml.SetAttributeValue("lang", langAttribute);

            // ACT 
            var sut = CodeElement.FromXml(xml);

            // ASSERT
            Assert.Equal(expectedLanguage, sut.Language);   
        }

    }
}
