using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.MMXX
{
    public class Day6 : MMXXDay
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
            int totalYes = CalculateYesAnswers(data, false);

            return totalYes.ToString();
        }

        private string PartB(IEnumerable<string> data)
        {
            int totalYes = CalculateYesAnswers(data, true);

            return totalYes.ToString();
        }

        private static int CalculateYesAnswers(IEnumerable<string> data, bool unanimous)
        {
            var totalYes = 0;
            var yesTotals = new List<int>();

            var groupCount = 0;
            var charCounts = new Dictionary<char, int>();

            foreach (var line in data)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    FinishGroup();
                }
                else
                {
                    groupCount++;
                    foreach (var answer in line)
                    {
                        if (!charCounts.ContainsKey(answer)) charCounts[answer] = 0;

                        charCounts[answer]++;
                    }
                }
            }

            FinishGroup();
            return totalYes;

            void FinishGroup()
            {
                var t = 0;
               
                if (unanimous) t = charCounts.Values.Where(v => v == groupCount).Count();
                else t = charCounts.Values.Count();

                totalYes += t;
                yesTotals.Add(t);
                Console.WriteLine("Group Answered Yes To: " + t);

                groupCount = 0;
                charCounts.Clear();
            }
        }
    }
}