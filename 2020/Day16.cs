using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode.MMXX
{
    public class Day16 : MMXXDay
    {
        public override string GetSolution()
        {
            var data = ReadData();
            ParseTicketInformation(data, out var rules, out var nearby, out var yours);
            List<int> invalidNumbers = FindInvalidNumbers(rules, nearby);
            var partA = invalidNumbers.Sum().ToString();

            var ordered = OrderRules(rules, nearby).ToArray();

            long partB = 1;
            for (int i = 0; i < ordered.Length; i++)
            {
                Console.WriteLine($"{ordered[i].Field} -- {yours.Values[i]}");
                if (ordered[i].Field.StartsWith("departure"))
                {
                    partB *= yours.Values[i];
                }
            }

            return
                partA + "\n" +
                partB;
        }

        private static IEnumerable<Rule> OrderRules(IEnumerable<Rule> rules, IEnumerable<Ticket> tickets)
        {
            var orderedRules = new Rule[rules.Count()];
            var satisfied = 0;
            var validRules = GatherValidIndexes(rules, tickets);
            while (satisfied < orderedRules.Length)
            {
                foreach (var rule in rules)
                {
                    var validFor = validRules[rule];
                    if (validFor.Count != 1) continue;

                    satisfied++;
                    var value = validFor[0];
                    orderedRules[value] = rule;
                    foreach (var r in rules)
                    {
                        validRules[r].Remove(value);
                    }
                }
            }

            return orderedRules;
        }

        private static Dictionary<Rule, List<int>> GatherValidIndexes(IEnumerable<Rule> rules, IEnumerable<Ticket> tickets)
        {
            var validRules = new Dictionary<Rule, List<int>>();
            foreach (var rule in rules)
            {
                for (int valueIndex = 0; valueIndex < rules.Count(); valueIndex++)
                {
                    bool valid = true;
                    foreach (var ticket in tickets)
                    {
                        var value = ticket.Values[valueIndex];
                        if (!rule.Values.Contains(value))
                        {
                            valid = false;
                            break;
                        }
                    }

                    if (valid)
                    {
                        if (!validRules.ContainsKey(rule)) validRules[rule] = new List<int>();
                        validRules[rule].Add(valueIndex);
                    }
                }
            }

            return validRules;
        }

        private static List<int> FindInvalidNumbers(List<Rule> rules, List<Ticket> nearby)
        {
            var allValidNumbers = rules.SelectMany(r => r.Values);
            var invalidNumbers = new List<int>();
            for (int i = nearby.Count - 1; i >= 0; i--)
            {
                var ticket = nearby[i];
                foreach (var value in ticket.Values)
                {
                    if (allValidNumbers.Contains(value)) continue;
                    {
                        invalidNumbers.Add(value);
                        nearby.RemoveAt(i);
                    }
                }
            }

            return invalidNumbers;
        }

        public class Ticket
        {
            public int[] Values;

            public Ticket(string csv)
            {
                Values = csv.AsInts().ToArray();
            }
        }

        public class Rule
        {
            public string Field;
            public HashSet<int> Values;

            public Rule(string rule)
            {
                var fieldAndRanges = rule.Split(':');
                Field = fieldAndRanges[0];
                Values = new HashSet<int>();
                var ranges = fieldAndRanges[1].Split(" or ");
                foreach (var range in ranges)
                {
                    var minMax = range.Split('-');
                    var min = int.Parse(minMax[0]);
                    var max = int.Parse(minMax[1]);
                    for (int i = min; i <= max; i++)
                    {
                        Values.Add(i);
                    }
                }
            }
        }

        private static void ParseTicketInformation(IEnumerable<string> data, out List<Rule> rules, out List<Ticket> nearby, out Ticket yours)
        {
            const string SECTION_ID_YOURS = "your ticket:";
            const string SECTION_ID_NEARBY = "nearby tickets:";
            var readingTicket = false;
            var readingNearby = false;

            rules = new List<Rule>();
            nearby = new List<Ticket>();
            yours = null;

            foreach (var line in data)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                if (readingTicket)
                {
                    if (line == SECTION_ID_NEARBY)
                    {
                        readingTicket = false;
                        readingNearby = true;
                        continue;
                    }

                    yours = new Ticket(line);
                }
                else if (readingNearby)
                {
                    nearby.Add(new Ticket(line));
                }
                else // reading rules
                {
                    if (line == SECTION_ID_YOURS)
                    {
                        readingTicket = true;
                        continue;
                    }

                    rules.Add(new Rule(line));
                }
            }
        }
    }
}