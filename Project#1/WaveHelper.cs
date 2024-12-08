using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_1
{
    internal class WaveHelper
    {
        public static List<decimal> CalculateFibonacciLevels(decimal high, decimal low)
        {
            return new List<decimal>
        {
            low,
            low + (high - low) * 0.236m,
            low + (high - low) * 0.382m,
            low + (high - low) * 0.5m,
            low + (high - low) * 0.618m,
            high
        };
        }
    }
}
