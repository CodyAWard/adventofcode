using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.MMXX
{
    public class Day2 : MMXXDay
    {
        public override string GetSolution()
        {
            var data = ReadData();
            return 
                PartA(data) + "\n" + 
                PartB(data);
        }

        private class PasswordPolicy
        {
            public string Password { get; }

            public char EnforcedCharacter { get; }
            public int MinCount { get; }
            public int MaxCount { get; }

            public PasswordPolicy(string password, char enforcedCharacter, int minCount, int maxCount)
            {
                Password = password;
                EnforcedCharacter = enforcedCharacter;
                MinCount = minCount;
                MaxCount = maxCount;
            }

            public bool IsValidForSledCompany()
            {
                var count = Password.Where(c => c == EnforcedCharacter).Count();
                return count >= MinCount && count <= MaxCount;
            }

            public bool IsValidForTobogganCompany()
            {
                var minChar = Password[MinCount];
                var maxChar = Password[MaxCount];

                return (minChar == EnforcedCharacter
                    || maxChar == EnforcedCharacter)
                    && minChar != maxChar;
            }

            public static PasswordPolicy Parse(string passwordAndPolicy)
            {
                var countBreak = passwordAndPolicy.IndexOf('-');
                var policyEnd = passwordAndPolicy.IndexOf(':');

                var minCountString = passwordAndPolicy.Substring(0, countBreak);
                var maxCountString = passwordAndPolicy.Substring(countBreak + 1, (policyEnd - 2) - (countBreak + 1));

                var minCount = int.Parse(minCountString);
                var maxCount = int.Parse(maxCountString);

                var enforcedChar = passwordAndPolicy[policyEnd - 1];
                var password = passwordAndPolicy.Substring(policyEnd + 1);

                return new PasswordPolicy(password, enforcedChar, minCount, maxCount);
            }
        }

        private static string PartA(IEnumerable<string> data)
        {
            var validTotal = 0;
            foreach (var passAndPolicy in data)
            {
                var policy = PasswordPolicy.Parse(passAndPolicy);
                if (policy.IsValidForSledCompany()) validTotal++;
            }

            return $"{validTotal} Valid Passwords";
        }

        private static string PartB(IEnumerable<string> data)
        {
            var validTotal = 0;
            foreach (var passAndPolicy in data)
            {
                var policy = PasswordPolicy.Parse(passAndPolicy);
                if (policy.IsValidForTobogganCompany()) validTotal++;
            }

            return $"{validTotal} Valid Passwords";
        }
    }
}