using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.MMXV
{
    public class Day1 : MMXVDay
    {
        public override string GetSolution()
        {
            var data = ReadData().ToArray()[0];
            return 
                PartA(data) + "\n" + 
                PartB(data);
        }

        private static string PartA(string data)
        {
            int floor = CalculateFloor(data, out _);

            return $"Floor: {floor}";
        }

        private static int CalculateFloor(string data, out int firstBasement)
        {
            var floor = 0;
            var i = 0;
            firstBasement = 0;
            
            foreach (var instruction in data)
            {
                i++;
                switch (instruction)
                {
                    case '(':
                        floor++;
                        break;
                    case ')':
                        floor--;
                        if (floor < 0 && firstBasement == 0) 
                            firstBasement = i; 
                        break;
                }
            }
            return floor;
        }

        private static string PartB(string data)
        {
            CalculateFloor(data, out int first);
            return "Basement At Char " + first;
        }
    }
}