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

            if (Tags.Select(t => t.Opening).Contains(document[0]) && LookForMatchingTag(document[0], document))
            {
                var pointer = 0;
                var thing = GetTagValue(document, pointer);
            }
            else
            {
                throw new LexException($"Found an invalid token : {document[0]}");
            }
            
            return null;
        }

        private (string, int) GetTagValue(string document, int pointer)
        {
            var firstQuot = document.IndexOf('\'') < 0 ? int.MaxValue : document.IndexOf('\'');
            var firstDblQuot = document.IndexOf('"') < 0 ? int.MaxValue : document.IndexOf('\'');
            
            pointer = Math.Min(firstQuot, firstDblQuot);
            var subDoc = document.Substring(pointer + 1);
            if (document[pointer] == '\'')
            {
                return (subDoc.Substring(0, subDoc.IndexOf('\'')), subDoc.IndexOf('\''));
            }
            else
            {
                return subDoc.Substring(0, subDoc.IndexOf('"')), subDoc.IndexOf('\'');
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
