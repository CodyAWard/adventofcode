using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode.MMXX
{
    public class Day13 : MMXXDay
    {
        public override string GetSolution()
        {
            var data = ReadData();

            return
                PartA(data) + "\n" +
                PartB(data);
        }

        public class Bus
        {
            public string ID;
            public bool Valid => ID != "x";
            public int Timestamp => int.Parse(ID);

            public int GetFirstPastTime(int time)
            {
                if (!Valid) return int.MaxValue;
                var val = 0;
                while (val < time)
                    val += Timestamp;

                return val;
            }
        }

        private string PartA(IEnumerable<string> data)
        {
            var lines = data.ToArray();
            var desiredTime = int.Parse(lines[0]);
            var busData = lines[1].Split(',');
            
            Bus earliestBus = null;
            int earliestBusTime = int.MaxValue;

            foreach (var b in busData)
            {
                var bus = new Bus() { ID = b };
                var t = bus.GetFirstPastTime(desiredTime);
                if (t < earliestBusTime)
                {
                    earliestBus = bus;
                    earliestBusTime = t;
                }
            }

            return "Answer: " + ((earliestBusTime - desiredTime) * earliestBus.Timestamp);
        }

        private string PartB(IEnumerable<string> data)
        {
            var lines = data.ToArray();
            var busData = lines[1].Split(',');
            var busses = new List<(Bus, int)>();

            for (int i = 0; i < busData.Length; i++)
            {
                var bus = new Bus() { ID = busData[i] };
                if (bus.Valid) busses.Add((bus, i));
            }

            long time = 0;
            long step = 1;
            foreach (var bus in busses)
            {
                while ((time + bus.Item2) % bus.Item1.Timestamp != 0)
                {
                    // keep searching for multiples of our previously searched id's
                    time += step;
                }

                // if we search by multiples of our previous results, we ensure our match is a correct multiple
                // and can speed up the search greatly
                step *= bus.Item1.Timestamp;
            }

            return time.ToString();
        }
    }
}