using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;

namespace JsonClassGen
{
    public class CodeGenerator
    {
        private List<string> _hierarchy = new List<string>();

        public object GenerateCodeFile(string jsonDocument)
        {
            var validator = new Validator();
            var tokenizer = new Tokenizer();
            var emitter = new Emitter();
            var sanitized = SanitizeDocument(jsonDocument);
            var tokens = tokenizer.Tokenize(sanitized);
            emitter.EmitClass(tokens);
            return null;
        }

        public string SanitizeDocument(string document) => Regex.Replace(document, @"\s+", "");


    }
}
