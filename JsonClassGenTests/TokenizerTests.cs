using JsonClassGen;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        [TestMethod]
        public void TestMethod5()
        {
            var input = @"{'cat':123.456e-12}";
            var noSpaces = Regex.Replace(input, @"\s+", "");
            var tokenizer = new Tokenizer();
            var output = tokenizer.Tokenize(noSpaces);
            Assert.AreEqual(TokenType.Decimal, output[0].Type);
        }

        [TestMethod]
        public void TestMethod6()
        {
            var input = @"{'cat':-123}";
            var noSpaces = Regex.Replace(input, @"\s+", "");
            var tokenizer = new Tokenizer();
            var output = tokenizer.Tokenize(noSpaces);
            Assert.AreEqual(TokenType.Int, output[0].Type);
        }

        [TestMethod]
        public void TestMethod7()
        {
            var input = @"{'cat':-123.456}";
            var noSpaces = Regex.Replace(input, @"\s+", "");
            var tokenizer = new Tokenizer();
            var output = tokenizer.Tokenize(noSpaces);
            Assert.AreEqual(TokenType.Decimal, output[0].Type);
        }

        [TestMethod]
        public void TestMethod8()
        {
            var input = @"{'cat':-123.456e-12}";
            var noSpaces = Regex.Replace(input, @"\s+", "");
            var tokenizer = new Tokenizer();
            var output = tokenizer.Tokenize(noSpaces);
            Assert.AreEqual(TokenType.Decimal, output[0].Type);
        }

        [TestMethod]
        public void TestMethod9()
        {
            var input = @"{'cat':-123.456e-12,'dog':'}'}";
            var noSpaces = Regex.Replace(input, @"\s+", "");
            var tokenizer = new Tokenizer();
            var output = tokenizer.FindObjectEnd(noSpaces);
            Assert.AreEqual(29, output);
        }
        [TestMethod]
        public void TestMethod10()
        {
            var input = @"['cat',-123.456e-12,'dog',']']}";
            var noSpaces = Regex.Replace(input, @"\s+", "");
            var tokenizer = new Tokenizer();
            var output = tokenizer.FindObjectEnd(noSpaces);
            Assert.AreEqual(29, output);
        }

        [TestMethod]
        public async Task RealWorldExample()
        {
            var rwInput = @"  {
    ""Id"": 0,
    ""Value"": ""string"",
    ""AttributeId"": 0,
    ""CreatedDate"": ""2017-12-20T06:06:38.292Z"",
    ""ModifiedDate"": ""2017-12-20T06:06:38.292Z"",
    ""CreatedBy"": 0,
    ""ModifiedBy"": 0
  }";
            var cg = new CodeGenerator();
            await cg.GenerateCodeFileAsync(rwInput);

        }
    }
}
