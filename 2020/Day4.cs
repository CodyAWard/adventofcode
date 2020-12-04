using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.MMXX
{
    static class Passporter
    {
        public static bool IsValidID(this string field)
        {
            return field.Length == 9
                && int.TryParse(field, out _);
        }

        public static bool IsValidEnum(this string field, params string[] e)
        {
            return e.Contains(field);
        }
        
        public static bool IsValidHexColor(this string field)
        {
            var builder = new StringBuilder(field);
            if (builder.Length != 7) return false;
            if (builder[0] != '#') return false;

            char[] validChars = new char[] {
                '0','1','2','3','4','5','6','7','8','9',
                'a','b','c','d','e','f',
            };

            for (int i = 6; i < 7; i++)
            {
                if (!validChars.Contains(builder[i])) return false;
            }

            return true;
        }

        public static bool IsValidHeight(this string field, params (string unit, int min, int max)[] checks)
        {
            foreach (var check in checks)
            {
                var unit = field.Substring(field.Length - check.unit.Length, check.unit.Length);
                if (unit != check.unit) continue;

                var heightStr = field.Substring(0, field.Length - check.unit.Length);
                var height = int.Parse(heightStr);

                return height >= check.min && height <= check.max;
            }

            return false;
        }

        public static bool IsValidYear(this string field, int minYear, int maxYear)
        {
            if (int.TryParse(field, out int year))
            {
                return year >= minYear && year <= maxYear;
            }

            return false;
        }
    }

    public class Day4 : MMXXDay
    {
        public override string GetSolution()
        {
            var data = ReadData();
            return 
                PartA(data) + "\n" + 
                PartB(data);
        }

        private class Document
        {
            public static class Keys
            {
                public static string BIRTH_YEAR = "byr"; // (Birth Year)
                public static string ISSUE_YEAR = "iyr"; // (Issue Year)
                public static string EXPIRATION_YEAR = "eyr"; // (Expiration Year)
                public static string HEIGHT = "hgt"; // (Height)
                public static string HAIR_COLOR = "hcl"; // (Hair Color)
                public static string EYE_COLOR = "ecl"; // (Eye Color)
                public static string PASSPORT_ID = "pid"; // (Passport ID)
                public static string COUNTRY_ID = "cid"; // (Country ID)
            }

            public readonly Dictionary<string, string> Fields = new Dictionary<string, string>();

            public bool IsValidPassport()
            {
                return
                    Fields.TryGetValue(Keys.BIRTH_YEAR, out string birthYear)
                    && Passporter.IsValidYear(birthYear, 1920, 2002)

                 && Fields.TryGetValue(Keys.ISSUE_YEAR, out string issueYear)
                    && Passporter.IsValidYear(issueYear, 2010, 2020)

                 && Fields.TryGetValue(Keys.EXPIRATION_YEAR, out string expYear)
                    && Passporter.IsValidYear(expYear, 2020, 2030)

                 && Fields.TryGetValue(Keys.HEIGHT, out string height)
                    && Passporter.IsValidHeight(height, ("cm", 150, 193), ("in", 59, 76))

                 && Fields.TryGetValue(Keys.HAIR_COLOR, out string hair)
                    && Passporter.IsValidHexColor(hair)

                 && Fields.TryGetValue(Keys.EYE_COLOR, out string eye)
                    && Passporter.IsValidEnum(eye, "amb", "blu", "brn", "gry", "grn", "hzl", "oth")

                && Fields.TryGetValue(Keys.PASSPORT_ID, out string pid)
                    && Passporter.IsValidID(pid);
            }

            public bool IsValidNorthPoleCredentials()
            {
                return
                  Fields.ContainsKey(Keys.BIRTH_YEAR)
               && Fields.ContainsKey(Keys.ISSUE_YEAR)
               && Fields.ContainsKey(Keys.EXPIRATION_YEAR)
               && Fields.ContainsKey(Keys.HEIGHT)
               && Fields.ContainsKey(Keys.HAIR_COLOR)
               && Fields.ContainsKey(Keys.EYE_COLOR)
               && Fields.ContainsKey(Keys.PASSPORT_ID);
            }
        }

        private static void ReadDocuments(IEnumerable<string> data, List<Document> allDocs)
        {
            Document currentDoc = null;

            foreach (var line in data)
            {
                if (string.IsNullOrWhiteSpace(line)) // full break
                {
                    StoreCurrent(allDocs, currentDoc);
                    currentDoc = null;
                }
                else // read data
                {
                    if (currentDoc == null)
                    {
                        Console.WriteLine("== New Document ==");
                        currentDoc = new Document();
                    }

                    var unparsedFields = line.Split(' ');
                    foreach (var unparsedField in unparsedFields)
                    {
                        var field = unparsedField.Split(':');
                        currentDoc.Fields.Add(field[0], field[1]);

                        Console.WriteLine($"   [{field[0]}] {field[1]}");
                    }
                }
            }
            
            StoreCurrent(allDocs, currentDoc);
        }

        private static void StoreCurrent(List<Document> allDocs, Document currentDoc)
        {
            if (currentDoc != null) // finish doc
            {
                Console.WriteLine($"== Completed {currentDoc.IsValidNorthPoleCredentials()} {currentDoc.IsValidPassport()} ==");
                allDocs.Add(currentDoc);
            }
        }

        private string PartA(IEnumerable<string> data)
        {
            var allDocs = new List<Document>();
            ReadDocuments(data, allDocs);

            return allDocs.Where(d => d.IsValidNorthPoleCredentials())
                .Count().ToString();
        }

        private string PartB(IEnumerable<string> data)
        {
            var allDocs = new List<Document>();
            ReadDocuments(data, allDocs);

            return allDocs.Where(d => d.IsValidPassport())
                .Count().ToString();
        }
    }
}