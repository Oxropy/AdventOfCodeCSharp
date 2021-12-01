using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Base;

namespace AdventOfCode.Year2019
{
    public class Day4 : BaseDay
    {
        public Day4() : base(2019, nameof(Day4))
        {
        }

        public override string StartVariant1()
        {
            var values = Input.Split("-");
            var min = int.Parse(values[0]);
            var max = int.Parse(values[1]);

            var count = 0;
            for (var i = min; i < max; i++)
            {
                var val6 = i % 10;
                var val5 = (i - val6) % 100 / 10;
                var val4 = (i - val5 - val6) % 1000 / 100;
                var val3 = (i - val4 - val5 - val6) % 10000 / 1000;
                var val2 = (i - val3 - val4 - val5 - val6) % 100000 / 10000;
                var val1 = i / 100000;

                if (val1 <= val2 && val2 <= val3 && val3 <= val4 && val4 <= val5 && val5 <= val6
                    && (val1 == val2 || val2 == val3 || val3 == val4 || val4 == val5 || val5 == val6))
                {
                    count++;
                }
            }

            return count.ToString();
        }

        public override string StartVariant2()
        {
            var values = Input.Split("-");
            var min = int.Parse(values[0]);
            var max = int.Parse(values[1]);

            var count = 0;
            for (var i = min; i < max; i++)
            {
                var val6 = i % 10;
                var val5 = (i - val6) % 100 / 10;
                var val4 = (i - val5 - val6) % 1000 / 100;
                var val3 = (i - val4 - val5 - val6) % 10000 / 1000;
                var val2 = (i - val3 - val4 - val5 - val6) % 100000 / 10000;
                var val1 = i / 100000;

                var val = new List<int>
                {
                    val1,
                    val2,
                    val3,
                    val4,
                    val5,
                    val6
                };
                
                if (val1 <= val2 && val2 <= val3 && val3 <= val4 && val4 <= val5 && val5 <= val6
                    && (val1 == val2 || val2 == val3 || val3 == val4 || val4 == val5 || val5 == val6)
                    && val.GroupBy(v => v).Select(v => v.Count()).Contains(2))
                {
                    count++;
                }
            }

            return count.ToString();
        }
    }
}