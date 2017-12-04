using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace JsonClassGen
{
    public class CodeGenerator
    {
        private string[] ClassNameStarts = new string[] { "Foo", "Fi", "Fo", "Fum", "Fa", "Fe" };
        private string[] ClassNameEnds = new string[] { "Bar", "Baz", "Boz", "Biz", "Buz", "Bez" };
        private char[] OpeningTags = new char[] { '[', '{', '\'', '"' };
        private char[] ClosingTags = new char[] { ']', '}', '\'', '"' };

        public object GenerateCodeFile(string jsonDocument)
        {
            var noSpaces = Regex.Replace(jsonDocument, @"\s+", "");
            var tokens = Tokenize(jsonDocument);
            return null;
        }

        private List<Token> Tokenize(string document)
        {
            var tokens = new List<Token>();

            if (OpeningTags.Contains(document[0]) && LookForMatchingTag(document[0], document))
            {
                
            }
            else
            {
                throw new LexException($"Found an invalid token : {document[0]}");
            }
            return null;
        }

        private bool LookForMatchingTag(char openingTag, string document)
        {
            if (openingTag != document[0])
                throw new LexException($"Invalid opening tag passed {openingTag} where (sub)document starts with {document[0]}");
            var tagStack = new Stack<char>();
            foreach (char c in document)
            {
                if (OpeningTags.Contains(c))
                {
                    tagStack.Push(c);
                }
                else if (ClosingTags.Contains(c) && tagStack.Peek() == c)
                {
                    tagStack.Pop();
                }
                else
                {
                    throw new LexException($"Invalid closing tag found {c} to match {tagStack.Peek()}");
                }
            }

            return tagStack.Count() == 0;
        }
    }
}
