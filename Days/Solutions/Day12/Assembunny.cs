using System.Collections.Generic;

namespace Solutions.Models.Day12
{
  public class Assembunny
  {
    public Dictionary<char, int> Registers { get; set; } = new Dictionary<char, int>();

    public Assembunny(bool part2)
    {
      if(part2)
      {
        Registers.Add('a', 0);
        Registers.Add('b', 0);
        Registers.Add('c', 1);
        Registers.Add('d', 0);
      }
      else
      {
        Registers.Add('a', 12);
        Registers.Add('b', 0);
        Registers.Add('c', 0);
        Registers.Add('d', 0);
      }
    }
    
    //This method will return its offset.
    public int ParseInput(string input, int currentPosition, string[] lines, out int? clock)
    {
      clock = null;

      var split = input.Split(' ');

      switch(split[0]) //Instruction
      {
        case "out":
        {
          if(char.IsLetter(split[1][0]))
          {
            clock = Registers[split[1][0]];
            System.Console.Write(Registers[split[1][0]]);
          }
          else
          {
            clock = int.Parse(split[1]);
            System.Console.Write(split[1]);
          }
          
          return 1;
        }
        case "cpy":
        {
          if(char.IsLetter(split[1][0]))
          {
            Registers[split[2][0]] = Registers[split[1][0]];
          }
          else
          {
            Registers[split[2][0]] = int.Parse(split[1]);
          }          
          return 1;
        }
        case "mlt":
        {
          var val1 = Registers[split[1][0]];
          var val2 = Registers[split[2][0]];
          Registers[split[3][0]] = val1 * val2;
          return 1;
        } 
        case "inc":
        {
          Registers[split[1][0]]++;
          return 1;
        } 
        case "dec":
        {
          Registers[split[1][0]]--;
          return 1;
        } 
        case "jnz":
        {
          if(!Registers.ContainsKey(split[1][0]))
          {
            if(int.Parse(split[1]) == 0)
            {
              return 1;
            }
            else
            {
              if(char.IsLetter(split[2][0]))
              {
                return Registers[split[2][0]];
              }
              else
              {
                return int.Parse(split[2]);
              }              
            }            
          }

          return Registers[split[1][0]] == 0 ? 1 : int.Parse(split[2]);
        }
        case "tgl":
        {
          int offset = 0;

          if(char.IsLetter(split[1][0]))
          {
            offset = Registers[split[1][0]]; 
          }
          else
          {
            offset = int.Parse(split[1]); 
          }

          if(offset != 0 && (currentPosition + offset) < lines.Length)
          {
            var lineToChange = lines[currentPosition + offset].Split(' ');

            if(lineToChange.Length == 2) //One argument Instruction
            {
              lineToChange[0] = lineToChange[0] == "inc" ? "dec" : "inc";
            }
            else if(lineToChange.Length == 3) //Two argument Instruction
            {
              lineToChange[0] = lineToChange[0] == "jnz" ? "cpy" : "jnz";
            }

            lines[currentPosition + offset] = string.Join(" ", lineToChange);
          }         
          
          return 1;
        }
        default:
        {
          return 1;
        }
      }
    }

    public void PrintRegisters()
    {
      foreach(var register in Registers)
      {
        System.Console.WriteLine("Register '{0}' has value {1}", register.Key, register.Value);
      }
    }
  }
}