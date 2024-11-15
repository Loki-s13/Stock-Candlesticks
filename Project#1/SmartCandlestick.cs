﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        // Loads a list of candlesticks from a CSV file
        public static List<Candlestick> LoadStockDataFromCSV(string filePath)
        {
            List<Candlestick> stockData = new List<Candlestick>();

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
                            stockData.Add(new Candlestick(date, open, high, low, close, volume));
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
