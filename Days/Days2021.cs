using System;
using System.IO;
using System.Linq;

public static partial class Days2021
{
  private const string InputBasePath = @"Days/Input/2021";

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

  #region Day3: Complete

  public static string Day3()
  {
    var input = File.ReadAllLines(Path.Combine(InputBasePath, "Day3.txt")).Select(x => x.ToCharArray()).ToArray();

    var inputLength = input.First().Length;
    var gamma = new char[inputLength]; var epsilon = new char[inputLength];

    var o2Source = input.ToArray(); var co2Source = input.ToArray();

    for (var x = 0; x < inputLength; x++)
    {
      gamma[x] = ReturnMostSignificantBit(input, x);
      epsilon[x] = gamma[x] == '1' ? '0' : '1'; //The least sigificant bit is the opposite of the most significant bit. There's probably a quicker way to calculate this, wonder if we need that for pt2...

      //pt2
      var mostSignificant = ReturnMostSignificantBit(o2Source, x);
      var leastSignificant = ReturnLeastSignificantBit(co2Source, x);

      if (o2Source.Count() != 1) o2Source = o2Source.Where(line => line[x] == mostSignificant).ToArray();
      if (co2Source.Count() != 1) co2Source = co2Source.Where(line => line[x] == leastSignificant).ToArray();
    }

    var decimalGamma = Convert.ToInt32(new string(gamma), 2);
    var decimalEpsilon = Convert.ToInt32(new string(epsilon), 2);

    var o2Rating = Convert.ToInt32(new string(o2Source.First()), 2);
    var co2Rating = Convert.ToInt32(new string(co2Source.First()), 2);

    return OutputResult((decimalGamma * decimalEpsilon).ToString(), (o2Rating * co2Rating).ToString());
  }

  //if the occurance of 1's is more than half of the list of inputs, we can tell that 1 is the most significant bit. If it's not, 0 is.
  private static char ReturnMostSignificantBit(char[][] input, int idx)
  {
    var count = input.Count(line => line[idx] == '1');
    var benchmark = Math.Ceiling(input.Length / (decimal)2);

    if (count == benchmark) return '1';

    return (count > benchmark) ? '1' : '0';
  }

  private static char ReturnLeastSignificantBit(char[][] input, int idx)
  {
    var count = input.Count(line => line[idx] == '1');
    var benchmark = Math.Ceiling(input.Length / (decimal)2);

    if (count == benchmark) return '0';

    return (count > benchmark) ? '0' : '1';
  }

  #endregion
}