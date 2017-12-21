using System;
using JsonClassGen;

namespace TestProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            var rwInput = @"  {
    ""Id"": 0,
    ""Value"": ""string"",
    ""AttributeId"": 0,
    ""CreatedDate"": ""2017-12-20T06:06:38.292Z"",
    ""ModifiedDate"": ""2017-12-20T06:06:38.292Z"",
    ""CreatedBy"": 0,
    ""ModifiedBy"": 0
  }";
            var tag = "{\"cat\":\"dog\"}";
            Console.WriteLine("Hello World!");
            new CodeGenerator().GenerateCodeFileAsync(rwInput).Wait();
        }
    }
}
