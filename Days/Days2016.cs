using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Solutions.Models.Day1;
using Solutions.Models.Day2;
using Solutions.Models.Day3;
using Solutions.Models.Day4;
using Solutions.Models.Day7;
using Solutions.Models.Day8;
using Solutions.Models.Day9;
using Solutions.Models.Day10;
using Solutions.Models.Day11;
using Solutions.Models.Day12;
using Solutions.Models.Day13;
using Solutions.Models.Day14;
using Solutions.Models.Day15;
using Solutions.Models.Day17;
using Solutions.Models.Day20;
using Solutions.Models.Day21;
using Solutions.Models.Day22;
using Solutions.Models.Day24;


public static partial class Days2016
{
  private const string Input1 = @"L4, L1, R4, R1, R1, L3, R5, L5, L2, L3, R2, R1, L4, R5, R4, L2, R1, R3, L5, R1, L3, L2, R5, L4, L5, R1, R2, L1, R5, L3, R2, R2, L1, R5, R2, L1, L1, R2, L1, R1, L2, L2, R4, R3, R2, L3, L188, L3, R2, R54, R1, R1, L2, L4, L3, L2, R3, L1, L1, R3, R5, L1, R5, L1, L1, R2, R4, R4, L5, L4, L1, R2, R4, R5, L2, L3, R5, L5, R1, R5, L2, R4, L2, L1, R4, R3, R4, L4, R3, L4, R78, R2, L3, R188, R2, R3, L2, R2, R3, R1, R5, R1, L1, L1, R4, R2, R1, R5, L1, R4, L4, R2, R5, L2, L5, R4, L3, L2, R1, R1, L5, L4, R1, L5, L1, L5, L1, L4, L3, L5, R4, R5, R2, L5, R5, R5, R4, R2, L1, L2, R3, R5, R5, R5, L2, L1, R4, R3, R1, L4, L2, L3, R2, L3, L5, L2, L2, L1, L2, R5, L2, L2, L3, L1, R1, L4, R2, L4, R3, R5, R3, R4, R1, R5, L3, L5, L5, L3, L2, L1, R3, L4, R3, R2, L1, R3, R1, L2, R4, L3, L3, L3, L1, L2";

  private const string Input2 = @"LURLDDLDULRURDUDLRULRDLLRURDUDRLLRLRURDRULDLRLRRDDULUDULURULLURLURRRLLDURURLLUURDLLDUUDRRDLDLLRUUDURURRULURUURLDLLLUDDUUDRULLRUDURRLRLLDRRUDULLDUUUDLDLRLLRLULDLRLUDLRRULDDDURLUULRDLRULRDURDURUUUDDRRDRRUDULDUUULLLLURRDDUULDRDRLULRRRUUDUURDULDDRLDRDLLDDLRDLDULUDDLULUDRLULRRRRUUUDULULDLUDUUUUDURLUDRDLLDDRULUURDRRRDRLDLLURLULDULRUDRDDUDDLRLRRDUDDRULRULULRDDDDRDLLLRURDDDDRDRUDUDUUDRUDLDULRUULLRRLURRRRUUDRDLDUDDLUDRRURLRDDLUUDUDUUDRLUURURRURDRRRURULUUDUUDURUUURDDDURUDLRLLULRULRDURLLDDULLDULULDDDRUDDDUUDDUDDRRRURRUURRRRURUDRRDLRDUUULLRRRUDD
DLDUDULDLRDLUDDLLRLUUULLDURRUDLLDUDDRDRLRDDUUUURDULDULLRDRURDLULRUURRDLULUDRURDULLDRURUULLDLLUDRLUDRUDRURURUULRDLLDDDLRUDUDLUDURLDDLRRUUURDDDRLUDDDUDDLDUDDUUUUUULLRDRRUDRUDDDLLLDRDUULRLDURLLDURUDDLLURDDLULLDDDRLUDRDDLDLDLRLURRDURRRUDRRDUUDDRLLUDLDRLRDUDLDLRDRUDUUULULUDRRULUDRDRRLLDDRDDDLULURUURULLRRRRRDDRDDRRRDLRDURURRRDDULLUULRULURURDRRUDURDDUURDUURUURUULURUUDULURRDLRRUUDRLLDLDRRRULDRLLRLDUDULRRLDUDDUUURDUDLDDDUDL
RURDRUDUUUUULLLUULDULLLDRUULURLDULULRDDLRLLRURULLLLLLRULLURRDLULLUULRRDURRURLUDLULDLRRULRDLDULLDDRRDLLRURRDULULDRRDDULDURRRUUURUDDURULUUDURUULUDLUURRLDLRDDUUUUURULDRDUDDULULRDRUUURRRDRLURRLUUULRUDRRLUDRDLDUDDRDRRUULLLLDUUUULDULRRRLLRLRLRULDLRURRLRLDLRRDRDRLDRUDDDUUDRLLUUURLRLULURLDRRULRULUDRUUURRUDLDDRRDDURUUULLDDLLDDRUDDDUULUDRDDLULDDDDRULDDDDUUUURRLDUURULRDDRDLLLRRDDURUDRRLDUDULRULDDLDDLDUUUULDLLULUUDDULUUDLRDRUDLURDULUDDRDRDRDDURDLURLULRUURDUDULDDLDDRUULLRDRLRRUURRDDRDUDDLRRLLDRDLUUDRRDDDUUUDLRRLDDDUDRURRDDUULUDLLLRUDDRULRLLLRDLUDUUUUURLRRUDUDDDDLRLLULLUDRDURDDULULRDRDLUDDRLURRLRRULRL
LDUURLLULRUURRDLDRUULRDRDDDRULDLURDDRURULLRUURRLRRLDRURRDRLUDRUUUULLDRLURDRLRUDDRDDDUURRDRRURULLLDRDRDLDUURLDRUULLDRDDRRDRDUUDLURUDDLLUUDDULDDULRDDUUDDDLRLLLULLDLUDRRLDUUDRUUDUDUURULDRRLRRDLRLURDRURURRDURDURRUDLRURURUUDURURUDRURULLLLLUDRUDUDULRLLLRDRLLRLRLRRDULRUUULURLRRLDRRRDRULRUDUURRRRULDDLRULDRRRDLDRLUDLLUDDRURLURURRLRUDLRLLRDLLDRDDLDUDRDLDDRULDDULUDDLLDURDULLDURRURRULLDRLUURURLLUDDRLRRUUDULRRLLRUDRDUURLDDLLURRDLRUURLLDRDLRUULUDURRDULUULDDLUUUDDLRRDRDUDLRUULDDDLDDRUDDD
DRRDRRURURUDDDRULRUDLDLDULRLDURURUUURURLURURDDDDRULUDLDDRDDUDULRUUULRDUDULURLRULRDDLDUDLDLULRULDRRLUDLLLLURUDUDLLDLDRLRUUULRDDLUURDRRDLUDUDRULRRDDRRLDUDLLDLURLRDLRUUDLDULURDDUUDDLRDLUURLDLRLRDLLRUDRDUURDDLDDLURRDDRDRURULURRLRLDURLRRUUUDDUUDRDRULRDLURLDDDRURUDRULDURUUUUDULURUDDDDUURULULDRURRDRDURUUURURLLDRDLDLRDDULDRLLDUDUDDLRLLRLRUUDLUDDULRLDLLRLUUDLLLUUDULRDULDLRRLDDDDUDDRRRDDRDDUDRLLLDLLDLLRDLDRDLUDRRRLDDRLUDLRLDRUURUDURDLRDDULRLDUUUDRLLDRLDLLDLDRRRLLULLUDDDLRUDULDDDLDRRLLRDDLDUULRDLRRLRLLRUUULLRDUDLRURRRUULLULLLRRURLRDULLLRLDUUUDDRLRLUURRLUUUDURLRDURRDUDDUDDRDDRUD";

  private const string Input3 = "./Days/Input/2016/Input3.txt";

  private const string Input4 = "./Days/Input/2016/Input4.txt";

  private const string TestInput4 = "aaaaa-bbb-z-y-x-123[abxyz]";

  private const string TestInput5 = "abc";

  private const string ActualInput5 = "ffykfhsq";

  private static int[] ShortcutIndexes = new[]{
      515840,
      844745,
      2968550,
      4034943,
      5108969,
      5257971,
      5830668,
      5833677,
      6497076,
      6681564,
      8793263,
      8962195,
      10715437,
      10999728,
      11399249,
      12046531,
      12105075,
      14775057,
      15502588,
      15872452,
      16105326,
      18804482,
      18830862,
      19388652,
      19474413,
      20787586,
      21302616,
      23462555,
      23551279,
      23853737,
      23867827,
      24090051,
      26246522,
      26383109};

  private const string TestInput6 = @"eedadn
drvtee
eandsr
raavrd
atevrs
tsrnev
sdttsa
rasrtv
nssdts
ntnada
svetve
tesnvt
vntsnd
vrdear
dvrsen
enarar";

  private const string ActualInput6 = "./Days/Input/2016/Input6.txt";

  private const string ActualInput7 = "./Days/Input/2016/Input7.txt";

  private static string[] TestInput7 = new[]{"abba[mnop]qrst",
  "abcd[bddb]xyyx",
  "aaaa[qwer]tyui",
  "ioxxoj[asdfgh]zxcvbn"};

  private const string ActualInput8 = "./Days/Input/2016/Input8.txt";

  private static string[] TestInput8 = new[]{
      "rect 3x2",
      "rotate column x=1 by 1",
      "rotate row y=0 by 4",
      "rotate column x=1 by 1",
    };

  private static string[] TestInput9 = new[]{
      "ADVENT",
      "A(1x5)BC",
      "(3x3)XYZ",
      "A(2x2)BCD(2x2)EFG",
      "(6x1)(1x3)A",
      "X(8x2)(3x3)ABCY",
    };

  private const string ActualInput9 = "./Days/Input/2016/Input9.txt";

  private const string ActualInput10 = "./Days/Input/2016/Input10.txt";

  private static string[] TestInput10 = new[]{"value 5 goes to bot 2",
"bot 2 gives low to bot 1 and high to bot 0",
"value 3 goes to bot 1",
"bot 1 gives low to output 1 and high to bot 0",
"bot 0 gives low to output 2 and high to output 0",
"value 2 goes to bot 2"};

  private const string ActualInput11 = "./Days/Input/2016/Input11.txt";

  private static string[] TestInput11 = new[]{
"The first floor contains a hydrogen-compatible microchip and a lithium-compatible microchip.",
"The second floor contains a hydrogen generator.",
"The third floor contains a lithium generator.",
"The fourth floor contains nothing relevant."
};

  private static string[] TestInput12 = new[]{
      "cpy 41 a",
"inc a",
"inc a",
"dec a",
"jnz a 2",
"dec a"
    };

  private const string ActualInput12 = "./Days/Input/2016/Input12.txt";

  private const string ActualInput15 = "./Days/Input/2016/Input15.txt";

  private const string ActualInput21 = "./Days/Input/2016/Input21.txt";

  private static string[] TestInput22 = new[]{
      "Filesystem            Size  Used  Avail  Use%",
"/dev/grid/node-x0-y0   10T    8T     2T   80%",
"/dev/grid/node-x0-y1   11T    6T     5T   54%",
"/dev/grid/node-x0-y2   32T   28T     4T   87%",
"/dev/grid/node-x1-y0    9T    7T     2T   77%",
"/dev/grid/node-x1-y1    8T    0T     8T    0%",
"/dev/grid/node-x1-y2   11T    7T     4T   63%",
"/dev/grid/node-x2-y0   10T    6T     4T   60%",
"/dev/grid/node-x2-y1    9T    8T     1T   88%",
"/dev/grid/node-x2-y2    9T    6T     3T   66%"
    };

  private const string ActualInput22 = "./Days/Input/2016/Input22.txt";

  private const string ActualInput23 = "./Days/Input/2016/Input23.txt";

  private static List<string> TestInput24 = new List<string>{
      "###########",
      "#0.1.....2#",
      "#.#######.#",
      "#4.......3#",
      "###########"};

  private const string ActualInput24 = "./Days/Input/2016/Input24.txt";

  private const string ActualInput25 = "./Days/Input/2016/Input25.txt";

  public static string Day1()
  {
    var input1Array = Input1.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

    var player = new Player();

    foreach (var instruction in input1Array)
    {
      var trimmed = instruction.Trim();

      player.HandleInput(trimmed[0], int.Parse(trimmed.Substring(1)));
    }

    return string.Concat(
      string.Format("Day 1 p1: Destination is at X: {0} Y: {1} Amount of blocks away: {2}", player.X, player.Y, player.AmountOfBlocksAwayFromStart()),
      Environment.NewLine,
      string.Format("Day 1 p2: {0}", player.Message)
    );
  }

  public static string Day2()
  {
    var input2Array = Input2.Split(new[]
    {
       Environment.NewLine
      }, StringSplitOptions.RemoveEmptyEntries);

    var keypad = new Keypad();
    var bsKeyPad = new BullShitKeyPad();
    var sb = new StringBuilder();
    var bssb = new StringBuilder();

    foreach (var line in input2Array)
    {
      sb.Append(keypad.ReturnButtonToPress(line));
      bssb.Append(bsKeyPad.ReturnButtonToPress(line));
    }

    return string.Concat(
      string.Format("Day2 p1: The code for the normal keypad is: {0}", sb),
      Environment.NewLine,
      string.Format("Day2 p2: The code for the bullshit keypad is: {0}", bssb));
  }

  public static string Day3()
  {
    var input3 = File.ReadLines(Input3);
    var triangle = new Triangle();

    var amountPossible = 0;

    foreach (var line in input3)
    {
      var splitInput = Regex.Split(line.Trim(), @"(\d+)[ ]+(\d+)[ ]+(\d+)");

      triangle.SetSides(
        int.Parse(splitInput[1].Trim()),
        int.Parse(splitInput[2].Trim()),
        int.Parse(splitInput[3].Trim())
      );

      if (triangle.IsPossible())
      {
        amountPossible++;
      }
    }

    var splitInput3 = input3.Select(x => Regex.Split(x.Trim(), @"(\d+)[ ]+(\d+)[ ]+(\d+)")).ToArray();

    var amountPossibleP2 = 0;

    for (var idx = 0; idx < splitInput3.Length; idx += 3)
    {
      if (idx + 2 >= splitInput3.Length)
      {
        continue;
      }

      for (var idx2 = 1; idx2 < 4; idx2++)
      {
        triangle.SetSides(
          int.Parse(splitInput3[idx][idx2]),
          int.Parse(splitInput3[idx + 1][idx2]),
          int.Parse(splitInput3[idx + 2][idx2])
        );

        if (triangle.IsPossible())
        {
          amountPossibleP2++;
        }
      }
    }

    return string.Concat(
      string.Format("Day 3 p1: The amount of possible triangles is : {0}", amountPossible),
      Environment.NewLine,
      string.Format("Day 3 p2: The amount of possible triangles is : {0}", amountPossibleP2)
    );
  }

  public static string Day4()
  {
    var result = new StringBuilder();

    var input4 = File.ReadLines(Input4);
    var room = new Room();

    var sumSector = 0;

    foreach (var line in input4)
    {
      var lastDashIndex = line.LastIndexOf('-');
      var firstBracketIndex = line.IndexOf('[');

      room.SetRoom(
        line.Substring(0, lastDashIndex),
        line.Substring(lastDashIndex + 1, firstBracketIndex - lastDashIndex - 1),
        line.Substring(firstBracketIndex)
      );

      if (room.IsReal())
      {
        sumSector += room.Sector;
        var name = room.DecryptRoomName();

        if (name.Contains("pole"))
        {
          result.AppendLine(string.Format("Day 4 p2: Sector ID: {0}, Decryption result: {1}", room.Sector, name));
        }
      }
    }

    result.AppendLine(string.Format("Day 4 p1: The sum of sector id's is: {0}", sumSector));

    return result.ToString();
  }

  public static string Day5()
  {
    System.Console.WriteLine("Beginning to crack passwords..");

    var passwordBuilder = new StringBuilder();

    var linePos = 0;

    foreach (var shortcutIndex in ShortcutIndexes)
    {
      if (passwordBuilder.Length == 8)
      {
        break;
      }

      var inputBytes = System.Text.Encoding.ASCII.GetBytes(string.Format("{0}{1}", ActualInput5, shortcutIndex));

      using (var md5 = System.Security.Cryptography.MD5.Create())
      {
        var hashBytes = md5.ComputeHash(inputBytes);

        var hex = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

        if (hex.StartsWith("00000"))
        {
          var character = hex[5];
          passwordBuilder.Append(character);
          System.Console.Write(character);
          Console.SetCursorPosition(++linePos, Console.CursorTop);
        }
      }
    }

    Console.WriteLine();

    var slightlyBetterPasswordBuilder = new char[8];

    var charactersFound = 0;

    foreach (var shortcutIndex in ShortcutIndexes)
    {
      if (charactersFound == 8)
      {
        break;
      }

      var inputBytes = System.Text.Encoding.ASCII.GetBytes(string.Format("{0}{1}", ActualInput5, shortcutIndex));

      using (var md5 = System.Security.Cryptography.MD5.Create())
      {
        var hashBytes = md5.ComputeHash(inputBytes);

        var hex = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

        if (hex.StartsWith("00000"))
        {
          int position;

          if (int.TryParse(hex[5].ToString(), out position))
          {
            if (position < 8)
            {
              if (slightlyBetterPasswordBuilder[position] == '\0')
              {
                slightlyBetterPasswordBuilder[position] = hex[6];
                charactersFound++;
                Console.SetCursorPosition(position, Console.CursorTop);
                System.Console.Write(hex[6]);
              }
            }
          }
        }
      }
    }

    Console.WriteLine();

    return string.Concat(string.Format("Day 5 p1: {0}", passwordBuilder.ToString()), Environment.NewLine, string.Format("Day 5 p2: {0}", string.Join("", slightlyBetterPasswordBuilder)));
  }

  public static string Day6()
  {
    var positionFrequencies = new Dictionary<int, Dictionary<char, int>>();

    foreach (var line in File.ReadAllText(ActualInput6).Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
    {
      for (var idx = 0; idx < line.Length; idx++)
      {
        if (!positionFrequencies.ContainsKey(idx))
        {
          positionFrequencies.Add(idx, new Dictionary<char, int>());
          positionFrequencies[idx].Add(line[idx], 1);
        }
        else
        {
          if (positionFrequencies[idx].Select(x => x.Key).Contains(line[idx]))
          {
            positionFrequencies[idx][line[idx]]++;
          }
          else
          {
            positionFrequencies[idx].Add(line[idx], 1);
          }
        }

      }
    }

    var sb1 = new StringBuilder();
    var sb2 = new StringBuilder();

    foreach (var freq in positionFrequencies)
    {
      sb1.Append(freq.Value.OrderByDescending(x => x.Value).First().Key);
    }

    foreach (var freq in positionFrequencies)
    {
      sb2.Append(freq.Value.OrderBy(x => x.Value).First().Key);
    }

    return string.Concat(
      string.Format("Day 6 p1: {0}", sb1.ToString()),
      Environment.NewLine,
      string.Format("Day 6 p2: {0}", sb2.ToString())
    );
  }

  public static string Day7()
  {
    var input = File.ReadAllLines(ActualInput7);
    var p1 = 0;
    var p2 = 0;

    foreach (var line in input)
    {
      var split = line.Split(new[] { '[', ']' });

      var ipv7s = new List<string>();
      var hyperNet = new List<string>();

      for (var idx = 0; idx < split.Length; idx++)
      {
        if (idx % 2 != 0)
        {
          //If the index is even, it is a hypernet.
          hyperNet.Add(split[idx]);
        }
        else
        {
          //If it's odd, it's not.
          ipv7s.Add(split[idx]);
        }
      }

      //P1

      if (ipv7s.Any(x => PalindromeHelper.ContainsFourLetterPalindrome(x)))
      {
        if (!hyperNet.Any(net => PalindromeHelper.ContainsFourLetterPalindrome(net)))
        {
          p1++;
        }
      }

      //P2

      var hyperNetAbas = hyperNet.SelectMany(x => PalindromeHelper.ABACheck(x));

      bool isSsl = false;

      foreach (var ipv7 in ipv7s)
      {
        foreach (var abaCheck in PalindromeHelper.ABACheck(ipv7))
        {
          var bab = string.Join("", new[] { abaCheck[1], abaCheck[0], abaCheck[1] });

          if (hyperNetAbas.Contains(bab))
          {
            isSsl = true;
          }
        }
      }

      if (isSsl)
      {
        p2++;
      }
    }

    return string.Concat(string.Format("Day7 p1: {0} ips are TLS supported.", p1), Environment.NewLine, string.Format("Day7 p2: {0} ips are SSL supported", p2));
  }

  public static string Day8()
  {
    var screen = new Screen();

    foreach (var line in File.ReadLines(ActualInput8))
    {
      screen.ParseInput(line);
    }

    screen.DrawScreen();

    return string.Format("Day8 p1: {0} pixels on", screen.ReturnPixelsOn());
  }

  public static string Day9()
  {
    var dec = new Decompression();

    var output1 = dec.DecompressInput(File.ReadAllLines(ActualInput9), false);

    var output2 = dec.DecompressInput(File.ReadAllLines(ActualInput9), true);

    return string.Concat(string.Format("Day 9 p1 {0}", output1), Environment.NewLine, string.Format("Day 9 p2 {0}", output2));
  }

  public static string Day10()
  {
    var bots = new List<Bot>();

    var lines = File.ReadLines(ActualInput10);

    var linesCompleted = new Dictionary<string, bool>();

    //First, create the first batch of bots by checking the lines in which said bots retrieve something from the input bin.
    foreach (var line in lines)
    {
      if (line.StartsWith("value"))
      {
        var split = line.Split(' '); var value = int.Parse(split[1]); var botNumber = int.Parse(split[5]);

        var bot = bots.FirstOrDefault(x => x.Id == botNumber);

        if (bot == null)
        {
          bots.Add(new Bot
          {
            Id = botNumber,
            Chips = new List<Chip>
              {
                new Chip
                {
                  Value = value
                }
              }
          });
        }
        else
        {
          bot.Chips.Add(
            new Chip
            {
              Value = value
            });
        }
      }
      else
      {
        linesCompleted.Add(line, false);
      }
    }

    var output = new Dictionary<int, List<Chip>>();

    while (!linesCompleted.All(x => x.Value))
    {
      foreach (var line in linesCompleted.Where(x => !x.Value).Select(x => x.Key).ToArray())
      {
        if (line.StartsWith("bot"))
        {
          var split = line.Split(' ');

          var currentBot = bots.FirstOrDefault(x => x.Id == int.Parse(split[1]));

          if (currentBot == null)
          {
            currentBot = new Bot
            {
              Id = int.Parse(split[1])
            };

            bots.Add(currentBot);
          }

          linesCompleted[line] = currentBot.ParseCommand(split.Skip(3).ToArray(), ref bots, ref output);
        }
      }
    }

    var datBot = bots.FirstOrDefault(x => x.datbot);

    return string.Concat(string.Format("Day 10 p1: {0}", datBot == null ? "not found!" : datBot.Id.ToString()), Environment.NewLine, string.Format("Day 10 p2: {0}", output[0].First().Value * output[1].First().Value * output[2].First().Value));
  }

  public static string Day11()
  {
    //First, construct the floors according to the layout. Create pairs of the same type.
    var pairs = ParseLayout.Go(TestInput11);

    var initialState = new Solutions.Models.Day11.State
    {
      Floor = 0,
      Distance = 0,
      Distributions = pairs
    };

    var queue = new Queue<Solutions.Models.Day11.State>();

    queue.Enqueue(initialState);

    var answer = string.Empty;

    do
    {
      var currentState = queue.Dequeue();

      if (currentState.Distributions.All(x => x.Item1 == 3 && x.Item2 == 3)) //All the pairs are on the top floor.
      {
        answer = currentState.Distance.ToString();
      }
      else
      {
        var possibleMoves = currentState.DeterminePossibleMoves();

        foreach (var possibleMove in possibleMoves)
        {
          queue.Enqueue(possibleMove);
        }

        //Determine possible states from current state.
      }
    }
    while (queue.Count() > 0);

    return answer;
  }

  public static string Day12()
  {
    var assembunny = new Assembunny(false);
    var assembunny2 = new Assembunny(true);

    var lines = File.ReadLines(ActualInput12).ToArray();

    for (var instruction = 0; instruction < lines.Length;)
    {
      if (instruction < lines.Length)
      {
        // instruction += assembunny.ParseInput(lines[instruction], instruction, lines);
      }
      else
      {
        instruction = lines.Length - 1;
      }
    }

    for (var instruction = 0; instruction < lines.Length;)
    {
      if (instruction < lines.Length)
      {
        // instruction += assembunny2.ParseInput(lines[instruction], instruction, lines);
      }
      else
      {
        instruction = lines.Length - 1;
      }
    }

    return string.Concat(string.Format("Day 12 p1: {0}", assembunny.Registers['a']), Environment.NewLine, string.Format("Day 12 p2: {0}", assembunny2.Registers['a']));
  }

  public static string Day13()
  {
    var seed = 1358;
    var mazeSizeY = 50;
    var mazeSizeX = 50;
    var answer = string.Empty;

    var maze = new Maze(seed, mazeSizeY, mazeSizeX);

    var target = new Tuple<int, int>(39, 31);

    var queue = new Queue<Solutions.Models.Day13.State>();
    var previousStates = new List<Tuple<int, int>>();
    var uniqueCoordinates = 0;
    queue.Enqueue(new Solutions.Models.Day13.State
    {
      Steps = 0,
      CurrentPosition = new Tuple<int, int>(1, 1)
    });

    do
    {
      var currentState = queue.Dequeue();

      if (currentState.Steps < 51 && !previousStates.Contains(currentState.CurrentPosition))
      {
        uniqueCoordinates++;
      }

      //maze.PrintMaze(currentState.CurrentPosition, target, previousStates);

      previousStates.Add(currentState.CurrentPosition);

      if (currentState.CurrentPosition.Item1 == target.Item1 && currentState.CurrentPosition.Item2 == target.Item2)
      {
        answer = currentState.Steps.ToString();
      }

      var possibleMoves = maze.ReturnPossibleMoves(currentState.CurrentPosition, previousStates);

      foreach (var possibleMove in possibleMoves)
      {
        queue.Enqueue(new Solutions.Models.Day13.State
        {
          Steps = currentState.Steps + 1,
          CurrentPosition = possibleMove
        });
      }
    }
    while (queue.Any());

    return string.Concat(string.Format("Day 13 p1: {0}", answer), Environment.NewLine, string.Format("Day 13 p2: {0}", uniqueCoordinates));
  }

  public static string Day14()
  {
    var input = "jlmsuwbz";
    var otp = new OneTimePad();

    var sw = new Stopwatch();
    sw.Start();
    var output = otp.Process(input, false);
    sw.Stop();

    System.Console.WriteLine(sw.ElapsedMilliseconds);

    return string.Concat(
      string.Format("Day 14 p1: {0}", otp.Process(input, false)),
      Environment.NewLine,
      string.Format("Day 14 p2: {0}", otp.Process(input, true))
    );
  }

  public static string Day15()
  {
    var discs = new List<Disc>();

    var input = File.ReadAllLines(ActualInput15);

    foreach (var line in input)
    {
      discs.Add(new Disc(line));
    }

    var time1 = 0; var smoothSailing = false;

    while (!smoothSailing)
    {
      var discNumber = 1;
      var tempTime = time1;

      foreach (var disk in discs)
      {
        if (disk.CanMoveThrough(tempTime, discNumber))
        {
          if (discNumber == discs.Count())
          {
            smoothSailing = true;
          }
        }
        else
        {
          break;
        }

        discNumber++;
      }

      time1++;
    }

    return string.Concat(string.Format("Day 15 p1: {0}", time1 - 1));
  }

  public static string Day16()
  {
    var input = "11101000110010100".Select(x => x == '1').ToList();

    var requiredLength = 35651584;

    while (input.Count() < requiredLength)
    {
      var a = input;

      var bBuilder = a.Select(x => !x).Reverse();

      input = new List<bool>(a);
      input.Add(false);
      input.AddRange(bBuilder);
    }

    input = input.Take(requiredLength).ToList();

    var checkSum = new List<bool>();

    while (checkSum.Count() % 2 == 0)
    {
      var tempCheckSum = new List<bool>();

      for (var i = 0; i < input.Count(); i += 2)
      {
        if (i + 1 >= input.Count())
        {
          break;
        }

        tempCheckSum.Add((input[i] == input[i + 1]));
      }

      input = tempCheckSum;
      checkSum = tempCheckSum;
    }

    return string.Format("Day 16 p1: {0}", string.Join("", checkSum.Select(x => x ? '1' : '0')));
  }

  public static string Day17()
  {
    var input = "awrkjxxr";

    var grid = new bool[4][];
    var md5 = System.Security.Cryptography.MD5.Create();

    var room1 = new Rooms(new Tuple<int, int>(3, 3));

    var answer1 = room1.Process(md5, input, false);

    var room2 = new Rooms(new Tuple<int, int>(3, 3));

    var answer2 = room2.Process(md5, input, true);

    return string.Concat(string.Format("Day 17 p1: {0}", answer1), Environment.NewLine, string.Format("Day 27 p2: {0}", answer2));
  }

  public static string Day18()
  {
    var input = ".^.^..^......^^^^^...^^^...^...^....^^.^...^.^^^^....^...^^.^^^...^^^^.^^.^.^^..^.^^^..^^^^^^.^^^..^".Select(x => x == '^').ToArray();
    var gridRows = 400000; //p2
                           // var gridRows = 40; //p1
    var grid = new bool[gridRows][];
    grid[0] = input;

    for (var row = 1; row < gridRows; row++)
    {
      grid[row] = new bool[input.Length];

      for (var tile = 0; tile < input.Length; tile++)
      {
        var left = tile == 0 ? false : grid[row - 1][tile - 1];
        var center = grid[row - 1][tile];
        var right = tile == input.Length - 1 ? false : grid[row - 1][tile + 1];

        grid[row][tile] = ((left && center && !right) ||
                            (!left && center && right) ||
                            (left && !center && !right) ||
                            (!left && !center && right)
                          );
      }
    }

    var safeTiles = grid.SelectMany(x => x.Select(y => y)).Count(x => !x);

    System.Console.WriteLine(safeTiles);

    return string.Format("Day 18: {0}", gridRows);
  }

  public static string Day19()
  {
    var input = 3004953;

    var tempInput = input;
    var remainder = 0;

    var power = 0d; var idx = 1;
    var powers = new List<double>();

    while (power < input)
    {
      power = Math.Pow(2, idx++);
      powers.Add(power);
    }

    while (!powers.Contains(tempInput))
    {
      tempInput -= 1;
      remainder++;
    }

    //This does the same as the above, but it's more fun.

    var inputForFun = Convert.ToString(input, 2);

    var answer = Convert.ToInt32(inputForFun.Substring(1) + inputForFun[0], 2); //lel

    int winner = 1;

    for (var iterator = 1; iterator < input; iterator++)
    {
      winner = winner % iterator + 1;
      if (winner > (iterator + 1) / 2)
      {
        winner++;
      }
    }

    return string.Concat(string.Format("Day 19 p1: {0} {1}", (remainder * 2) + 1), answer, Environment.NewLine, string.Format("Day 19 p2: {0}", winner));
  }

  public static string Day20()
  {
    var lines = File.ReadAllLines(@"./Days/Input/2016/Input20.txt");

    var pairs = new List<Pair>();

    foreach (var line in lines)
    {
      pairs.Add(new Pair(line));
    }

    var orderedPairs = pairs.OrderBy(x => x.LowerBound).ThenBy(x => x.UpperBound).ToArray();

    uint answer = 0;

    var iterations = 0;

    while (answer == 0)
    {
      var startPoint = orderedPairs.Skip(iterations).First();

      var endPoint = orderedPairs.Skip(iterations + 1).First();

      if (startPoint.UpperBound < endPoint.LowerBound && startPoint.UpperBound + 1 != endPoint.LowerBound)
      {
        answer = startPoint.UpperBound + 1;
      }

      //Between the startPoint and the endpoint we need to check for the first number that doesn't fall inside one of the ranges. If there's no number, we need to move on to the next pair.

      iterations++;
    }

    foreach (var orderedPair in orderedPairs)
    {
      var startPoint = orderedPair;

      var endPoint = orderedPairs[Array.IndexOf(orderedPairs, orderedPair) + 1];
    }

    uint ip = 0;
    uint answer2 = 0;
    var pair = pairs.FirstOrDefault(x => x.LowerBound <= ip && x.UpperBound >= ip);
    //p2
    while (ip < uint.MaxValue)
    {
      while (pair != null)
      {
        ip = pair.UpperBound + 1;
        pair = pairs.FirstOrDefault(x => x.LowerBound <= ip && x.UpperBound >= ip);
      }

      if (ip < uint.MaxValue)
      {
        answer2++;
        ip++;
        pair = pairs.FirstOrDefault(x => x.LowerBound <= ip && x.UpperBound >= ip);
      }
    }

    return string.Format("Day 20 P1: {0} Day 20 P2: {1}", answer, answer2);
  }

  public static string Day21()
  {
    //p1

    var input = "fbgdceah".ToCharArray();

    var scrambler = new Scrambler();

    //var output = scrambler.Scramble(input.ToArray(), File.ReadAllLines(ActualInput21), false);
    var output = string.Empty;
    //p2

    var output2 = scrambler.Scramble(input.ToArray(), File.ReadAllLines(ActualInput21).Reverse().ToArray(), true);

    return string.Concat(string.Format("Day 21 P1: {0}", string.Join("", output)), Environment.NewLine, string.Format("Day 21 P2: {0}", output2));
  }

  public static string Day22()
  {
    var dfResults = new List<DfResult>();

    foreach (var line in File.ReadAllLines(ActualInput22))
    {
      if (line.StartsWith("/"))
      {
        dfResults.Add(new DfResult(line));
      }
    }

    //P1

    var viablePairs = 0;

    foreach (var dfResult in dfResults)
    {
      viablePairs += dfResult.CalculateNumberOfViableNodes(dfResults);
    }

    //P2

    //First, create the grid. For visualisation.

    var dfGrid = new DfResult[dfResults.Max(df => df.Y) + 1][];
    var maxX = dfResults.Max(df => df.X + 1);
    var largeNodes = dfResults.Where(x => x.Used > dfResults.Max(y => y.Avail));

    var printGrid = true;

    var queue = new Queue<GridState>();

    for (var y = 0; y < dfGrid.Length; y++)
    {
      dfGrid[y] = new DfResult[maxX];

      for (var x = 0; x < maxX; x++)
      {
        var node = dfResults.First(df => df.ID == string.Format("{0}{1}", y, x));
        dfGrid[y][x] = node;

        if (printGrid)
        {
          if (y == 0 && x == 0)
          {
            Console.Write("(.)");
          }
          else if (node.UsePercentage == 0)
          {
            Console.Write(" _ ");
          }
          else if (y == 0 && x == dfResults.Max(df => df.X))
          {
            Console.Write(" G ");
          }
          else if (largeNodes.Select(n => n.ID).Contains(string.Format("{0}{1}", y, x)))
          {
            Console.Write(" # ");
          }
          else
          {
            Console.Write(" . ");
          }
        }
      }

      if (printGrid)
      {
        Console.WriteLine();
      }
    }

    //Really, visualisation is all you need. Fuck the police.

    return string.Concat(string.Format("Day 22 p1: {0}", viablePairs), Environment.NewLine, string.Format("Day 22 p2: {0}", @"¯\_(ツ)_/¯"));
  }

  public static string Day23()
  {
    var assembunny = new Assembunny(false);

    var lines = File.ReadLines(ActualInput23).ToArray();

    for (var instruction = 0; instruction < lines.Length;)
    {
      assembunny.PrintRegisters();

      if (instruction < lines.Length)
      {
        // instruction += assembunny.ParseInput(lines[instruction], instruction, lines);
      }
      else
      {
        instruction = lines.Length - 1;
      }
    }

    return assembunny.Registers['a'].ToString();
  }

  public static string Day24()
  {
    var lines = File.ReadAllLines(ActualInput24);

    var grid = new int?[lines.Count()][];

    var positions = new Dictionary<int, Tuple<int, int>>();

    for (var lineNr = 0; lineNr < lines.Count(); lineNr++)
    {
      var line = lines[lineNr];

      grid[lineNr] = new int?[line.Length];

      for (var charNr = 0; charNr < line.Length; charNr++)
      {
        switch (line[charNr])
        {
          case '#':
            {
              grid[lineNr][charNr] = -1;
            }
            break;
          case '.':
            {
              grid[lineNr][charNr] = null;
            }
            break;
          default:
            {
              positions.Add(int.Parse(new string(line[charNr], 1)), new Tuple<int, int>(lineNr, charNr));
              grid[lineNr][charNr] = int.Parse(new string(line[charNr], 1));
            }
            break;
        }
      }
    }

    var nodes = new List<Node>();

    var start = new Node
    {
      Name = 0,
      Distance = 0,
      Position = positions[0]
    };

    nodes.Add(start);

    foreach (var position in positions.Where(x => x.Key > 0))
    {
      nodes.Add(new Node
      {
        Name = position.Key,
        Distance = int.MaxValue,
        Position = position.Value
      });
    }

    foreach (var node in nodes)
    {
      node.Neighbors = nodes.Except(new[] { node, nodes.First(x => x.Name == 0) }).ToList();
    }

    var initial = nodes.First(x => x.Name == 0);

    foreach (var current in initial.Neighbors.OrderBy(x => x.Distance))
    {
      current.Distance = current.CalculateDistance(initial, grid);

      var prev = current;
      current.Path.AddRange(new[] { initial, current });

      var currentNeighbors = current.Neighbors.Select(x => x).ToList();

      for (var i = 0; i < prev.Neighbors.Count(); i++)
      {
        var neighbor = currentNeighbors.OrderBy(x => (prev.CalculateDistance(x, grid) + initial.CalculateDistance(x, grid)) / 2).First();

        currentNeighbors.Remove(neighbor);

        current.Path.Add(neighbor);

        current.TotalDistance += neighbor.CalculateDistance(prev, grid);
        prev = neighbor;
      }

      current.Visited = true;
      current.TotalDistance += current.Distance;
    }

    var quickestPath = nodes.Where(x => x.TotalDistance != 0).OrderBy(x => x.TotalDistance).First().TotalDistance;

    //p2

    var quickest = nodes.Where(x => x.TotalDistance != 0).OrderBy(x => x.TotalDistance).First();

    var p2 = int.MaxValue;

    foreach (var node in nodes.Where(x => x.Name > 0))
    {
      var totalDistance = node.TotalDistance + node.Path.Last().CalculateDistance(initial, grid);

      p2 = Math.Min(p2, totalDistance);
    }

    return string.Concat(
      string.Format("Day 24 p1: {0}", quickestPath),
      Environment.NewLine,
      string.Format("Day 24 p2: {0}", p2)
    );
  }

  public static string Day25()
  {
    var bunny = new Assembunny(false);

    var lines = File.ReadAllLines(ActualInput25);

    for (var i = 0; i < int.MaxValue; i++)
    {
      bunny = new Assembunny(false);
      bunny.Registers['a'] = i;

      System.Console.WriteLine();
      System.Console.WriteLine("starting for {0}", bunny.Registers['a']);
      System.Console.WriteLine();
      bool? clock = null;
      var sb = new StringBuilder();

      for (var instruction = 0; instruction < lines.Length;)
      {
        if (sb.Length > 100)
        {
          System.Console.WriteLine($"The answer is {i}!");
          return i.ToString();
        }

        if (instruction < lines.Length)
        {
          int? outClock;

          instruction += bunny.ParseInput(lines[instruction], instruction, lines, out outClock);

          if (outClock != null)
          {
            if (clock == null)
            {
              if (outClock != 0)
              {
                break;
              }
            }
            else
            {
              if (outClock == 1 == clock)
              {
                break; //the sequence is broken.
              }
            }

            clock = outClock == 1;
            sb.Append(outClock);
          }
        }
        else
        {
          instruction = lines.Length - 1;
        }
      }
    }

    return string.Empty;
  }
}