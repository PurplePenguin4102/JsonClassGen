using System;
using JsonClassGen;

namespace TestProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            var tag = "{\"cat\":\"dog\"}";
            Console.WriteLine("Hello World!");
            var Bunny = new CodeGenerator().GenerateCodeFile(@"{'cat' : 'dog'}");
        }
    }
}
