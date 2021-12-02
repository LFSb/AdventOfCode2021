using System;
using System.Collections.Generic;
using System.Linq;

namespace Solutions.Models.Day8
{
    public class Screen
    {
        private const int Width = 50;

        private const int Height = 6;
        
        private Dictionary<int, Dictionary<int, bool>> Grid = new Dictionary<int, Dictionary<int, bool>>();

        public Screen()
        {
            for(var h = 0; h < Height; h++)
            {
                for(var w = 0; w < Width; w++)
                {
                    if(!Grid.ContainsKey(h))
                    {
                        Grid.Add(h, new Dictionary<int, bool>{ { w, false } });
                    }
                    else
                    {
                        Grid[h].Add(w, false);
                    }
                    
                }                
            }
        }

        public int ReturnPixelsOn()
        {
            var count = 0;
            foreach(var g in Grid)
            {
                foreach(var l in g.Value)
                {
                    if(l.Value)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public void ParseInput(string input)
        {
            var splitInput = input.Split(' ');

            switch(splitInput[0])
            {
                case "rect":
                {
                    ActivateRect(string.Join("", splitInput.Skip(1)));
                } break;
                case "rotate":
                {
                    if(splitInput[1] == "column")
                    {
                        RotateColumn(string.Join("", splitInput.Skip(2)));
                    }
                    else if(splitInput[1] == "row")
                    {
                        RotateRow(string.Join("", splitInput.Skip(2)));
                    }

                } break;
                default :
                {
                    throw new InvalidOperationException();
                }
            }
            
            //DrawScreen();
        }

        public void ActivateRect(string input)
        {
            var splitInput = input.Split('x');
            
            var width = int.Parse(splitInput[0].ToString());

            var height = int.Parse(splitInput[1].ToString());

            for(var h = 0; h < height; h++)
            {
                for(var w = 0; w < width; w++)
                {
                    Grid[h][w] = true;
                }
            }            
        }

        public void ParseMovement(string input, out int first, out int second) 
        {
            var splits = input.Split(new []{ "by" }, StringSplitOptions.None).Select(x => x.Trim()).ToArray();

            first = int.Parse(splits[0].Substring(splits[0].IndexOf('=') + 1));
            second = int.Parse(splits[1]);
        }

        public void RotateRow(string input)
        {
            int rowNumber;
            int shifts;

            ParseMovement(input, out rowNumber, out shifts);

            for(var shift = 0; shift < shifts; shift++)
            {
                bool temp = Grid[rowNumber][Width - 1];

                for(var movement = Width - 1; movement > 0; movement--)
                {
                    Grid[rowNumber][movement] = Grid[rowNumber][movement - 1];
                }
                
                Grid[rowNumber][0] = temp;
            }                  
        }

        public void RotateColumn(string input)
        {
            int columnNumber;
            int shifts;

            ParseMovement(input, out columnNumber, out shifts);
            
            for(var shift = 0; shift < shifts; shift++)
            {
                bool temp = Grid[Height - 1][columnNumber];

                for(var movement = Height - 1; movement > 0; movement--)
                {
                    Grid[movement][columnNumber] = Grid[movement - 1][columnNumber];
                }
                
                Grid[0][columnNumber] = temp;
            }
        }

        public void DrawScreen()
        {
            var consolePos = Console.CursorTop;

            for(var height = 0; height < Height; height++)
            {
                for(var width = 0; width < Width; width++)
                {
                    Console.SetCursorPosition(width, consolePos + height);
                    Console.Write(Grid[height][width] ? "#" : ".");
                }
            }

            System.Console.WriteLine();
            System.Console.WriteLine();
        }
    }   
}