using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static partial class Days2020
{
  private const string InputBasePath = @"Days/Input/2020";

  private static string OutputResult(string part1 = "", string part2 = "")
  {
    return $"{Environment.NewLine}- Part 1: {part1}{Environment.NewLine}- Part 2: {part2}";
  }

  #region Day1: Solved!

  public static string Day1()
  {
    var target = 2020;
    var input = File.ReadAllLines(Path.Combine(InputBasePath, "Day1.txt")).Select(int.Parse);
    var inputStack = new Stack<int>(input);

    var part1 = string.Empty; //For part 1, we need to find a pair of numbers that together sum to 2020.

    while (part1 == string.Empty)
    {
      var current = inputStack.Pop();

      if (current.CanSumTo(target, inputStack, out var candidate))
      {
        part1 = (current * candidate).ToString();
      }
    }

    var part2 = string.Empty; //For part 2, we need to find three numbers that together sum to 2020.

    inputStack = new Stack<int>(input);

    while (part2 == string.Empty)
    {
      var current = inputStack.Pop();

      foreach (var next in inputStack.ToArray())
      {
        if ((current + next).CanSumTo(target, new Stack<int>(inputStack.Except(new[] { next })), out var candidate))
        {
          part2 = (current * next * candidate).ToString();
        }
      }
    }

    return OutputResult(part1, part2);
  }

  private static bool CanSumTo(this int source, int target, IEnumerable<int> numbers, out int candidate)
  {
    candidate = numbers.FirstOrDefault(x => x == (target - source));
    return candidate != 0;
  }

  #endregion

  #region Day2: Done!

  public static string Day2()
  {
    var inputs = File.ReadAllLines(Path.Combine(InputBasePath, "Day2.txt"));

    var rules = inputs.Select(input => new Day2Rule(input));

    var part1 = rules.Count(x => x.IsValidP1());

    var part2 = rules.Count(x => x.IsValidP2());

    return OutputResult(part1.ToString(), part2.ToString());
  }

  private class Day2Rule
  {
    public int Min { get; set; }

    public int Max { get; set; }

    public char Rule { get; set; }

    public string PassWord { get; set; }

    public Day2Rule(string input)
    {
      var minMax = input.Substring(0, input.IndexOf(' ')).Split('-');

      Min = int.Parse(minMax[0]);
      Max = int.Parse(minMax[1]);
      Rule = input[input.IndexOf(' ') + 1];
      PassWord = input.Substring(input.IndexOf(':') + 2);
    }

    public bool IsValidP1()
    {
      var count = PassWord.Count(x => x == Rule);

      return count >= Min && count <= Max;
    }

    public bool IsValidP2()
    {
      var min = PassWord[Min - 1] == Rule;
      var max = PassWord[Max - 1] == Rule;
      return min ^ max;
    }
  }

  #endregion

  #region Day3: Solved!

  public static string Day3()
  {
    var input = File.ReadAllLines(Path.Combine(InputBasePath, "Day3.txt"));

    var steps = new[]
    {
      new Tuple<int, int>(1, 1),
      new Tuple<int, int>(3, 1),
      new Tuple<int, int>(5, 1),
      new Tuple<int, int>(7, 1),
      new Tuple<int, int>(1, 2)
    };

    var d3grid = new Day3Grid(input, input.Length * steps.Max(x => x.Item1));

    var yStep = 1;
    var xStep = 3;

    return OutputResult(d3grid.P1(yStep, xStep).ToString(), d3grid.P2(steps).ToString());
  }

  private class Day3Grid
  {
    public bool[][] Grid { get; private set; }

    private int CurrentX { get; set; }

    private int CurrentY { get; set; }

    public Day3Grid(string[] inputs, int maxWidth)
    {
      Grid = new bool[inputs.Length][];

      var xdepth = inputs[0].Length;

      for (var y = 0; y < inputs.Length; y++)
      {
        for (var x = 0; x < maxWidth; x++)
        {
          if (Grid[y] == null)
          {
            Grid[y] = new bool[maxWidth];
          }

          Grid[y][x] = inputs[y][x % xdepth] == '#';
        }
      }
    }

    public int P1(int ystep, int xstep)
    {
      var count = 0;

      while (CurrentY < Grid.Length - 1)
      {
        CurrentY += ystep;
        CurrentX += xstep;

        if (Grid[CurrentY][CurrentX])
        {
          count++;
        }
      }

      //Reset the X/Y values for the next part.

      CurrentY = 0;
      CurrentX = 0;

      return count;
    }

    public int P2(Tuple<int, int>[] inputs)
    {
      var result = 1; //To avoid multiplying by 0..

      foreach (var input in inputs)
      {
        result *= P1(input.Item2, input.Item1);
      }

      return result;
    }
  }

  #endregion

  #region Day4: Solved!
  public static string Day4()
  {
    var input = File.ReadAllText(Path.Combine(InputBasePath, "Day4.txt"));

    var passports = input.Split(new[] { $"{Environment.NewLine}{Environment.NewLine}" }, StringSplitOptions.RemoveEmptyEntries);

    var parsedPassports = new List<Day4Passport>();

    foreach (var passport in passports)
    {
      parsedPassports.Add(new Day4Passport(passport));
    }

    return OutputResult(parsedPassports.Count(x => x.P1()).ToString(), parsedPassports.Count(x => x.P2()).ToString());
  }

  public class Day4Passport
  {
    public string byr { get; private set; }

    public string iyr { get; private set; }

    public string eyr { get; private set; }

    public string hgt { get; private set; }

    public string hcl { get; private set; }

    public string ecl { get; private set; }

    public string pid { get; private set; }

    public string cid { get; private set; }

    public Day4Passport(string passport)
    {
      var split = passport.Replace(Environment.NewLine, " ").Split(' ').ToDictionary(x => x.Split(':')[0], x => x.Split(':')[1]);

      var props = this.GetType().GetProperties().ToArray();

      foreach (var prop in props)
      {
        if (split.ContainsKey(prop.Name))
        {
          prop.SetValue(this, split[prop.Name]);
        }
      }
    }

    public bool P1()
    {
      var validCount = this.GetType().GetProperties().Select(x => x.GetValue(this)).Count(x => x != null);

      return (validCount == 8 || (validCount == 7 && this.cid == null));
    }

    private string[] ValidEyeColor = new string[]
    {
      "amb","blu","brn","gry","grn","hzl","oth"
    };

    public bool P2()
    {
      var isValid = true;

      isValid &= byr != null && byr.Length == 4 && int.TryParse(byr, out var parsedByr) && (parsedByr >= 1920 && parsedByr <= 2002);

      isValid &= iyr != null && iyr.Length == 4 && int.TryParse(iyr, out var parsedIyr) && (parsedIyr >= 2010 && parsedIyr <= 2020);

      isValid &= eyr != null && eyr.Length == 4 && int.TryParse(eyr, out var parsedEyr) && (parsedEyr >= 2020 && parsedEyr <= 2030);

      if (hgt != null && hgt.EndsWith("in"))
      {
        isValid &= int.TryParse(hgt.Substring(0, hgt.IndexOf('i')), out var parsedHeight) && parsedHeight >= 59 && parsedHeight <= 76;
      }
      else if (hgt != null && hgt.EndsWith("cm"))
      {
        isValid &= int.TryParse(hgt.Substring(0, hgt.IndexOf('c')), out var parsedHeight) && parsedHeight >= 150 && parsedHeight <= 193;
      }
      else
      {
        return false;
      }

      isValid &= hcl != null && hcl.StartsWith("#") && hcl.Length == 7 && hcl.Substring(1).All(x => char.IsLetterOrDigit(x));

      isValid &= ecl != null && ValidEyeColor.Contains(ecl);

      isValid &= pid != null && pid.Length == 9 && pid.All(x => char.IsDigit(x));

      return isValid;
    }
  }
  #endregion

  #region Day5: Solved!

  public static string Day5()
  {
    var inputs = File.ReadAllLines(Path.Combine(InputBasePath, "Day5.txt")); //new[] { "FBFBBFFRLR", "BFFFBBFRRR", "FFFBBBFRRR", "BBFFBBFRLL" };

    var seatAmount = 128;

    var columnAmount = 8;

    var p1 = 0;

    var seats = new Dictionary<int, int[]>();

    foreach (var input in inputs)
    {
      var test = new Day5Seats(input, seatAmount, columnAmount, ref seats);

      p1 = Math.Max(p1, test.SeatID);
    }

    //For P2, we need to find the SeatID of the Seat that is missing. Both the first and the last rows have seats missing, so disregard those.
    seats.Remove(seats.Keys.Min());
    seats.Remove(seats.Keys.Max());

    //Next, we flatten the list of seat ID's
    var flattenedSeats = seats.SelectMany(x => x.Value);

    //And P2 is the missing seat, aka the first seat not present in the list + 1, with + 1 added to its seat id.
    var p2 = flattenedSeats.First(x => !flattenedSeats.Contains(x + 1)) + 1;

    return OutputResult(p1.ToString(), p2.ToString());
  }

  public class Day5Seats
  {
    //F = front, B = Back, L = Left, R = right

    private int Row { get; set; }

    private int Column { get; set; }

    public int SeatID => (Row * 8) + Column;

    public Day5Seats(string input, int seatAmount, int columns, ref Dictionary<int, int[]> seats)
    {
      Row = PartitionByInput(new Queue<char>(input.Substring(0, 7)), seatAmount);

      Column = PartitionByInput(new Queue<char>(input.Substring(7)), columns);

      if (!seats.ContainsKey(Row))
      {
        seats.Add(Row, new int[columns]);
      }

      seats[Row][Column] = SeatID;
    }

    public int PartitionByInput(Queue<char> input, int limit)
    {
      var min = 0;
      var max = limit - 1;

      while (input.Any())
      {
        switch (input.Dequeue())
        {
          case 'L':
          case 'F':
            {
              max -= Partition(min, max);
            }
            break;
          case 'B':
          case 'R':
            {
              min += Partition(min, max);
            }
            break;
          default:
            {
              throw new Exception("Someone really fucked up.");
            }
        }
      }

      return max;
    }

    private int Partition(int min, int max)
    {
      return (int)Math.Ceiling(((decimal)max - min) / 2);
    }

  }
  #endregion

  #region Day6: Solved!

  public static string Day6()
  {
    var parsedInput = new Day6Groups(new Queue<string>(File.ReadAllLines(Path.Combine(InputBasePath, "Day6.txt"))));

    var p1 = parsedInput.Groups.Sum(x => x.SelectMany(y => y.ToCharArray()).Distinct().Count()); //We want to know the sum of the amount of people that answered yes to any question.

    var p2 = 0; //For p2, we want to know the sum of the amount of answers that were answered as "yes" by everyone in the group.

    foreach (var group in parsedInput.Groups)
    {
      foreach (var answer in group.SelectMany(x => x.ToCharArray()).Distinct().ToList())
      {
        if (group.All(x => x.Contains(answer)))
        {
          p2++;
        }
      }
    }

    return OutputResult(p1.ToString(), p2.ToString());
  }

  public class Day6Groups
  {
    public List<List<string>> Groups { get; set; }

    public Day6Groups(Queue<string> inputs)
    {
      Groups = new List<List<string>> { new List<string>() };

      while (inputs.Any())
      {
        var current = inputs.Dequeue();

        if (string.IsNullOrEmpty(current))
        {
          Groups.Add(new List<string>());
        }
        else
        {
          Groups.Last().Add(current);
        }
      }
    }
  }

  #endregion

  #region Day7: Solved! (but very slow)

  public static string Day7()
  {
    var inputs = File.ReadAllLines(Path.Combine(InputBasePath, "Day7.txt"));

    var bags = new List<Day7Bag>();

    //We should parse this input in two stages. First, we need all the different colors of bags. We do this by only parsing the part before "bags" and then building a list of all possible bags.
    foreach (var input in inputs)
    {
      var bagName = input.Substring(0, input.IndexOf("bags")).Trim();

      bags.Add(new Day7Bag(bagName, 1));
    }

    //Afterwards, we go through the inputs again but this time, we fill the contents of each bag.
    foreach (var input in inputs.Where(x => !x.Contains("no other bags")))
    {
      var bagName = input.Substring(0, input.IndexOf("bags")).Trim();
      var current = bags.First(x => x.Name == bagName);
      var contents = input.Substring(input.IndexOf("contain") + "contain".Length).Split(',');

      foreach (var content in contents)
      {
        var split = content.Trim().Split(' ');
        var amount = int.Parse(split[0]);
        var qualifier = split[1];
        var color = split[2];
        var bagToMove = bags.First(x => x.Qualifier == qualifier && x.Color == color);

        if (bagToMove != null)
        {
          for (var idx = 0; idx < amount; idx++)
          {
            current.Contents.Add(bagToMove);
          }
        }
      }
    }

    var p1 = bags.Where(x => x.ContainsTarget() && x.Name != "shiny gold").Select(x => x.Name).Distinct(); //We need to know what bags contain the shiny gold bag.

    var p2 = bags.First(x => x.Name == "shiny gold").SumContents();

    return OutputResult(p1.Count().ToString(), p2.ToString());
  }

  public class Day7Bag
  {
    public string Qualifier { get; set; }

    public string Color { get; set; }

    public string Name => $"{Qualifier} {Color}";

    public bool Target { get; set; }

    public List<Day7Bag> Contents { get; private set; }

    public Day7Bag(string name, int amount)
    {
      var split = name.Split(' ');
      Qualifier = split[0];
      Color = split[1];

      if (Name == "shiny gold")
        Target = true;

      Contents = new List<Day7Bag>();
    }

    public bool ContainsTarget()
    {
      var retVal = Target || Contents.Any(x => x.ContainsTarget());
      Target = retVal;
      return retVal;
    }

    public int SumContents()
    {
      var contents = Contents.Count();

      contents += Contents.Sum(z => z.SumContents());
      return contents;
    }
  }

  #endregion

  #region Day8: Solved! Without even debugging!

  public static string Day8()
  {
    var input = File.ReadAllLines(Path.Combine(InputBasePath, "Day8.txt"));

    var p1 = Day8p1(input, out var _);

    var p2 = 0;
    //For p2, we're going to have to swap some operations around. nop has to be swapped with jmp, and jmp has to be swapped with nop. There's a permutation in there that'll make sure the program succesfully terminates.

    //First, get all the indexes of the operations that we're going to swap around.
    var swappables = new Queue<int>();

    foreach (var i in input)
    {
      Day8SplitStatement(i, out var operation, out var value);

      if (operation == "nop" || operation == "jmp")
      {
        swappables.Enqueue(Array.IndexOf(input, i));
      }
    }

    //then iterate through all of these operations, and flip them around before running the boot code again.
    while (p2 == 0)
    {
      var localInput = input.ToArray();

      var operationIndex = swappables.Dequeue();
      var operationToSwap = localInput[operationIndex];

      Day8SplitStatement(operationToSwap, out var operation, out var value);

      operation = operation == "nop" ? "jmp" : "nop";

      localInput[operationIndex] = $"{operation} {value}";

      var output = Day8p1(localInput, out var completed);

      if (completed)
      {
        p2 = output;
      }
    }

    return OutputResult(p1.ToString(), p2.ToString());
  }

  public static void Day8SplitStatement(string input, out string operation, out string value)
  {
    var splitStatement = input.Split(' ');

    operation = splitStatement[0].Trim();
    value = splitStatement[1];
  }

  public static int Day8p1(string[] input, out bool completed)
  {
    var acc = 0;
    completed = false;

    var completedStatements = new List<int>();

    for (var idx = 0; idx < input.Length; idx++)
    {
      var statement = input[idx];

      if (completedStatements.Contains(idx))
      {
        return acc;
      }
      else
      {
        completedStatements.Add(idx);
      }

      Day8SplitStatement(input[idx], out var operation, out var value);

      //System.Console.WriteLine($"idx {idx}. Operation {operation}, value {value}.");
      Day8Interpret(ref idx, ref acc, operation, value);
    }

    completed = true;

    return acc;
  }

  public static void Day8Interpret(ref int idx, ref int acc, string operation, string value)
  {
    switch (operation)
    {
      case "nop":
        {
          //do nothing, simply progress to the next step.
        }
        break;
      case "acc": //Interpret the value and add it to the accumulator
        {
          var val = int.Parse(value.Substring(1));

          if (value[0] == '+')
          {
            acc += val;
          }
          else
          {
            acc -= val;
          }
        }
        break;
      case "jmp": //Interpret the value and jump idx relative to the value.
        {
          var val = int.Parse(value.Substring(1));

          if (value[0] == '+')
          {
            idx += val - 1;
          }
          else
          {
            idx -= val + 1;
          }
        }
        break;
      default:
        {
          throw new Exception("Someone fucked up");
        }
    }
  }

  #endregion

  #region Day9: Solved!

  public static string Day9()
  {
    var input = File.ReadAllLines(Path.Combine(InputBasePath, "Day9.txt"));

    var parsedInput = input
      .Select(long.Parse)
      .ToArray();

    int preambleLength = parsedInput.Length > 20 ? 25 : 5;

    long p1 = 0;

    for (int idx = preambleLength; idx < parsedInput.Length; idx++)
    {
      if (idx + 1 == parsedInput.Length)
        break;

      var candidateArray = parsedInput.Skip(idx - preambleLength).Take(preambleLength);
      var candidates = new Queue<long>(candidateArray);

      var target = parsedInput[idx];
      var valid = false;

      while (candidates.Any())
      {
        var current = candidates.Dequeue();

        valid |= current.CanSumTo(target, candidateArray, out var _);
      }

      if (!valid)
      {
        System.Console.WriteLine($"{target} is not valid!");
        p1 = target;
        break;
      }
    }

    long p2 = 0;

    //Now that we have p1, we will use it to find p2. We must find a sequence of at least 2 numbers that add up to p1. Then, take the smallest and the largest number in this range, and add them together to get to p2.

    for (var idx2 = 0; idx2 < parsedInput.Length; idx2++)
    {
      var candidates = new Queue<long>(parsedInput);

      while (candidates.Any() && p2 == 0)
      {
        var current = candidates.Dequeue();

        var localQueue = new Queue<long>(candidates);

        var sum = current;

        long smallest = long.MaxValue;
        long highest = 0;

        while (sum < p1 && localQueue.Any())
        {
          var add = localQueue.Dequeue();

          sum += add;
          highest = Math.Max(highest, add);
          smallest = Math.Min(smallest, add);

          if (sum.CanSumTo(p1, localQueue, out var _))
          {
            p2 = smallest + highest;
            break;
          }
        }
      }
    }

    return OutputResult(p1.ToString(), p2.ToString());
  }

  private static bool CanSumTo(this long source, long target, IEnumerable<long> numbers, out long candidate)
  {
    candidate = numbers.FirstOrDefault(x => x == (target - source));
    return candidate != 0;
  }

  #endregion

  #region Day10: Solved!

  public static string Day10()
  {
    var input = File.ReadAllLines(Path.Combine(InputBasePath, "Day10.txt"))
    .Select(int.Parse)
    .ToArray();

    var sortedQueue = new Queue<int>(input.OrderBy(x => x));

    int current = 0; int oneJolt = 0; int threeJolt = 1; //p1: pretty easy, just order from low to high and count the amount of times the diff is either one or three.

    while (sortedQueue.Any())
    {
      var next = sortedQueue.Dequeue();

      var diff = Math.Abs(next - current);

      if (diff == 1)
      {
        oneJolt++;
      }
      else if (diff == 3)
      {
        threeJolt++;
      }

      current = next;
    }

    var p1 = oneJolt * threeJolt;

    //p2: A bit more difficult, instead of just ordering them, we need to find out how many valid combinations there are to get to the highest value. A valid adapter has a rating of either 1, 2 or 3 jolts higher than the current one.

    var list = input.OrderBy(x => x).ToList();

    //We're going to add both the outlet as well as the device itself as valid points in order to calculate the valid amount of combinations.

    list.Insert(0, 0);
    list.Insert(input.Length - 1, input.Max() + 3);

    var p2 = new long[list.Count];

    for (var index = 0; index < p2.Length; index++)
    {
      p2[index] = index == 0 ? 1 : 0;

      for (var index2 = index - 1; index2 >= 0; index2--)
      {
        if (list[index] - list[index2] <= 3)
          p2[index] += p2[index2];
        else
          break;
      }
    }

    return OutputResult(p1.ToString(), p2[p2.Length - 1].ToString());
  }

  private static int ValidCombinations(this int input, int[] adapters, int target)
  {
    var candidates = adapters.Where(x => x > input && x < (input + 4));

    if (candidates.Contains(target))
    {
      return 1;
    }

    return candidates.Sum(x => x.ValidCombinations(adapters, target));
  }

  #endregion

  #region Day11: Solved!

  public static string Day11()
  {
    var input = File.ReadAllLines(Path.Combine(InputBasePath, "Day11.txt"));

    var p1 = CalculateDay11(input, 4);
    var p2 = CalculateDay11(input, 5);

    return OutputResult(p1.ToString(), p2.ToString());
  }

  private static int CalculateDay11(string[] input, int limit)
  {
    var grid = new Day11Grid(input);
    var previousDiff = 0;

    while (true)
    {
      var diff = grid.Occupy(limit);

      if (previousDiff == diff)
      {
        break;
      }

      previousDiff = diff;
    }

    return grid.OccupiedSeats;
  }

  public class Day11Grid
  {
    public bool?[][] Grid { get; set; }

    public string[] OutputGrid { get; set; }

    public int OccupiedSeats => Grid.Sum(x => x.Count(y => (y ?? false)));

    public Day11Grid(string[] input)
    {
      BuildInputGrid(input);
    }

    public void BuildInputGrid(string[] input)
    {
      Grid = new bool?[input.Length][];

      for (var idx = 0; idx < input.Length; idx++)
      {
        var line = input[idx];

        Grid[idx] = new bool?[line.Length];

        for (var idx2 = 0; idx2 < line.Length; idx2++)
        {
          Grid[idx][idx2] = ConvertTo(line[idx2]);
        }
      }

      BuildOutputGrid();
    }

    public void BuildOutputGrid()
    {
      OutputGrid = new string[Grid.Length];

      for (var row = 0; row < Grid.Length; row++)
      {
        var line = Grid[row];

        for (var seat = 0; seat < line.Length; seat++)
        {
          OutputGrid[row] += ConvertFrom(Grid[row][seat]);
        }
      }
    }

    private static char ConvertFrom(bool? input)
    {
      if (!input.HasValue) return '.';

      return input.Value ? '#' : 'L';
    }

    private static bool? ConvertTo(char input)
    {
      if (input == '.') return null;

      return input == '#';
    }

    //Just count the adjacent seats in a 3x3 grid around the current position.
    public int CountOccupiedAdjacentSeats(int row, int seat)
    {
      var count = 0;

      for (var rowOffSet = -1; rowOffSet < 2; rowOffSet++)
      {
        var rowIndex = row + rowOffSet;

        if (rowIndex >= 0 && rowIndex < Grid.Length)
        {
          for (var seatOffSet = -1; seatOffSet < 2; seatOffSet++)
          {
            var seatIndex = seat + seatOffSet;

            if (seatIndex >= 0 && seatIndex < Grid[rowIndex].Length && !(rowOffSet == 0 && seatOffSet == 0)) //We wanna filter out the current seat, which is 0,0.
            {
              count += (Grid[rowIndex][seatIndex] ?? false) ? 1 : 0;
            }
          }
        }
      }

      return count;
    }

    //This one is going to be a bit more difficult, we're going to have to look for the first seat that is "visible" from a certain seat.
    //This means that just looking one seat the left, right etc is not enough, we have to travel the grid until we find either the edge of the grid, or we find a seat.
    //We're going to use two indices as "directions" that we'll use to walk the grid until we either find a seat, or the edge of the grid.
    public int CountOccupiedVisibleSeats(int row, int seat)
    {
      var count = 0;

      for (var x = -1; x <= 1; x++)
      {
        for (var y = -1; y <= 1; y++)
        {
          var xOffset = 0;
          var yOffset = 0;

          while (true)
          {
            xOffset += x;
            yOffset += y;

            var newRow = row + xOffset;
            var newSeat = seat + yOffset;

            if ((x == 0 && y == 0) || newRow < 0 || newSeat < 0 || newRow >= Grid.Length || newSeat >= Grid[newRow].Length) //If we've reached out of bounds or our search direction leads to nowhere, break.
            {
              break;
            }

            if (Grid[newRow][newSeat].HasValue)
            {
              count += Grid[newRow][newSeat].Value ? 1 : 0;
              break;
            }
          }
        }
      }

      return count;
    }

    public int Occupy(int limit)
    {
      var localGrid = new Day11Grid(OutputGrid).Grid;

      var changed = 0;

      for (var row = 0; row < Grid.Length; row++)
      {
        for (var seat = 0; seat < Grid[row].Length; seat++)
        {
          var occupiedSeats = limit == 4
            ? CountOccupiedAdjacentSeats(row, seat)
            : CountOccupiedVisibleSeats(row, seat);

          var isOccupied = Grid[row][seat];

          if (isOccupied != null)
          {
            var newValue = (bool?)(!isOccupied.Value ? occupiedSeats == 0 : occupiedSeats < limit);

            if (newValue != Grid[row][seat])
            {
              changed++;
            }

            localGrid[row][seat] = newValue;
          }
        }
      }

      Grid = localGrid;

      BuildOutputGrid();

      return changed;
    }
  }
  #endregion

  #region Day12: Solved!

  public static string Day12()
  {
    var input = File.ReadAllLines(Path.Combine(InputBasePath, "Day12.txt"));

    var p1 = new Day12Grid(input).ManhattanDistance;
    var p2 = new Day12Grid(input, false).ManhattanDistance;

    return OutputResult(p1.ToString(), p2.ToString());
  }

  public class Day12Grid
  {
    public int X { get; set; }

    public int Y { get; set; }

    public Day12Directions CurrentDirection { get; set; }

    public int ManhattanDistance => Math.Abs(X) + Math.Abs(Y);

    public Day12Grid(string[] input, bool p1 = true)
    {
      CurrentDirection = Day12Directions.East; //We start off east.

      var xOffset = 10; //p2: The waypoint starts at 10 east, 1 north.
      var yOffset = 1;

      foreach (var line in input)
      {
        var action = line[0];
        var distance = int.Parse(line.Substring(1));

        if (p1)
        {
          switch (action)
          {
            case 'N': Move(Day12Directions.North, distance); break;

            case 'S': Move(Day12Directions.South, distance); break;

            case 'E': Move(Day12Directions.East, distance); break;

            case 'W': Move(Day12Directions.West, distance); break;

            case 'L': CurrentDirection = (Day12Directions)(((int)CurrentDirection - distance + 360) % 360); break;

            case 'R': CurrentDirection = (Day12Directions)(((int)CurrentDirection + distance) % 360); break;

            case 'F': Move(CurrentDirection, distance); break;
          }
        }
        else
        {
          //Part 2:
          //Turns out, we're moving a waypoint, not the ship itself.

          //N, S, E, W: Move the waypoint north/south/east/west.
          //L: rotate the waypoint left (counter-clockwise)
          //R: rotate the waypoint right (clockwise)

          //F means to move forward to the waypoint X times. The waypoint's position is relative, so when the ship moves, the waypoint moves equally.

          switch (action)
          {
            case 'N': yOffset += distance; break;
            case 'S': yOffset -= distance; break;
            case 'E': xOffset += distance; break;
            case 'W': xOffset -= distance; break;
            case 'L': Rotate(ref xOffset, ref yOffset, distance / 90, false); break;
            case 'R': Rotate(ref xOffset, ref yOffset, distance / 90, true); break;
            case 'F':
              {
                for (var idx = 0; idx < distance; idx++)
                {
                  X += xOffset;
                  Y += yOffset;
                }
              }
              break;
          }
        }
      }
    }

    //direction = true = clockwise
    public void Rotate(ref int xOffset, ref int yOffset, int rotations, bool direction)
    {
      //By rotating clockwise 90 degrees, the positive value on the Y axis becomes a positive value on the X axis. The positive value on the X axis becomes the negative value on the Y axis.
      //The negative value on the Y axis becomes a negative value on the X axis, and a negative value on the X axis becomes a positive value on the Y axis.

      //By rotating counter-clockwise 90 degrees, the positive value on the Y axis becomes a negative value on the X axis.
      //The negative value on the X axis becomes a negative value on the Y axis. The negative value on the Y axis becomes a positive value on the X axis, and a positive value on the X axis becomes a positive value on the Y axis.

      //Simplified: If rotating clockwise, X becomes Y, and Y becomes the inverse of X. If rotating counter-clockwise, Y becomes X, and X becomes the inverse of Y.

      while (rotations > 0)
      {
        var mem = direction ? xOffset : yOffset;

        if (direction)
        {
          xOffset = yOffset;
          yOffset = -(mem);
        }
        else
        {
          yOffset = xOffset;
          xOffset = -(mem);
        }

        rotations--;
      }
    }

    public void Move(Day12Directions orientation, int distance)
    {
      switch (orientation)
      {
        case Day12Directions.West:
          {
            X -= distance;
          }
          break;
        case Day12Directions.East:
          {
            X += distance;
          }
          break;
        case Day12Directions.North:
          {
            Y += distance;
          }
          break;
        case Day12Directions.South:
          {
            Y -= distance;
          }
          break;
      }
    }
  }

  public enum Day12Directions
  {
    North = 0,
    West = 270,
    South = 180,
    East = 90
  }

  #endregion

  #region Day13: Solved!

  public static string Day13()
  {
    var input = new[]
    {
      "939",
      "1789,37,47,1889"
    };

    input = File.ReadAllLines(Path.Combine(InputBasePath, "Day13.txt"));

    var day = new Day13Schedule(input);

    return OutputResult(day.P1().ToString(), day.P2().ToString());
  }

  public class Day13Schedule
  {
    public long[] Busses { get; set; }

    public long EarliestDeparture { get; set; }

    public Day13Schedule(string[] input)
    {
      EarliestDeparture = long.Parse(input[0]);
      Busses = input[1].Replace('x', '0').Split(',').Select(x => long.Parse(x)).ToArray();
    }

    public long P1()
    {
      long busId = 0;
      var busDeparture = long.MaxValue;

      foreach (var bus in Busses.Where(x => x != 0))
      {
        long current = 0;

        while (current < EarliestDeparture)
        {
          current += bus;
        }

        if (current < busDeparture)
        {
          busId = bus;
          busDeparture = current;
        }
      }

      return busId * (busDeparture - EarliestDeparture);
    }

    public long P2()
    {
      //We're looking for the first minute where each of the busses leaves at an offset of that minute that is equal to its position in the array. So the bus at index 0 leaves at minute + 0, the bus at index 1 leaves minute + 1.
      //Busses that have the busid 0 need to be skipped. 

      long minute = 0;

      var interval = Busses.Max();

      var indexOfInterval = Array.IndexOf(Busses, interval);
      var partitions = 4;
      var partition = long.MaxValue / partitions;
      var options = new ParallelOptions
      {
        MaxDegreeOfParallelism = partitions
      };

      Parallel.For(0, partitions, (int i, ParallelLoopState state) =>
      {
        var start = i * partition;
        var result = BruteForceP2(Busses, start, start + partition, interval, indexOfInterval);
        System.Console.WriteLine($"{i}: {result}");
        minute = Math.Max(result, minute);
        state.Stop();
      });

      return minute;
    }
  }

  private static long BruteForceP2(long[] buses, long minute, long limit, long interval, long indexOfInterval)
  {
    //There must also be some other way to make this one faster, but how?
    while (minute < limit)
    {
      minute += interval;

      var startingMinute = minute - indexOfInterval;

      if (buses.Where(x => x != 0).All(x => (startingMinute + Array.IndexOf(buses, x)) % x == 0))
      {
        return startingMinute;
      }
    }

    return -1;
  }

  #endregion

  #region Day14: Done!
  public static string Day14()
  {
    var input = File.ReadAllLines(Path.Combine(InputBasePath, "Day14.txt"));

    var p1 = Day14Calculate(input);

    var p2 = Day14Calculate(input, true);

    return OutputResult(p1.ToString(), p2.ToString());
  }

  private static long Day14Calculate(string[] input, bool part2 = false)
  {
    var mem = new Dictionary<decimal, decimal>();
    var mask = new bool?[36];

    foreach (var line in input)
    {
      if (line.StartsWith("mask"))
      {
        mask = ParseMask(line.Replace("mask = ", string.Empty).ToCharArray());
      }
      else
      {
        var bracketIndex = line.IndexOf('[') + 1;
        var position = decimal.Parse(line.Substring(bracketIndex, line.IndexOf(']') - bracketIndex));
        var value = decimal.Parse(line.Substring(line.LastIndexOf(' ') + 1));

        if (part2)
        {
          var bitArray = new BitArray(Decimal.GetBits(position)); //In part 2, we're decoding the memory address(es) to write to, not the actual value.

          var output = Decode(bitArray, mask); //The output will contain NULL values. These are the "floating" bits. We must iterate through all possible combinations for these floating bits and write the value to all corresponding memory addressses.

          var addresses = DetermineAddresses(output);

          foreach (var memAdr in addresses)
          {
            if (mem.ContainsKey(memAdr))
            {
              mem[memAdr] = value;
            }
            else
            {
              mem.Add(memAdr, value);
            }
          }
        }
        else
        {
          var bitArray = new BitArray(Decimal.GetBits(value));

          var output = ApplyMask(bitArray, mask);

          var stringOutput = VisualizeBitArray(output);
          var maskedVal = Convert.ToInt64(stringOutput, 2);

          if (mem.ContainsKey(position))
          {
            mem[position] = maskedVal;
          }
          else
          {
            mem.Add(position, maskedVal);
          }
        }
      }
    }

    return (long)mem.Sum(x => x.Value);
  }

  private static decimal[] DetermineAddresses(bool?[] input)
  {
    var floatingAmount = input.Count(i => i == null);
    var combinations = (int)Math.Pow(2, floatingAmount);

    var uniqueCombinations = GenerateUniqueBinaryCombinations(floatingAmount, new bool[floatingAmount], 0);

    var outcomes = new string[combinations];
    var combinationLength = uniqueCombinations.First().Length;

    for (var combination = 0; combination < combinations; combination++)
    {
      var queue = new Queue<char>(uniqueCombinations[combination]);

      var local = new bool[input.Length];

      for (var idx = 0; idx < input.Length; idx++)
      {
        local[idx] = input[idx] == null
          ? queue.Dequeue() == '1'
          : input[idx].Value;
      }

      outcomes[combination] = VisualizeBitArray(local);
    }

    return outcomes.Select(outcome => (decimal)Convert.ToInt64(outcome, 2)).ToArray();
  }

  private static List<string> GenerateUniqueBinaryCombinations(int length, bool[] combinations, int index)
  {
    var returnList = new List<string>();

    if (index == length)
    {
      returnList.Add(string.Join("", combinations.Select(x => x ? '1' : '0')));
      return returnList;
    }

    for (var i = 0; i < 2; i++)
    {
      combinations[index] = !combinations[index];
      returnList.AddRange(GenerateUniqueBinaryCombinations(length, combinations, index + 1));
    }

    return returnList;
  }

  private static bool?[] Decode(BitArray input, bool?[] mask)
  {
    var output = new bool?[mask.Length];

    for (var i = 0; i < mask.Length; i++)
    {
      output[i] = mask[i].HasValue
        ? mask[i].Value
          ? true
          : (bool?)input[i]
        : null; //This is where we have to do some wild shit.
    }

    return output;
  }

  private static bool[] ApplyMask(BitArray input, bool?[] mask)
  {
    var output = new bool[mask.Length];

    for (var i = 0; i < mask.Length; i++)
    {
      output[i] = mask[i].HasValue ? mask[i].Value : input[i];
    }

    return output;
  }

  private static bool?[] ParseMask(char[] maskRep)
  {
    maskRep = maskRep.Reverse().ToArray();

    var output = new bool?[36];

    for (var i = 0; i < 36; i++)
    {
      output[i] = (maskRep[i] == 'X' ? (bool?)null : maskRep[i] == '1');
    }

    return output;
  }

  private static string VisualizeBitArray(BitArray input)
  {
    var visual = "";

    for (var i = 35; i >= 0; i--) //We're only interested in the 36
    {
      visual += input[i] ? 1 : 0;
    }
    return visual;
  }

  private static string VisualizeBitArray(bool[] input)
  {
    return VisualizeBitArray(new BitArray(input));
  }
  #endregion

  #region Day15: Optimized!
  public static string Day15()
  {
    var input = new int[] { 7, 12, 1, 0, 16, 2 };

    //input = new int[] { 1, 3, 2 };

    var p1 = CalculateDay15(input);

    var p2 = CalculateDay15(input, true);

    return OutputResult(p1.ToString(), p2.ToString());
  }

  private static int CalculateDay15(int[] input, bool p2 = false)
  {
    var length = p2 ? 30000000 : 2020;

    var dict = new Dictionary<int, Day15Memory>();

    for (var i = 0; i < input.Length; i++) //First, seed the array with the values we know to be true.
    {
      var num = input[i];

      dict.Add(num, new Day15Memory(num) { Current = i });
    }

    var previousNumber = dict.Values.Last();

    for (var i = input.Length; i < length; i++)
    {
      if (previousNumber.Previous == null) //If there's no previous value, then obviously this is the first occurance.
      {
        SwapIfExists(ref dict, 0, i);

        previousNumber = dict[0];
      }
      else
      {
        var number = previousNumber.Current - previousNumber.Previous.Value;

        SwapIfExists(ref dict, number, i);

        previousNumber = dict[number];
      }
    }

    return previousNumber.Number;
  }

  private static void SwapIfExists(ref Dictionary<int, Day15Memory> dict, int number, int i)
  {
    if (dict.ContainsKey(number))
    {
      var cur = dict[number];
      cur.Previous = cur.Current;
      cur.Current = i;
    }
    else
    {
      dict.Add(number, new Day15Memory(number) { Current = i });
    }
  }

  private class Day15Memory
  {
    public int Current { get; set; }

    public int? Previous { get; set; }

    public int Number { get; private set; }

    public Day15Memory(int number)
    {
      Number = number;
    }
  }
  #endregion

  #region Day16: Done!
  public static string Day16()
  {
    var input = File.ReadAllLines(Path.Combine(InputBasePath, "Day16.txt"));

    var ranges = new List<Day16Range>();
    var tickets = new List<List<int>>();
    List<int> myTicket = null;

    for (var line = 0; line < input.Length; line++)
    {
      var current = input[line];

      if (current.Contains(":"))
      {
        var unparsedRanges = current.Substring(current.IndexOf(':') + 1).Split(new[] { "or" }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim(' '));

        foreach (var unParsedRange in unparsedRanges)
        {
          var split = unParsedRange.Split('-');
          ranges.Add(new Day16Range(current.Substring(0, current.IndexOf(':')), int.Parse(split[0]), int.Parse(split[1])));
        }
      }

      if (current.Contains(','))
      {
        var split = current.Split(',');
        if (myTicket == null)
        {
          myTicket = split.Select(int.Parse).ToList();
        }
        else
        {
          tickets.Add(split.Select(int.Parse).ToList());
        }
      }
    }

    var errorRate = 0;
    var validTickets = new List<List<int>>();

    foreach (var ticket in tickets)
    {
      var valid = true;

      foreach (var prop in ticket)
      {
        valid &= IsValidNumberDay16(ranges, prop, out var error);
        errorRate += error;
      }

      if (valid)
      {
        validTickets.Add(ticket);
      }
    }

    var p1 = errorRate;

    var taken = new List<string>();
    long p2 = 1;

    while (taken.Count() < ranges.Select(x => x.Name).Distinct().Count())
    {
      for (var idx = 0; idx < validTickets.First().Count(); idx++)
      {
        var values = validTickets.Select(val => val[idx]).ToArray();

        var foundRanges = FindRangesForInput(ranges.Where(x => !taken.Contains(x.Name)), values); //There's going to be indices that have more than one candidate. Skip these for now, the field will narrow down eventually.

        if (foundRanges.Count() == 1) //If we find exactly one, we found our main candidate.
        {
          var range = foundRanges.First();

          taken.Add(range);

          if (range.StartsWith("departure"))
          {
            p2 *= myTicket[idx];
          }
        }
      }
    }

    return OutputResult(p1.ToString(), p2.ToString());
  }

  private class Day16Range
  {
    public string Name { get; private set; }

    public int Min { get; private set; }

    public int Max { get; private set; }

    public Day16Range(string name, int min, int max)
    {
      Name = name;
      Min = min;
      Max = max;
    }
  }

  private static IEnumerable<string> FindRangesForInput(IEnumerable<Day16Range> ranges, IEnumerable<int> values)
  {
    var dict = new Dictionary<string, int>();

    foreach (var val in values)
    {
      var validRanges = ranges.Where(x => x.Min <= val && x.Max >= val);

      foreach (var validRange in validRanges)
      {
        if (!dict.ContainsKey(validRange.Name))
        {
          dict.Add(validRange.Name, 0);
        }

        dict[validRange.Name]++;
      }
    }

    return dict.Where(x => x.Value == values.Count()).Select(x => x.Key);
  }

  private static bool IsValidNumberDay16(List<Day16Range> ranges, int number, out int numberToAdd)
  {
    numberToAdd = 0;

    if (ranges.Max(x => x.Max) < number || ranges.Min(x => x.Min) > number) //If the number falls completely outside of any ranges, it's not a valid number.
    {
      numberToAdd = number;
      return false;
    }
    else
    {
      if (ranges.Any(x => x.Min <= number && x.Max >= number))
      {
        return true;
      }
      else
      {
        numberToAdd = number;
        return false;
      }
    }
  }

  #endregion

  #region Day17: todo
  public static string Day17()
  {
    var input = new[]{
      ".#.",
      "..#",
      "###",};

    return OutputResult();
  }

  public class Day17ConwayCube
  {
    public int x { get; set; }

    public int y { get; set; }

    public int z { get; set; }

    public bool Active { get; set; }

    public Day17ConwayCube(bool active, int x, int y, int z)
    {
      Active = active;
    }
  }
  #endregion

  #region Day18: todo
  public static string Day18()
  {
    var input = File.ReadAllLines(Path.Combine(InputBasePath, "Day18.txt"));
    // new[] { 
    //   //"1 + 2 * 3 + 4 * 5 + 6" //checked, 71
    //  //,"1 + (2 * 3) + (4 * (5 + 6))"  //checked, 51
    //  //"2 * 3 + (4 * 5)" //Checked, 26
    //  // "5 + (8 * 3 + 9 + 3 * 4 * 3)" //Checked, 437
    //  // "5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))" //Checked 12240
    //  // "((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2" //Checked 13632
    // };

    long p1 = 0;

    foreach (var line in input)
    {
      //First, we're going to replace all the statements in parentheses with their product for easy calculation.
      var localLine = line;

      localLine = Day18ResolveParantheses(localLine);

      var split = localLine.Split(' ');

      long x = 0;

      //Afterwards, just solve the problem.

      for (var idx = 0; idx < split.Length - 2; idx += 2)
      {
        Day18Solve(ref x, split, idx);
      }

      p1 += x;
    }

    return OutputResult(p1.ToString());
  }

  private static string Day18ResolveParantheses(string localLine)
  {
    while (localLine.Contains("("))
    {
      var input = localLine.Contains(')') 
        ? localLine.Substring(localLine.IndexOf('('), (localLine.IndexOf(')', localLine.IndexOf('(')) - localLine.IndexOf('(') + 1)) 
        : localLine.Substring(localLine.IndexOf('('));

      var sub = Day18ResolveParantheses(input.Trim(new[] { '(', ')' }));
      var subSplit = sub.Split(' ');

      long newVal = 0;

      for (var idx = 0; idx < subSplit.Length - 2; idx += 2)
      {
        Day18Solve(ref newVal, subSplit, idx);
      }

      localLine = localLine.Replace(input, newVal.ToString()).Trim(')');
    }

    return localLine;
  }

  private static void Day18Solve(ref long x, string[] split, int idx)
  {
    if (x == 0)
    {
      x = long.Parse(split[idx].Trim(new[] { ')', '(' }));
    }

    var op = split[idx + 1];
    var y = split[idx + 2];

    switch (op)
    {
      case "+":
        {
          x += long.Parse(y.Trim(new[] { ')', '(' }));
        }
        break;
      case "*":
        {
          x *= long.Parse(y.Trim(new[] { ')', '(' }));
        }
        break;
    }
  }

  #endregion

  #region Day19: todo
  public static string Day19()
  {
    return OutputResult();
  }
  #endregion

  #region Day20: todo
  public static string Day20()
  {
    return OutputResult();
  }
  #endregion

  #region Day21: todo
  public static string Day21()
  {
    return OutputResult();
  }
  #endregion

  #region Day22: todo
  public static string Day22()
  {
    return OutputResult();
  }
  #endregion

  #region Day23: todo
  public static string Day23()
  {
    return OutputResult();
  }
  #endregion

  #region Day24: todo
  public static string Day24()
  {
    return OutputResult();
  }
  #endregion

  #region Day25: todo
  public static string Day25()
  {
    return OutputResult();
  }
  #endregion
}