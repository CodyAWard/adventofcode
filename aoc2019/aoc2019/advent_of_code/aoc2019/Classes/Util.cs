using System.Collections.Generic;
using System.Linq;

namespace Ward
{
    public static class Util
    {
        public static long[] GetDigits(long val)
        {
            IEnumerable<long> dig()
            {
                while (val > 0)
                {
                    var digit = val % 10;
                    val /= 10;
                    yield return digit;
                }
            }

            return dig().ToArray();
        }
    }
}
