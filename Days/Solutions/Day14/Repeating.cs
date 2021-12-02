using System.Text;

namespace Solutions.Models.Day14
{
  public static class Repeating
  {
    public static bool ContainsRepeatingChar(string input, int repeatCount, out char character)
    {
      character = (char)0;

      for (int i = 0; i < input.Length; i++)
      {
        var sb = new StringBuilder();

        character = input[i];
        sb.Append(character);

        for(var repeat = 1; repeat < repeatCount && i + repeat < input.Length; repeat++)
        {
          if(input[i + repeat] == character)
          {
            sb.Append(input[i + repeat]);
          }
        }

        if(sb.ToString().Length == repeatCount)
        {
          return true;
        }
      }

      return false;
    }
  }
}