using System;
using AdventOfCode.Base;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            if (new DaySelection().StartDay())
            {
                Console.ReadKey();    
            }
            Console.WriteLine("Exit program");
        }
    }
}