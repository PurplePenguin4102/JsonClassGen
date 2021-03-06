﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsonClassGen
{
    public class Emitter
    {
        private readonly string[] ClassNameStarts = new string[] { "Foo", "Fi", "Fo", "Fum", "Fa", "Fe" };
        private readonly string[] ClassNameEnds = new string[] { "Bar", "Baz", "Boz", "Biz", "Buz", "Bez" };

        public (string fileName, string fileData) EmitClass(List<Token> tokens)
        {
            var properties = GetBasicProperties(tokens);
            var className = RandomClassName();
            var classFill = string.Format(ClassFormat, className, properties.ToString());
            var codeFile = string.Format(CodeFileFormat, classFill);
            return ($"{className}.cs", codeFile);
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
                    case TokenType.Int: properties.Append(BasicIntProperty(tok)); break;
                    case TokenType.Decimal: properties.Append(BasicDecimalProperty(tok)); break;
                    case TokenType.Boolean: properties.Append(BasicBooleanProperty(tok)); break;
                    case TokenType.String: properties.Append(BasicStringProperty(tok)); break;
                    case TokenType.DateTime: properties.Append(BasicDateTimeProperty(tok)); break;
                }
            }
            return properties;
        }

        private string BasicDateTimeProperty(Token tok)
        {
            return string.Format(PropertyFormat, "DateTime?", tok.Value);
        }

        private string BasicDecimalProperty(Token tok)
        {
            return string.Format(PropertyFormat, "decimal?", tok.Value);
        }

        private string BasicStringProperty(Token tok)
        {
            return string.Format(PropertyFormat, "string", tok.Value);
        }

        private string BasicBooleanProperty(Token tok)
        {
            return string.Format(PropertyFormat, "bool?", tok.Value);
        }

        private string BasicIntProperty(Token tok)
        {
            return string.Format(PropertyFormat, "int?", tok.Value);
        }

        private string BasicObjectProperty(Token tok)
        {
            return string.Format(PropertyFormat, "object", tok.Value);
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
        private string PropertyFormat = "\t\t\tpublic {0} {1} {{ get; set; }}\r\n";
    }
}
