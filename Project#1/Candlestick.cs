using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

public class Candlestick
{
    
	public DateTime Date { get; set; }
	public decimal Open {  get; set; }
	public decimal High { get; set; }
	public decimal Low { get; set; }
	public decimal Close { get; set; }
	public ulong Volume { get; set; }


	public Candlestick(DateTime date, decimal open, decimal high, decimal low, decimal close, ulong volume)
	{
		Date = date;
		Open = open;
		High = high;
		Low = low;
		Close = close;
		Volume = volume;

	}

    public override string ToString()
    {
        return base.ToString();
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
