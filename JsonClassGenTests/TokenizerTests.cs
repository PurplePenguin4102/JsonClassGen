using JsonClassGen;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace JsonClassGenTests
{
    [TestClass]
    public class TokenizerTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var input = @"{'cat': true, 'dog' : 5.2, 'emu' : ['1', '2', '3'], 'id' : 1}";
            var noSpaces = Regex.Replace(input, @"\s+", "");
            var tokenizer = new Tokenizer();
            var output = tokenizer.Tokenize(noSpaces);
            Assert.AreEqual(output.Count, 4);
        
        }

        [TestMethod]
        public void TestMethod2()
        {
            var input = @"{'cat':'2017-12-15'}";
            var noSpaces = Regex.Replace(input, @"\s+", "");
            var tokenizer = new Tokenizer();
            var output = tokenizer.Tokenize(noSpaces);
            Assert.AreEqual(TokenType.DateTime, output[0].Type);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var input = @"{'cat':123}";
            var noSpaces = Regex.Replace(input, @"\s+", "");
            var tokenizer = new Tokenizer();
            var output = tokenizer.Tokenize(noSpaces);
            Assert.AreEqual(TokenType.Int, output[0].Type);
        }

        [TestMethod]
        public void TestMethod4()
        {
            var input = @"{'cat':123.456}";
            var noSpaces = Regex.Replace(input, @"\s+", "");
            var tokenizer = new Tokenizer();
            var output = tokenizer.Tokenize(noSpaces);
            Assert.AreEqual(TokenType.Decimal, output[0].Type);
        }
    }
}
