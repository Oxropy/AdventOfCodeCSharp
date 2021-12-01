using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using AdventOfCode.Base;

namespace AdventOfCode.Year2019
{
    public class Day6 : BaseDay
    {
        public Day6() : base(2019, nameof(Day6))
        {
        }

        public override string StartVariant1()
        {
            var lines = Input.Split("\r\n");
            var orbits =  GetConnections(lines);
            var roots = GetRoots(orbits);

            return roots.Sum(uniqueValue => GetOrbitNumber(orbits, uniqueValue, 0)).ToString();
        }

        public override string StartVariant2()
        {
            var lines = Input.Split("\r\n");
            var orbits =  GetConnections(lines);
            const string root = "COM";

            return GetTransfers(orbits, root, 0).ToString();
        }

        private static OrbitConnection GetConnections(string[] input)
        {
            var orbits = new OrbitConnection();

            foreach (var line in input)
            {
                var values = line.Split(")");

                var orbit = values[0];
                var direct = values[1];

                if (orbits.ContainsKey(orbit))
                {
                    var directOrbits = orbits[orbit];
                    directOrbits.Add(direct);
                }
                else
                {
                    orbits.Add(orbit, new List<string>() {direct});
                }
            }

            return orbits;
        }

        private static IEnumerable<string> GetRoots(OrbitConnection orbits)
        {
            // Find roots
            var orbitKeys = orbits.Keys.ToList();
            var orbitValues = orbits.Values.SelectMany(o => o);
            var roots = orbitKeys.Where(o => !orbitValues.Contains(o));
            return roots;
        }

        private static int GetOrbitNumber(OrbitConnection connection, string orbit, int successorCount)
        {
            if (connection.ContainsKey(orbit))
            {
                successorCount += connection[orbit].Sum(orb => GetOrbitNumber(connection, orb, successorCount + 1));
            }

            return successorCount;
        }

        private static int GetTransfers(OrbitConnection connection, string orbit, int successorCount)
        {
            if (connection.ContainsKey(orbit))
            {
                var lengths = connection[orbit].Select(d => GetTransfers(connection, d, successorCount + 1)).Where(n => n != -1).ToArray();
                switch (lengths.Length)
                {
                    case 2:
                        return lengths.Sum(v => v - successorCount);
                    case 1:
                        return lengths.First();
                }
            }

            if (orbit == "YOU" || orbit == "SAN")
            {
                return successorCount - 1;
            }
            return -1;
        }
        
        private class OrbitConnection : Dictionary<string, List<string>>
        {
        }
    }
}