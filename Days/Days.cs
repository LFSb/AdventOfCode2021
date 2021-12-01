using System;
using System.IO;
using System.Linq;

public static partial class Days
{
  private const string InputBasePath = @"Days/Input/";

  private static string OutputResult(string part1 = "", string part2 = "")
  {
    return $"{Environment.NewLine}- Part 1: {part1}{Environment.NewLine}- Part 2: {part2}";
  }

  public static string Day1()
  {
//     var testInput = @"199
// 200
// 208
// 210
// 200
// 207
// 240
// 269
// 260
// 263";

    var input = File.ReadAllLines(Path.Combine(InputBasePath, "Day1.txt")).Select(int.Parse);

    var start = 0;
    var current = 0;
    var increases = 0;

    foreach (var line in input)
    {
      current = line;

      if (current > start && start > 0) increases++;

      start = current;
    }

    return OutputResult(increases.ToString(), "day2");
  }
}