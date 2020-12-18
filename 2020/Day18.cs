using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode.MMXX
{
    public class Day18 : MMXXDay
    {
        public override string GetSolution()
        {
            var data = ReadData();

            return PartA(data) + "\n" +
                   PartB(data);
        }

        private string PartA(IEnumerable<string> data)
        {
            long sum = 0;
            foreach (var line in data)
            {
                sum += Equate(line);
            }
            return sum.ToString();
        }

        private string PartB(IEnumerable<string> data)
        {
            long sum = 0;
            foreach (var line in data)
            {
                sum += EquateWithPrecedence(line);
            }
            return sum.ToString();
        }

        public static long Equate(string equation)
        {
            return Equate(equation, out _);
        }

        public static long EquateWithPrecedence(string equation)
        {
            // just wrap every plus equation with parethensis 
            var builder = new StringBuilder(equation);
            for (int i = 0; i < builder.Length; i++)
            {
                if (builder[i] == '+')
                {
                    i = InsertLeft(builder, i);
                    i = InsertRight(builder, i);
                }
            }

            return Equate(builder.ToString(), out _);
        }

        private static int InsertLeft(StringBuilder builder, int i)
        {
            var pars = 0;
            for (int j = i - 1; j >= 0; j--) // search left and insert (
            {
                var c = builder[j];
                if (c == ')')
                {
                    pars++;
                    continue;
                }
                if (c == '(')
                {
                    pars--;
                    if (pars <= 0)
                    {
                        builder.Insert(j, '(');
                        i++;
                        break;
                    }
                }
                else if (pars <= 0 && int.TryParse(builder[j].ToString(), out _))
                {
                    builder.Insert(j, '(');
                    i++;
                    break;
                }
            }

            return i;
        }

        private static int InsertRight(StringBuilder builder, int i)
        {
            var pars = 0;
            for (int j = i; j < builder.Length; j++) // search right and insert )
            {
                var c = builder[j];
                if (c == '(')
                {
                    pars++;
                    continue;
                }
                if (c == ')')
                {
                    pars--;
                    if (pars <= 0)
                    {
                        builder.Insert(j + 1, ')');
                        break;
                    }
                }
                else if (pars <= 0 && int.TryParse(builder[j].ToString(), out _))
                {
                    builder.Insert(j + 1, ')');
                    break;
                }
            }

            return i;
        }

        public static long Equate(string equation, out int index)
        {
            long? left = null;
            long? right = null;
            Func<long, long, long> operation = null;

            void Apply()
            {
                if (left.HasValue && right.HasValue)
                {
                    left = operation.Invoke(left.Value, right.Value);
                    right = null;
                    operation = null;
                }
            }

            for (int i = 0; i < equation.Length; i++)
            {
                var c = equation[i];
                Apply();

                switch (c)
                {
                    case ' ':
                        continue;
                    case '(':
                        var answer = Equate(equation.Substring(i + 1), out var offset);
                        i += offset + 1;
                        if (!left.HasValue) left = answer;
                        else right = answer;
                        break;
                    case ')':
                        index = i;
                        return left.Value;
                    case '+':
                        operation = (r, l) => r + l;
                        break;
                    case '*':
                        operation = (r, l) => r * l;
                        break;
                    default:
                        if (!left.HasValue) left = long.Parse(c.ToString());
                        else right = long.Parse(c.ToString());
                        break;
                }
            }

            Apply();
            index = equation.Length;
            return left.Value;
        }
    }
}