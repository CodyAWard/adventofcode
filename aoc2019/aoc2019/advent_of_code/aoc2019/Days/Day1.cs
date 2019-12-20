using System.Collections.Generic;
using System.IO;

namespace Ward.Days
{
    public class Day1 : BaseDay
    {
        protected override int Day() => 1;

        protected override string GetSolution()
        {
            var input = GenerateInput();
            int total = 0;

            foreach (var value in input)
            {
                var module = new Module(value);
                var fuel = FuelUtil.GetRequiredFuelForModule(module);
                total += fuel;
            }

            return $"{total} Litres of Fuel";
        }

        private int[] GenerateInput()
        {
            var input = new List<int>();
            var line = string.Empty;
            StreamReader file = new StreamReader($@"..\..\Data\Day{Day()}Input.txt");
            while ((line = file.ReadLine()) != null)
            {
                int value = int.Parse(line);
                input.Add(value);
            }

            file.Close();

            return input.ToArray();
        }
    }
}