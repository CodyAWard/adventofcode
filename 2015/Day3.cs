using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode.MMXV
{
    public class Day3 : MMXVDay
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
            List<Vector2> visited = GetVisitedHouses(data, 1);

            return visited.Count().ToString();
        }

        private static string PartB(string data)
        {
            List<Vector2> visited = GetVisitedHouses(data, 2);

            return visited.Count().ToString();
        }

        private static List<Vector2> GetVisitedHouses(string data, int routes)
        {
            var visited = new List<Vector2>();
            var pos = new Vector2[routes];
            int turn = 0;

            foreach (var instruction in data)
            {
                switch (instruction)
                {
                    case '>':
                        pos[turn].X++;
                        break;
                    case '<':
                        pos[turn].X--;
                        break;
                    case 'v':
                        pos[turn].Y--;
                        break;
                    case '^':
                        pos[turn].Y++;
                        break;
                }

                if (!visited.Contains(pos[turn])) visited.Add(pos[turn]);

                turn++;
                if (turn >= pos.Length) turn = 0;
            }

            return visited;
        }
    }
}