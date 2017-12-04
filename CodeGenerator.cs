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
                string tagValue = GetTagValue(document);
            }
            else
            {
                throw new LexException($"Found an invalid token : {document[0]}");
            }
            return null;
        }

        private string GetTagValue(string document)
        {
            var tagStack = new Stack<char>();
            var tagContents = new StringBuilder();
            foreach (char c in document)
            {
                char peek = tagStack.Count > 0 ? tagStack.Peek() : ' '; // all spaces were removed
                if (tagStack.Count > 0)
                {
                    tagContents.Append(c);
                }
                if (OpeningTags.Contains(c) && peek != '"' && peek != '\'')
                {
                    tagStack.Push(c);
                }
                else if (ClosingTags.Contains(c) && peek == c)
                {
                    tagStack.Pop();
                }
                if (tagStack.Count == 0)
                {
                    tagContents.Remove(tagContents.Length - 1, 1);
                    break;
                }
            }
            return tagContents.ToString();
        }

        private bool LookForMatchingTag(char openingTag, string document)
        {
            if (openingTag != document[0])
                throw new LexException($"Invalid opening tag passed {openingTag} where (sub)document starts with {document[0]}");
            var tagStack = new Stack<char>();
            foreach (char c in document)
            {
                char peek = tagStack.Count > 0 ? tagStack.Peek() : ' '; // all spaces were removed

                if (OpeningTags.Contains(c) && peek != '"' && peek != '\'')
                {
                    tagStack.Push(c);
                }
                else if (ClosingTags.Contains(c) && peek == c)
                {
                    tagStack.Pop();
                }
                else if (ClosingTags.Contains(c) && peek != c)
                {
                    throw new LexException($"Invalid closing tag found {c} to match {tagStack.Peek()}");
                }


            }

            return tagStack.Count() == 0;
        }
    }
}
