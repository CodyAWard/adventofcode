using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.MMXX
{
    public class Day5 : MMXXDay
    {
        public override string GetSolution()
        {
            var data = ReadData();
            return 
                PartA(data) + "\n" + 
                PartB(data);
        }

        public static int GetSeatID(string boardingPass)
        {
            var minX = 0;
            var maxX = 7;

            var minY = 0;
            var maxY = 127;

            foreach (var val in boardingPass)
            {
                var width = maxY - minY;
                var height = maxX - minX;

                switch (val)
                {
                    case 'F':
                        maxY = (int)(maxY - (width / 2f));
                        break;
                    case 'B':
                        minY = (int)(minY + (width / 2f));
                        break;
                    case 'L':
                        maxX = (int)(maxX - (height / 2f));
                        break;
                    case 'R':
                        minX = (int)(minX + (height / 2f));
                        break;
                }
            }

            return maxY * 8 + maxX;
        }

        private string PartA(IEnumerable<string> data)
        {
            var max = 0;

            foreach (var line in data)
            {
                var id = GetSeatID(line);
                if (id > max) max = id;
            }

            return max.ToString();
        }

        private string PartB(IEnumerable<string> data)
        {
            var allIds = data.Select(s => GetSeatID(s))
                .OrderBy(i => i)
                .ToArray();

            var last = allIds[0];
            for (int i = 1; i < allIds.Length; i++)
            {
                Console.WriteLine("Checking " + allIds[i] + " last: " + last);
                if (allIds[i] - last > 1)
                {
                    return "Your Ticket -> " + (last + 1);
                }

                last = allIds[i];
            }

            return "You have no ticket, you filthy liar";
        }
    }
}