using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Solutions.Models.Day13
{
  public class Maze
  {
    public int[][] Grid { get; set;}

    public Maze(int seed, int mazeSizeY, int mazeSizeX)
    {
      Grid = new int[mazeSizeY][];

      for(var y = 0; y < Grid.Length; y++)
      {
        Grid[y] = new int[mazeSizeX];

        for(var x = 0; x < Grid[y].Length; x++)
        {
          int amountOf1s = 0;
          
          var bitArray = new BitArray(
            new int[]{
              seed + (x * x + 3 * x + 2 * x * y + y + y * y)
            }
          );
          
          foreach(bool bit in bitArray)
          {
            if(bit)
            {
              amountOf1s++;
            }
          }

          Grid[y][x] = amountOf1s;
        }
      }
    }

    public List<Tuple<int, int>> ReturnPossibleMoves(Tuple<int, int> currentPosition, List<Tuple<int, int>> previousPositions)
    {
      var returnList = new List<Tuple<int, int>>();

      for(var direction = 0; direction < 4; direction++)
      {
        var newPosition = default(Tuple<int, int>);

        if(direction % 2 == 0) //Move on Y axis
        {
          var yShift = direction - 1;
          if(currentPosition.Item1 + yShift >= 0 && currentPosition.Item1 + yShift < Grid.Length && Grid[currentPosition.Item1 + yShift][currentPosition.Item2] % 2 == 0)
          {
            newPosition = new Tuple<int, int>(currentPosition.Item1 + yShift, currentPosition.Item2);
          }          
        }
        else
        {
          var xShift = direction - 2;
          if(currentPosition.Item2 + xShift >= 0 && currentPosition.Item2 + xShift < Grid.First().Length && Grid[currentPosition.Item1][currentPosition.Item2 + xShift] % 2 == 0)
          {
            newPosition = new Tuple<int, int>(currentPosition.Item1, currentPosition.Item2 + xShift);
          }
        }

        if(newPosition != null && !previousPositions.Contains(newPosition))
        {
          returnList.Add(newPosition);
        }       
      }

      return returnList;
    }

    //This was the old implementation. New one will(should) be much better.
    public int Move(ref Tuple<int, int> currentPosition, ref List<Tuple<int, int>> visitedCoordinates, List<Tuple<int, int>> uniqueCoordinates, int xMove, int yMove)
    {
      var xy = false;
      var moves = 0;

      while(moves < 4)
      {
        if(!xy) //Move along the X axis
        {
          if(Grid[currentPosition.Item1][currentPosition.Item2 + xMove] % 2 == 0 
          && !uniqueCoordinates.Select(x => $"{x.Item1}{x.Item2}").Contains($"{currentPosition.Item1}{currentPosition.Item2 + xMove}")
          && xMove != 0)
          {
            currentPosition = new Tuple<int, int>(currentPosition.Item1, currentPosition.Item2 + xMove);
            visitedCoordinates.Add(currentPosition);
            return 1;
          }
          else //If you can't move on the X axis, try the Y axis.
          {
            xMove = xMove == 1 ? -1 : 1;
            xy = !xy;
          }
        }
        else //Move along the Y axis
        {
          if(Grid[currentPosition.Item1+ yMove][currentPosition.Item2] % 2 == 0 
          && !uniqueCoordinates.Select(x => $"{x.Item1}{x.Item2}").Contains($"{currentPosition.Item1 + yMove}{currentPosition.Item2}")
          && yMove != 0)
          {
            currentPosition = new Tuple<int, int>(currentPosition.Item1 + yMove, currentPosition.Item2);
            visitedCoordinates.Add(currentPosition);
            return 1;
          }
          else //If you can't move on the Y axis, try the X axis again.
          {
            yMove = yMove == 1 ? -1 : 1;
            xy = !xy;
          }
        }
        
        moves++;
      }

      //We've gotten stuck. Ruh roh.
      //We need to calculate how much moves we need to go back to become un-stuck. How can we know that?
      currentPosition = visitedCoordinates[visitedCoordinates.IndexOf(currentPosition) - 1];
      visitedCoordinates.RemoveAt(visitedCoordinates.Count() - 1);
      return -1;
    }

    public void PrintMaze(Tuple<int, int> currentPosition, Tuple<int, int> targetCoordinate, List<Tuple<int, int>> coordinatesVisited)
    {
      for(var y = 0; y < Grid.Length; y++)
      {
        for(var x = 0; x < Grid[y].Length; x++)
        {
          if(y == currentPosition.Item1 && x == currentPosition.Item2)
          {
            System.Console.Write("O");
          }
          else if(y == targetCoordinate.Item1 && x == targetCoordinate.Item2)
          {
            System.Console.Write("!");
          }
          else if(coordinatesVisited.Contains(new Tuple<int, int>(y, x)))
          {
            System.Console.Write("X");
          }
          else
          {
            System.Console.Write(Grid[y][x] % 2 == 0 ? "." : "#");
          }          
        }
        
        System.Console.WriteLine();
      }
    }
  }
}