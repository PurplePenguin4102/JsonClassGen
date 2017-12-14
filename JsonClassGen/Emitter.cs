using System;
using System.Collections.Generic;
using System.Text;

namespace JsonClassGen
{
    public class Emitter
    {
        private readonly string[] ClassNameStarts = new string[] { "Foo", "Fi", "Fo", "Fum", "Fa", "Fe" };
        private readonly string[] ClassNameEnds = new string[] { "Bar", "Baz", "Boz", "Biz", "Buz", "Bez" };

        public void Emit(List<Token> tokens)
        {
            
        }

        private string CodeFile =
$@"
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonClassGen
{{
    {0}
}}
";
    }
}
