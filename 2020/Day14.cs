using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode.MMXX
{
    public class Day14 : MMXXDay
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
            var currentMask = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            Dictionary<int, long> memory = new Dictionary<int, long>();

            var maskID = "mask = ";
            foreach (var line in data)
            {
                if (line.StartsWith(maskID))
                {
                    currentMask = line.Substring(maskID.Length);
                }
                else
                {
                    var address = ParseAddress(line);
                    var value = ParseValue(line);
                    value = ApplyMaskToValue(value, currentMask);
                    memory[address] = Convert.ToInt64(value, 2);
                }
            }

            long sum = 0;
            foreach (var value in memory.Values)
                sum += value;

            return sum.ToString();
        }

        private string PartB(IEnumerable<string> data)
        {
            var currentMask = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            Dictionary<long, long> memory = new Dictionary<long, long>();

            var maskID = "mask = ";
            foreach (var line in data)
            {
                if (line.StartsWith(maskID))
                {
                    currentMask = line.Substring(maskID.Length);
                }
                else
                {
                    var address = ParseAddress(line);
                    var value = ParseValue(line);
                    var binary = Convert.ToString(address, 2);
                    binary = binary.PadLeft(36, '0');
                    var addresses = GetMaskedAddresses(binary, currentMask);
                    var longValue = Convert.ToInt64(value, 2); 
                    foreach (var a in addresses)
                    {
                        var ad = Convert.ToInt64(a.ToString(), 2);
                        memory[ad] = longValue;
                    }
                }
            }

            long sum = 0;
            foreach (var value in memory.Values)
                sum += value;

            return sum.ToString();
        }

        private static string ParseValue(string line)
        {
            var split = line.Split(" = ");
            var value = long.Parse(split[1]);
            var binary = Convert.ToString(value, 2);
            binary = binary.PadLeft(36, '0');
            return binary;
        }

        private static List<StringBuilder> GetMaskedAddresses(string address, string binaryMask)
        {
            var addressBuilder = new StringBuilder(address);
            var addresses = new List<StringBuilder> { new StringBuilder() };

            for (int i = 0; i < binaryMask.Length; i++)
            {
                var mask = binaryMask[i];
                if (mask == '1')
                {
                    for (int j = 0; j < addresses.Count; j++) addresses[j].Append('1');
                }
                else if (mask == 'X')
                {
                    var range = new List<StringBuilder>();
                    for (int j = 0; j < addresses.Count; j++)
                    {
                        var sb1 = new StringBuilder(addresses[j].ToString());
                        sb1.Append('1');

                        var sb0 = new StringBuilder(addresses[j].ToString());
                        sb0.Append('0');

                        range.Add(sb1);
                        range.Add(sb0);
                    }
                    addresses = range;
                }
                else
                {
                    for (int j = 0; j < addresses.Count; j++) addresses[j].Append(addressBuilder[i]);
                }
            }

            return addresses;
        }

        private static string ApplyMaskToValue(string binary, string binaryMask)
        {
            var maskBuilder = new StringBuilder(binary);
            for (int i = 0; i < binaryMask.Length; i++)
            {
                var mask = binaryMask[i];
                if (mask == '0') maskBuilder[i] = '0';
                else if (mask == '1') maskBuilder[i] = '1';
            }

            return maskBuilder.ToString();
        }

        private static int ParseAddress(string line)
        {
            var split = line.Split(" = ");
            var memAdd = split[0];
            return int.Parse(memAdd.Substring(4, memAdd.Length - 5));
        }
    }
}