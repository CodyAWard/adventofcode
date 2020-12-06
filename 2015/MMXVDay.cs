using AdventOfCode.Days;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.MMXV
{
    public abstract class MMXVDay : BaseDay
    {
        public override int Year() => 2015;
        public override int Day() => int.Parse(GetType().Name.Replace("Day", string.Empty));

        public virtual IEnumerable<string> ReadData()
        {
            return File.ReadAllLines($"~/../../../../2015/Data/day{Day()}.data");
        }
    }
}