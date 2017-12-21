using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

namespace JsonClassGen
{
    public class CodeGenerator
    {
        private List<string> _hierarchy = new List<string>();

        public async Task<object> GenerateCodeFileAsync(string jsonDocument)
        {
            var validator = new Validator();
            var tokenizer = new Tokenizer();
            var emitter = new Emitter();
            var sanitized = SanitizeDocument(jsonDocument);
            var tokens = tokenizer.Tokenize(sanitized);
            var codeFile = emitter.EmitClass(tokens);
            using (FileStream fs = File.OpenWrite(codeFile.fileName))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    await sw.WriteAsync(codeFile.fileData);
                }
            }
            return null;
        }

        public string SanitizeDocument(string document) => Regex.Replace(document, @"\s+", "");


    }
}
