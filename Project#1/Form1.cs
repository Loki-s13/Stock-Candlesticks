using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Project_1
{
    public partial class Form_StockViewer : Form
    {

        private BindingList<SmartCandlestick> boundCandlesticks = null;
        private List<SmartCandlestick> listOfCandlesticks = null;
        private SmartCandlestick firstSelection = null;
        private SmartCandlestick secondSelection = null;

        public Form_StockViewer()
        {
            InitializeComponent();


        }

        public Form_StockViewer(String filename, DateTime startDate, DateTime endDate)
        {
            InitializeComponent();
            numericUpDown_leeway.Value = 0.5m;


            dateTime_start.Value = startDate;
            dateTime_end.Value = endDate;

            listOfCandlesticks = SmartCandlestick.LoadStockDataFromCSV(filename);
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
            boundCandlesticks = new BindingList<SmartCandlestick>(listOfCandlesticks
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
        /// Displays the loaded data in the chart. Adds annotations for peak or valley candlesticks
        /// </summary>
        private void displayCandlesticks()
        {
            candlestickChart.DataSource = boundCandlesticks;
            candlestickChart.Annotations.Clear();

            for (int i = 0; i < boundCandlesticks.Count; i++)
            {

                var item = boundCandlesticks[i];
                // Add the candlestick data to the SeriesCandlestick
                int pointIndex = candlestickChart.Series["SeriesCandlestick"].Points.AddXY(item.Date, item.High, item.Low, item.Open, item.Close);
                candlestickChart.Series["SeriesCandlestick"].Points[pointIndex].Color = item.Close > item.Open ? System.Drawing.Color.LawnGreen : System.Drawing.Color.Red;

                // Add the volume data to the SeriesVolume
                candlestickChart.Series["SeriesVolume"].Points.AddXY(item.Date, item.Volume);

                SmartCandlestick.CalculatePeaksAndValleys(boundCandlesticks, onPeak: (candlestick) =>
                {
                    AddPeakAnnotation(candlestick);
                }, onValley: (candlestick) =>
                {
                    AddValleyAnnotation(candlestick);
                });

            }
        }


        /// <summary>
        /// Adds an annotation and a horizontal line for a peak
        /// </summary>
        /// <param name="date"></param>
        /// <param name="high"></param>
        private void AddPeakAnnotation(SmartCandlestick candlestick)
        {
            var high = candlestick.High;
            // Draw a green horizontal line across the chart at the peak level
            var peakLine = new HorizontalLineAnnotation
            {
                Name = $"PeakLine_{Guid.NewGuid()}",
                AxisY = candlestickChart.ChartAreas["ChartAreaCandlestick"].AxisY,
                ClipToChartArea = "ChartAreaCandlestick",
                IsInfinitive = true,
                Y = (double)high,
                LineColor = Color.Green,
                LineDashStyle = ChartDashStyle.Dash
            };
            candlestickChart.Annotations.Add(peakLine);


        }


        /// <summary>
        /// Adds annotation and a horizontal line for a peak
        /// </summary>

        /// <param name="date"></param>
        /// <param name="low"></param>
        private void AddValleyAnnotation(SmartCandlestick candlestick)
        {
            var low = candlestick.Low;
            var valleyLine = new HorizontalLineAnnotation
            {
                Name = $"ValleyLine_{Guid.NewGuid()}",
                AxisY = candlestickChart.ChartAreas["ChartAreaCandlestick"].AxisY,
                ClipToChartArea = "ChartAreaCandlestick",
                IsInfinitive = true,
                Y = (double)low,
                LineColor = Color.Red,
                LineDashStyle = ChartDashStyle.Dash
            };
            candlestickChart.Annotations.Add(valleyLine);


        }


        // Event handler for start date picker change
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime startDate = dateTime_start.Value;

        }

        // Event handler for end date picker change
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            DateTime endDate = dateTime_end.Value;
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
                        listOfCandlesticks = SmartCandlestick.LoadStockDataFromCSV(filePath);
                        filterCandlesticks();
                        displayCandlesticks();
                        this.Text = $"Stock Viewer - {System.IO.Path.GetFileName(filePath)}";
                    }

                    if (fileNames.Length == 1)
                    {
                        displayOne(fileNames[0]);
                    }
                    else if (fileNames.Length > 1)
                    {
                        displayOne(fileNames[0]);
                        foreach (var filePath in fileNames.Skip(1))
                        {
                            var childForm = new Form_StockViewer(filePath, dateTime_start.Value, dateTime_end.Value);
                            childForm.Show();
                        }
                    }
                    else
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
            filterCandlesticks();
            displayCandlesticks();
        }

        private void candlestickChart_Click(object sender, EventArgs e)
        {
        }


        /// <summary>
        /// Funtion to handle mouse click on the candlestick to assign first candlestick and second
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void candlestickChart_MouseClick(object sender, MouseEventArgs e)
        {
            var hit = candlestickChart.HitTest(e.X, e.Y);

            //cheks if user candlestick is valid
            if (hit.ChartElementType == ChartElementType.DataPoint)
            {
                var selectedPointIndex = hit.PointIndex;
                var selectedCandle = boundCandlesticks[selectedPointIndex];

                // Handle the first selection
                if (firstSelection == null)
                {
                    if (selectedCandle.IsPeak || selectedCandle.IsValley)
                    {
                        firstSelection = selectedCandle;
                        MessageBox.Show($"First candlestick selected: {firstSelection.Date.ToShortDateString()}");
                    }
                    else
                    {
                        MessageBox.Show("Please select a Peak or a Valley candlestick");
                    }


                }
                // Handle the second selection
                else if (firstSelection != null && secondSelection == null)
                {
                    secondSelection = selectedCandle;

                    // Validate and compute Fibonacci levels
                    if (ValidateWave(firstSelection, secondSelection))
                    {
                        decimal highPrice = Math.Max(firstSelection.High, secondSelection.High);
                        decimal lowPrice = Math.Min(firstSelection.Low, secondSelection.Low);

                        var fibLevels = WaveHelper.CalculateFibonacciLevels(highPrice, lowPrice);

                        // Draw Fibonacci levels on the chart
                        DrawFibonacciLevels(fibLevels, firstSelection.Date, secondSelection.Date);

                        MessageBox.Show("Wave selected and Fibonacci levels calculated.");
                    }
                }
                else if (firstSelection != null && secondSelection != null)
                {
                    MessageBox.Show("More than two candlesticks selected, resetting selection.");
                    ClearFibonacciLevels();
                    firstSelection = null;
                    secondSelection = null;
                }
            }
        }


        /// <summary>
        /// Function to validate wheather the selected wave is valid or not
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        private bool ValidateWave(SmartCandlestick first, SmartCandlestick second)
        {

            if (firstSelection == null || secondSelection == null)
            {
                MessageBox.Show("Please select both the start and end candlesticks before calculating Beauty.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;

            }
            else if (first.Date >= second.Date)
            {
                MessageBox.Show("Invalid wave: The second candlestick must come after the first.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            return true;
        }

        /// <summary>
        /// Draws and labels the fibonacci levels on the chart
        /// </summary>
        /// <param name="fibLevels"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        private void DrawFibonacciLevels(List<decimal> fibLevels, DateTime startDate, DateTime endDate)
        {
            // Clear existing Fibonacci levels
            ClearFibonacciLevels();

            int levelIndex = 0;

            // Convert the end date to a chart coordinate
            double endDatePosition = endDate.ToOADate();

            foreach (var level in fibLevels)
            {
                // Draw the horizontal line
                var fibLine = new HorizontalLineAnnotation
                {
                    Name = $"FibLevel_{levelIndex++}",
                    AxisY = candlestickChart.ChartAreas["ChartAreaCandlestick"].AxisY,
                    ClipToChartArea = "ChartAreaCandlestick",
                    IsInfinitive = true,
                    Y = (double)level,
                    LineColor = Color.Blue,
                    LineDashStyle = ChartDashStyle.Dash
                };
                candlestickChart.Annotations.Add(fibLine);

                // Add the label for the Fibonacci level near the line
                var fibLabel = new TextAnnotation
                {
                    Name = $"FibLabel_{levelIndex}",
                    Text = $"{level:F2}", // Format to two decimal places
                    AxisX = candlestickChart.ChartAreas["ChartAreaCandlestick"].AxisX,
                    AxisY = candlestickChart.ChartAreas["ChartAreaCandlestick"].AxisY,
                    ClipToChartArea = "ChartAreaCandlestick",
                    X = endDatePosition - 10, // Place label slightly left of the end date
                    Y = (double)level, // Align with the Fibonacci line
                    Alignment = System.Drawing.ContentAlignment.MiddleRight, // Align text to the right
                    ForeColor = Color.Blue,
                    Font = new Font("Arial", 10, FontStyle.Regular)
                };
                candlestickChart.Annotations.Add(fibLabel);
            }
        }

        /// <summary>
        /// Displays and compute the beauty of the selected wave based on the Fibonacci levels
        /// </summary>
        private void ComputeAndDisplayWaveBeauty()
        {
            if (firstSelection == null || secondSelection == null)
            {
                MessageBox.Show("Wave start and end must be properly set.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateWave(firstSelection, secondSelection))
            {
                MessageBox.Show("The selected wave is not valid.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            decimal highPrice = Math.Max(firstSelection.High, secondSelection.High);
            decimal lowPrice = Math.Min(firstSelection.Low, secondSelection.Low);

            // Calculate Fibonacci levels
            var fibLevels = Beauty.CalculateFibonacciLevels(highPrice, lowPrice);

            // Get the candlesticks in the wave
            var waveCandlesticks = boundCandlesticks
                .Where(c => c.Date >= firstSelection.Date && c.Date <= secondSelection.Date)
                .ToList();

            // Compute the total Beauty of the wave
            decimal leeway = numericUpDown_leeway.Value;
            int waveBeauty = Beauty.ComputeWaveBeauty(waveCandlesticks, fibLevels, leeway);

            MessageBox.Show($"Wave Beauty: {waveBeauty}");
            ClearFibonacciLevels();
            // Reset selections
            firstSelection = null;
            secondSelection = null;
        }


        /// <summary>
        /// Clears all Fibonacci levels and labels from the chart.
        /// </summary>
        private void ClearFibonacciLevels()
        {
            // Remove annotations related to Fibonacci levels
            var annotationsToRemove = candlestickChart.Annotations
               .Where(a => a.Name != null && (a.Name.StartsWith("FibLevel") || a.Name.StartsWith("FibLabel")))
               .ToList();

            foreach (var annotation in annotationsToRemove)
            {
                candlestickChart.Annotations.Remove(annotation);
            }
        }


        private void endDate_picker_Click(object sender, EventArgs e)
        {

        }

        private void button_computeBeauty_Click(object sender, EventArgs e)
        {
            ComputeAndDisplayWaveBeauty();
        }
    }
}


