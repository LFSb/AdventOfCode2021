using System;
using System.Linq;
using System.Collections.Generic;

namespace Solutions.Models.Day11
{
  public class State 
  {
    public int Floor { get; set; }

    public int Distance { get; set; }

    public List<Tuple<int, int>> Distributions { get; set; } //Each tuple represents a pair of generator/microchip.

    public List<State> DeterminePossibleMoves()
    {
      var returnList = new List<State>();
      var possibleMoves = new List<State>();

      var choices = Distributions.Where(x => x.Item1 == Floor || x.Item2 == Floor); //These are the Distributions where one of the items is on the same floor as the current floor.

      for(var dir = 0; dir < 2; dir++) //Elevator can go either up or down. Always try to go up first.
      {
        var tempDir = dir == 0 ? 1 : -1;

        foreach(var choice in choices)
        {
          Distributions.Remove(choice);

          if(choice.Item1 == choice.Item2) //Items are on the same floor, so we will try to move both.
          {
             Distributions.Add(new Tuple<int, int>(choice.Item1 + tempDir, choice.Item2 + tempDir));
          }
          else
          {
            Distributions.Add(new Tuple<int, int>(choice.Item1 == Floor ? choice.Item1 + tempDir : choice.Item1, choice.Item2 == Floor ? choice.Item2 + tempDir : choice.Item2));
          }

          possibleMoves.Add(new State
          {
            Floor = Floor + tempDir,
            Distance = Distance + tempDir,
            Distributions = Distributions
          });
        }
      }

      return returnList; //The returnList needs to be filled with all the possible moves that are actually legal.
    }

    private static bool IsLegal(State state) //We need a way to determine what states are legal and what aren't.
    {
      return true;
    }
  }
}