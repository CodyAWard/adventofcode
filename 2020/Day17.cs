using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode.MMXX
{
    /// 
    /// TODO - Clean it up and speed it up
    /// 


    public struct Cell
    {
        // instead of maintaining two grids, just swap between two bools
        public bool Active_buffer1;
        public bool Active_buffer2;
        public int Index;
    }

    public class PocketDimension4D : PocketDimension<Vector4>
    {
        private Cell[] grid;
        public override Cell[] Grid => grid;

        public void Run(int cycles, string[] initialState)
        {
            Initialize(initialState);

            for (int i = 0; i < cycles; i++)
            {
                var usingFirstBuffer = i % 2 == 0;

                for (int x = 1; x < GRID_SIZE - 1; x++)
                    for (int y = 1; y < GRID_SIZE - 1; y++)
                        for (int z = 1; z < GRID_SIZE - 1; z++)
                            for (int w = 1; w < GRID_SIZE - 1; w++)
                            {
                                CheckNeighbours(usingFirstBuffer, x, y, z, w);
                            }
            }
        }

        private void CheckNeighbours(bool usingFirstBuffer, int x, int y, int z, int w)
        {
            var pos = new Vector4(x, y, z, w);
            var index = ToIndex(pos);

            var activeN = 0;
            for (int xx = -1; xx <= 1; xx++)
                for (int yy = -1; yy <= 1; yy++)
                    for (int zz = -1; zz <= 1; zz++)
                        for (int ww = -1; ww <= 1; ww++)
                        {
                        var nPos = new Vector4(x + xx, y + yy, z + zz, w + ww);
                        if (nPos == pos) continue;

                        var n = grid[ToIndex(nPos)];
                        if (usingFirstBuffer) { if (n.Active_buffer1) activeN++; }
                        else { if (n.Active_buffer2) activeN++; }
                    }

            UpdateBasedOnNeighbours(usingFirstBuffer, index, activeN);
        }

        private void Initialize(string[] initialState)
        {
            grid = new Cell[GRID_SIZE * GRID_SIZE * GRID_SIZE * GRID_SIZE];
            ApplyInitialState(initialState);
        }

        public override int ToIndex(Vector4 position)
        {
            return (int)(
                position.W * GRID_SIZE * GRID_SIZE * GRID_SIZE
              + position.Z * GRID_SIZE * GRID_SIZE 
              + position.Y * GRID_SIZE 
              + position.X);
        }

        public override int ToIndex(int x, int y)
        {
            var position = new Vector4(x, y, GRID_SIZE / 2, GRID_SIZE / 2);
            return ToIndex(position);
        }
    }

    public class PocketDimension3D : PocketDimension<Vector3>
    {
        private Cell[] grid;
        public override Cell[] Grid => grid;

        public void Run(int cycles, string[] initialState)
        {
            Initialize(initialState);

            for (int i = 0; i < cycles; i++)
            {
                var usingFirstBuffer = i % 2 == 0;

                for (int x = 1; x < GRID_SIZE - 1; x++)
                    for (int y = 1; y < GRID_SIZE - 1; y++)
                        for (int z = 1; z < GRID_SIZE - 1; z++)
                        {
                            CheckNeighbours(usingFirstBuffer, x, y, z);
                        }
            }
        }

        private void CheckNeighbours(bool usingFirstBuffer, int x, int y, int z)
        {
            var pos = new Vector3(x, y, z);
            var index = ToIndex(pos);

            var activeN = 0;
            for (int xx = -1; xx <= 1; xx++)
                for (int yy = -1; yy <= 1; yy++)
                    for (int zz = -1; zz <= 1; zz++)
                    {
                        var nPos = new Vector3(x + xx, y + yy, z + zz);
                        if (nPos == pos) continue;

                        var n = grid[ToIndex(nPos)];
                        if (usingFirstBuffer) { if (n.Active_buffer1) activeN++; }
                        else { if (n.Active_buffer2) activeN++; }
                    }

            UpdateBasedOnNeighbours(usingFirstBuffer, index, activeN);
        }

        private void Initialize(string[] initialState)
        {
            grid = new Cell[GRID_SIZE * GRID_SIZE * GRID_SIZE];
            ApplyInitialState(initialState);
        }

        public override int ToIndex(Vector3 position)
        {
            return (int)(position.Z * GRID_SIZE * GRID_SIZE + position.Y * GRID_SIZE + position.X);
        }

        public override int ToIndex(int x, int y)
        {
            var position = new Vector3(x, y, GRID_SIZE / 2);
            return ToIndex(position);
        }
    }

    public abstract class PocketDimension<T>
    {
        // supposed to be infinite, but we can just initialize a big ol array 
        public const int GRID_SIZE = 50;
        public abstract Cell[] Grid { get; }

        public abstract int ToIndex(T position);
        public abstract int ToIndex(int x, int y);

        public void UpdateBasedOnNeighbours(bool usingFirstBuffer, int index, int activeN)
        {
            var me = Grid[index];
            if (usingFirstBuffer)
            {
                if (me.Active_buffer1 &&
                    (activeN == 2 || activeN == 3))
                {
                    me.Active_buffer2 = true;
                }
                else if (!me.Active_buffer1 &&
                    (activeN == 3))
                {
                    me.Active_buffer2 = true;
                }
                else
                {
                    me.Active_buffer2 = false;
                }
            }
            else
            {
                if (me.Active_buffer2 &&
                     (activeN == 2 || activeN == 3))
                {
                    me.Active_buffer1 = true;
                }
                else if (!me.Active_buffer2 &&
                    (activeN == 3))
                {
                    me.Active_buffer1 = true;
                }
                else
                {
                    me.Active_buffer1 = false;
                }
            }
            Grid[index] = me;
        }

        public void ApplyInitialState(string[] initialState)
        {
            for (int y = 0; y < initialState.Length; y++)
            {
                var line = initialState[y];
                for (int x = 0; x < line.Length; x++)
                {
                    var isActive = line[x] == '#';

                    // offset to grid center
                    var half = GRID_SIZE / 2;
                    var index = ToIndex(half + x, half + y);
                    Grid[index] = new Cell()
                    {
                        Active_buffer1 = isActive,
                        Index = index
                    };
                }
            }
        }
    }

    public class Day17 : MMXXDay
    {
        public override string GetSolution()
        {
            var data = ReadData();

            return PartA(data) + "\n" +
                   PartB(data);
        }

        private string PartA(IEnumerable<string> data)
        {
            var dimension = new PocketDimension3D();
            dimension.Run(6, data.ToArray());

            return dimension.Grid
                       .Where(c => c.Active_buffer1)
                       .Count()
                       .ToString();
        }

        private string PartB(IEnumerable<string> data)
        {
            var dimension = new PocketDimension4D();
            dimension.Run(6, data.ToArray());

            return dimension.Grid
                       .Where(c => c.Active_buffer1)
                       .Count()
                       .ToString();
        }
    }
}