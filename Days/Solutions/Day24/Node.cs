using System.Collections.Generic;
using System;

namespace Solutions.Models.Day24
{
  public class Node
  {
    public int Name { get; set; }

    public int Distance { get; set; }

    public int TotalDistance { get; set; }

    public bool Visited { get; set; }

    public List<Node> Path { get; set; } = new List<Node>();

    public Tuple<int, int> Position { get; set; }

    public List<Node> Neighbors { get; set; } = new List<Node>();

    public int CalculateDistance(Node neighbor, int?[][] grid) 
    {
      var steps = 0;
      var queue = new Queue<BotState>();

      queue.Enqueue(new BotState
      {
        Steps = 0,
        Position = this.Position
      });

      var visitedLocations = new List<Tuple<int, int>>();
      
      visitedLocations.Add(this.Position);

      while(steps == 0)
      {
        var currentState = queue.Dequeue();
        
        if(currentState.Position.Item1 == neighbor.Position.Item1 && currentState.Position.Item2 == neighbor.Position.Item2)
        {
          steps = currentState.Steps;
        }
        else
        {
          var nextSteps = currentState.ReturnPossibleStates(grid, visitedLocations);

          foreach(var step in nextSteps)
          {
            visitedLocations.Add(step);

            queue.Enqueue(new BotState
            {
              Steps = currentState.Steps + 1,
              Position = step
            });
          }
        }
      }

      return steps;
    }
  }
}