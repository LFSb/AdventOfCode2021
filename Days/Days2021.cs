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

  #region Day4: Solved!
  public static string Day4()
  {
    var input = File.ReadAllLines(Path.Combine(InputBasePath, "Day4.txt")).ToArray();

    var bingo = new BingoDay4(input);

    Console.Clear();
    bingo.PlayBingo(out var p1, out var p2);
    Console.Clear();

    return OutputResult(p1.ToString(), p2.ToString());
  }

  public class BingoDay4
  {
    public int[] BingoNumbers { get; private set; }

    public List<BingoBoard> BingoBoards { get; private set; }

    public void Visualize(string message = null)
    {
      var consolewidth = Console.WindowWidth;
      int boardWidth = 18;
      int boardHeight = 7;

      int boardsPerRow = 18;

      foreach (var board in BingoBoards)
      {
        var bingoBoard = board.Board;
        var boardIndex = BingoBoards.IndexOf(board);
        var row = boardIndex / boardsPerRow;

        for (var y = 0; y < bingoBoard.Max(c => c.Y) + 1; y++)
        {
          Console.SetCursorPosition((int)boardWidth * (boardIndex % boardsPerRow), y + (row * boardHeight));

          for (var x = 0; x < bingoBoard.Max(c => c.X) + 1; x++)
          {
            var coord = bingoBoard.First(c => c.X == x && c.Y == y);

            if (coord.Crossed && !board.Solved) Console.ForegroundColor = ConsoleColor.Red;
            if (board.Solved) Console.ForegroundColor = ConsoleColor.Green;

            Console.Write(coord.Number.ToString().PadLeft(2, ' '));

            Console.ForegroundColor = ConsoleColor.White;

            Console.Write(' ');
          }

          Console.WriteLine();
        }
      }

      System.Console.WriteLine();
      System.Console.WriteLine(message);

      // System.Threading.Thread.Sleep(10);
    }

    public void PlayBingo(out int p1, out int p2)
    {
      p1 = 0; p2 = 0;

      foreach (var num in BingoNumbers)
      {
        Visualize($"{num} was pulled.");

        foreach (var board in BingoBoards)
        {
          var target = board.Board.FirstOrDefault(x => x.Number == num);

          if (target != null)
          {
            target.Crossed = true;
          }

          var bingo = board.Bingo();

          if (bingo != 0)
          {
            if (p1 == 0) p1 = bingo * num;

            board.Solved = true;
          }

          if (BingoBoards.All(x => x.Solved))
          {
            System.Console.WriteLine($"Board #{board.BoardNumber} was solved last with num {num} being pulled.");

            p2 = board.Board.Where(c => !c.Crossed).Sum(c => c.Number) * num;
          }

          if (p1 != 0 && p2 != 0) return;
        }
      }
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

    public bool Solved { get; set; }

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

    //A board has a bingo if there's N X or Y coordinates in a row that are Crossed.
    public int Bingo()
    {
      var sum = 0;

      for (var idx = 0; idx < Board.Max(coord => coord.X) + 1; idx++)
      {
        var allVerticals = Board.Where(coord => coord.X == idx);
        var allHorizontals = Board.Where(coord => coord.Y == idx);

        if (allVerticals.All(coord => coord.Crossed) || allHorizontals.All(coord => coord.Crossed))
        {
          sum = Board.Where(c => !c.Crossed).Sum(coord => coord.Number);
          Solved = true;
        }
      }

      return sum;
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

  #region Day5: Solved!

  public static string Day5()
  {
    var testinput = @"0,9 -> 5,9
8,0 -> 0,8
9,4 -> 3,4
2,2 -> 2,1
7,0 -> 7,4
6,4 -> 2,0
0,9 -> 2,9
3,4 -> 1,4
0,0 -> 8,8
5,5 -> 8,2".Split(Environment.NewLine).ToArray();

    var gridSize = 999;
    var input = File.ReadAllLines(Path.Combine(InputBasePath, "Day5.txt")).ToArray();

    var grid = new Day5Grid(gridSize, input);
    var p1 = grid.ProcessInstructions(false);

    var p2grid = new Day5Grid(gridSize, input);
    var p2 = p2grid.ProcessInstructions(true);

    return OutputResult(p1, p2);
  }

  public class Day5Grid
  {
    public int[][] Grid { get; private set; }

    public Queue<string> Instructions { get; private set; }

    public Day5Grid(int gridSize, string[] instructions)
    {
      Grid = new int[gridSize][];

      for (var x = 0; x < gridSize; x++) Grid[x] = new int[gridSize];

      Instructions = new Queue<string>(instructions);
    }

    public void Visualize()
    {
      for (var x = 0; x < Grid.Length; x++)
      {
        for (var y = 0; y < Grid[x].Length; y++)
        {
          Console.SetCursorPosition(x, y);
          var val = Grid[x][y];
          Console.Write(val == 0 ? "." : val.ToString());
        }
      }
      Console.ReadLine();
    }

    public string ProcessInstructions(bool part2, bool visualize = false)
    {
      if (visualize) Console.Clear();

      while (Instructions.Any())
      {
        ProcessInstruction(Instructions.Dequeue(), part2);

        if (visualize)
        {
          Visualize();
        }
      }

      return Grid.Sum(x => x.Count(y => y > 1)).ToString();
    }

    private int DetermineOrientation(int x1, int x2, int y1, int y2)
    {
      int orientation;

      if (x1 > x2 && y1 > y2) //if both source points are greater than the targets, we're moving diagonally to bottom left. Example: 3,3 -> 1,1. This will cover 3,3, 2,2 and then finally 1,1.
      {
        orientation = 3;
      }
      else if (x1 < x2 && y1 < y2) //if both source points are smaller than the targets, we're moving diagonally to top right. Example, 1,1 -> 3,3. This will cover 1,1, 2,2 and then finally 3,3.
      {
        orientation = 1;
      }
      else if (x1 > x2 && y1 < y2) //If x1 is greater than x2, but y1 is smaller than y2, we're moving diagonally to top left. Example, 3,1 -> 1,3. This will cover 3,1, 2,2 and then 1,3.
      {
        orientation = 4;
      }
      else if (x1 < x2 && y1 > y2) //if x1 is smaller than x2, but y1 is greather than y2, we're moving diagonally to bottom right. Example, 1,3 -> 3,1. This will cover 1,3, 2,2 and then 3,1.
      {
        orientation = 2;
      }
      else
      {
        orientation = -1; //If we turn up here, we've obviously fucked up.
      }

      return orientation;
    }

    private void ProcessInstruction(string instruction, bool part2)
    {
      var split = instruction.Split("->", StringSplitOptions.TrimEntries);

      var source = split[0].Split(',');

      var target = split[1].Split(',');

      var x1 = int.Parse(source[0]); var y1 = int.Parse(source[1]);
      var x2 = int.Parse(target[0]); var y2 = int.Parse(target[1]);

      if (x1 == x2 || y1 == y2) //For part 1, only consider instructions where we're going vertical, or horizontal.
      {
        var orientation = y1 == y2 ? true : false; //orientation being true means we're going horizontal, otherwise we're moving vertical.

        while (true)
        {
          //first: we're going to mark the current spot (x1, y1)
          Grid[x1][y1]++;

          //Then, we should increment (or decrement) x1 or y1 (depending on orientation).

          if (x1 == x2 && y1 == y2) break;

          if (orientation)
          {
            x1 += x1 > x2 ? -1 : 1;
          }
          else
          {
            y1 += y1 > y2 ? -1 : 1;
          }
        }
      }
      else if (part2)
      {
        var orientation = DetermineOrientation(x1, x2, y1, y2);

        while (true) //Here, we should traverse diagonally. 1,1 -> 3,3 covers points 1,1, 2,2 and 3,3. Basically, we should do the same in the case of vertical/horizontal movement, only moving in both directions at the same time.
        {
          Grid[x1][y1]++;

          if (x1 == x2 && y1 == y2) break;

          switch (orientation)
          {
            case 1:
              {
                x1++; y1++;
              }
              break;
            case 2:
              {
                x1++; y1--;
              }
              break;
            case 3:
              {
                x1--; y1--;
              }
              break;
            case 4:
              {
                x1--; y1++;
              }
              break;
            default:
              {
                throw new Exception("Nope.");
              }
          }
        }
      }
    }
  }
  #endregion

  #region Day6: Solved!

  public static string Day6()
  {
    var input = File.ReadAllText(Path.Combine(InputBasePath, "Day6.txt")).Split(',').Select(x => int.Parse(x)).ToArray();

    var p1 = CalculateLanternFish(input, 80);

    var p2 = CalculateLanternFish(input, 256);

    return OutputResult(p1.ToString(), p2.ToString());
  }

  private static long CalculateLanternFish(int[] initialState, int target)
  {
    var registers = new long[9];

    foreach (var item in initialState)
    {
      registers[item]++;
    }

    for (var day = 0; day < target; day++)
    {
      long tmp = registers[0];

      for (var idx = 1; idx < registers.Length; idx++)
      {
        registers[idx - 1] = registers[idx];
      }

      registers[6] += tmp;
      registers[8] = tmp;
    }

    return registers.Sum(x => x);
  }

  #endregion
}