using System;

namespace AdventOfCode.Days
{
    public abstract class BaseDay
    {
        public abstract int Year();
        public abstract int Day();
        public abstract string GetSolution();

        public virtual void Run()
        {
            var solution = GetSolution();
            Console.WriteLine($"=================\n" +
                              $"[SOLUTION {Year()}.{Day()}]\n" +
                              $"\n" +
                              $"{solution}\n" +
                              $"=================");
        }
    }
}
