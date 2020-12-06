using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.MMXV
{
    public class Day4 : MMXVDay
    {
        public override string GetSolution()
        {
            var data = ReadData().ToArray()[0];
            return 
                PartA(data) + "\n" + 
                PartB(data);
        }

        private static string GetHash(MD5 md5, string input)
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private static string PartA(string data)
        {
            return FindLowestSuffix(data, 5);
        }

        private static string PartB(string data)
        {
            return FindLowestSuffix(data, 6);
        }

        private static int suffix = 0;
        private static string FindLowestSuffix(string data, int leadingZero)
        {
            MD5 md5 = MD5.Create();
            var checkString = "";
            for (int i = 0; i < leadingZero; i++)
                checkString += "0";

            while (true)
            {
                var hash = GetHash(md5, $"{data}{suffix}");
                if (hash.Substring(0, leadingZero) == checkString) return suffix.ToString();
                suffix++;
            }
        }
    }
}