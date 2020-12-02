using System;
using System.Diagnostics;

namespace AdventOfCode
{
    public class CodysCoolComputer
    {
        public Action<long> Output;

        private long inputCounter;
        private Func<long[]> inputs;
        public long Input
        {
            get
            {
                var allInputs = inputs();
                if (inputCounter >= allInputs.Length) inputCounter = allInputs.Length - 1;
                var input = allInputs[inputCounter];
                inputCounter++;
                return input;
            }
        }

        public bool IsPaused;
        
        public void SetInputs(Func<long[]> input)
        {
            inputs = input;
        }

        public long[] Memory { get; private set; }
        public long CurrentPosition { get; private set; }
        public long RelativeBase { get; private set; }

        public void RunProgram(long[] program)
        {
            InitializeMemory(program);
           
            RunFromStart();
        }

        private class Instruction
        {
            public Opperation DE;
            public ParameterMode C;
            public ParameterMode B;
            public ParameterMode A;

            public Instruction(long instruction)
            {
                var digits = Util.GetDigits(instruction);

                switch (digits.Length)
                {
                    case 0:
                        DE = Opperation.DATA;
                        C = ParameterMode.OMIT;
                        B = ParameterMode.OMIT;
                        A = ParameterMode.OMIT;
                        break;
                    case 1:
                        DE = (Opperation)digits[0];
                        C = ParameterMode.OMIT;
                        B = ParameterMode.OMIT;
                        A = ParameterMode.OMIT;
                        break;
                    case 2:
                        DE = (Opperation)((digits[1]* 10) + digits[0]);
                        C = ParameterMode.OMIT;
                        B = ParameterMode.OMIT;
                        A = ParameterMode.OMIT;
                        break;
                    case 3:
                        DE = (Opperation)((digits[1] * 10) + digits[0]);
                        C = (ParameterMode)digits[2];
                        B = ParameterMode.OMIT;
                        A = ParameterMode.OMIT;
                        break;
                    case 4:
                        DE = (Opperation)((digits[1] * 10) + digits[0]);
                        C = (ParameterMode)digits[2];
                        B = (ParameterMode)digits[3];
                        A = ParameterMode.OMIT;
                        break;
                    case 5:
                        DE = (Opperation)((digits[1] * 10) + digits[0]);
                        C = (ParameterMode)digits[2];
                        B = (ParameterMode)digits[3];
                        A = (ParameterMode)digits[4];
                        break;
                }
            }
        }

        private enum ParameterMode
        {
            OMIT = POSITION,
            POSITION = 0,
            IMMEDIATE = 1,
            RELATIVE = 2,
        }

        private enum Opperation
        {
            DATA = 0,
            ADD = 1,
            MULTIPLY = 2,
            INPUT = 3,
            OUTPUT = 4,
            JUMP_IF_TRUE = 5,
            JUMP_IF_FALSE = 6,
            LESS_THAN = 7,
            EQUALS = 8,
            RELATIVE_BASE_OFFSET = 9,

            HALT = 99,
        }

        private void InitializeMemory(long[] program)
        {
            Memory = new long[program.Length * 1000];
            program.CopyTo(Memory, 0);
        }
        
        private Opperation GetOpperationFromValue(long value)
        {
            try
            {
                return (Opperation)value;
            }
            catch
            {
                return Opperation.DATA;
            }
        }

        private long ReadAddress(long address)
        {
            return Memory[address];
        }

        private void SetAddress(ParameterMode mode, long address, long value)
        {
            switch (mode)
            {
                case ParameterMode.POSITION:
                    Memory[address] = value;
                    return;
                case ParameterMode.RELATIVE:
                    Memory[RelativeBase + address] = value;
                    return;
            }

            throw new Exception();
        }

        private long ReadAddressFromPosition(long from)
        {
            return ReadAddress(CurrentPosition + from);
        }
        
        public void RunFromStart()
        {
            CurrentPosition = 0;
            RunFromPosition();
        }

        public void RunFromPosition()
        {
            bool shouldRun()
            {
                return CurrentPosition != -1 && !IsPaused;
            }

            while (shouldRun())
            {
                Run();
            }
        }

        private void Run()
        {
            var inst = ReadAddress(CurrentPosition);
            var instruction = new Instruction(inst);
            DoInstruction(instruction);
        }

        private long GetValue(ParameterMode mode, long address)
        {
            switch (mode)
            {
                case ParameterMode.POSITION:
                    return ReadAddress(address);
                case ParameterMode.IMMEDIATE:
                    return address;
                case ParameterMode.RELATIVE:
                    return ReadAddress(RelativeBase + address);
            }

            throw new Exception();
        }

        private void DoInstruction(Instruction instruction)
        {
            switch (instruction.DE)
            {
                case Opperation.DATA:
                    CurrentPosition = -1;
                    break;
                case Opperation.INPUT:
                    {
                        var address1 = ReadAddressFromPosition(1);
                        SetAddress(instruction.C, address1, Input);
                        
                    }
                    CurrentPosition += 2;
                    break;
                case Opperation.OUTPUT:
                    {
                        var address1 = ReadAddressFromPosition(1);

                        var val = GetValue(instruction.C, address1);
                        Output?.Invoke(val);
                    }
                    CurrentPosition += 2;
                    break;
                case Opperation.ADD:
                    {
                        var address1 = ReadAddressFromPosition(1);
                        var address2 = ReadAddressFromPosition(2);
                        var address3 = ReadAddressFromPosition(3);

                        var value1 = GetValue(instruction.C, address1);
                        var value2 = GetValue(instruction.B, address2);

                        var sum = value1 + value2;

                        SetAddress(instruction.A, address3, sum);
                    }
                    CurrentPosition += 4;
                    break;
                case Opperation.MULTIPLY:
                    {
                        var address1 = ReadAddressFromPosition(1);
                        var address2 = ReadAddressFromPosition(2);
                        var address3 = ReadAddressFromPosition(3);

                        var value1 = GetValue(instruction.C, address1);
                        var value2 = GetValue(instruction.B, address2);

                        var mult = value1 * value2;

                        SetAddress(instruction.A, address3, mult);
                    }
                    CurrentPosition += 4;
                    break;
                case Opperation.JUMP_IF_TRUE:
                    {
                        var address1 = ReadAddressFromPosition(1);
                        var address2 = ReadAddressFromPosition(2);

                        var value1 = GetValue(instruction.C, address1);
                        var value2 = GetValue(instruction.B, address2);

                        if (value1 != 0) CurrentPosition = value2;
                        else CurrentPosition += 3;
                    }
                    break;
                case Opperation.JUMP_IF_FALSE:
                    {
                        var address1 = ReadAddressFromPosition(1);
                        var address2 = ReadAddressFromPosition(2);

                        var value1 = GetValue(instruction.C, address1);
                        var value2 = GetValue(instruction.B, address2);

                        if (value1 == 0) CurrentPosition = value2;
                        else CurrentPosition += 3;
                    }
                    break;
                case Opperation.LESS_THAN:
                    {
                        var address1 = ReadAddressFromPosition(1);
                        var address2 = ReadAddressFromPosition(2);
                        var address3 = ReadAddressFromPosition(3);

                        var value1 = GetValue(instruction.C, address1);
                        var value2 = GetValue(instruction.B, address2);

                        var val = value1 < value2 ? 1 : 0;

                        SetAddress(instruction.A, address3, val);
                    }
                    CurrentPosition += 4;
                    break;
                case Opperation.EQUALS:
                    {
                        var address1 = ReadAddressFromPosition(1);
                        var address2 = ReadAddressFromPosition(2);
                        var address3 = ReadAddressFromPosition(3);

                        var value1 = GetValue(instruction.C, address1);
                        var value2 = GetValue(instruction.B, address2);

                        var val = value1 == value2 ? 1 : 0;

                        SetAddress(instruction.A, address3, val);
                    }
                    CurrentPosition += 4;
                    break;
                case Opperation.RELATIVE_BASE_OFFSET:
                    {
                        var address1 = ReadAddressFromPosition(1);

                        var val = GetValue(instruction.C, address1);
                        RelativeBase += val;
                    }
                    CurrentPosition += 2;
                    break;
                case Opperation.HALT:
                    CurrentPosition = -1;
                    break;
            }
        }
    }
}
