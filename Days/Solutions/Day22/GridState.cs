using System.Collections.Generic;

namespace Solutions.Models.Day22
{
  public class GridState
  {
    public int Steps { get; set; }

    public DfResult[][] Grid { get ; set; }

    public List<GridState> ReturnPossibleStates()
    {
      return new List<GridState>();
    }
  }
}