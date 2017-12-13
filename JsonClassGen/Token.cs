using System;
using System.Collections.Generic;
using System.Text;

namespace JsonClassGen
{
    public class Token
    {
        public TokenType Type { get; set; }
        public string Value { get; set; }
        public override string ToString()
        {
            return $"{{{Value} : {Type.ToString()}}}";
        }
    }
}
