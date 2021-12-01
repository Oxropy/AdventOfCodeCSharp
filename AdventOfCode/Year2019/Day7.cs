using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Base;

namespace AdventOfCode.Year2019
{
    public class Day7 : BaseDay
    {
        public Day7() : base(2019, nameof(Day7))
        {
        }

        public override string StartVariant1()
        {
            var sequenceValues = new List<int> {0, 1, 2, 3, 4};

            var highestValue = 0;
            foreach (var values in Permute(sequenceValues, sequenceValues.Count))
            {
                var lastResult = 0;
                for (int i = 0; i < sequenceValues.Count; i++)
                {
                    var value = values[i];
                    var input = new List<int> {value, lastResult};
                    lastResult = GetOutput(input).Output;
                    if (i == sequenceValues.Count - 1 && lastResult > highestValue)
                    {
                        highestValue = lastResult;
                    }
                }
            }

            return highestValue.ToString();
        }

        public override string StartVariant2()
        {
            var sequenceValues = new List<int> {5, 6, 7, 8, 9};

            var highestValue = 0;
            foreach (var values in Permute(sequenceValues, sequenceValues.Count))
            {
                var state1 = GetOutput(new List<int> {values[0], 0});
                var state2 = GetOutput(new List<int> {values[1], state1.Output});
                var state3 = GetOutput(new List<int> {values[2], state2.Output});
                var state4 = GetOutput(new List<int> {values[3], state3.Output});
                var state5 = GetOutput(new List<int> {values[4], state4.Output});

                while (state5.ProgramCounter < state5.Program.Length)
                {
                    state1 = GetOutput(state1.Program, state1.ProgramCounter, new List<int> {state5.Output});
                    state2 = GetOutput(state2.Program, state2.ProgramCounter, new List<int> {state1.Output});
                    state3 = GetOutput(state3.Program, state3.ProgramCounter, new List<int> {state2.Output});
                    state4 = GetOutput(state4.Program, state4.ProgramCounter, new List<int> {state3.Output});
                    state5 = GetOutput(state5.Program, state5.ProgramCounter, new List<int> {state4.Output});
                }

                if (state5.Output > highestValue)
                {
                    highestValue = state5.Output;
                }
            }

            return highestValue.ToString();
        }

        private ProgramState GetOutput(IList<int> input)
        {
            var program = Input.Split(",")
                .Select(int.Parse)
                .ToArray();

            return GetOutput(program, 0, input);
        }

        private static ProgramState GetOutput(int[] program, int programCounter, IList<int> input)
        {
            var output = -1;

            // Program counter
            for (; programCounter < program.Length;)
            {
                var opCode = program[programCounter] % 100;
                if (opCode == 3 && input.Count == 0)
                {
                    return new ProgramState(program, programCounter, output);
                }

                programCounter = opCode switch
                {
                    1 => BinaryOperator(program, programCounter, (a, b) => a + b),
                    2 => BinaryOperator(program, programCounter, (a, b) => a * b),
                    3 => SetInput(program, programCounter, input),
                    4 => SetOutput(program, programCounter, out output),
                    5 => JumpIf(program, programCounter, true),
                    6 => JumpIf(program, programCounter, false),
                    7 => Comparison(program, programCounter, (a, b) => a < b),
                    8 => Comparison(program, programCounter, (a, b) => a == b),
                    99 => program.Length,
                    _ => throw new NotImplementedException()
                };
            }

            return new ProgramState(program, programCounter, output);
        }

        private static int JumpIf(IReadOnlyList<int> memory, int pc, bool isNonZero)
        {
            var code = memory[pc] / 100;
            var parameter1Mode = code % 10;
            var parameter2Mode = code / 10 % 10;

            var index1 = pc + 1;
            var value1 = GetValue(memory, index1, parameter1Mode);

            if ((isNonZero && value1 != 0) || (!isNonZero && value1 == 0))
            {
                var index2 = pc + 2;
                var value2 = GetValue(memory, index2, parameter2Mode);
                return value2;
            }

            return pc + 3;
        }

        private static int Comparison(int[] memory, int pc, Func<int, int, bool> comp)
        {
            return BinaryOperator(memory, pc, (a, b) => comp(a, b) ? 1 : 0);
        }

        private static int BinaryOperator(int[] memory, int pc, Func<int, int, int> op)
        {
            var code = memory[pc] / 100;
            var parameter1Mode = code % 10;
            var parameter2Mode = code / 10 % 10;

            var index1 = pc + 1;
            var value1 = GetValue(memory, index1, parameter1Mode);
            var index2 = pc + 2;
            var value2 = GetValue(memory, index2, parameter2Mode);
            var index3 = pc + 3;
            var value3Imm = memory[index3];

            memory[value3Imm] = op(value1, value2);
            return pc + 4;
        }

        private static int SetInput(IList<int> memory, int pc, IList<int> input)
        {
            var index1 = pc + 1;
            var value1Imm = memory[index1];

            memory[value1Imm] = input[0];
            input.RemoveAt(0);
            return pc + 2;
        }

        private static int SetOutput(IReadOnlyList<int> memory, int pc, out int output)
        {
            var index1 = pc + 1;
            var value1Imm = memory[memory[index1]];

            output = value1Imm;
            return pc + 2;
        }

        private static int GetValue(IReadOnlyList<int> memory, int pc, int mode)
        {
            var positionValue = memory[pc];
            if (mode == 1)
            {
                return positionValue;
            }

            return memory[positionValue];
        }

        private static IEnumerable<IList<T>> Permute<T>(IList<T> sequence, int count)
        {
            if (count == 1)
            {
                yield return sequence;
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    foreach (var perm in Permute(sequence, count - 1))
                    {
                        yield return perm;
                    }

                    RotateRight(sequence, count);
                }
            }
        }

        private static void RotateRight<T>(IList<T> sequence, int count)
        {
            var index = count - 1;
            var tmp = sequence[index];
            sequence.RemoveAt(index);
            sequence.Insert(0, tmp);
        }

        private struct ProgramState
        {
            public int[] Program { get; }
            public int ProgramCounter { get; }
            public int Output { get; }

            public ProgramState(int[] program, int programCounter, int output)
            {
                Program = program;
                ProgramCounter = programCounter;
                Output = output;
            }
        }
    }
}