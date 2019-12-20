using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Ward.Days
{
    public class Day4 : BaseDay
    {
        protected override int Day() => 4;

        protected override string GetSolution()
        {
            return GetCode().ToString();
        }

        private int GetCode()
        {
            int total = 0;
            for (int i = 264793; i <= 803935; i++) //The value is within the range given in your puzzle input.
            {
                if (!Asscends(i) || !HasPair(i)) continue;
                Debug.WriteLine(i);
                total++;
            }

            return total;
        }

        private bool Asscends(int i)
        {
            var digits = Util.GetDigits(i);
            for (int j = 1; j < digits.Length - 1; j++)
            {
                var prev = digits[j - 1];
                var here = digits[j];
                var next = digits[j + 1];

                if (prev < here || here < next) return false;
            }

            return true;
        }

        private bool HasPair(int i)
        {
            var digits = Util.GetDigits(i);

            bool hasDouble = false;
            long doub = 0;
            long trip = 0;
            bool hasTriple = false;

            for (int j = 1; j < digits.Length - 1; j++)
            {
                var prev = digits[j - 1];
                var here = digits[j];
                var next = digits[j + 1];

                if (prev == here && here == next)
                {
                    trip = here;
                    hasTriple = true; 
                }
                else if (prev == here || here == next)
                {
                    doub = here;
                    hasDouble = true; break;
                }
            }

            if (hasTriple && !hasDouble) return false;
            if(hasTriple && hasDouble) return doub != trip;
            return hasDouble;
        }
    }
}