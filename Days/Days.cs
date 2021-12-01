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
    var testInput = @"199
200
208
210
200
207
240
269
260
263";

    var input = //testInput.Split(Environment.NewLine).Select(x => int.Parse(x)).ToArray(); //
    File.ReadAllLines(Path.Combine(InputBasePath, "Day1.txt")).Select(x => int.Parse(x)).ToArray();

    var start = 0;
    var current = 0;
    var p1 = 0;
    var p2 = 0;

    for (var idx = 0; idx < input.Length; idx++)
    {
      current = input[idx];

      if (current > start && start > 0) p1++;

      if (idx < input.Length - 3)
      {
        //we still have enough to calculate B.
        var a = input[idx] + input[idx + 1] + input[idx + 2];
        var b = input[idx + 1] + input[idx + 2] + input[idx + 3];

        if (b > a) p2++;
      }

      start = current;
    }

    return OutputResult(p1.ToString(), p2.ToString());
  }
}