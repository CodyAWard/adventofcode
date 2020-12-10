using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.MMXX
{
    public class Day10 : MMXXDay
    {
        public override string GetSolution()
        {
            var data = ReadData();

            return
                PartA(data) + "\n" +
                PartB(data);
        }

        private string PartA(IEnumerable<string> data)
        {
            var jolts = data.AsInts().OrderBy(j => j).ToArray(); // sort

            var diffs = new int[] { 0, 1, 0, 1 }; // starting difs
            for (int i = 0; i < jolts.Length - 1; i++) // foreach jolt
                diffs[jolts[i + 1] - jolts[i]]++;   // calculate dif, use as index to itterate diffs

            return (diffs[1] * diffs[3]) + ""; // diffs of 1 * diffs of 3
        }

        private string PartB(IEnumerable<string> data)
        {
            var js = data.AsInts()
               .OrderBy(j => j)
               .ToList();
            js.Insert(0, 0);
            js.Add(js[js.Count - 1] + 3);
            var jolts = js.ToArray();

            var length = jolts.Length;
            var permutations = new long[length];
            permutations[0] = 1; // base permutation
            void check(int a, int b)
            {
                // if we are within the dif range, we can skip
                if (b >= 0 && jolts[a] - jolts[b] <= 3)
                {
                    // if we skipped, sum up the permutations
                    permutations[a] += permutations[b];
                }
            }

            for (int i = 1; i < length; i++)
            {
                check(i, i - 1);
                check(i, i - 2);
                check(i, i - 3);
            }

            return permutations[length - 1].ToString();
        }
    }
}