using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Project_1
{
    public class SmartCandlestick : Candlestick
    {
        private bool _isPeak = false;
        private bool _isValley = false;

        public decimal Range { get { return High - Low; } }
        public decimal BodyRange { get { return Math.Abs(Close - Open); } }
        public decimal TopPrice { get { return Math.Max(Open, Close); } }
        public decimal BottomPrice { get { return Math.Min(Open, Close); } }
        public decimal UpperTail { get { return High - TopPrice; } }
        public decimal LowerTail { get { return BottomPrice - Low; } }
        public bool IsPeak { get { return _isPeak; } set { _isPeak = value; } }
        public bool IsValley { get { return _isValley; } set { _isValley = value; } }




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


        /// <summary>
        /// 
        /// If candle length is greater than previous, and the direction multiplied by previous direction is down, and the candle direction equals the direction we pass in, and if the volume is greater than the previous, calculate the form.
        /// Depending on the value of direction, it will return whether it is a valley or peak.
        /// </summary>
        /// <param name="candlesticks"></param>
        /// <param name="index"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private bool calculateForm(BindingList<SmartCandlestick> candlesticks, int index, int direction)
        {
            if (direction != -1 && direction != 1)
            {
                throw new ArgumentException("Direction must be -1 (peak) or 1 (valley).");
            }

            var previous = candlesticks[index - 1];

            // Candle direction, 1 means up, -1 means down.
            var candleDirection = this.Close > this.Open ? 1 : -1;
            var previousCandleDirection = previous.Close > previous.Open ? 1 : -1;
            var candlelen = this.BodyRange;
            var previousCandlelen = previous.BodyRange;

            return candlelen > previousCandlelen && candleDirection * previousCandleDirection == -1 && candleDirection == direction && this.Volume < previous.Volume;
        }

        public static void CalculatePeaksAndValleys(BindingList<SmartCandlestick> candlesticks, Action<SmartCandlestick> onPeak, Action<SmartCandlestick> onValley)

        {
            for (int i = 0; i < candlesticks.Count; i++)
            {
                if (isIndexPeak(candlesticks, i))
                {
                    candlesticks[i].IsPeak = true;
                    onPeak(candlesticks[i]);
                }
                else if (isIndexValley(candlesticks, i))
                {
                    candlesticks[i].IsValley = true;
                    onValley(candlesticks[i]);
                }
            }
        }
        private static bool isIndexPeak(BindingList<SmartCandlestick> candlesticks, int index)
        {
            if (index > 0 && index < candlesticks.Count - 1)
            {

                return candlesticks[index].calculateForm(candlesticks, index, -1);
            }
            return false;

        }

        private static bool isIndexValley(BindingList<SmartCandlestick> candlesticks, int index)
        {
            if (index > 0 && index < candlesticks.Count - 1)
            {
                return candlesticks[index].calculateForm(candlesticks, index, 1);
            }
            return false;
        }



        // Loads a list of candlesticks from a CSV file
        public static List<SmartCandlestick> LoadStockDataFromCSV(string filePath)
        {
            List<SmartCandlestick> stockData = new List<SmartCandlestick>();

            try
            {
                // Use StreamReader with UTF8 encoding
                using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
                {
                    var lines = reader.ReadToEnd().Split('\n');

                    // Check if the first line is a header
                    bool isFirstLine = true;

                    foreach (var line in lines)
                    {
                        // Skip empty lines
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        // Skip the header line if present
                        if (isFirstLine)
                        {
                            isFirstLine = false;
                            if (line.Contains("Date") || line.Contains("date")) continue; // Adjust for your specific header
                        }

                        var fields = line.Split(',');

                        // Trim each field to remove leading/trailing spaces and quotes
                        fields = fields.Select(field => field.Trim(' ', '"')).ToArray();

                        // Check if the fields array has the expected number of elements
                        if (fields.Length < 6)
                        {
                            MessageBox.Show($"Incomplete data in line: {line}");
                            continue; // Skip this line
                        }

                        // Attempt to parse the date field
                        if (!DateTime.TryParseExact(fields[0], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                        {
                            // Show detailed error message for the problematic line
                            MessageBox.Show($"Invalid date format in line: {line}\nField[0]: {fields[0]}");
                            continue; // Skip this line
                        }

                        try
                        {
                            // Parse OHLC and volume values
                            decimal open = decimal.Parse(fields[1]);
                            decimal high = decimal.Parse(fields[2]);
                            decimal low = decimal.Parse(fields[3]);
                            decimal close = decimal.Parse(fields[4]);
                            ulong volume = ulong.Parse(fields[5]);

                            // Add a new aCandlestick object to the list
                            stockData.Add(new SmartCandlestick(date, open, high, low, close, volume));
                        }
                        catch (Exception ex)
                        {
                            // Show error for the problematic line
                            MessageBox.Show($"Parsing error in line: {line}\nError: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading file: {ex.Message}");
            }

            return stockData;
        }
    }
}
