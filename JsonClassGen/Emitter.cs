using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsonClassGen
{
    public class Emitter
    {
        private readonly string[] ClassNameStarts = new string[] { "Foo", "Fi", "Fo", "Fum", "Fa", "Fe" };
        private readonly string[] ClassNameEnds = new string[] { "Bar", "Baz", "Boz", "Biz", "Buz", "Bez" };

        public void EmitClass(List<Token> tokens)
        {
            var properties = GetBasicProperties(tokens);
            var classFill = string.Format(ClassFormat, RandomClassName(), properties.ToString());
            var codeFile = string.Format(CodeFileFormat, classFill);
        }

        private StringBuilder GetBasicProperties(List<Token> tokens)
        {
            var properties = new StringBuilder();
            var basTokens = tokens.Where(t => t.Type != TokenType.Array && t.Type != TokenType.Object);
            foreach (var tok in basTokens)
            {
                switch (tok.Type)
                {
                    case TokenType.Null: properties.Append(BasicObjectProperty(tok)); break;
                    case TokenType.Number: properties.Append(BasicNumberProperty(tok)); break;
                    case TokenType.Boolean: properties.Append(BasicBooleanProperty(tok)); break;
                    case TokenType.String: properties.Append(BasicStringProperty(tok)); break;
                }
            }
            return properties;
        }

        private string BasicStringProperty(Token tok)
        {
            throw new NotImplementedException();
        }

        private string BasicBooleanProperty(Token tok)
        {
            return string.Format(PropertyFormat, "bool?", tok.Value);
        }

        private string BasicNumberProperty(Token tok)
        {
            throw new NotImplementedException();
        }

        private string BasicObjectProperty(Token tok)
        {
            throw new NotImplementedException();
        }

        private string RandomClassName()
        {
            var rand = new Random();
            var startIdx = rand.Next(ClassNameStarts.Length);
            var endIdx = rand.Next(ClassNameEnds.Length);
            return ClassNameStarts[startIdx] + ClassNameEnds[endIdx];
        }

        private string ClassFormat =
@"public class {0}
    {{
        {1}
    }}";

        private string CodeFileFormat =
@"using System;
using System.Collections.Generic;
using System.Text;

namespace JsonClassGen.Entities
{{
{0}
}}";
        private string PropertyFormat = "\t\t\tpublic {0} {1} {{ get; set; }}";
    }
}
