namespace Solutions.Models.Day2
{
  public class Keypad
  {
    private const int MaxPosition = 2;
    private const int MinPosition = 0;

    private static readonly int[][] KeyPad = new[]
    {
      new[] { 1, 2, 3 }, 
      new[] { 4, 5, 6 }, 
      new[] { 7, 8, 9 }
    };

    public Keypad()
    {
      X = 1;
      Y = 1;
    }

    private int X { get; set; }

    private int Y { get; set; }

    public int ReturnButtonToPress(string line)
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

      return KeyPad[Y][X];
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
        Y += (increase ? 1 : -1);
      }
      else
      {
        X += (increase ? 1 : -1);
      }
    }

    private bool CheckBounds(int input)
    {
      return input >= MinPosition && input <= MaxPosition;
    }
  }
}