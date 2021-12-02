using System;
using System.Linq;
using System.Collections.Generic;

namespace Solutions.Models.Day11
{
  public static class ParseLayout
  {
    //This will result in a list of tuples, each containing the floor of the generator and the microchip of each type.
    public static List<Tuple<int, int>> Go(string[] input)
    {
      var outputList = new List<Tuple<int, int>>();

      //First, map all of the "types" per floor in a dictionary.
      var internalDict = new Dictionary<int, List<string>>();

      for(var floorNumber = 0; floorNumber < input.Length; floorNumber++)
      {
        var elementList = input[floorNumber]
                      .Split(new []{" a "}, StringSplitOptions.RemoveEmptyEntries)
                      .Skip(1)
                      .Select(s => s.Split(new []{ " ", "-" }, StringSplitOptions.RemoveEmptyEntries)[0]
                    ).ToList();

        internalDict.Add(floorNumber, elementList);
      }

      //Next, figure out what distinct types there are.
      var distinctTypes = internalDict.SelectMany(x => x.Value).Distinct().ToArray();

      foreach(var distinctType in distinctTypes)
      {
        //Add the floors on which this type is present to a Tuple.
        var floorKeys = internalDict.Where(x => x.Value.Contains(distinctType)).Select(x => x.Key).ToArray();

        if(floorKeys.Length == 1)
        {
          outputList.Add(new Tuple<int, int>(floorKeys[0], floorKeys[0]));
        }
        else
        {
          outputList.Add(new Tuple<int, int>(floorKeys[0], floorKeys[1]));
        }        
      }

      return outputList;
    }
  }
}