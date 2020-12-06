using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.MMXV
{
    public class Day2 : MMXVDay
    {
        public override string GetSolution()
        {
            var data = ReadData();
            return 
                PartA(data) + "\n" + 
                PartB(data);
        }

        private static int GetRibbonForPressent(string presentDimensions)
        {
            GetDimensions(presentDimensions, out int w, out int h, out int l);
            var perims = new int[]
                {
                    w + w + h + h,
                    w + w + l + l,
                    h + h + l + l,
                };

            var minPerim = Math.Min(Math.Min(perims[0], perims[1]), perims[2]);
            var bow = w * h * l;

            return minPerim + bow;
        }

        private static int GetPresentWrappingSqFt(string presentDimensions)
        {
            GetDimensions(presentDimensions, out int w, out int h, out int l);
            var areas = new int[]
                {
                    w * h,
                    w * l,
                    h * l
                };
            var minArea = Math.Min(Math.Min(areas[0], areas[1]), areas[2]);

            return
                (2 * areas[0]) +
                (2 * areas[1]) +
                (2 * areas[2]) +
                minArea;
        }

        private static void GetDimensions(string presentDimensions, out int w, out int h, out int l)
        {
            var dimensions = presentDimensions.Split('x');
            w = int.Parse(dimensions[0]);
            h = int.Parse(dimensions[1]);
            l = int.Parse(dimensions[2]);
        }

        private static string PartA(IEnumerable<string> data)
        {
            var total = 0;
            foreach (var line in data)
            {
                total += GetPresentWrappingSqFt(line);
            }

            return total + " ft";
        }

        private static string PartB(IEnumerable<string> data)
        {
            var total = 0;
            foreach (var line in data)
            {
                total += GetRibbonForPressent(line);
            }

            return total + " ft";
        }
    }
}