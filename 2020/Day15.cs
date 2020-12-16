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

        struct Info
        {
            public int Last;
            public int Age;

            public void Set(int turn)
            {
                Age = turn - Last;
                Last = turn;
            }
        }

        private static string RunUntil(IEnumerable<string> data, int target)
        {
            var input = data.First().AsInts().ToArray();
            var spokenNumbers = new Info[target];
            var lastSpoken = -1;
            int turnCounter = 0;

            for (int i = 0; i < input.Length; i++)
            {
                turnCounter++;
                lastSpoken = input[i];
                spokenNumbers[lastSpoken].Set(turnCounter);
            }

            while (turnCounter < target)
            {
                turnCounter++;

                var info = spokenNumbers[lastSpoken];
                if (info.Age == info.Last) // was first
                {
                    lastSpoken = 0;
                    spokenNumbers[0].Set(turnCounter);
                }
                else
                {
                    lastSpoken = info.Age;
                    spokenNumbers[info.Age].Set(turnCounter);
                }
            }

            return lastSpoken.ToString();
        }
    }
}