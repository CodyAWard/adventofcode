using System.Collections.Generic;

namespace AdventOfCode.MMXX
{
    public class Day1 : MMXXDay
    {
        public override string GetSolution()
        {
            var data = ReadData().AsInts();
            return 
                PartA(data) + "\n" + 
                PartB(data);
        }

        private static string PartA(IEnumerable<int> data)
        {
            int a = 0, b = 0;

            // find two numbers who sum is 2020

            // naive solution
            bool isDone = false;
            foreach (var aa in data)
            {
                foreach (var bb in data)
                {
                    if (aa + bb == 2020)
                    {
                        a = aa;
                        b = bb;
                        isDone = true;
                        break;
                    }
                }

                if (isDone) break;
            }

            // multiply those numbers
            var answer = (a * b).ToString();

            return $"A:{a} B:{b}\n" +
                $"Answer = {answer}";
        }

        private static string PartB(IEnumerable<int> data)
        {
            int a = 0, b = 0, c = 0;

            // find three numbers who sum is 2020

            // naive solution
            var isDone = false;
            foreach (var aa in data)
            {
                foreach (var bb in data)
                {
                    foreach (var cc in data)
                    {
                        if (aa + bb + cc == 2020)
                        {
                            a = aa;
                            b = bb;
                            c = cc;
                            isDone = true;
                            break;
                        }
                    }

                    if (isDone) break;
                }

                if (isDone) break;
            }

            // multiply those numbers
            var answer = (a * b * c).ToString();

            return $"A:{a} B:{b} C:{c}\n" +
                $"Answer = {answer}";
        }
    }
}