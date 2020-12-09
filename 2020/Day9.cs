using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.MMXX
{
    public class Day9 : MMXXDay
    {
        public override string GetSolution()
        {
            var data = ReadData();
            return
                PartA(data) + "\n" +
                PartB(data);
        }

        public class XMASHAX
        {
            private readonly Queue<long> values = new Queue<long>();

            public XMASHAX(long[] preamble)
            {
                foreach (var number in preamble)
                {
                    values.Enqueue(number);
                }
            }

            public bool Next(long number)
            {
                foreach (var a in values)
                    foreach (var b in values)
                    {
                        if (a == b) continue;
                        if (a + b == number)
                        {
                            values.Dequeue();
                            values.Enqueue(number);
                            return true;
                        }
                    }

                return false;
            }
        }

        private static long FindInvalidNumber(long[] cypher)
        {
            var index = 25;
            var preamble = cypher.Take(index).ToArray();
            var hax = new XMASHAX(preamble);

            for (; index < cypher.Length; index++)
            {
                var number = cypher[index];
                if (!hax.Next(number)) return number;
            }

            return -1;
        }

        private static (int, int) FindContiguousSum(long[] cypher, long target)
        {
            for (int a = 0; a < cypher.Length; a++)
            {
                var tally = cypher[a];
                for (int b = a + 1; b < cypher.Length; b++)
                {
                    tally += cypher[b];

                    if (tally == target)
                        return (a, b);
                }
            }

            return (-1, -1);
        }

        private string PartA(IEnumerable<string> data)
        {
            var cypher = data.AsLongs().ToArray();
            var invalid = FindInvalidNumber(cypher);
            return invalid.ToString();
        }

        private string PartB(IEnumerable<string> data)
        {
            var cypher = data.AsLongs().ToArray();
            var invalid = FindInvalidNumber(cypher);
            var bounds = FindContiguousSum(cypher, invalid);

            var min = long.MaxValue;
            var max = long.MinValue;

            for (int i = bounds.Item1; i <= bounds.Item2; i++)
            {
                var val = cypher[i];
                if (val > max) max = val;
                if (val < min) min = val;
            }

            return "Sum - " + (min + max);
        }
    }
}