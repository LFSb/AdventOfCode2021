namespace Solutions.Models.Day15
{
  public class Disc
  {
    public int TotalPositions { get; private set;}

    public int StartPosition { get; private set; }

    public bool Gotcha { get; set; }

    public Disc(string input)
    {
      var split = input.Split(' ');

      TotalPositions = int.Parse(split[3]);

      StartPosition = int.Parse(split[11].Trim('.'));      
    }

    public bool CanMoveThrough(int time, int diskNumber)
    {
      return (time + diskNumber + StartPosition ) % TotalPositions == 0;
    }    
  }
}