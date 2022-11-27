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
    /// Tests for <see cref="SeeElement"/>
    /// </summary>
    public class SeeElementTest
    {

        [Fact]
        public void FromXml_returns_expected_result_01()
        {
            // ARRANGE
            var xml = XElement.Parse(""""
                <see cref="T:SomeType" />
                """");

            // ACT 
            var result = SeeElement.FromXml(xml);

            // ASSERT
            var seeCRefElement = Assert.IsType<SeeCRefElement>(result);
            Assert.Equal("T:SomeType", seeCRefElement.CRef);
        }



        [Fact]
        public void FromXml_returns_expected_result_02()
        {
            // ARRANGE
            var xml = XElement.Parse(""""
                <see href="http://example.com" />
                """");

            // ACT 
            var result = SeeElement.FromXml(xml);

            // ASSERT
            var seeHRefElement = Assert.IsType<SeeHRefElement>(result);
            Assert.Equal("http://example.com", seeHRefElement.HRef);
        }
    }
}
