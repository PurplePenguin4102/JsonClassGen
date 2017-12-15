using System;
using System.Collections.Generic;
using System.Text;

namespace JsonClassGen
{
    public class Emitter
    {
        private readonly string[] ClassNameStarts = new string[] { "Foo", "Fi", "Fo", "Fum", "Fa", "Fe" };
        private readonly string[] ClassNameEnds = new string[] { "Bar", "Baz", "Boz", "Biz", "Buz", "Bez" };

        public void EmitClass(List<Token> tokens)
        {
            var property = string.Format(PropertyFormat, "Billy", "Bob");
            var classFill = string.Format(ClassFormat, RandomClassName(), property);
            var codeFile = string.Format(CodeFileFormat, classFill);
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
        private string PropertyFormat = "public {0}? {1} {{ get; set; }}";
    }
}
