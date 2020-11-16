using AdventOfCode.Days;

namespace AdventOfCode.MMXX
{
    public abstract class MMXXDay : BaseDay
    {
        public override int Year() => 2020;
        public override int Day() => int.Parse(GetType().Name.Replace("Day", string.Empty));
    }
}