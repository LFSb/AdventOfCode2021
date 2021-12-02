using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Solutions.Models.Day22
{
  public class DfResult
  {
    public string ID { get; set;}

    public int X { get; set; }

    public int Y { get; set; }

    public int Size { get; set; }

    public int Used { get; set; }

    public int Avail { get; set; }

    public int UsePercentage { get; set; }

    public DfResult(string line)
    {
      var match = Regex.Match(line, @"x(\d*)-y(\d*)\s*(\d*)T\s*(\d*)T\s*(\d*)T\s*(\d*)%", RegexOptions.Compiled);

      X = int.Parse(match.Groups[1].Value);

      Y = int.Parse(match.Groups[2].Value);

      ID = string.Format("{0}{1}", Y, X);

      Size = int.Parse(match.Groups[3].Value);

      Used = int.Parse(match.Groups[4].Value);

      Avail = int.Parse(match.Groups[5].Value);

      UsePercentage = int.Parse(match.Groups[6].Value);
    }

    public int CalculateNumberOfViableNodes(List<DfResult> nodes)
    {
      if(this.Used == 0)
      {
        return 0;
      }

      var viableNodes = nodes.Count(x => x.ID != this.ID && x.Avail >= this.Used);

      return viableNodes;
    }
  }
}