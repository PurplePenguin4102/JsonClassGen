﻿using System;
using JsonClassGen;

namespace TestProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var Bunny = new CodeGenerator().GenerateCodeFile(@"{'cat' : 'dog'}");
        }
    }
}
