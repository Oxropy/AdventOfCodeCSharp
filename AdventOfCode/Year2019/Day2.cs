using System.Linq;
using AdventOfCode.Base;

namespace AdventOfCode.Year2019
{
    public class Day2 : BaseDay
    {
        public Day2() : base(2019, nameof(Day2))
        {
        }

        public override string StartVariant1()
        {
            return GetOutputString(12, 2).ToString();
        }

        public override string StartVariant2()
        {
            for (var noun = 0; noun < 100; noun++)
            {
                for (var verb = 0; verb < 100; verb++)
                {
                    var output = GetOutput(noun, verb);
                    if (output == 19690720)
                    {
                        return noun + verb.ToString("00");
                    }
                }
            }

            return null;
        }

        private int GetOutput(int noun, int verb)
        {
            var lines = Input.Split(",").Select(int.Parse).ToArray();
            
            lines[1] = noun;
            lines[2] = verb;

            for (var position0 = 0; position0 < lines.Length; position0 += 4)
            {
                var opCode = lines[position0];
                if (opCode == 99) break;

                var position1 = position0 + 1;
                var position2 = position0 + 2;
                var position3 = position0 + 3;
                var index1 = lines[position1];
                var index2 = lines[position2];
                var index3 = lines[position3];
                var value1 = lines[index1];
                var value2 = lines[index2];

                lines[index3] = opCode switch
                {
                    1 => value1 + value2,
                    2 => value1 * value2,
                    _ => throw new System.NotImplementedException()
                };
            }

            return lines[0];
        }
        
        private int GetOutputString(int noun, int verb)
        {
            var lines = Input.Split(",");
            
            lines[1] = noun.ToString();
            lines[2] = verb.ToString();

            for (var position0 = 0; position0 < lines.Length; position0 += 4)
            {
                var opCodeIndex = lines[position0];
                var opCode = int.Parse(opCodeIndex);
                
                if (opCode == 99) break;

                var position1 = position0 + 1;
                var position2 = position0 + 2;
                var position3 = position0 + 3;
                var index1 = int.Parse(lines[position1]);
                var index2 = int.Parse(lines[position2]);
                var index3 = int.Parse(lines[position3]);
                var value1 = int.Parse(lines[index1]);
                var value2 = int.Parse(lines[index2]);

                lines[index3] = opCode switch
                {
                    1 => (value1 + value2).ToString(),
                    2 => (value1 * value2).ToString(),
                    _ => throw new System.NotImplementedException()
                };
            }

            return int.Parse(lines[0]);
        }
    }
}