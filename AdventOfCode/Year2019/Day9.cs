using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Base;

namespace AdventOfCode.Year2019
{
    public class Day9 : BaseDay
    {
        public Day9() : base(2019, nameof(Day9))
        {
        }

        public override string StartVariant1()
        {
            return GetOutput(new List<long> {1}).Output.ToString();
        }

        public override string StartVariant2()
        {
            throw new NotImplementedException();
        }

        private ProgramState GetOutput(IList<long> input)
        {
            var program = Input.Split(",")
                .Select((s, i) => new {value = long.Parse(s), index = i})
                .ToDictionary(v => (long) v.index, v => v.value);

            return GetOutput(program, 0, 0, input);
        }

        private static ProgramState GetOutput(IDictionary<long, long> program, long programCounter, long relativeBase, IList<long> input)
        {
            var output = -1L;

            // Program counter
            for (; programCounter < program.Count;)
            {
                var opCode = program[programCounter] % 100;
                if (opCode == 3 && input.Count == 0)
                {
                    return new ProgramState(program, programCounter, output, relativeBase);
                }

                programCounter = opCode switch
                {
                    1 => BinaryOperator(program, programCounter, relativeBase, (a, b) => a + b),
                    2 => BinaryOperator(program, programCounter, relativeBase, (a, b) => a * b),
                    3 => SetInput(program, programCounter, relativeBase, input),
                    4 => SetOutput(program, programCounter, relativeBase, out output),
                    5 => JumpIf(program, programCounter, relativeBase, true),
                    6 => JumpIf(program, programCounter, relativeBase, false),
                    7 => Comparison(program, programCounter, relativeBase, (a, b) => a < b),
                    8 => Comparison(program, programCounter, relativeBase, (a, b) => a == b),
                    9 => SetRelativeBase(program, programCounter, ref relativeBase),
                    99 => program.Count,
                    _ => throw new NotImplementedException()
                };
            }

            return new ProgramState(program, programCounter, output, relativeBase);
        }

        private static long Comparison(IDictionary<long, long> memory, long pc, long relativeBase, Func<long, long, bool> comp)
        {
            return BinaryOperator(memory, pc, relativeBase, (a, b) => comp(a, b) ? 1 : 0);
        }

        private static long BinaryOperator(IDictionary<long, long> memory, long pc, long relativeBase, Func<long, long, long> op)
        {
            var code = new OpCode(GetValue(memory, pc));

            var index1 = pc + 1;
            var value1 = GetMemoryValue(memory, index1, code.Parameter1Mode, relativeBase);
            var index2 = pc + 2;
            var value2 = GetMemoryValue(memory, index2, code.Parameter2Mode, relativeBase);
            var index3 = pc + 3;
            var value3Imm = GetValue(memory, index3);

            memory[value3Imm] = op(value1, value2);
            return pc + 4;
        }

        private static long SetInput(IDictionary<long, long> memory, long pc, long relativeBase, IList<long> input)
        {
            var code = new OpCode(GetValue(memory, pc));

            var index1 = pc + 1;
            var value1Imm = GetMemoryValue(memory, index1, code.Parameter1Mode, relativeBase);

            memory[value1Imm] = input[0];
            input.RemoveAt(0);
            return pc + 2;
        }

        private static long SetOutput(IDictionary<long, long> memory, long pc, long relativeBase, out long output)
        {
            var code = new OpCode(GetValue(memory, pc));

            var index1 = pc + 1;
            var value1Imm = GetMemoryValue(memory, index1, code.Parameter1Mode, relativeBase);

            output = value1Imm;
            return pc + 2;
        }

        private static long JumpIf(IDictionary<long, long> memory, long pc, long relativeBase, bool isNonZero)
        {
            var code = new OpCode(GetValue(memory, pc));

            var index1 = pc + 1;
            var value1 = GetMemoryValue(memory, index1, code.Parameter1Mode, relativeBase);

            if ((isNonZero && value1 != 0) || (!isNonZero && value1 == 0))
            {
                var index2 = pc + 2;
                var value2 = GetMemoryValue(memory, index2, code.Parameter2Mode, relativeBase);
                return value2;
            }

            return pc + 3;
        }

        private static long SetRelativeBase(IDictionary<long, long> memory, long pc, ref long relativeBase)
        {
            var index1 = pc + 1;
            var value1Imm = GetValue(memory, index1);

            relativeBase += value1Imm;
            return pc + 2;
        }

        private static long GetMemoryValue(IDictionary<long, long> memory, long pc, long mode, long relativeBase)
        {
            var positionValue = GetValue(memory, pc);

            return mode switch
            {
                0 => GetValue(memory, positionValue),
                1 => positionValue,
                2 => relativeBase + positionValue,
                _ => throw new NotImplementedException()
            };
        }

        private static long GetValue(IDictionary<long, long> memory, long index)
        {
            if (memory.TryGetValue(index, out var result))
            {
                return result;
            }

            memory.Add(index, 0);
            return 0;
        }

        private struct ProgramState
        {
            public IDictionary<long, long> Program { get; }
            public long ProgramCounter { get; }
            public long Output { get; }
            public long RelativeBase { get; }

            public ProgramState(IDictionary<long, long> program, long programCounter, long output, long relativeBase)
            {
                Program = program;
                ProgramCounter = programCounter;
                Output = output;
                RelativeBase = relativeBase;
            }
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