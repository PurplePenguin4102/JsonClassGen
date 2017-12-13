using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;

namespace JsonClassGen
{
    public class CodeGenerator
    {
        private string[] ClassNameStarts = new string[] { "Foo", "Fi", "Fo", "Fum", "Fa", "Fe" };
        private readonly string[] ClassNameEnds = new string[] { "Bar", "Baz", "Boz", "Biz", "Buz", "Bez" };

        private List<string> _hierarchy = new List<string>();

        public object GenerateCodeFile(string jsonDocument)
        {
            var tokenizer = new Tokenizer();
            var noSpaces = Regex.Replace(jsonDocument, @"\s+", "");
            var tokens = tokenizer.Tokenize(noSpaces);
            return null;
        }

        
    }
}
