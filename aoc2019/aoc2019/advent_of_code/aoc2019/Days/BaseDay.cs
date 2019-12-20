using System.Diagnostics;

namespace Ward.Days
{
    public abstract class BaseDay
    {
        protected abstract int Day();
        protected abstract string GetSolution();

        public virtual void Run()
        {
            var solution = GetSolution();
            Debug.WriteLine($"[SOLUTION] [DAY {Day()}] {solution}");
        }
    }
}
