using System;
using System.Globalization;
using System.Linq;
using AdventOfCode.Base;

namespace AdventOfCode.Year2019
{
    public class Day1 : BaseDay
    {
        public Day1() : base(2019, nameof(Day1))
        {
        }

        public override string StartVariant1()
        {
            var lines = Input.Split("\r\n");
            var sum = lines.Select(long.Parse).Select(value => Math.Floor(value / 3.0) - 2).Sum();

            return sum.ToString(CultureInfo.CurrentCulture);
        }

        public override string StartVariant2()
        {
            var lines = Input.Split("\r\n");

            double sum = 0;
            foreach (var line in lines)
            {
                double internalSum = 0;
                var value = long.Parse(line);
                double rest = value;
                while (rest > 0)
                {
                    rest = Math.Floor(rest / 3.0) - 2;
                    if (rest > 0)
                    {
                        internalSum += rest;
                    }
                }

                sum += internalSum;
            }

            return sum.ToString(CultureInfo.CurrentCulture);
        }
    }
}