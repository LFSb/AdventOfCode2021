using System;
using System.Diagnostics;

namespace AdventOfCode2021
{
  class Program
  {
    static void Main(string[] args)
    {
      var sw = new Stopwatch();
      
      sw.Start();
      Console.WriteLine(Days2021.Day7());
      sw.Stop();

      System.Console.WriteLine($"Took: {sw.ElapsedMilliseconds} ms...");
    }
  }
}