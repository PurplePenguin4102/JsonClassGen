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
        private Tag[] Tags = new Tag[] 
        {
            new Tag()
            {
                Opening = '[',
                Closing = ']'
            }, new Tag()
            {
                Opening = '{',
                Closing = '}'
            }, new Tag()
            {
                Opening = '"',
                Closing = '"'
            }, new Tag()
            {
                Opening = '\'',
                Closing = '\''
            }
        };
        private readonly char[] Dividers = new char[] { ':', ',' };
        private List<string> _hierarchy = new List<string>();
    

        public object GenerateCodeFile(string jsonDocument)
        {
            var noSpaces = Regex.Replace(jsonDocument, @"\s+", "");
            var tokens = Tokenize(noSpaces);
            return null;
        }

        private List<Token> Tokenize(string document)
        {
            var tokens = new List<Token>();
            if (!ValidateDocument(document))
            {
                throw new LexException($"Unexpected Character -- at position -- ");
            }

            var pointer = 0;
            var type = GetNextTagType(document, pointer);
            bool expectKvp = false, expectCsl = false;
            if (type == TokenType.Object)
            {
                expectKvp = true;
            }
            else if (type == TokenType.Array)
            {
                expectCsl = true;
            }

            if (expectKvp)
            {
                while (true)
                {
                    var key = GetNextTagValue(document, pointer);
                    pointer = key.ptr;
                    string tokenKey = key.tag;
                    if (document[pointer + 1] != ':')
                    {
                        throw new LexException($"Unexpected Character -- at position -- ");
                    }
                    pointer++;
                    var val = GetTokenType(document, pointer);
                    pointer = val.ptr;
                    var tokenType = val.tagType;
                    var token = new Token
                    {
                        Value = tokenKey, // regexMatch to get rid of junk chars
                        Type = tokenType
                    };
                    tokens.Add(token);
                    if (document[pointer + 1] == ',')
                    {
                        continue;
                    }
                    else if (document[pointer + 1] == '}')
                    {
                        break;
                    }
                }
            }


            return tokens;
        }

        private (TokenType tagType, int ptr) GetTokenType(string document, int pointer)
        {
            char[] boolStart = new char[] { 't', 'T', 'f', 'F' };
            char[] strStart = new char[] { '\'', '"' };
            char objStart = '{';
            char arrStart = '[';
            char[] nullStart = new char[] { 'n', 'N' };
            char negStart = '-';


            if (document[pointer] == ':')
                pointer++;
            char valStart = document[pointer];
            
            if (boolStart.Contains(valStart) && IsBool(document))
            {
                var substr = document.Substring(document.IndexOfAny(new char[] { ',', '}' }));
            }

           
            return (TokenType.String, 0);
        }

        private bool IsBool(string document) => Regex.IsMatch(document, @"^[Tt]rue") || Regex.IsMatch(document, @"^[Ff]alse");

        private TokenType GetNextTagType(string document, int pointer)
        {
            return document[pointer] == '{' ? TokenType.Object : TokenType.Array;
        }

        private bool ValidateDocument(string document)
        {
            return true;
        }



        private (string tag, int ptr) GetNextTagValue(string document, int pointer)
        {
            var firstQuot = document.IndexOf('\'') < 0 ? int.MaxValue : document.IndexOf('\'');
            var firstDblQuot = document.IndexOf('"') < 0 ? int.MaxValue : document.IndexOf('\'');
            
            pointer = Math.Min(firstQuot, firstDblQuot);
            var subDoc = document.Substring(++pointer);
            if (firstQuot < firstDblQuot)
            {
                return (subDoc.Substring(0, subDoc.IndexOf('\'')), pointer + subDoc.IndexOf('\''));
            }
            else
            {
                return (subDoc.Substring(0, subDoc.IndexOf('"')), pointer + subDoc.IndexOf('"'));
            }
        }

        private bool LookForMatchingTag(char openingTag, string document)
        {
            if (openingTag != document[0])
                throw new LexException($"Invalid opening tag passed {openingTag} where (sub)document starts with {document[0]}");
            var tagStack = new Stack<Tag>();
            foreach (char c in document)
            {
                var peek = tagStack.Count > 0 ? tagStack.Peek() : new Tag { Opening = ' ', Closing = ' '};

                if (Tags.Select(t => t.Opening).Contains(c) && peek.Opening != '"' && peek.Opening != '\'')
                {
                    tagStack.Push(Tags.First(t => t.Opening == c));
                }
                else if (Tags.Select(t => t.Closing).Contains(c) && peek.Closing == c)
                {
                    tagStack.Pop();
                }
                else if (Tags.Select(t => t.Closing).Contains(c) && peek.Closing != c)
                {
                    throw new LexException($"Invalid closing tag found {c} to match {tagStack.Peek()}");
                }


            }

            return tagStack.Count() == 0;
        }
    }
}
