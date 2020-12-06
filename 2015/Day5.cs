using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.MMXV
{
    public class Day5 : MMXVDay
    {
        public override string GetSolution()
        {
            var data = ReadData();
            return 
                PartA(data) + "\n" + 
                PartB(data);
        }

        private static readonly string[] INVALID_SUBSTRINGS = new string[]
            {
                "ab", "cd", "pq", "xy"
            };

        private static readonly char[] VOWELS = new char[] { 'a', 'e', 'i', 'o', 'u' };

        public static bool IsActuallyNice(string str)
        {
            int tripletIndex = -1;
            string foundPair = null;

            var secondLastC = '!';
            var lastC = ' ';
            for (int i = 0; i < str.Length; i++)
            {
                var c = str[i];
                if (tripletIndex == -1)
                {
                    if (secondLastC == c) tripletIndex = i;
                }

                if (foundPair == null  && i < str.Length - 1)
                {
                    var pair = str.Substring(i, 2);
                    for (int j = i + 2; j < str.Length - 1; j++)
                    {
                        var search = str.Substring(j, 2);
                        if (search == pair) foundPair = search;
                    }
                }

                secondLastC = lastC;
                lastC = c;
            }

            if (tripletIndex == -1)
            {
                Console.WriteLine($"{str} NAUGHTY - no triplet");
                return false;
            }

            if (foundPair == null)
            {
                Console.WriteLine($"{str} NAUGHTY - no pair");
                return false;
            }

            Console.WriteLine($"{str} REALLY NICE - {foundPair} {str.Substring(tripletIndex - 2, 3)}");
            return true;
        }

        public static bool IsNice(string str)
        {
            foreach (var invalidSub in INVALID_SUBSTRINGS)
                if (str.Contains(invalidSub))
                {
                    Console.WriteLine($"{str} NAUGHTY - contains '{invalidSub}'");
                    return false;
                }

            var vowelCount = 0;
            var foundDouble = false;

            var lastC = ' ';
            foreach (var c in str)
            {
                if (c == lastC) foundDouble = true;
                if (VOWELS.Contains(c)) vowelCount++;

                lastC = c;
            }

            if (vowelCount < 3)
            {
                Console.WriteLine($"{str} NAUGHTY - vowels '{vowelCount}'");
                return false; 
            }

            if (!foundDouble)
            {
                Console.WriteLine($"{str} NAUGHTY - no double");
                return false;
            }

            Console.WriteLine($"{str} NICE");
            return true;
        }

        private static string PartA(IEnumerable<string> data)
        {
            return data.Where(s => IsNice(s)).Count()
                .ToString();
        }

        private static string PartB(IEnumerable<string> data)
        {
            return data.Where(s => IsActuallyNice(s)).Count()
                .ToString();
        }
    }
}