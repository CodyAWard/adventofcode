using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Ward.Days
{
    public class Day3 : BaseDay
    {
        protected override int Day() => 3;
        
        class WireData
        {
            public string Direction;
            public int Amount;

            public WireData(string direction, int amount)
            {
                Direction = direction;
                Amount = amount;
            }
        }

        class WireSegment
        {
            public Point Start;
            public Point End;

            public WireSegment(Point start, Point end)
            {
                Start = start;
                End = end;
            }
        }

        class Wire
        {
            public List<WireData> Pieces;
            public List<WireSegment> Segments = new List<WireSegment>();

            public Wire(List<WireData> pieces)
            {
                Pieces = pieces;
                
                var position = new Point(0, 0);
                foreach (var piece in pieces)
                {
                    var start = new Point(position.X, position.Y);

                    switch (piece.Direction)
                    {
                        case "U":
                            position.Y += piece.Amount;
                            break;
                        case "D":
                            position.Y -= piece.Amount;
                            break;
                        case "R":
                            position.X += piece.Amount;
                            break;
                        case "L":
                            position.X -= piece.Amount;
                            break;
                    }

                    var segment = new WireSegment(start, position);
                    Segments.Add(segment);
                }
            }
        }

        protected override string GetSolution()
        {
            GenerateInput(out Wire wire1, out Wire wire2);

            float shortest = float.MaxValue;
            int shortestSteps = int.MaxValue;

            for (int i = 0; i < wire1.Segments.Count; i++)
            {
                var seg1 = wire1.Segments[i];
                for (int j = 0; j < wire2.Segments.Count; j++)
                {
                    var seg2 = wire2.Segments[j];
                    if ((seg1.Start.X != 0 && seg1.Start.Y != 0) || (seg2.Start.X != 0 && seg2.Start.Y != 0))
                    {
                        FindIntersection(seg1.Start, seg1.End, seg2.Start, seg2.End, out bool whoCares, out bool intersected, out Point intersection, out Point c1, out Point c2);
                        if (intersected)
                        {
                            var val = Math.Abs(intersection.X) + Math.Abs(intersection.Y); if (shortest > val) shortest = val;

                            int totalSteps = 0;
                            for (int k = 0; k <= i; k++)
                            {
                                var piece = wire1.Pieces[k];
                                int amount = 0;
                                switch (piece.Direction)
                                {
                                    case "U":
                                        amount = (int)(piece.Amount - Math.Abs(seg1.Start.Y - intersection.Y));
                                        break;
                                    case "D":
                                        amount = (int)(piece.Amount - Math.Abs(intersection.Y - seg1.Start.Y));
                                        break;
                                    case "R":
                                        amount = (int)(piece.Amount - Math.Abs(seg1.Start.X - intersection.X));
                                        break;
                                    case "L":
                                        amount = (int)(piece.Amount - Math.Abs(intersection.X - seg1.Start.X));
                                        totalSteps += amount;
                                        break;
                                }
                                totalSteps += amount;
                            }
                            for (int k = 0; k <= j; k++)
                            {
                                var piece = wire2.Pieces[k];
                                int amount = 0;
                                switch (piece.Direction)
                                {
                                    case "U":
                                        amount = (int)(piece.Amount - Math.Abs(seg2.Start.Y - intersection.Y));
                                        break;
                                    case "D":
                                        amount = (int)(piece.Amount - Math.Abs(intersection.Y - seg2.Start.Y));
                                        break;
                                    case "R":
                                        amount = (int)(piece.Amount - Math.Abs(seg2.Start.X - intersection.X));
                                        break;
                                    case "L":
                                        amount = (int)(piece.Amount - Math.Abs(intersection.X - seg2.Start.X));
                                        totalSteps += amount;
                                        break;
                                }
                                totalSteps += amount;
                            }

                            if (totalSteps < shortestSteps)
                            {
                                shortestSteps = totalSteps;
                            }
                        }
                    }
                }
            }

            return $"{shortest} Is the shortest distance, and {shortestSteps} is the shortest step count";
        }

        struct Point
        {
            public float X;
            public float Y;

            public Point(float x, float y)
            {
                X = x;
                Y = y;
            }
        }
        
        private void FindIntersection(Point p1, Point p2, Point p3, Point p4,
            out bool lines_intersect, out bool segments_intersect,
            out Point intersection,
            out Point close_p1, out Point close_p2)
        {
            // Get the segments' parameters.
            float dx12 = p2.X - p1.X;
            float dy12 = p2.Y - p1.Y;
            float dx34 = p4.X - p3.X;
            float dy34 = p4.Y - p3.Y;

            // Solve for t1 and t2
            float denominator = (dy12 * dx34 - dx12 * dy34);

            float t1 =
                ((p1.X - p3.X) * dy34 + (p3.Y - p1.Y) * dx34)
                    / denominator;
            if (float.IsInfinity(t1))
            {
                // The lines are parallel (or close enough to it).
                lines_intersect = false;
                segments_intersect = false;
                intersection = new Point(float.NaN, float.NaN);
                close_p1 = new Point(float.NaN, float.NaN);
                close_p2 = new Point(float.NaN, float.NaN);
                return;
            }
            lines_intersect = true;

            float t2 =
                ((p3.X - p1.X) * dy12 + (p1.Y - p3.Y) * dx12)
                    / -denominator;

            // Find the point of intersection.
            intersection = new Point(p1.X + dx12 * t1, p1.Y + dy12 * t1);

            // The segments intersect if t1 and t2 are between 0 and 1.
            segments_intersect =
                ((t1 >= 0) && (t1 <= 1) &&
                 (t2 >= 0) && (t2 <= 1));

            // Find the closest points on the segments.
            if (t1 < 0)
            {
                t1 = 0;
            }
            else if (t1 > 1)
            {
                t1 = 1;
            }

            if (t2 < 0)
            {
                t2 = 0;
            }
            else if (t2 > 1)
            {
                t2 = 1;
            }

            close_p1 = new Point(p1.X + dx12 * t1, p1.Y + dy12 * t1);
            close_p2 = new Point(p3.X + dx34 * t2, p3.Y + dy34 * t2);
        }


        private void GenerateInput(out Wire wireOne, out Wire wireTwo)
        {
            var line = string.Empty;
            StreamReader file = new StreamReader($@"..\..\Data\Day{Day()}Input.txt");

            wireOne = GenerateWireFromString(file.ReadLine());
            wireTwo = GenerateWireFromString(file.ReadLine());

            file.Close();
        }

        private Wire GenerateWireFromString(string data)
        {
            var split = data.Split(',');
            var pieces = new List<WireData>();
            foreach (var value in split)
            {
                var direction = value.Substring(0, 1);
                var amount = value.Substring(1, value.Length-1);

                pieces.Add(new WireData(direction, int.Parse(amount)));
            }

            return new Wire(pieces);
        }
    }
}