using System;
using System.Diagnostics;

namespace AdventOfCode.Days
{
    public abstract class BaseDay
    {
        public abstract int Year();
        public abstract int Day();
        public abstract string GetSolution();

        public virtual void Run()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var solution = GetSolution();
            stopwatch.Stop();
            Console.WriteLine($"=================\n" +
                              $"[SOLUTION {Year()}.{Day()}]\n" +
                              $"\n" +
                              $"{solution}\n" +
                              $"in {stopwatch.ElapsedMilliseconds}ms\n"+
                              $"=================");
        }
    }
}
