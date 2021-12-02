using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solutions.Models.Day4
{
  public class Room
  {
    private readonly Dictionary<char, int> _letterOccurance = new Dictionary<char, int>();

    public string Name { get; private set; }

    public int Sector { get; private set; }

    public string CheckSum { get; private set; }

    public void SetRoom(string name, string sector, string checkSum)
    {
      Name = name;
      Sector = int.Parse(sector);
      CheckSum = checkSum.Trim('[', ']');
      _letterOccurance.Clear();
    }

    public bool IsReal()
    {
      //First, score the occurances of letters in the name.
      foreach (var character in Name.Where(char.IsLetter))
      {
        if (!_letterOccurance.ContainsKey(character))
        {
          _letterOccurance.Add(character, 1);
        }
        else
        {
          _letterOccurance[character]++;
        }
      }

      var orderedOccurance = _letterOccurance
        .GroupBy(keyValuePair => keyValuePair.Value)
        .SelectMany(keyValuePairs => keyValuePairs)
        .OrderByDescending(keyValuePair => keyValuePair.Value)
        .ThenBy(keyValuePair => keyValuePair.Key)
        .Take(5)
        .ToArray();

      for (var idx = 0; idx < 5; idx++)
      {
        if (CheckSum[idx] != orderedOccurance[idx].Key)
        {
          return false;
        }
      }

      return true;
    }

    public string DecryptRoomName()
    {
      var sb = new StringBuilder();

      foreach (var character in Name)
      {
        var localChar = character;

        if (character == '-')
        {
          sb.Append(' ');
        }
        else
        {
          for (var idx = 0; idx < Sector; idx++)
          {
            if ((int)localChar == 122)
            {
              localChar = (char)97;
            }
            else
            {
              localChar++;
            }
          }
          
          sb.Append(localChar);
        }
      }

      return sb.ToString();
    }
  }
}
