using System; 
using System.Linq; 
using System.Collections.Generic; 

namespace Solutions.Models.Day10 
{
  public class Bot
  {
    public int Id { get; set; }

    public bool datbot { get; set; }

    public List<Chip> Chips { get; set; } = new List<Chip>(); 

    public bool ParseCommand(string[] split, ref List <Bot> otherBots, ref Dictionary <int, List<Chip>> output) 
    {
      var highOrLow = split[0]; 

      var botOrOutput = split[2] == "bot"; 

      var botOrOutputNumber = int.Parse(split[3]); 

      var chipToAdd = default(Chip); 

      var otherBot = default(Bot);

      if (botOrOutput) 
      {
        //If true, it's a bot. Check if a bot by that number exists, and if not, create it.

        otherBot = otherBots.FirstOrDefault(x => x.Id == botOrOutputNumber); 

        if (otherBot == null) 
        {
          otherBot = new Bot 
          {
            Id = botOrOutputNumber
          };

          otherBots.Add(otherBot); 
        }
      }
      else
      {
        //If false, move to output.
        
        if (!output.ContainsKey(botOrOutputNumber))       
        {                                                 
          output.Add(botOrOutputNumber, new List<Chip>());
        }                                                 
      }

      if(this.Chips.Count() == 2)
      {
        if(this.Chips.Select(x => x.Value).Contains(61) && this.Chips.Select(x => x.Value).Contains(17))
        {
          datbot = true;
        }

        chipToAdd = DetermineChip(highOrLow);
        
        var result = true;

        if(split.Count() > 4)
        {
          result = ParseCommand(split.Skip(5).ToArray(), ref otherBots, ref output);
        }

        if(botOrOutput)
        {
          otherBot.Chips.Add(chipToAdd);
        }
        else
        {
          output[botOrOutputNumber].Add(chipToAdd);
        }
        
        this.Chips.Remove(chipToAdd);

        return result;
      }
      else
      {
        return false;
      }
    }

    public Chip DetermineChip(string input)
    {
      var chipToAdd = default(Chip);

      switch (input) 
      {
        case "low": 
        {
          chipToAdd = this.Chips.OrderBy(x => x.Value).First();
        } break; 
        case "high": 
        {
          chipToAdd = this.Chips.OrderByDescending(x => x.Value).First();
        } break; 
        default: 
        {
          throw new Exception("Ya done goofed."); 
        }
      }

      return chipToAdd;
    }
  }
}