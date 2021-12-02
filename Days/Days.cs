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

  #region Day1: Solved! 

  public static string Day1()
  {
    var input = File.ReadAllLines(Path.Combine(InputBasePath, "Day1.txt")).Select(x => int.Parse(x)).ToArray();

    var start = 0; var current = 0; var p1 = 0; var p2 = 0;

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

  #endregion

  #region Day2: Solved!

  public static string Day2()
  {
    var input = File.ReadAllLines(Path.Combine(InputBasePath, "Day2.txt")).ToArray();

    var x1 = 0; var y1 = 0;
    var x2 = 0; var y2 = 0; var aim = 0;

    foreach (var line in input)
    {
      var split = line.Split(' ');
      var num = int.Parse(split[1]);
      switch (split[0])
      {
        case "forward":
          {
            x1 += num;
            x2 += num;
            y2 += aim * num;
          }
          break;
        case "down":
          {
            y1 += num;
            aim += num;
          }
          break;
        case "up":
          {
            y1 -= num;
            aim -= num;
          }
          break;
      }
    }

    return OutputResult((x1 * y1).ToString(), (x2 * y2).ToString());
  }

  #endregion
}