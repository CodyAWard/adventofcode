using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode.MMXV
{
    public class Day9 : MMXVDay
    {
        public override string GetSolution()
        {
            var data = ReadData();
            return
                PartA(data) + "\n" +
                PartB(data);
        }

        public class Measurement
        {
            public string A;
            public string B;
            public int Distance;

            public Measurement(string data)
            {
                var locationsAndDistance = data.Split(" = ");
                var locations = locationsAndDistance[0].Split(" to ");
                var distance = int.Parse(locationsAndDistance[1]);

                A = locations[0];
                B = locations[1];
                Distance = distance;
            }
        }

        private string PartA(IEnumerable<string> data)
        {
            ParseData(data, out var measurements, out var locations);
            var distances = CalculateDistancesForAllRoutes(measurements, locations);

            return distances.Min().ToString();
        }

        private string PartB(IEnumerable<string> data)
        {
            ParseData(data, out var measurements, out var locations);
            var distances = CalculateDistancesForAllRoutes(measurements, locations);

            return distances.Max().ToString();
        }

        private static IEnumerable<int> CalculateDistancesForAllRoutes(List<Measurement> measurements, List<string> locations)
        {
            var routes = PermutationUtil.Permute(locations);
            var distances = new List<int>();
            foreach (var route in routes)
            {
                var distance = 0;
                var routArr = route.ToArray();
                for (int i = 0; i < routArr.Length - 1; i++)
                {
                    var a = routArr[i];
                    var b = routArr[i + 1];
                    var measure = measurements.Where(m => (m.A == a && m.B == b) || (m.A == b && m.B == a)).First();
                    distance += measure.Distance;
                }

                distances.Add(distance);
            }

            return distances;
        }

        private static void ParseData(IEnumerable<string> data, out List<Measurement> measurements, out List<string> locations)
        {
            measurements = new List<Measurement>();
            locations = new List<string>();
            foreach (var line in data)
            {
                var measure = new Measurement(line);
                measurements.Add(measure);

                if (!locations.Contains(measure.A)) locations.Add(measure.A);
                if (!locations.Contains(measure.B)) locations.Add(measure.B);
            }
        }
    }

    public static class PermutationUtil
    {
        public static IEnumerable<IEnumerable<T>> Permute<T>(IEnumerable<T> vals)
        {
            var list = new List<IList<T>>();
            return DoPermute(vals.ToArray(), 0, vals.Count() - 1, list);
        }

        static IEnumerable<IEnumerable<T>> DoPermute<T>(T[] vals, int start, int end, IList<IList<T>> list)
        {
            if (start == end)
            {
                // We have one of our possible n! solutions,
                // add it to the list.
                list.Add(new List<T>(vals));
            }
            else
            {
                for (var i = start; i <= end; i++)
                {
                    Swap(ref vals[start], ref vals[i]);
                    DoPermute(vals, start + 1, end, list);
                    Swap(ref vals[start], ref vals[i]);
                }
            }

            return list;
        }

        public static void Swap<T>(ref T a, ref T b)
        {
            var temp = a;
            a = b;
            b = temp;
        }
    }
}