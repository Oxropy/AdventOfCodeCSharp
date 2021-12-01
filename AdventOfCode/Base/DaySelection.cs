using System;
using System.Diagnostics;
using System.Reflection;

namespace AdventOfCode.Base
{
    public class DaySelection
    {
        public bool StartDay()
        {
            var year = 0;
            var day = 0;
            var variant = 0;

            while (true)
            {
                if (year == 0)
                {
                    var yearResult = GetYearInput();
                    if (yearResult == 0) continue;
                    if (yearResult < 0) return false;
                    year = yearResult;
                }

                if (day == 0)
                {
                    var dayResult = GetDayInput();
                    if (dayResult == 0) continue;
                    if (dayResult == -3)
                    {
                        year = 0;
                        continue;
                    }
                    if (dayResult < 0) return false;
                    day = dayResult;
                }

                if (variant == 0)
                {
                    var variantResult = GetDayVariantInput();
                    if (variantResult == 0) continue;
                    if (variantResult == -3)
                    {
                        day = 0;
                        continue;
                    }
                    if (variantResult < 0) return false;
                    variant = variantResult;
                }
                

                var stopwatch = Stopwatch.StartNew();
                var result = GetDayResult(year, day, variant);
                stopwatch.Stop();
                Console.WriteLine($"Time {stopwatch.ElapsedMilliseconds} ms");
                
                if (result != null)
                {
                    Console.WriteLine(result);
                }
                return true;
            }
        }

        private static int GetInput()
        {
            var value = Console.ReadLine();
            // No value
            if (value == null) return -1;
            var valueLower = value.ToLower();
            // exit program
            if (valueLower == "exit") return -2;
            // back to last input
            if (valueLower == "back") return -3;

            if (int.TryParse(value, out var result))
            {
                return result;
            }

            // no number
            return 0;
        }

        private static int GetYearInput()
        {
            Console.Write("Year: ");
            var year = GetInput();
            if (year < 0) return year;
            if (year > 2015) return year;
            // not allowed value
            return 0;
        }

        private static int GetDayInput()
        {
            Console.Write("Day 1 to 25: ");
            var day = GetInput();
            if (day < 0) return day;
            if (day > 0 && day < 26) return day;
            // not allowed value
            return 0;
        }

        private static int GetDayVariantInput()
        {
            Console.Write("Variant 1 or 2: ");
            var variant = GetInput();
            if (variant < 0) return variant;
            if (variant > 0 && variant < 3) return variant;
            // not allowed value
            return 0;
        }

        private static string GetDayResult(int year, int day, int variant)
        {
            var dayType = Type.GetType($"AdventOfCode.Year{year}.Day{day}");
            if (dayType == null) return null;
            var dayInstance = (BaseDay)Activator.CreateInstance(dayType);
            return StartDay(dayInstance, variant);
        }
        
        private static string StartDay(BaseDay day, int variant)
        {
            if (variant == 1)
            {
                return day.StartVariant1();
            }

            return day.StartVariant2();
        }
    }
}