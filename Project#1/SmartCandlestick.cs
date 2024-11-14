using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_1
{
    public class SmartCandlestick : Candlestick
    {
        public decimal Range { get { return High - Low; } }
        public decimal BodyRange { get { return Math.Abs(Close - Open); } }
        public decimal TopPrice { get { return Math.Max(Open, Close); } }
        public decimal BottomPrice { get { return Math.Min(Open, Close); } }
        public decimal UpperTail { get { return High - TopPrice; } }
        public decimal LowerTail { get { return BottomPrice - Low; } }





        public SmartCandlestick(DateTime date, decimal open, decimal high, decimal low, decimal close, ulong volume)
            : base(date, open, high, low, close, volume) { }


        public bool IsBullish => Close > Open;

        public bool IsBearish => Close < Open;

        public bool IsNeutral => Close == Open;

        public bool IsMarubozu => UpperTail == 0 && LowerTail == 0;

        public bool IsHammer => LowerTail > (2 * BodyRange) && UpperTail < BodyRange;

        public bool IsDoji => BodyRange < 0.1m * Range;

        public bool IsDragonflyDoji => IsDoji && UpperTail == 0 && LowerTail > 0;

        public bool IsGravestoneDoji => IsDoji && LowerTail == 0 && UpperTail > 0;

        /// <summary>
        /// Returns a string representing the type of pattern the candlestick represents.
        /// </summary>
        /// <returns></returns>
        public string GetPatternType()
        {
            if (IsMarubozu)
                return "Marubozu";
            if (IsHammer)
                return "Hammer";
            if (IsDragonflyDoji)
                return "Dragonfly Doji";
            if (IsGravestoneDoji)
                return "Gravestone Doji";
            if (IsDoji)
                return "Doji";
            if (IsBullish)
                return "Bullish";
            if (IsBearish)
                return "Bearish";
            return "Neutral";
        }




        public bool IsPeak(List<Candlestick> candlesticks, int index)
        {
            if (index > 0 && index < candlesticks.Count - 1)
            {
                return this.High > candlesticks[index - 1].High && this.High > candlesticks[index + 1].High;
            }
            return false;

        }
        public bool IsValley(List<Candlestick> candlesticks, int index)
        {
            if (index > 0 && index < candlesticks.Count - 1)
            {
                return this.Low < candlesticks[index - 1].Low && this.Low < candlesticks[index + 1].Low;
            }
            return false;
        }
    }
}
