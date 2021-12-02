using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Solutions.Models.Day17
{
  public class Rooms
  {
    public bool[][] Grid { get; set; }

    public Tuple<int, int> TargetPosition { get; set; } 

    public Rooms(Tuple<int, int> targetPosition)
    {
      Grid = new bool[4][];
      TargetPosition = targetPosition;
    }

    public string Process(MD5 md5, string input, bool part2)
    {
      string result = string.Empty;
      var posibilities = new bool[4];

      var queue = new Queue<KeyValuePair<string, Tuple<int, int>>>(new []{ new KeyValuePair<string, Tuple<int, int>>(string.Empty, new Tuple<int, int>(0, 0)) });

      do
      {
        var state = queue.Dequeue();
        var hash = BitConverter.ToString(md5.ComputeHash(Encoding.ASCII.GetBytes(input + state.Key))).Replace("-", string.Empty).ToLower().Substring(0, 4);

        if(state.Value.Item1 == TargetPosition.Item1 && state.Value.Item2 == TargetPosition.Item2)
        {
          if(part2)
          {
            result = Math.Max(result.Length, state.Key.Length).ToString();
          }
          else
          {
            result = string.IsNullOrEmpty(result) ? state.Key : result;
          }
        }
        else
        {
          for(var direction = 0; direction < 4; direction++)
          {
            switch(hash[direction])
            {
              case 'b':
              case 'c':
              case 'd':
              case 'e':
              case 'f':
              {
                posibilities[direction] = true; //Door is open and unlocked.
              } break;
              default:
              {
                posibilities[direction] = false; //Door is locked.
              } break;
            }
          }

          var possibleStates = CalculatePossibleStates(state.Value, TargetPosition, posibilities);

          foreach(var possibleState in possibleStates)
          {
            queue.Enqueue(new KeyValuePair<string, Tuple<int, int>>(state.Key + possibleState.Key, possibleState.Value));
          }
        }
      }
      while(queue.Count() > 0);

      return result;
    }

    private List<KeyValuePair<char, Tuple<int, int>>> CalculatePossibleStates(Tuple<int, int> currentPosition, Tuple<int, int> targetPosition, bool[] posibilities)
    {
      var possibleStates = new List<KeyValuePair<char, Tuple<int, int>>>();

      for(var direction = 0; direction < 4; direction++)
      {
        if(posibilities[direction])
        {
          switch(direction)
          {
            case 0: //Go up.
            {
              if(currentPosition.Item1 > 0)
              {
                possibleStates.Add(new KeyValuePair<char, Tuple<int, int>>('U', new Tuple<int, int>(currentPosition.Item1 - 1, currentPosition.Item2)));
              }
            } break;
            case 1: //Go down.
            {
              if(currentPosition.Item1 < targetPosition.Item1)
              {
                possibleStates.Add(new KeyValuePair<char, Tuple<int, int>>('D', new Tuple<int, int>(currentPosition.Item1 + 1, currentPosition.Item2)));
              }
            } break;
            case 2: // Go left.
            {
              if(currentPosition.Item2 > 0)
              {
                possibleStates.Add(new KeyValuePair<char, Tuple<int, int>>('L', new Tuple<int, int>(currentPosition.Item1, currentPosition.Item2 - 1)));
              }
            } break;
            case 3: //Go right.
            {
              if(currentPosition.Item2 < targetPosition.Item2)
              {
                possibleStates.Add(new KeyValuePair<char, Tuple<int, int>>('R', new Tuple<int, int>(currentPosition.Item1, currentPosition.Item2 + 1)));
              }
            } break;
          }
        }
      }

      return possibleStates;
    }
  }
}