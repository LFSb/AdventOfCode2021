using System.Collections.Generic;

namespace Solutions.Models.Day3
{
  public class Triangle
  {
    public int Side1 { get; private set; }

    public int Side2 { get; private set; }

    public int Side3 { get; private set; }

    public void SetSides(int side1, int side2, int side3)
    {
      Side1 = side1;
      Side2 = side2;
      Side3 = side3;
    }

    public bool IsPossible()
    {
      var ordered = new List<int>
      {
        Side1, 
        Side2, 
        Side3
      };

      ordered.Sort();

      if (ordered[0] + ordered[1] > ordered[2])
      {
        return true;
      }

      return false;
    }
  }
}
