using DataProcessor.Application.Interfaces;

namespace DataProcessor.Presentation
{
    /// <summary>
    /// Output writer implementation for console output
    /// </summary>
    internal class ConsoleWriter : IOutputWriter
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }
}
