using System;
using System.IO;

namespace AdventOfCode.Base
{
    public abstract class BaseDay
    {
        protected readonly string Input;

        public bool HasInput
        {
            get { return Input != null; }
        }
        
        protected BaseDay(int year, string name)
        {
            Input = GetFile(year, name);
        }

        public abstract string StartVariant1();
        public abstract string StartVariant2();
        
        private static string GetFile(int year, string name)
        {
            var path = Path.Combine(Environment.CurrentDirectory, $"..\\..\\..\\Year{year}\\Input\\{name}.txt");
            return File.Exists(path) ? File.ReadAllText(path) : null;
        }        
    }
}