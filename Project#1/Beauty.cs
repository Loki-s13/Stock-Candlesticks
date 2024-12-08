using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_1
{
    internal class Beauty
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

        /// <summary>
        /// Computes the beauty (confirmations) of a single candlestick based on Fibonacci levels.
        /// </summary>
        public static int CalculateBeauty(SmartCandlestick candlestick, List<decimal> fibLevels, decimal leeway)
        {
            int confirmations = 0;

            foreach (var level in fibLevels)
            {
                if (Math.Abs(candlestick.High - level) <= leeway ||
                    Math.Abs(candlestick.Low - level) <= leeway ||
                    Math.Abs(candlestick.Open - level) <= leeway ||
                    Math.Abs(candlestick.Close - level) <= leeway)
                {
                    confirmations++;
                }
            }

            return confirmations;
        }



        /// <summary>
        /// Computes the total beauty of a wave by summing up the beauty of all candlesticks in the wave.
        /// </summary>
        public static int ComputeWaveBeauty(List<SmartCandlestick> waveCandlesticks, List<decimal> fibLevels, decimal leeway)
        {
            int totalBeauty = 0;

            foreach (var candlestick in waveCandlesticks)
            {
                totalBeauty += CalculateBeauty(candlestick, fibLevels, leeway);
            }

            return totalBeauty;
        }
    }
}
