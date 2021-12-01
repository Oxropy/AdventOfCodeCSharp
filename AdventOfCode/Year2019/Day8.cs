using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Base;

namespace AdventOfCode.Year2019
{
    public class Day8 : BaseDay
    {
        public Day8() : base(2019, nameof(Day8))
        {
        }

        public override string StartVariant1()
        {
            const int width = 25;
            const int height = 6;
            const int pixels = width * height;
            
            var layer = SplitChunk(Input, pixels);

            return layer.OrderBy(l => l.Count(c => c == '0'))
                .Select(l => l.Count(c => c == '1') * l.Count(c => c == '2'))
                .First()
                .ToString();
        }

        public override string StartVariant2()
        {
            const int width = 25;
            const int height = 6;
            const int pixels = width * height;
            
            var layer = SplitChunk(Input, pixels);
            
            var result = new char[pixels];
            for (int i = 0; i < pixels; i++)
            {
                result[i] = layer.First(l => l[i] != '2')[i];
            }

            var sb = new StringBuilder();
            foreach (var line in SplitChunk(new string(result.Select(c => c == '0' ? ' ' : 'X').ToArray()), width))
            {
                sb.AppendLine(line);
            }

            return sb.ToString();
        }

        private static IEnumerable<string> SplitChunk(string input, int chunkSize)
        {
            return Enumerable.Range(0, input.Length / chunkSize)
                .Select(i => input.Substring(i * chunkSize, chunkSize));
        }
    }
}