using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.MMXX
{
    public class Day8 : MMXXDay
    {
        public override string GetSolution()
        {
            var data = ReadData();
            return 
                PartA(data) + "\n" + 
                PartB(data);
        }

        public class GameKidCart
        {
            public class Instruction
            {
                public string Operation { get; set; }
                public int Argument { get; set; }

                public Instruction(string operation, int argument)
                {
                    Operation = operation;
                    Argument = argument;
                }

                public static Instruction Parse(string str)
                {
                    var data = str.Split(' ');
                    return new Instruction(data[0], int.Parse(data[1]));
                }
            }

            public Instruction[] Instructions { get; }

            public GameKidCart(Instruction[] instructions)
            {
                Instructions = instructions;
            }

            public static GameKidCart Parse(IEnumerable<string> data)
            {
                var instructions = data.Select(d => Instruction.Parse(d)).ToArray();
                return new GameKidCart(instructions);
            }
        }

        public class GameKid
        {
            public int Accumulator { get; private set; }
            public int Address { get; private set; }
            public readonly HashSet<int> UsedInstructions = new HashSet<int>();

            public static bool LOGGING = false;
            private void Log(
                string message,
                GameKidCart.Instruction instruction)
            {
                if (!LOGGING) return;
                Console.WriteLine($"[a={Accumulator}] [{Address}] [{instruction.Operation} {instruction.Argument}] {message}");
            }

            public bool RunCart(GameKidCart cart)
            {
                Accumulator = 0;
                Address = 0;
                UsedInstructions.Clear();

                while (true)
                {
                    if (Address >= cart.Instructions.Length)
                    {
                        Console.WriteLine("Finished");
                        return true;
                    }

                    var i = cart.Instructions[Address];

                    if (UsedInstructions.Contains(Address))
                    {
                        Log("INFINITE LOOP", i);
                        return false;
                    }
                                       
                    UsedInstructions.Add(Address);

                    Log("", i);
                    switch (i.Operation)
                    {
                        case "acc":
                            Accumulator += i.Argument;
                            Address += 1;
                            break;
                        case "nop":
                            Address += 1;
                            break;
                        case "jmp":
                            Address += i.Argument;
                            break;
                        default:
                            Log("UNSUPPORTED", i);
                            return false;
                    }
                }
            }
        }

        private string PartA(IEnumerable<string> data)
        {
            var gameKid = new GameKid();
            var cart = GameKidCart.Parse(data);
            gameKid.RunCart(cart);

            return "Accumulator = " + gameKid.Accumulator;
        }

        private string PartB(IEnumerable<string> data)
        {
            var gameKid = new GameKid();
            var cart = GameKidCart.Parse(data);
            gameKid.RunCart(cart);

            var usedInstructions = gameKid.UsedInstructions.ToArray();
            // narrow our search by only modifying instructions we actually use

            foreach (var i in usedInstructions) 
            {
                var instruction = cart.Instructions[i];
                var prevOp = instruction.Operation;

                if (prevOp == "jmp")
                    instruction.Operation = "nop";
                else if (prevOp == "nop")
                    instruction.Operation = "jmp";
                else continue;

                if (gameKid.RunCart(cart))
                {
                    return "Fixed Accumulator = " + gameKid.Accumulator;
                }

                // undo
                instruction.Operation = prevOp;
            }

            return "Failed";
        }
    }
}