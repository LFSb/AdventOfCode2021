namespace Solutions.Models.Day20
{
  public class Pair
  {
    public uint LowerBound { get; set; }

    public uint UpperBound { get; set; }

    public Pair(string input)
    {
      var split = input.Split('-');

      LowerBound = uint.Parse(split[0]);

      UpperBound = uint.Parse(split[1]);
    }
  }
}