using System;
using System.Collections.Generic;
using System.Text;

namespace JsonClassGen
{
    internal class LexException : Exception
    {
        internal LexException(string msg) : base(msg) { }
    }
}
