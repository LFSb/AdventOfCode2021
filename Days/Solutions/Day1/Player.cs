using System;
using System.Collections.Generic;
using System.IO;

namespace Solutions.Models.Day1
{
  public class Player
  {
    public Player()
    {
      Direction = DirectionEnum.North; //Initial direction.
      X = 0;
      Y = 0;
      PreviousLocations = new List<KeyValuePair<int, int>>();
    }

    public DirectionEnum Direction { get; private set; }

    public int X { get; private set; }

    public int Y { get; private set; }

    public List<KeyValuePair<int, int>> PreviousLocations { get; private set; }

    public string Message { get; private set; }

    public void HandleInput(char direction, int steps)
    {
      //First, change direction
      
      switch (char.ToLower(direction))
      {
        case 'l':
          {
            Direction--;
          } break;
        case 'r':
          {
            Direction++;
          } break;
        default:
          {
            throw new InvalidDataException("Invalid input");
          }
      }

      Direction = (DirectionEnum)((int)Direction % 4);

      if (Direction < 0)
      {
        Direction += 4;
      }

      //Afterwards, start walking.
      ChangePosition(Direction, steps);
    }

    public int AmountOfBlocksAwayFromStart()
    {
      return Math.Abs(X) + Math.Abs(Y);
    }

    private void LogPosition()
    {
      if (PreviousLocations.Contains(new KeyValuePair<int, int>(X, Y)) && string.IsNullOrEmpty(Message))
      {
        Message = string.Format("X {0} Y {1} was visited twice. Amount of Blocks away: {2}", X, Y, AmountOfBlocksAwayFromStart());
      }
      else
      {
        PreviousLocations.Add(new KeyValuePair<int, int>(X, Y));
      }
    }

    private void ChangePosition(DirectionEnum walkingDirection, int steps)
    {
      for (var idx = 0; idx < steps; idx++)
      {
        LogPosition();

        switch (walkingDirection)
        {
          case DirectionEnum.North:
            {
              Y++;
            }
            break;
          case DirectionEnum.East:
            {
              X++;
            }
            break;
          case DirectionEnum.South:
            {
              Y--;
            }
            break;
          case DirectionEnum.West:
            {
              X--;
            }
            break;
        }
      }
    }
  }
}