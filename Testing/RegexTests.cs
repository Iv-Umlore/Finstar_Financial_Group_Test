using Common.Functions;
using Common.Models;
using FluentAssertions;
using NUnit.Framework;
using System.Text;
using System.Text.RegularExpressions;

namespace Testing
{
    // Честно сообщаю, что с регулярными выражениями сталкивался
    // Но самостоятельно не писал
    // Решил что оставлю здесь свои поиски
    [TestFixture]
    public class RegexTest
    {

        [Test]
        public void Regex_FindSimple_Test1() {
            var reg = new Regex("^{.*}$");

            string mainString = "{asdsd ad}";

            bool validateResult = reg.Match(mainString).Success;

            validateResult.Should().BeTrue();
        }

        [Test]
        public void Regex_FindSimple_Test2()
        {
            var reg = new Regex("^{\".*\".*:.*\".*\".*}.*$");

            string mainString = "{\"code\": \"ufjnr\"}";

            bool validateResult = reg.Match(mainString).Success;

            validateResult.Should().BeTrue();
        }

        [Test]
        public void Regex_FindSimple_Test3()
        {
            var reg = new Regex("^{\".*\".*:.*\".*\".*}.*$");

            string mainString = "{\"code: \"ufjnr\"}";

            bool validateResult = reg.Match(mainString).Success;

            validateResult.Should().BeFalse();
        }

        [Test]
        public void MainRegex_Test()
        {
            var reg = RegexModel.techReqRegex;
            string mainString = "{\"1\": \" Value14 \" }";

            bool validateResult = reg.Match(mainString).Success;

            validateResult.Should().BeTrue();           
        }

        [Test]
        public void SplitByRegex_Test()
        {
            //{ ",\r\n", ",\t", ", ", ", \t" }
            Regex splitReg = new Regex("^(}).*,.*({)");
            string mainString = "{\"1\": \"value1\"}, \t{\"5\": \"value2\"}, {\"10\": \"value32\"}";

            string[] res = Regex.Split(mainString, "(})\\s?\\t?,\\s?\\t?({)");
            res.Length.Should().Be(7);
        }

        [Test]
        public void SplitByRegex_FinalTest()
        {
            //{ ",\r\n", ",\t", ", ", ", \t" }
            string expectedValue1 = "{\"1\": \"value1\"}";
            string expectedValue2 = "{\"5\": \"value2\"}";
            string expectedValue3 = "{\"10\": \"value32\"}";

            string mainString = "{\"1\": \"value1\"}, \t{\"5\": \"value2\"}, {\"10\": \"value32\"}";

            string[] res = Regex.Split(mainString, RegexModel.splitReq);
            List<string> result = new List<string>();

            StringBuilder tmpBuilder = new StringBuilder();

            foreach (string s in res)
            {
                tmpBuilder.Append(s);
                if (s == "}")
                {
                    result.Add(tmpBuilder.ToString());
                    tmpBuilder.Clear();
                }
            }

            result.Add(tmpBuilder.ToString());

            result.Count.Should().Be(3);
            result[0].Should().Be(expectedValue1);
            result[1].Should().Be(expectedValue2);
            result[2].Should().Be(expectedValue3);
        }

        [Test]
        public void SplitByRegex_FinalTest2()
        {
            // Arrange
            string expectedValue1 = "{\"1\": \"value1\"}";
            string expectedValue2 = "{\"5\": \"value2\"}";
            string expectedValue3 = "{\"10\": \"value32\"}";

            string mainString = "{\"1\": \"value1\"}, \t{\"5\": \"value2\"}, {\"10\": \"value32\"}";

            // Act
            List<string> result = Helper.SplitStringByRegex(mainString, RegexModel.splitReq);

            // Assert
            result.Count.Should().Be(3);

            result[0].Should().Be(expectedValue1);
            result[1].Should().Be(expectedValue2);
            result[2].Should().Be(expectedValue3);
        }
    }
}