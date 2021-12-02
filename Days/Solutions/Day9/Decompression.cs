using System.Linq;

namespace Solutions.Models.Day9
{
  public class Decompression
  {
    public long DecompressInput(string[] inputLines, bool part2)
    {
      long outputLength = 0;

      foreach(var input in inputLines)
      {
        outputLength += ParseInput(input, part2);
      }

      return outputLength;
    }

    public long ParseInput(string input, bool part2)
    {
      long count = 0;

      for(var idx = 0; idx < input.Length; idx++)
      {
        if(input[idx] == '(')
        {
          var sub = input.Substring(idx, input.IndexOf(')', idx) - idx);
          
          var split = sub.Split('x').Select(x => x.Trim(new [] { '(', ')' })).ToArray();
          
          var charsToRepeat = int.Parse(split[0]);
          var amountToRepeat = int.Parse(split[1]);

          var repeat = input.Substring(input.IndexOf(')', idx) + 1, charsToRepeat);
          long repeatCount = 0;

          if(repeat.StartsWith("(") && part2) //We've got ourselves another marker.
          {
            repeatCount += ParseInput(repeat, part2);
          }
          else
          {
            repeatCount = repeat.Length;
          }

          for(var idx2 = 0; idx2 < int.Parse(split[1]); idx2++)
          {
            count += repeatCount;
          }

          idx = input.IndexOf(')', idx) +  charsToRepeat;
        }
        else
        {
          count++;
        }
      }

      return count;
    }
  }
}