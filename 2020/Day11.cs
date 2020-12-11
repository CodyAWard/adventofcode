using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.MMXX
{
    public class Day11 : MMXXDay
    {
        public override string GetSolution()
        {
            var data = ReadData();

            return
                PartA(data) + "\n" +
                PartB(data);
        }

        private static int ToIndex(int x, int y, int w) => x + (y * w);
        public enum TileStates
        {
            None,
            Empty,
            Occupied,
        }

        public class Grid
        {
            private TileStates[] tiles;
            private int width;
            private int height;

            public Grid(IEnumerable<string> data)
            {
                var tiles = new List<TileStates>();
                height = data.Count();

                foreach (var line in data)
                {
                    width = line.Length;
                    foreach (var tile in line)
                    {
                        switch (tile)
                        {
                            case 'L':
                                tiles.Add(TileStates.Empty);
                                break;
                            case '#':
                                tiles.Add(TileStates.Occupied);
                                break;
                            default:
                                tiles.Add(TileStates.None);
                                break;

                        }
                    }
                }

                this.tiles = tiles.ToArray();
            }

            public class Transition
            {
                public int Index;
                public TileStates State;

                public Transition(int index, TileStates state)
                {
                    Index = index;
                    State = state;
                }
            }

            public TileStates GetStateInDirection(int dx, int dy, int x, int y)
            {
                x += dx;
                y += dy;

                if (x >= width || x < 0 || y >= height || y < 0) return TileStates.None;

                var i = ToIndex(x, y, width);

                var s = tiles[i];
                if (s == TileStates.None)
                    s = GetStateInDirection(dx, dy, x, y);
               
                return s;
            }

            public IEnumerable<Transition> GatherLineOfSightTransitions()
            {
                var transitions = new List<Transition>();
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        var i = ToIndex(x, y, width);
                        var state = tiles[i];
                        if (state == TileStates.None) continue;

                        var occupiedNeighbours = 0;
                        for (int nx = -1; nx <= 1; nx++)
                        {
                            for (int ny = -1; ny <= 1; ny++)
                            {
                                if (nx == 0 && ny == 0) continue;
                                var stateInDir = GetStateInDirection(nx, ny, x, y);
                                if (stateInDir == TileStates.Occupied) occupiedNeighbours++;
                            }
                        }

                        if (state == TileStates.Empty && occupiedNeighbours == 0)
                        {
                            transitions.Add(new Transition(i, TileStates.Occupied));
                        }
                        else if (state == TileStates.Occupied && occupiedNeighbours >= 5)
                        {
                            transitions.Add(new Transition(i, TileStates.Empty));
                        }
                    }
                }

                return transitions;
            }

            public IEnumerable<Transition> GatherNeighbourTransitions()
            {
                var transitions = new List<Transition>();
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        var i = ToIndex(x, y, width);
                        var state = tiles[i];
                        if (state == TileStates.None) continue;

                        var occupiedNeighbours = 0;
                        for (int nx = -1; nx <= 1; nx++)
                        {
                            for (int ny = -1; ny <= 1; ny++)
                            {
                                if (nx == 0 && ny == 0) continue;
                                if (nx + x < 0 || nx + x >= width) continue;
                                if (ny + y < 0 || ny + y >= height) continue;

                                var neighbour = tiles[ToIndex(x + nx, y + ny, width)];
                                if (neighbour == TileStates.Occupied) occupiedNeighbours++;
                            }
                        }

                        if (state == TileStates.Empty && occupiedNeighbours == 0)
                        {
                            transitions.Add(new Transition(i, TileStates.Occupied));
                        }
                        else if (state == TileStates.Occupied && occupiedNeighbours >= 4)
                        {
                            transitions.Add(new Transition(i, TileStates.Empty));
                        }
                    }
                }

                return transitions;
            }

            public void ApplyTransitions(IEnumerable<Transition> transitions)
            {
                foreach (var transition in transitions)
                {
                    tiles[transition.Index] = transition.State;
                }
            }

            public void Draw()
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        var s = tiles[ToIndex(x, y, width)];
                        var c = ".";
                        if (s == TileStates.Occupied) c = "#";
                        if (s == TileStates.Empty) c = "L";

                        Console.Write(c);
                    }
                    Console.WriteLine(" ");
                }

                Console.WriteLine();
            }

            public int CountTiles(TileStates tileState)
            {
                var count = 0;
                foreach (var tile in tiles)
                {
                    if (tile == tileState) count++;
                }

                return count;
            }
        }

        private string PartA(IEnumerable<string> data)
        {
            var grid = new Grid(data);

            while (true)
            {
                var transitions = grid.GatherNeighbourTransitions();
                if (transitions.Any()) grid.ApplyTransitions(transitions);
                else break;
            }

            grid.Draw();

            return grid.CountTiles(TileStates.Occupied).ToString();
        }

        private string PartB(IEnumerable<string> data)
        {
            var grid = new Grid(data);

            while (true)
            {
                var transitions = grid.GatherLineOfSightTransitions();
                if (transitions.Any()) grid.ApplyTransitions(transitions);
                else break;
            }

            grid.Draw();

            return grid.CountTiles(TileStates.Occupied).ToString();
        }
    }
}