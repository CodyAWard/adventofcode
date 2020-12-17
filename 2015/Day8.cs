using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode.MMXV
{
    public class Day8 : MMXVDay
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
            var totals = (0, 0);
            foreach (var line in data)
            {
                var (code, str) = Calculate(line);
                totals.Item1 += code;
                totals.Item2 += str;
            }

            return (totals.Item1 - totals.Item2).ToString();
        }

        private string PartB(IEnumerable<string> data)
        {
            var totals = (0, 0);
            foreach (var line in data)
            {
                var (code, str) = Calculate(Encode(line));
                totals.Item1 += code;
                totals.Item2 += str;
            }

            return (totals.Item1 - totals.Item2).ToString();
        }

        public string Encode(string fullString)
        {
            string newString = "\"";
            string encode = "\\";

            foreach (char ch in fullString)
            {
                if (ch == '\\' || ch == '"')
                {
                    newString += encode;
                }

                newString += ch;
            }

            return newString;
        }

        public (int code, int str) Calculate(string fullString)
        {
            // include qutoes
            var code = 2;
            var str = 0;

            for (int i = 1; i < fullString.Length - 1; i++)
            {
                if (fullString[i] == '\\')
                {
                    if (fullString[i + 1] == 'x')
                    {
                        str += 1;
                        code += 4;
                        i += 3;
                    }
                    else
                    {
                        str += 1;
                        code += 2;
                        i++;
                    }
                }
                else
                {
                    str++;
                    code++;
                }
            }

            return (code, str);
        }
    }
}