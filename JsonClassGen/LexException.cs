using System;
using System.Collections.Generic;
using System.Text;

namespace JsonClassGen
{
    public class LexException : Exception
    {
        internal LexException(string msg) : base(msg) { }
    }
}
