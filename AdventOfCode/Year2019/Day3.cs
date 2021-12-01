using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using AdventOfCode.Base;

namespace AdventOfCode.Year2019
{
    public class Day3 : BaseDay
    {
        public Day3() : base(2019, nameof(Day3))
        {
        }

        public override string StartVariant1()
        {
            var lines = Input.Split("\r\n");

            var path1 = GetPositions(lines[0]);
            var path2 = GetPositions(lines[1]);

            var next = path1.Intersect(path2).Select(p => new Vector2(Math.Abs(p.X), Math.Abs(p.Y))).OrderBy(p => p.X + p.Y).First();

            return (next.X + next.Y).ToString(CultureInfo.InvariantCulture);
        }

        public override string StartVariant2()
        {
            var lines = Input.Split("\r\n");

            var path1 = GetPositions(lines[0]);
            var path2 = GetPositions(lines[1]);

            var crossPosition = path1.Intersect(path2);
            var steps = new List<int>();
            foreach (var position in crossPosition)
            {
                var index1 = path1.Select((p, i) => (p, i)).First(p => p.p.Equals(position)).i;
                var index2 = path2.Select((p, i) => (p, i)).First(p => p.p.Equals(position)).i;
                steps.Add(index1 + index2);
            }

            return (steps.Min() + 2).ToString();
        }

        private static IEnumerable<Vector2> GetPositions(string line)
        {
            var path = new List<Vector2>();
            var operations = line.Split(",");
            foreach (var operation in operations)
            {
                var direction = operation.Substring(0, 1);
                var length =  int.Parse(operation.Substring(1));

                var lastPosition = path.Count == 0 ? new Vector2(0, 0) : path[^1];
                var startX = lastPosition.X;
                var startY = lastPosition.Y;
                     
                for (var i = 0; i < length; i++)
                {
                    switch (direction)
                    {
                        case "U":
                            startY++;
                            break;
                        case "R":
                            startX++;
                            break;
                        case "D":
                            startY--;
                            break;
                        case "L":
                            startX--;
                            break;
                    }
                    path.Add(new Vector2(startX, startY));
                }
            }

            return path;
        }
    }
}