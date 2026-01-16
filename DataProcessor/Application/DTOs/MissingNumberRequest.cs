using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessor.Application.DTOs
{
    /// <summary>
    /// Data transfor object representing the missing number use case request structure
    /// </summary>
    public class MissingNumberRequest
    {
        public int[] NumberArray { get; } = [];
        public MissingNumberRequest(int[] numbers)
        {
            NumberArray = numbers;
        }
    }
}
