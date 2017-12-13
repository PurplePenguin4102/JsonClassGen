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
            var Bunny = new CodeGenerator().GenerateCodeFile(@"{'cat' : true, 'dog' : 5.2, 'emu' : ['1', '2', '3'], 'id' : 1}");
        }
    }
}
