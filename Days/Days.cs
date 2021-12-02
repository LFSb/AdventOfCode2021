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

  #region Day2: WIP

  public static string Day2()
  {
var test = @"forward 5
down 5
forward 8
up 3
down 8
forward 2";

    var input = File.ReadAllLines(Path.Combine(InputBasePath, "Day2.txt")).ToArray();

    var x = 0; var y = 0;

    foreach(var line in input)
    {
      var split = line.Split(' ');

      switch(split[0])
      {
        case "forward": x+= int.Parse(split[1]); break;
        case "down": y+= int.Parse(split[1]); break;
        case "up": y-= int.Parse(split[1]); break;
      }
    }

    return OutputResult((x * y).ToString());
  }

  #endregion
}