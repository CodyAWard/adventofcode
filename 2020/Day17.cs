using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode.MMXX
{
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
            dimension.Run(6, data);

            return dimension.ActiveCells.Count().ToString();
        }

        private string PartB(IEnumerable<string> data)
        {
            var dimension = new PocketDimension4D();
            dimension.Run(6, data);

            return dimension.ActiveCells.Count().ToString();
        }


        #region Pocket Dimensions

        public class PocketDimension4D : PocketDimension<Vector4>
        {
            private Vector2 XBounds;
            private Vector2 YBounds;
            private Vector2 ZBounds;
            private Vector2 WBounds;

            public void Run(int cycles, IEnumerable<string> initialState)
            {
                ApplyInitialState(initialState.ToArray(),
                                  (int x, int y) => new Vector4(x, y, 0, 0));

                XBounds = new Vector2(-1, initialState.ToArray()[0].Length + 1);
                YBounds = new Vector2(-1, initialState.Count() + 1);
                ZBounds = new Vector2(-1, 1);
                WBounds = new Vector2(-1, 1);

                for (int i = 0; i < cycles; i++)
                    Cycle();
            }

            protected virtual void Cycle()
            {
                var neighbourCounts = GatherCounts();
                UpdateCells(neighbourCounts);

                XBounds = UpdateBounds(ActiveCells.Select(r => r.X));
                YBounds = UpdateBounds(ActiveCells.Select(r => r.Y));
                ZBounds = UpdateBounds(ActiveCells.Select(r => r.Z));
                WBounds = UpdateBounds(ActiveCells.Select(r => r.W));
            }

            private Dictionary<Vector4, int> GatherCounts()
            {
                var neighbourCounts = new Dictionary<Vector4, int>();
                ItterateBounds(neighbourCounts);

                return neighbourCounts;
            }

            protected virtual void ItterateBounds(Dictionary<Vector4, int> neighbourCounts)
            {
                for (int posX = (int)XBounds.X; posX <= XBounds.Y; posX++)
                    for (int posY = (int)YBounds.X; posY <= YBounds.Y; posY++)
                        for (int posZ = (int)ZBounds.X; posZ <= ZBounds.Y; posZ++)
                            for (int posW = (int)WBounds.X; posW <= WBounds.Y; posW++)
                            {
                                CheckPosition(posX, posY, posZ, posW, out Vector4 pos, out int n);
                                neighbourCounts.Add(pos, n);
                            }
            }

            protected void CheckPosition(int posX, int posY, int posZ, int posW, out Vector4 pos, out int n)
            {
                pos = new Vector4(posX, posY, posZ, posW);
                n = 0;
                for (int x = -1; x <= 1; x++)
                    for (int y = -1; y <= 1; y++)
                        for (int z = -1; z <= 1; z++)
                            for (int w = -1; w <= 1; w++)
                            {
                                if (x == 0 && y == 0 && z == 0 && w == 0) continue; // this is ourselves

                                var nPos = new Vector4(pos.X + x, pos.Y + y, pos.Z + z, pos.W + w);
                                if (ActiveCells.Contains(nPos)) n++;
                            }
            }
        }

        public class PocketDimension3D : PocketDimension<Vector3>
        {
            private Vector2 XBounds;
            private Vector2 YBounds;
            private Vector2 ZBounds;

            public void Run(int cycles, IEnumerable<string> initialState)
            {
                ApplyInitialState(initialState.ToArray(),
                                  (int x, int y) => new Vector3(x, y, 0));

                XBounds = new Vector2(-1, initialState.ToArray()[0].Length + 1);
                YBounds = new Vector2(-1, initialState.Count() + 1);
                ZBounds = new Vector2(-1, 1);

                for (int i = 0; i < cycles; i++)
                    Cycle();
            }

            protected virtual void Cycle()
            {
                var neighbourCounts = GatherCounts();
                UpdateCells(neighbourCounts);

                XBounds = UpdateBounds(ActiveCells.Select(r => r.X));
                YBounds = UpdateBounds(ActiveCells.Select(r => r.Y));
                ZBounds = UpdateBounds(ActiveCells.Select(r => r.Z));
            }

            private Dictionary<Vector3, int> GatherCounts()
            {
                var neighbourCounts = new Dictionary<Vector3, int>();
                ItterateBounds(neighbourCounts);

                return neighbourCounts;
            }

            protected virtual void ItterateBounds(Dictionary<Vector3, int> neighbourCounts)
            {
                for (int posX = (int)XBounds.X; posX <= XBounds.Y; posX++)
                    for (int posY = (int)YBounds.X; posY <= YBounds.Y; posY++)
                        for (int posZ = (int)ZBounds.X; posZ <= ZBounds.Y; posZ++)
                        {
                            CheckPosition(posX, posY, posZ, out Vector3 pos, out int n);
                            neighbourCounts.Add(pos, n);
                        }
            }

            protected void CheckPosition(int posX, int posY, int posZ, out Vector3 pos, out int n)
            {
                pos = new Vector3(posX, posY, posZ);
                n = 0;
                for (int x = -1; x <= 1; x++)
                    for (int y = -1; y <= 1; y++)
                        for (int z = -1; z <= 1; z++)
                        {
                            if (x == 0 && y == 0 && z == 0) continue; // this is ourselves

                            var nPos = new Vector3(pos.X + x, pos.Y + y, pos.Z + z);
                            if (ActiveCells.Contains(nPos)) n++;
                        }
            }
        }

        public abstract class PocketDimension<T>
        {
            public HashSet<T> ActiveCells { get; } = new HashSet<T>();

            protected void UpdateCells(Dictionary<T, int> neighbourCounts)
            {
                var keys = neighbourCounts.Keys.ToArray();
                var values = neighbourCounts.Values.ToArray();
                for (int i = 0; i < neighbourCounts.Count; i++)
                    UpdateBasedOnNeighbours((keys[i], values[i]));
            }

            protected static Vector2 UpdateBounds(IEnumerable<float> range)
            {
                return new Vector2(range.Min() - 1, range.Max() + 1);
            }

            protected void UpdateBasedOnNeighbours((T position, int activeNeighbours) cell)
            {
                var isActive = ActiveCells.Contains(cell.position);
                var newActive = false;

                if (isActive &&
                    (cell.activeNeighbours == 2 || cell.activeNeighbours == 3))
                {
                    newActive = true;
                }
                else if (!isActive && cell.activeNeighbours == 3)
                {
                    newActive = true;
                }

                if (isActive && !newActive) ActiveCells.Remove(cell.position);
                else if (!isActive && newActive) ActiveCells.Add(cell.position);
            }

            protected void ApplyInitialState(string[] initialState, Func<int, int, T> newPos)
            {
                for (int y = 0; y < initialState.Length; y++)
                {
                    var line = initialState[y];
                    for (int x = 0; x < line.Length; x++)
                    {
                        var isActive = line[x] == '#';
                        if (isActive) ActiveCells.Add(newPos(x, y));
                    }
                }
            }
        }

        #endregion
    }
}