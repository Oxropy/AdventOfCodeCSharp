using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019
{
    public class IntcodeComputer
    {
        public int Day { get; }
        public int Variant { get; }
        public IDictionary<long, long> Memory { get; }
        public long ProgramCounter { get; private set; }
        public long Output { get; private set; }
        public long RelativeBase { get; private set; }

        public IntcodeComputer(int day, int variant, string input) : this(day, variant, GetProgram(input))
        {
        }

        public IntcodeComputer(int day, int variant, IDictionary<long, long> memory)
        {
            Day = day;
            Variant = variant;
            Memory = memory;
        }

        public static IDictionary<long, long> GetProgram(string input)
        {
            return input.Split(",")
                .Select((s, i) => new {value = long.Parse(s), index = i})
                .ToDictionary(v => (long) v.index, v => v.value);
        }

        public long GetOutput(IList<long> input)
        {
            // Program counter
            for (; ProgramCounter < Memory.Count;)
            {
                var opCode = new OpCode(GetValue(ProgramCounter));
                if (opCode.Operation == 3 && input.Count == 0)
                {
                    return Output;
                }

                ProgramCounter += opCode.Operation switch
                {
                    1 => BinaryOperator((a, b) => a + b),
                    2 => BinaryOperator((a, b) => a * b),
                    3 => SetInput(input),
                    4 => SetOutput(),
                    6 => JumpIf(false),
                    5 => JumpIf(true),
                    7 => Comparison((a, b) => a < b),
                    8 => Comparison((a, b) => a == b),
                    9 => SetRelativeBase(),
                    99 => Memory.Count,
                    _ => throw new NotImplementedException()
                };
            }

            return Output;
        }

        private long Comparison(Func<long, long, bool> comp)
        {
            return BinaryOperator((a, b) => comp(a, b) ? 1 : 0);
        }

        private long BinaryOperator(Func<long, long, long> op)
        {
            var code = new OpCode(GetValue(ProgramCounter));

            var index1 = ProgramCounter + 1;
            var value1 = GetMemoryValue(index1, code.Parameter1Mode);
            var index2 = ProgramCounter + 2;
            var value2 = GetMemoryValue(index2, code.Parameter2Mode);
            var index3 = ProgramCounter + 3;
            var value3Imm = GetValue(index3);

            Memory[value3Imm] = op(value1, value2);
            return 4;
        }

        private long SetInput(IList<long> input)
        {
            var code = new OpCode(GetValue(ProgramCounter));

            var index1 = ProgramCounter + 1;
            var value1Imm = GetMemoryValue(index1, code.Parameter1Mode);

            Memory[value1Imm] = input[0];
            input.RemoveAt(0);
            return 2;
        }

        private long SetOutput()
        {
            var code = new OpCode(GetValue(ProgramCounter));

            var index1 = ProgramCounter + 1;
            var value1Imm = GetMemoryValue(index1, code.Parameter1Mode);

            Output = value1Imm;
            return 2;
        }

        private long JumpIf(bool isNonZero)
        {
            var code = new OpCode(GetValue(ProgramCounter));

            var index1 = ProgramCounter + 1;
            var value1 = GetMemoryValue(index1, code.Parameter1Mode);

            if ((isNonZero && value1 != 0) || (!isNonZero && value1 == 0))
            {
                var index2 = ProgramCounter + 2;
                var value2 = GetMemoryValue(index2, code.Parameter2Mode);
                return value2;
            }

            return 3;
        }

        private long SetRelativeBase()
        {
            var index1 = ProgramCounter + 1;
            var value1Imm = GetValue(index1);

            RelativeBase += value1Imm;
            return 2;
        }

        private long GetMemoryValue(long index, long mode)
        {
            var positionValue = GetValue(index);

            return mode switch
            {
                0 => GetValue(positionValue),
                1 => positionValue,
                2 => RelativeBase + positionValue,
                _ => throw new NotImplementedException()
            };
        }

        private long GetValue(long index)
        {
            if (Memory.TryGetValue(index, out var result))
            {
                return result;
            }

            Memory.Add(index, 0);
            return 0;
        }

        private struct OpCode
        {
            public long Operation { get; }
            public long Parameter1Mode { get; }
            public long Parameter2Mode { get; }
            public long Parameter3Mode { get; }

            public OpCode(long opCode)
            {
                Operation = opCode % 100;
                var parameterModes = opCode / 100;
                Parameter1Mode = parameterModes % 10;
                Parameter2Mode = parameterModes / 10 % 10;
                Parameter3Mode = parameterModes / 100 % 10;
            }
        }
    }
}