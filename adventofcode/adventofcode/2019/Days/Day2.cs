using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode.Days
{
    public class Day2 : BaseDay
    {
        public override int Year() => 2019;
        public override int Day() => 2;

        public override string GetSolution()
        {
            var computer = new CodysCoolComputer();
           
            for (int i = 1; i <= 99; i++)
            {
                for (int j = 1; j <= 99; j++)
                {
                    var input = GenerateInput();
                    input[1] = i;
                    input[2] = j;

                    computer.RunProgram(input);

                    if (computer.Memory[0] == 19690720)
                    {
                        return $"noun{i} verb{j}";
                    }
                }
            }

            return "error";

            //StringBuilder builder = new StringBuilder(); // part 1
            //builder.AppendLine("Data:");

            //var output = computer.Memory;
            //for (int i = 0; i < output.Length; i++)
            //{
            //    builder.AppendLine($"[{i}] {output[i]}");
            //}

            //return builder.ToString();
        }

        private long[] GenerateInput()
        {
            var input = new List<long>();
            var line = string.Empty;
            StreamReader file = new StreamReader($@"..\..\Data\Day{Day()}Input.txt");
            var data = file.ReadToEnd();
            var dataValues = data.Split(',');
            foreach (var dataValue in dataValues)
            {
                var value = long.Parse(dataValue);
                input.Add(value);
            }

            file.Close();

            return input.ToArray();
        }
    }
}