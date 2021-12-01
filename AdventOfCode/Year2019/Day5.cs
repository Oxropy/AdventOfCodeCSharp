using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Base;

namespace AdventOfCode.Year2019
{
    public class Day5 : BaseDay
    {
        public Day5() : base(2019, nameof(Day5))
        {
        }

        public override string StartVariant1()
        {
            return GetOutput(1).ToString();
        }

        public override string StartVariant2()
        {
            return GetOutput(5).ToString();
        }

        private int GetOutput(int input)
        {
            var program = Input.Split(",")
                .Select(int.Parse)
                .ToArray();

            var output = -1;

            // Program counter
            for (var pc = 0; pc < program.Length;)
            {
                var opCode = program[pc] % 100;
                pc = opCode switch
                {
                    1 => BinaryOperator(program, pc, (a, b) => a + b),
                    2 => BinaryOperator(program, pc, (a, b) => a * b),
                    3 => SetInput(program, pc, input),
                    4 => SetOutput(program, pc, out output),
                    5 => JumpIf(program, pc, true),
                    6 => JumpIf(program, pc, false),
                    7 => Comparison(program, pc, (a, b) => a < b),
                    8 => Comparison(program, pc, (a, b) => a == b),
                    99 => program.Length,
                    _ => throw new NotImplementedException()
                };
            }

            return output;
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

        private static int SetInput(IList<int> memory, int pc, int input)
        {
            var index1 = pc + 1;
            var value1Imm = memory[index1];

            memory[value1Imm] = input;
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
    }
}