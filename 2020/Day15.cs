using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode.MMXX
{
    public static class StringUtil
    {
        public static IEnumerable<int> AsInts(this string csv)
        {
            return csv.Split(',').Select(s => int.Parse(s));
        }
    }

    public class Day15 : MMXXDay
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
            return RunUntil(data, 2020);
        }

        private string PartB(IEnumerable<string> data)
        {
            return RunUntil(data, 30000000);
        }

        private static string RunUntil(IEnumerable<string> data, int target)
        {
            var input = data.First().AsInts().ToArray();
            var spokenNumbers = new Dictionary<int, List<int>>();
            var lastSpoken = -1;
            int turnCounter = 0;
            void speak(int number)
            {
                lastSpoken = number;

                if (spokenNumbers.TryGetValue(number, out var info))
                {
                    info.Add(turnCounter);
                }
                else
                {
                    spokenNumbers[number] = new List<int>() { turnCounter };
                }
            }


            for (int i = 0; i < input.Length; i++)
            {
                turnCounter++;
                speak(input[i]);
            }

            void turn()
            {
                turnCounter++;

                var spokenTurns = spokenNumbers[lastSpoken];
                var count = spokenTurns.Count;

                if (count == 1) // was first
                    speak(0);
                else
                    speak(spokenTurns[count - 1] - spokenTurns[count - 2]);
            }

            while (true)
            {
                if (turnCounter == target)
                    return lastSpoken.ToString();

                turn();
            }
        }
    }
}