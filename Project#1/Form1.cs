using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace Project_1
{
    public partial class Form_StockViewer : Form
    {

        private BindingList<Candlestick> boundCandlesticks = null;
        private List<Candlestick> listOfCandlesticks = null;

        public Form_StockViewer()
        {
            InitializeComponent();            
        }

        public Form_StockViewer(String filename, DateTime startDate, DateTime endDate)
        {
            InitializeComponent();
            dateTime_start.Value = startDate;
            dateTime_end.Value = endDate;

            listOfCandlesticks = Candlestick.LoadStockDataFromCSV(filename);
            filterCandlesticks();
            displayCandlesticks();

        }

        /// <summary>
        ///  Normalizes the stock data by adjusting the Y-axis range and interval.
        ///  This ensures that the chart displays a visually appealing and meaningful range of prices.
        /// </summary>
        /// <param name="stockData"></param>
        /// <returns>A normalized list of candlesticks</returns>
        private void filterCandlesticks()
        {
            if (listOfCandlesticks.Count == 0) return;

            // Get start and end dates from the DateTimePickers
            DateTime startDate = dateTime_start.Value;
            DateTime endDate = dateTime_end.Value;

            // Filter data by selected date range
             boundCandlesticks = new BindingList<Candlestick>(listOfCandlesticks
                .Where(item => item.Date >= startDate && item.Date <= endDate)
                .ToList());

            // Check if data is available for the selected date range
            if (boundCandlesticks.Count == 0)
            {
                MessageBox.Show("No data available for the selected date range.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Normalize the chart display with the info from our candlesticks
            decimal minPrice = Math.Floor((decimal)0.98 * boundCandlesticks.Min(c => c.Low));
            decimal maxPrice = Math.Ceiling((decimal)1.02 * boundCandlesticks.Max(c => c.High));
            double interval = CalculateInterval(minPrice, maxPrice);

            candlestickChart.ChartAreas["ChartAreaCandlestick"].AxisY.Minimum = (double)minPrice;
            candlestickChart.ChartAreas["ChartAreaCandlestick"].AxisY.Maximum = (double)maxPrice;
            candlestickChart.ChartAreas["ChartAreaCandlestick"].AxisY.Interval = interval;

        }

        /// <summary>
        ///  Calculates the interval for the Y-axis based on the given data range.
        /// </summary>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice">asdad</param>
        /// <returns></returns>
        private double CalculateInterval(decimal minPrice, decimal maxPrice)
        {
            // Determine a reasonable interval based on the data range
            decimal range = maxPrice - minPrice;
            if (range <= 1) return 0.1;        // Small range, finer interval
            if (range <= 10) return 1;         // Moderate range
            if (range <= 100) return 10;       // Larger range
            return 50;                         // Very large range
        }

        /// <summary>
        /// Displays the loaded data in the chart.
        /// </summary>
        private void displayCandlesticks()
        {
            candlestickChart.DataSource = boundCandlesticks;
            // Clear existing chart data
            candlestickChart.Series["SeriesCandlestick"].Points.Clear();
            candlestickChart.Series["SeriesVolume"].Points.Clear();

            foreach (var item in boundCandlesticks)
            {
                // Add the candlestick data to the SeriesCandlestick
                int pointIndex = candlestickChart.Series["SeriesCandlestick"].Points.AddXY(item.Date, item.High, item.Low, item.Open, item.Close);
                candlestickChart.Series["SeriesCandlestick"].Points[pointIndex].Color = item.Close > item.Open ? System.Drawing.Color.LawnGreen : System.Drawing.Color.Red;

                // Add the volume data to the SeriesVolume
                candlestickChart.Series["SeriesVolume"].Points.AddXY(item.Date, item.Volume);
            }
        }

        // Event handler for start date picker change
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime startDate = dateTime_start.Value;
            MessageBox.Show($"Start date changed to: {startDate.ToShortDateString()}");
        }

        // Event handler for end date picker change
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            DateTime endDate = dateTime_end.Value;
            MessageBox.Show($"End date changed to: {endDate.ToShortDateString()}");
        }

        // Event handler for Load Stock button
        private void button_loadData(object sender, EventArgs e)
        {

            // Open file dialog to select a CSV file
            if (openFileDialog_stock.ShowDialog() == DialogResult.OK)
            {

                var fileNames = openFileDialog_stock.FileNames;

                try
                {
                    void displayOne(String filePath)
                    {
                        listOfCandlesticks = Candlestick.LoadStockDataFromCSV(filePath);
                        filterCandlesticks();
                        displayCandlesticks();
                    }

                    if (fileNames.Length == 1)
                    {
                        displayOne(fileNames[0]);
                    } else if (fileNames.Length > 1)
                    {
                        displayOne(fileNames[0]);
                        foreach (var filePath in fileNames.Skip(1)) {
                            var childForm = new Form_StockViewer(filePath, dateTime_start.Value, dateTime_end.Value);
                            childForm.Show();
                        }
                    } else
                    {
                        MessageBox.Show("No files selected");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void update_StockData(object sender, EventArgs e)
        {


        }
    }
}
