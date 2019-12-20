using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Ward.Days
{
    public class Day7 : BaseDay
    {
        protected override int Day() => 7;
        
        private long[] input;

        public static long MaxOutput;
        public static long[] MaxPerm;

        private class AmplificationCircuit
        {
            public int Count = 5;

            public long[] Phases;
            public Func<long[]> GetInput;

            private readonly Amplifier[] amplifiers;

            private int index;
            bool shouldRun = false;
            private long? currentOutput = 0;

            public AmplificationCircuit(long[] phases, Func<long[]> getInput)
            {
                Phases = phases;
                GetInput = getInput;

                amplifiers = new Amplifier[Count];
                for (int i = 0; i < Count; i++)
                {
                    amplifiers[i] = new Amplifier(Phases[i]);
                }
            }

            public void Run()
            {
                shouldRun = true;

                while (shouldRun)
                {
                    var output = currentOutput.HasValue ? currentOutput.Value : 0;
                    currentOutput = null;
                    Debug.WriteLine("Set Signal For: " + index + " To: " + output);
                    amplifiers[index].SetSignal(output);
                    Debug.WriteLine("Running " + index);
                    amplifiers[index].Run(GetInput(), GetOutput);
                    
                    shouldRun = false;
                    foreach (var amp in amplifiers)
                    {
                        var paused = false;
                        if (amp.Computer != null)
                        {
                            paused = amp.Computer.IsPaused || amp.Computer.CurrentPosition == -1;
                        }
                        if (paused) continue;
                        shouldRun = true;
                    }
                }

                Debug.WriteLine("=======================");
            }

            private void GetOutput(long output)
            {
                Debug.WriteLine("Output For: " + index + " : " + output);

                if (output > MaxOutput)
                {
                    MaxOutput = output;
                    MaxPerm = Phases;
                }

                amplifiers[index].Computer.IsPaused = true;

                index++;
                if (index >= Count) index = 0;

                currentOutput = output;
                
                if(amplifiers[index].Computer != null && amplifiers[index].Computer.CurrentPosition != - 1) amplifiers[index].Computer.IsPaused = false;
            }
        }

        protected override string GetSolution()
        {
            input = GenerateInput();

            foreach (var perm in GetPermutations(new long[] { 5, 6, 7, 8, 9 }, 5))
            {
                var ciruit = new AmplificationCircuit(perm.ToArray(), () => input);
                ciruit.Run();
            }

            Debug.WriteLine($"Max: {MaxOutput}    {MaxPerm[0]} {MaxPerm[1]} {MaxPerm[2]} {MaxPerm[3]} {MaxPerm[4]} ");

            return "end";
        }

        static IEnumerable<IEnumerable<T>>GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }


        private class Amplifier
        {
            private readonly long phase;

            private List<long> intCodeInput;
            private Func<long[]> Inputs => () => intCodeInput.ToArray();

            public CodysCoolComputer Computer;

            public void SetSignal(long signal)
            {
                intCodeInput.Add(signal);
            }

            public Amplifier(long phase)
            {
                this.phase = phase;

                intCodeInput = new List<long>() { phase };
            }

            public void Run(long[] input, Action<long> output)
            {
                if (Computer != null)
                {
                    Computer.RunFromPosition();
                    return;
                }

                Computer = new CodysCoolComputer();

                Computer.SetInputs(Inputs);
                Computer.Output = (i) =>
                {
                    output.Invoke(i);
                };

                Computer.RunProgram(input);
            }
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
                var value = int.Parse(dataValue);
                input.Add(value);
            }

            file.Close();

            return input.ToArray();
        }
    }
}