using AdventOfCode.Days;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.MMXX
{
    public abstract class MMXXDay : BaseDay
    {
        public override int Year() => 2020;
        public override int Day() => int.Parse(GetType().Name.Replace("Day", string.Empty));

        public virtual IEnumerable<string> ReadData()
        {
            return File.ReadAllLines($"~/../../../../2020/Data/day{Day()}.data");
        }
    }

    public static class DataUtil
    {
        public static IEnumerable<int> AsInts(this IEnumerable<string> data)
        {
            return data.Select(s => int.Parse(s));
        }

        public static IEnumerable<long> AsLongs(this IEnumerable<string> data)
        {
            return data.Select(s => long.Parse(s));
        }
    }
}