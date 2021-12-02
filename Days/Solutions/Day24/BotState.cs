using System;
using System.Collections.Generic;

namespace Solutions.Models.Day24
{
  public struct BotState
  {
    public int Steps { get; set; }

    public Tuple<int, int> Position { get; set; }

    public List<Tuple<int, int>> ReturnPossibleStates(int?[][] grid, List<Tuple<int, int>> visitedLocations)
    {
      var tuples = new List<Tuple<int, int>>();

      Tuple<int, int> tuple = null;

      for(var direction = 0; direction < 4; direction++)
      {
        switch(direction)
        {
          case 0: // move up
          {
            if(grid[Position.Item1 - 1][Position.Item2] != -1)
            {
              tuple = new Tuple<int, int>(Position.Item1 - 1, Position.Item2);

              if(!visitedLocations.Contains(tuple))
              {
                tuples.Add(tuple);
              }
            }
          } break;
          case 1: // move down
          {
            if(grid[Position.Item1 + 1][Position.Item2] != -1)
            {
              tuple = new Tuple<int, int>(Position.Item1 + 1, Position.Item2);
              
              if(!visitedLocations.Contains(tuple))
              {
                tuples.Add(tuple);
              }
            }
          } break;
          case 2: //move left
          {
            if(grid[Position.Item1][Position.Item2 - 1] != -1)
            {
              tuple = new Tuple<int, int>(Position.Item1, Position.Item2 - 1);

              if(!visitedLocations.Contains(tuple))
              {
                tuples.Add(tuple);
              }
            }
          } break;
          case 3: //move right
          {
            if(grid[Position.Item1][Position.Item2 + 1] != -1)
            {
              tuple = new Tuple<int, int>(Position.Item1, Position.Item2 + 1);
              
              if(!visitedLocations.Contains(tuple))
              {
                tuples.Add(tuple);
              }
            }
          } break;
          default:
          {
            throw new Exception();
          }
        }
      }

      return tuples;
    }
  }
}