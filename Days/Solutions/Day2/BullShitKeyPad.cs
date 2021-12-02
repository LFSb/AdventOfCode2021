namespace Solutions.Models.Day2
{
  public class BullShitKeyPad
  {
    private const int MaxPosition = 4;
    private const int MinPosition = 0;
    private const int NotAButton = -1;

    private static readonly int[][] BullshitKeyPad = new[]
    {
      new[] {-1, -1, 1, -1, -1},
      new[] {-1, 2, 3, 4, -1},
      new[] {5, 6, 7, 8, 9}, 
      new[] {-1, 97, 98, 99, -1},
      new[] {-1, -1, 100, -1, -1}
    };

    public BullShitKeyPad()
    {
      X = 0;
      Y = 2; //This is also 5.
    }

    private int X { get; set; }

    private int Y { get; set; }

    public string ReturnButtonToPress(string line)
    {
      foreach (var instruction in line)
      {
        switch (char.ToLower(instruction))
        {
          case 'u':
            {
              Move(true, false);
            } break;
          case 'r':
            {
              Move(false, true);
            } break;
          case 'd':
            {
              Move(true, true);
            } break;
          case 'l':
            {
              Move(false, false);
            } break;
        }
      }
      var output = BullshitKeyPad[Y][X];

      return char.IsLetter((char)output)
        ? string.Format("{0}", char.ToUpper((char)output))
        : string.Format("{0}", output);
    }

    private void Move(bool axis, bool increase)
    {
      var input = (axis ? Y : X) + (increase ? 1 : -1);

      if (!CheckBounds(input))
      {
        return;
      }

      if (axis)
      {
        if (BullshitKeyPad[input][X] == NotAButton)
        {
          return;
        }

        Y += (increase ? 1 : -1);
      }
      else
      {
        if (BullshitKeyPad[Y][input] == NotAButton)
        {
          return;
        }

        X += (increase ? 1 : -1);
      }
    }

    private static bool CheckBounds(int input)
    {
      return input >= MinPosition && input <= MaxPosition;
    }
  }
}