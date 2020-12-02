using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace AdventOfCode.Days
{
    public class Day9 : BaseDay
    {
        public override int Year() => 2019;
        public override int Day() => 9;

        public override string GetSolution()
        {
            var computer = new CodysCoolComputer();

            var input = GenerateInput();

            computer.SetInputs(() => new long[] { 2 });
            computer.Output = (i) => Debug.WriteLine(i);
            computer.RunProgram(input);

            return "end";
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