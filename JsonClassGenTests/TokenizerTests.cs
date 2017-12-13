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
    }
}
