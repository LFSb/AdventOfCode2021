using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System;

namespace Solutions.Models.Day14
{
  public class OneTimePad
  {
    public MD5 MD5 { get; set; }

    public Dictionary<int, string> Hashes { get; set; }

    public OneTimePad()
    {
      MD5 = MD5.Create();
    }
    public int Process(string input, bool part2)
    {
      var idx = 0; var keys = new List<int>();
      var hashes = new Dictionary<int, string>();

      while(keys.Count < 64)
      {
        string hex = string.Empty;
        var hashInput = input + idx;

        if(hashes.ContainsKey(idx))
        {
          hex = hashes[idx];
        }
        else
        {
          for(var year = 0; part2 ? year < 2017 : year < 1; year++)
          {
            hex = BitConverter.ToString(MD5.ComputeHash(Encoding.ASCII.GetBytes(hashInput))).Replace("-",string.Empty).ToLower();

            if(part2)
            {
              hashInput = hex;
            }
          }

          hashes.Add(idx, hex);
        }
        
        char character;

        if(Repeating.ContainsRepeatingChar(hex, 3, out character))
        {
          for(var idx2 = 1; idx2 <= 1000; idx2++)
          {
            hashInput = string.Format("{0}{1}", input, (idx + idx2));

            if(hashes.ContainsKey(idx + idx2))
            {
              hex = hashes[idx + idx2];
            }
            else
            {
              for(var year = 0; part2 ? year < 2017 : year < 1; year++)
              {
                hex = BitConverter.ToString(
                        MD5.ComputeHash(Encoding.ASCII.GetBytes(hashInput))
                      ).Replace("-","").ToLower();
              
                if(part2)
                {
                  hashInput = hex;
                }
              }

              hashes.Add(idx + idx2, hex);
            }

            if(hex.Contains(new string(character, 5)))
            {
              // System.Console.WriteLine($"{idx}: {character} {hex} at {idx + idx2}");
              if(!keys.Contains(idx))
              {
                keys.Add(idx);
              }               
            }            
          }
        }

        idx++;
      }

      return idx - 1;
    }
  }
}