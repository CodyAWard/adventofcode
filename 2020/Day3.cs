using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.MMXX
{
    public class Day3 : MMXXDay
    {
        public override string GetSolution()
        {
            var data = ReadData();
            return 
                PartA(data) + "\n" + 
                PartB(data);
        }

        private static void CalculateRouteForSlope(IEnumerable<string> data,
            int incrementX, 
            int incrementY, 
            out int open, 
            out int trees)
        {
            var posX = 0;
            var posY = 0;
            
            open = 0;
            trees = 0;
            var lines = data.ToArray();
            for (posY = incrementY; posY < lines.Length; posY += incrementY)
            {
                posX += incrementX;
                var line = lines[posY];

                // wrap the x value, as the pattern repeats
                posX = posX % line.Length;
                var value = line[posX];
                if (value == '#') trees++;
                else open++;


                var builder = new StringBuilder(line);
                builder[posX] = value == '#' ? 'X' : 'O';
                Console.WriteLine(builder.ToString());
            }

            Console.WriteLine();
            Console.WriteLine("Finished Slope " + trees);
            Console.WriteLine();
        }

        private string PartA(IEnumerable<string> data)
        {
            int open, trees;
            CalculateRouteForSlope(data, 3, 1, out open, out trees);

            return $"Open: {open} Trees:{trees}";
        }

        private string PartB(IEnumerable<string> data)
        {
            // Right 1, down 1.
            CalculateRouteForSlope(data, 1, 1, out int oa, out int ta);
            // Right 3, down 1. (This is the slope you already checked.)
            CalculateRouteForSlope(data, 3, 1, out int ob, out int tb);
            // Right 5, down 1.
            CalculateRouteForSlope(data, 5, 1, out int oc, out int tc);
            // Right 7, down 1.
            CalculateRouteForSlope(data, 7, 1, out int od, out int td);
            // Right 1, down 2.
            CalculateRouteForSlope(data, 1, 2, out int oe, out int te);

            long answer = (long)ta*tb*tc*td*te;
            return answer.ToString();
        }
    }
}