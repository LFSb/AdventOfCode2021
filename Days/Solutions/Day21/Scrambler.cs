using System;
using System.Linq;

namespace Solutions.Models.Day21
{
  public class Scrambler
  {
    public string Scramble(char[] input, string[] scrambling, bool reverse)
    {
      foreach(var line in scrambling)
      {
        var split = line.Split(' ');

        switch(split[0])
        {
          case "swap":
          {
            var indexA = 0;
            var indexB = 0;

            if(char.IsLetter(split[2][0]))
            {
              indexA = Array.IndexOf(input, split[2][0]);
              indexB = Array.IndexOf(input, split[5][0]);
            }
            else
            {
              indexA = int.Parse(split[2]);
              indexB = int.Parse(split[5]);
            }

            var tmp = input[indexA];
            input[indexA] = input[indexB];
            input[indexB] = tmp;

          } break;
          case "rotate":
          {
            switch(split[1])
            {
              case "left":
              {
                if(reverse)
                {
                  Shift(ref input, int.Parse(split[2]), true);
                }
                else
                {
                  Shift(ref input, int.Parse(split[2]), false);
                }                
              } break;
              case "right":
              {
                if(reverse)
                {
                  Shift(ref input, int.Parse(split[2]), false);
                }
                else
                {
                  Shift(ref input, int.Parse(split[2]), true);
                }                
              } break;
              case "based":
              {
                var amountOfShifts = Array.IndexOf(input, split[6][0]);

                if(reverse)
                {
                  switch(amountOfShifts)
                  {
                    case 0:
                    {
                      Shift(ref input, 1, false);
                    }break;
                    case 1:
                    {
                      Shift(ref input, 1, false);
                    }break;
                    case 2:
                    {
                      Shift(ref input, 2, true);
                    }break;
                    case 3:
                    {
                      Shift(ref input, 2, false);
                    }break;
                    case 4:
                    {
                      Shift(ref input, 1, true);
                    }break;
                    case 5:
                    {
                      Shift(ref input, 3, false);
                    }break;
                    case 6:
                    {
                      //Don't shift a damn thing.
                    }break;
                    case 7:
                    {
                      Shift(ref input, 4, true);
                    }break;
                  }
                }
                else
                {
                  amountOfShifts = amountOfShifts >= 4 ? amountOfShifts + 2 : amountOfShifts + 1;

                  Shift(ref input, amountOfShifts, true);
                }
              } break;
            }

          } break;
          case "reverse":
          {
            var start = int.Parse(split[2]);
            var end = int.Parse(split[4]);      

            var subset = input.Skip(start).Take(end - start + 1).Reverse().ToArray();

            var subIndex = 0;

            for(var idx = start; idx < end + 1; idx++)
            {
              input[idx] = subset[subIndex++];
            }

          } break;
          case "move":
          {
            int startPosition = 0;
            int inputPosition = 0;

            if(reverse)
            {
              startPosition = int.Parse(split[5]);
              inputPosition = int.Parse(split[2]);
            }
            else
            {
              startPosition = int.Parse(split[2]);
              inputPosition = int.Parse(split[5]);
            }           

            var fromChar = input[startPosition];

            if(startPosition < inputPosition)
            {
              for(var idx = startPosition; idx <= inputPosition; idx++)
              {
                if(idx == inputPosition)
                {
                  input[idx] = fromChar;
                }
                else
                {
                  if(idx == input.Length - 1)
                  {
                    input[idx] = input[0];
                  }
                  else
                  {
                    input[idx] = input[idx + 1];
                  }
                }
              }
            }
            else
            {
              for(var idx = startPosition; idx >= inputPosition; idx--)
              {
                if(idx == inputPosition)
                {
                  input[idx] = fromChar;
                }
                else
                {
                  if(idx == 0)
                  {
                    input[idx] = input[input.Length - 1];
                  }
                  else
                  {
                    input[idx] = input[idx - 1];
                  }
                }
              }
            }
          } break;
        }

        // System.Console.WriteLine("{0}: {1}", line, string.Join("", input));
      }

      return string.Join("", input);
    }

    public void Shift(ref char[] input, int shiftAmounts, bool right)
    {
      for(var shiftNumber = 0; shiftNumber < shiftAmounts; shiftNumber++)
      {
        if(right)
        {
          var temp = input[input.Length - 1];

          for(var character = input.Length - 1; character > 0 ; character--)
          {
            input[character] = right ? input[character - 1] : input[character + 1];
          }

          input[0] = temp;
        }
        else
        {
          var temp = input[0];

          for(var character = 0; character < input.Length - 1; character++)
          {
            input[character] = input[character + 1];
          }

          input[input.Length - 1] = temp;
        }      
      }
    }
  }
}