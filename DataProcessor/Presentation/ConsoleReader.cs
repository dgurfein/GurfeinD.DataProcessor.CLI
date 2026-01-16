using DataProcessor.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessor.Presentation
{
    /// <summary>
    /// Input provider implementation for console input
    /// </summary>
    internal class ConsoleReader : IInputProvider
    {
        public string Read(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine() ?? string.Empty;
        }
    }
}
