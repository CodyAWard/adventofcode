using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode.MMXV
{
    public class Day6 : MMXVDay
    {
        public override string GetSolution()
        {
            var data = ReadData();
            return 
                PartA(data) + "\n" + 
                PartB(data);
        }

        enum Types
        {
            On, Off, Toggle,
        }

        class Instruction
        {
            Types Type;
            Vector2 Min;
            Vector2 Max;

            public static Instruction Parse(string str)
            {
                var type = Types.Toggle;
                if (str.StartsWith("turn on"))
                {
                    str = str.Replace("turn on ", string.Empty);
                    type = Types.On;
                }
                else if (str.StartsWith("turn off"))
                {
                    str = str.Replace("turn off ", string.Empty);
                    type = Types.Off;
                }
                else
                {
                    str = str.Replace("toggle ", string.Empty);
                }

                var rect = str.Split(" through ");
                var min = rect[0].Split(',');
                var max = rect[1].Split(',');

                return new Instruction()
                {
                    Type = type,
                    Min = new Vector2(int.Parse(min[0]), int.Parse(min[1])),
                    Max = new Vector2(int.Parse(max[0]), int.Parse(max[1]))
                };
            }

            public void Apply(bool[,] grid)
            {
                for (int x = (int)Min.X; x <= Max.X; x++)
                    for (int y = (int)Min.Y; y <= Max.Y; y++)
                    {
                        switch (Type)
                        {
                            case Types.On:
                                grid[x, y] = true;
                                break;
                            case Types.Off:
                                grid[x, y] = false;
                                break;
                            case Types.Toggle:
                                grid[x, y] = !grid[x, y];
                                break;
                        }
                    }
            }

            public void Apply(int[,] grid)
            {
                for (int x = (int)Min.X; x <= Max.X; x++)
                    for (int y = (int)Min.Y; y <= Max.Y; y++)
                    {
                        switch (Type)
                        {
                            case Types.On:
                                grid[x, y] += 1;
                                break;
                            case Types.Off:
                                grid[x, y] -= 1;
                                if (grid[x, y] < 0) grid[x, y] = 0;
                                break;
                            case Types.Toggle:
                                grid[x, y] += 2;
                                break;
                        }
                    }
            }
        }

        private static string PartA(IEnumerable<string> data)
        {
            var grid = new bool[1000, 1000];
            foreach (var line in data)
            {
                Instruction.Parse(line).Apply(grid);
            }

            var on = 0;
            for (int y = 0; y < 1000; y++)
            for (int x = 0; x < 1000; x++)
                {
                    if (grid[x, y]) on++;
                }

            return $"{on} lights on";
        }

        private static string PartB(IEnumerable<string> data)
        {
            var grid = new int[1000, 1000];
            foreach (var line in data)
            {
                Instruction.Parse(line).Apply(grid);
            }

            var bright = 0;
            for (int y = 0; y < 1000; y++)
            for (int x = 0; x < 1000; x++)
                {
                    bright += grid[x, y];
                }

            return $"{bright} lumens";
        }
    }
}