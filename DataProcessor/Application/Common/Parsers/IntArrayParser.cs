using DataProcessor.Application.Common.Parsers;
using DataProcessor.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessor.Application.Parsers
{
    internal class IntArrayParser : ArrayParser<int>
    {
        protected override int ParseElement(string element)
        {
            try 
            { 
                return int.Parse(element);
            }
            catch 
            {
                throw;
            }
        }
    }
}
