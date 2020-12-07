using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.MMXX
{
    public class Day7 : MMXXDay
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
            var rules = GatherRules(data);

            int canHoldGold = 0;
            foreach (var rule in rules)
            {
                var canHold = new List<string>();
                rule.WhatCanIHold(rules, canHold);
                foreach (var item in canHold)
                    if (item.Contains("shiny gold")) canHoldGold++;
            }

            return canHoldGold.ToString();
        }

        private string PartB(IEnumerable<string> data)
        {
            var rules = GatherRules(data);

            var gold = rules.Where(r => r.Descriptor.Contains("shiny gold")).First();
            return gold.TotalInternalBags(rules).ToString();
        }



        private static List<LuggageRule> GatherRules(IEnumerable<string> data)
        {
            var rules = new List<LuggageRule>();
            foreach (var line in data)
            {
                rules.Add(LuggageRule.Parse(line));
            }

            return rules;
        }

        class LuggageRule
        {
            public class Rule
            {
                public int Count { get; set; }
                public string Descriptor { get; set; }
            }

            public string Descriptor { get; set; }
            public Rule[] Rules { get; set; }

            public void WhatCanIHold(List<LuggageRule> allRules, List<string> canHold)
            {
                foreach (var rule in Rules)
                {
                    if (canHold.Contains(rule.Descriptor)) continue;

                    canHold.Add(rule.Descriptor);
                    var ruleForRule = allRules.Where(r => r.Descriptor == rule.Descriptor).First();
                    ruleForRule.WhatCanIHold(allRules, canHold);
                }
            }

            public int TotalInternalBags(List<LuggageRule> allRules)
            {
                var total = 0;
                foreach (var rule in Rules)
                {
                    var ruleForRule = allRules.Where(r => r.Descriptor == rule.Descriptor).First();
                    var childTotal = ruleForRule.TotalInternalBags(allRules);
                    total += (childTotal * rule.Count);
                    total += rule.Count;
                }

                return total;
            }

            public static LuggageRule Parse(string str)
            {
                // descriptor //             //    rule        // count - descriptor //
                // drab lavender bags contain 2 shiny white bags, 2 muted orange bags, 1 mirrored crimson bag, 1 dotted aqua bag.

                // split the descriptor from the rules
                var split = str.Split(" bags contain "); // split the descriptor from the rules
                var descriptor = split[0];               // grab the descriptor
                var ruleStrs = split[1];                 // grab all the rules
                var rulesSplit = ruleStrs.Split(',');    // split the rules up
                var rules = new List<Rule>();
                foreach (var r in rulesSplit)            // parse each rule
                {
                    if (r.StartsWith("no other")) continue;

                    var s = r.TrimStart().Split(' ');    // each rule follows {count} {actual descriptor} {bag(s)}

                    rules.Add(
                        new Rule()
                        {
                            Count = int.Parse(s[0]),
                            Descriptor = $"{s[1]} {s[2]}"
                        });
                }

                return new LuggageRule()
                {
                    Descriptor = descriptor,
                    Rules = rules.ToArray(),
                };
            }
        }
    }
}