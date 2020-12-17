using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode.MMXV
{
    public class Day10 : MMXVDay
    {
        public override string GetSolution()
        {
            var data = ReadData();
            return
                PartA(data) + "\n" +
                PartB(data);
        }

        private string PartA(IEnumerable<string> data)
        {
            var input = data.First();
            for (int i = 0; i < 40; i++)
            {
                input = LookAndSay(input);
            }

            return input.Length.ToString();
        }

        private string PartB(IEnumerable<string> data)
        {
            var input = data.First();
            for (int i = 0; i < 50; i++)
            {
                input = LookAndSay(input);
            }

            return input.Length.ToString();
        }

        private string LookAndSay(string input)
        {
            var output = new StringBuilder();
            var currentChar = input[0];
            var count = 1;
            for (int i = 1; i < input.Length; i++)
            {
                var c = input[i];
                if (c == currentChar)
                {
                    count++;
                }
                else
                {
                    output.Append(count);
                    output.Append(currentChar);

                    currentChar = c;
                    count = 1;
                }
            }

            output.Append(count);
            output.Append(currentChar);
            return output.ToString();
        }
    }
}