using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode.MMXV
{
    public class Day7 : MMXVDay
    {
        public override string GetSolution()
        {
            var data = ReadData();
            return 
                PartA(data) + "\n" + 
                PartB(data);
        }

        public class Instruction
        {
            public class Input
            {
                public string ID;

                public Input(string id)
                {
                    ID = id;
                }

                public bool TryGetValue(
                    Dictionary<string, ushort> wires,
                    Dictionary<string, ushort> overrides, out ushort value)
                {
                    if (overrides != null)
                    {
                        if (overrides.ContainsKey(ID))
                        {
                            value = overrides[ID];
                            return true;
                        }
                    }

                    if (ushort.TryParse(ID, out value))
                    {
                        return true;
                    }
                    if (wires.ContainsKey(ID))
                    {
                        value = wires[ID];
                        return true;
                    }
                    value = 0;
                    return false;
                }
            }

            public const string GATE_AND = "AND";
            public const string GATE_OR = "OR";
            public const string GATE_NOT = "NOT";
            public const string GATE_LSHIFT = "LSHIFT";
            public const string GATE_RSHIFT = "RSHIFT";

            public const string OUTPUT_LIMITTER = " -> ";

            public Input Signal;
            public Input InputA;
            public Input InputB;
            public string Gate;
            public string Output;

            public bool Complete;

            public Func<bool> RequiresInput;
            private Action<
                Dictionary<string, ushort>,
                Dictionary<string, ushort>> application;

            public Instruction(string instruction)
            {
                var inputAndOutput = instruction.Split(OUTPUT_LIMITTER);
                var fullInput = inputAndOutput[0];
                var output = inputAndOutput[1];

                if (fullInput.Contains(GATE_AND)) AndGate();
                else if (fullInput.Contains(GATE_OR)) OrGate();
                else if (fullInput.Contains(GATE_LSHIFT)) LShiftGate();
                else if (fullInput.Contains(GATE_RSHIFT)) RShiftGate();
                else if (fullInput.Contains(GATE_NOT)) NotGate();
                else SignalTransfer();

                void NotGate()
                {
                    var b = fullInput.Split($"{GATE_NOT} ");
                    InputB = new Input(b[1]);
                    Gate = GATE_NOT;
                    application = (w, o) =>
                    {
                        if (InputB.TryGetValue(w, o, out var val))
                        {
                            Complete = true;
                            w[output] = (ushort)(~val);
                        }
                    };
                }

                void ParseGate(string gate)
                {
                    var ab = fullInput.Split($" {gate} ");
                    InputA = new Input(ab[0]);
                    InputB = new Input(ab[1]);
                    Gate = gate;
                }

                void AndGate()
                {
                    ParseGate(GATE_AND);
                    application = (w, o) =>
                    {
                        if (InputA.TryGetValue(w, o, out var a))
                        {
                            if (InputB.TryGetValue(w, o, out var b))
                            {
                                Complete = true;
                                w[output] = (ushort)(a & b);
                            }
                        }
                    };
                }

                void OrGate()
                {
                    ParseGate(GATE_OR);
                    application = (w, o) =>
                    {
                        if (InputA.TryGetValue(w, o, out var a))
                        {
                            if (InputB.TryGetValue(w, o, out var b))
                            {
                                Complete = true;
                                w[output] = (ushort)(a | b);
                            }
                        }
                };
                }

                void LShiftGate()
                {
                    ParseGate(GATE_LSHIFT);
                    application = (w, o) =>
                    {
                        if (InputA.TryGetValue(w, o, out var a))
                        {
                            if (InputB.TryGetValue(w, o, out var b))
                            {
                                Complete = true;
                                w[output] = (ushort)(a << b);
                            }
                        }
                    };
                }

                void RShiftGate()
                {
                    ParseGate(GATE_RSHIFT);
                    application = (w, o) =>
                    {
                        if (InputA.TryGetValue(w, o, out var a))
                        {
                            if (InputB.TryGetValue(w, o, out var b))
                            {
                                Complete = true;
                                w[output] = (ushort)(a >> b);
                            }
                        }
                    };
                }

                void SignalTransfer()
                {
                    Signal = new Input(fullInput);
                    Output = output;

                    application = (w, o) =>
                    {
                        if (Signal.TryGetValue(w, o, out var val))
                        {
                            Complete = true;
                            w[Output] = val;
                        }
                    };
                }
            }

            public void Apply(Dictionary<string, ushort> wires, Dictionary<string, ushort> overrides)
            {
                if (Complete) return;
                application.Invoke(wires, overrides);
            }
        }

        private static void RunData(
            IEnumerable<string> data, 
            Dictionary<string, ushort> wires,
            Dictionary<string, ushort> overrides = null)
        {
            var instructions = new List<Instruction>();
            foreach (var line in data)
            {
                instructions.Add(new Instruction(line));
            }

            int completed = 0;
            while (completed < instructions.Count)
            {
                completed = 0;
                foreach (var instruction in instructions)
                {
                    instruction.Apply(wires, overrides);
                    if (instruction.Complete) completed++;
                }
            }
        }


        private static string PartA(IEnumerable<string> data)
        {
            var wires = new Dictionary<string, ushort>();
            RunData(data, wires);

            return wires["a"].ToString();
        }

        private static string PartB(IEnumerable<string> data)
        {
            var wires = new Dictionary<string, ushort>();
            RunData(data, wires);
            var overrides = new Dictionary<string, ushort>() { 
                { "b", wires["a"] } 
            };
            wires.Clear();
            RunData(data, wires, overrides);

            return wires["a"].ToString();
        }
    }
}