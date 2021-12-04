using System;
using System.Collections.Generic;
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

      if (current > start && start > 0)
        p1++;

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

  #region Day3: Solved!

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
      if (o2Source.Count() > 1)
      {
        var mostSignificant = ReturnMostSignificantBit(o2Source, x);
        o2Source = o2Source.Where(line => line[x] == mostSignificant).ToArray();
      }

      if (co2Source.Count() > 1)
      {
        var leastSignificant = ReturnLeastSignificantBit(co2Source, x);
        co2Source = co2Source.Where(line => line[x] == leastSignificant).ToArray();
      }
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

  #region Day4: WIP
  public static string Day4()
  {

    var testinput = @"7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1

22 13 17 11  0
 8  2 23  4 24
21  9 14 16  7
 6 10  3 18  5
 1 12 20 15 19

 3 15  0  2 22
 9 18 13 17  5
19  8  7 25 23
20 11 10 24  4
14 21 16 12  6

14 21 17 24  4
10 16 15  9 19
18  8 23 26 20
22 11 13  6  5
 2  0 12  3  7";

    var input = testinput.Split(Environment.NewLine).ToArray();

    var bingo = new BingoDay4(input);

    bingo.Part1();

    return OutputResult();
  }

  public class BingoDay4
  {
    public int[] BingoNumbers { get; private set; }

    public List<BingoBoard> BingoBoards { get; private set; }

    public int Part1()
    {
      foreach (var num in BingoNumbers)
      {
        foreach (var board in BingoBoards)
        {
          var target = board.Board.FirstOrDefault(x => x.Number == num);
          if (target != null)
          {
            System.Console.WriteLine($"Board {board.BoardNumber} has {num}!");
            target.Crossed = true;
          }

          //TODO: After each number was drawn, we should check all boards to see if they have 5 in a row, either on the X coordinate or on the Y coordinate.
        }
      }

      throw new NotImplementedException();
    }

    public BingoDay4(string[] input)
    {
      BingoBoards = new List<BingoBoard>();

      BingoNumbers = input[0].Split(',').Select(x => int.Parse(x)).ToArray();

      var boards = new Queue<string>(input.Skip(1));

      var currentBoardLines = new List<string>();
      var number = 1;

      while (boards.Any())
      {
        var line = boards.Dequeue();

        if (line == string.Empty) //Either we've encountered the start of a new board, or the end of the current board.
        {
          if (currentBoardLines.Any())
          {
            BingoBoards.Add(new BingoBoard(currentBoardLines.ToArray(), number++));
            currentBoardLines = new List<string>();
          }
        }
        else
        {
          currentBoardLines.Add(line);
        }
      }

      BingoBoards.Add(new BingoBoard(currentBoardLines.ToArray(), number++));
    }
  }

  public class BingoBoard
  {
    public List<BingoCoordinate> Board { get; set; }

    public int BoardNumber { get; set; }

    public BingoBoard(string[] input, int boardNumber)
    {
      Board = new List<BingoCoordinate>();
      BoardNumber = boardNumber;

      for (var y = 0; y < input.Length; y++)
      {
        var split = input[y].Split(' ', options: StringSplitOptions.RemoveEmptyEntries);

        for (var x = 0; x < split.Length; x++)
        {
          Board.Add(new BingoCoordinate(y, x, int.Parse(split[x])));
        }
      }
    }
  }

  public class BingoCoordinate
  {
    public int Y { get; private set; }
    public int X { get; private set; }
    public int Number { get; private set; }
    public bool Crossed { get; set; }

    public BingoCoordinate(int y, int x, int number)
    {
      Y = y;
      X = x;
      Number = number;
    }
  }
  #endregion
}