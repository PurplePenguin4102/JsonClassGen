using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JsonClassGen
{
    public class Tokenizer
    {
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

        public List<Token> Tokenize(string document)
        {

            var tokens = new List<Token>();

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
                    pointer++;
                    if (document[pointer] != ':')
                    {
                        throw new LexException($"Unexpected Character -- at position -- ");
                    }

                    var val = GetTokenType(document, pointer);
                    pointer = val.ptr;
                    var tokenType = val.tagType;
                    var token = new Token
                    {
                        Value = tokenKey,
                        Type = tokenType
                    };
                    tokens.Add(token);
                    if (document[pointer] == ',')
                    {
                        continue;
                    }
                    else if (document[pointer] == '}')
                    {
                        break;
                    }
                }
            }
            return tokens;
        }

        private char[] strStart = new char[] { '\'', '"' };
        public (TokenType tagType, int ptr) GetTokenType(string document, int pointer)
        {
            char[] boolStart = new char[] { 't', 'T', 'f', 'F' };
            char objStart = '{';
            char arrStart = '[';
            char[] nullStart = new char[] { 'n', 'N' };
            char negStart = '-';
            var tokenType = TokenType.Null;

            if (document[pointer] == ':')
                pointer++;
            var valStart = document[pointer];
            var docFromPtr = document.Substring(pointer);
            var ptrStart = pointer;
            if (boolStart.Contains(valStart) && IsBool(docFromPtr))
            {
                tokenType = TokenType.Boolean;
                pointer += FindNextDivider(docFromPtr);
            }
            else if (strStart.Contains(valStart))
            {
                tokenType = TokenType.String;
                pointer += FindStringEnd(docFromPtr) + 1;
                var value = document.Substring(ptrStart + 1, pointer - ptrStart - 2);
                if (DateTime.TryParse(value, out var _))
                {
                    tokenType = TokenType.DateTime;
                }
            }
            else if (nullStart.Contains(valStart))
            {
                tokenType = TokenType.Null;
                pointer += FindNextDivider(docFromPtr);
            }
            else if (valStart == objStart)
            {
                tokenType = TokenType.Object;
                pointer += FindObjectEnd(docFromPtr) + 1;
            }
            else if (valStart == arrStart)
            {
                tokenType = TokenType.Array;
                pointer += FindArrayEnd(docFromPtr) + 1;
            }
            else if (char.IsDigit(valStart) || valStart == negStart)
            {
                pointer += FindNextDivider(docFromPtr);
                var value = document.Substring(ptrStart, pointer - ptrStart);
                
                if (int.TryParse(value, NumberStyles.Integer, new CultureInfo("en-US"), out var _))
                {
                    tokenType = TokenType.Int;
                }
                else if (decimal.TryParse(value, NumberStyles.Float, new CultureInfo("en-US"), out var _))
                {
                    tokenType = TokenType.Decimal;
                }
                else
                {
                    throw new LexException("unknown number type");
                }
            }

            return (tokenType, pointer);
        }

        public int FindStringEnd(string subStr)
        {
            return subStr[0] == '\'' ? subStr.IndexOf('\'', 1) : subStr.IndexOf('"', 1);
        }

        public int FindArrayEnd(string subStr)
        {
            return FindContainerEnd(subStr, ContainerType.Array);
        }

        public int FindObjectEnd(string subStr)
        {
            return FindContainerEnd(subStr, ContainerType.Object);
        }

        private enum ContainerType
        {
            Array,
            Object
        }

        private int FindContainerEnd(string subStr, ContainerType ct)
        {
            var inString = false;
            char strType = '\0';
            var endChar = ct == ContainerType.Array ? ']' : '}';
            for (int i = 0; i < subStr.Length; i++)
            {
                var ch = subStr[i];
                if (inString && ch == strType)
                {
                    inString = false;
                    continue;
                }
                if (strStart.Contains(ch))
                {
                    inString = true;
                    strType = ch;
                    continue;
                }
                if (!inString && ch == endChar)
                {
                    return i;
                }
            }
            throw new LexException("Unexpected end of file");
        }

        public int FindNextDivider(string subStr)
        {
            return subStr.IndexOfAny(new char[] { '}', ',' });
        }

        public bool IsBool(string document) => Regex.IsMatch(document, @"^[Tt]rue[,}]") || Regex.IsMatch(document, @"^[Ff]alse[,}]");

        public TokenType GetNextTagType(string document, int pointer)
        {
            return document[pointer] == '{' ? TokenType.Object : TokenType.Array;
        }

        public (string tag, int ptr) GetNextTagValue(string document, int pointer)
        {
            var docFromPtr = document.Substring(pointer);
            var firstQuot = docFromPtr.IndexOf('\'') < 0 ? int.MaxValue : docFromPtr.IndexOf('\'');
            var firstDblQuot = docFromPtr.IndexOf('"') < 0 ? int.MaxValue : docFromPtr.IndexOf('"');

            pointer += Math.Min(firstQuot, firstDblQuot);
            docFromPtr = document.Substring(++pointer);
            if (firstQuot < firstDblQuot)
            {
                return (docFromPtr.Substring(0, docFromPtr.IndexOf('\'')), pointer + docFromPtr.IndexOf('\''));
            }
            else
            {
                return (docFromPtr.Substring(0, docFromPtr.IndexOf('"')), pointer + docFromPtr.IndexOf('"'));
            }
        }

        public bool LookForMatchingTag(char openingTag, string document)
        {
            if (openingTag != document[0])
                throw new LexException($"Invalid opening tag passed {openingTag} where (sub)document starts with {document[0]}");
            var tagStack = new Stack<Tag>();
            foreach (char c in document)
            {
                var peek = tagStack.Count > 0 ? tagStack.Peek() : new Tag { Opening = ' ', Closing = ' ' };

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
