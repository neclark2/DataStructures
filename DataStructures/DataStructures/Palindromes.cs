using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DataStructures
{
    public class Palindromes
    {

        

        public static IEnumerable<string> FindAllPalindromes(string source)
        {
            var palindromes = new Dictionary<int, List<Tuple<int, int>>>();

            for (var i = 0; i < source.Length - 1; i++)
            {
                if (!palindromes.ContainsKey(1))
                {
                    palindromes.Add(1, new List<Tuple<int, int>> {new Tuple<int, int>(i, i)});
                }
                else
                {
                    palindromes[1].Add(new Tuple<int, int>(i, i));
                }
            }

            for (var i = 0; i < source.Length - 2; i++)
            {
                if (i == i + 1)
                {
                    if (!palindromes.ContainsKey(2))
                    {
                        palindromes.Add(2, new List<Tuple<int, int>> { new Tuple<int, int>(i, i+1) });
                    }
                    else
                    {
                        palindromes[2].Add(new Tuple<int, int>(i, i+1));
                    }
                }
            }

            var palSize = 1;

            while (palindromes.ContainsKey(palSize) || palindromes.ContainsKey(palSize + 1))
            {
                if(palindromes.ContainsKey(palSize))
                {
                    var pals = palindromes[palSize];
                    var newPalSize = palSize + 2;
                    foreach (var pal in pals)
                    {
                        if (pal.Item1 - 1 > 0 && pal.Item2 + 1 < source.Length && source[pal.Item1-1] == source[pal.Item2 + 1])
                        {
                            if (!palindromes.ContainsKey(newPalSize))
                            {
                                palindromes.Add(newPalSize, new List<Tuple<int, int>> { new Tuple<int, int>(pal.Item1 - 1, pal.Item2 + 1) });
                            }
                            else
                            {
                                palindromes[newPalSize].Add(new Tuple<int, int>(pal.Item1 - 1, pal.Item2 + 1));
                            }
                        }
                    }
                }
                palSize++;
            }

            var list = new List<string>();

            foreach (var kvp in palindromes)
            {
                foreach (var pal in kvp.Value)
                {
                    list.Add(source.Substring(pal.Item1, kvp.Key));
                }
            }
            return list;

        } 
    }

    [TestFixture]
    public class PalindromeTests
    {

        [Test]
        public void GetPals()
        {
            var x = "this is some sample text racecar";
            var pals = Palindromes.FindAllPalindromes(x);
        }
    }
}
