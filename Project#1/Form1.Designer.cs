namespace Project_1
{
    partial class Form_StockViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea7 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea8 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.stockLoad_button = new System.Windows.Forms.Button();
            this.openFileDialog_stock = new System.Windows.Forms.OpenFileDialog();
            this.dateTime_start = new System.Windows.Forms.DateTimePicker();
            this.dateTime_end = new System.Windows.Forms.DateTimePicker();
            this.startDate_picker = new System.Windows.Forms.Label();
            this.endDate_picker = new System.Windows.Forms.Label();
            this.aCandlestickBindingSource_stock = new System.Windows.Forms.BindingSource(this.components);
            this.candlestickChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.button_update = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.numericUpDown_leeway = new System.Windows.Forms.NumericUpDown();
            this.button_computeBeauty = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.aCandlestickBindingSource_stock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.candlestickChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_leeway)).BeginInit();
            this.SuspendLayout();
            // 
            // stockLoad_button
            // 
            this.stockLoad_button.Location = new System.Drawing.Point(244, 31);
            this.stockLoad_button.Margin = new System.Windows.Forms.Padding(2);
            this.stockLoad_button.Name = "stockLoad_button";
            this.stockLoad_button.Size = new System.Drawing.Size(94, 32);
            this.stockLoad_button.TabIndex = 0;
            this.stockLoad_button.Text = "Load Stock";
            this.stockLoad_button.UseVisualStyleBackColor = true;
            this.stockLoad_button.Click += new System.EventHandler(this.button_loadData);
            // 
            // openFileDialog_stock
            // 
            this.openFileDialog_stock.FileName = "openFileDialog_stockpicker";
            this.openFileDialog_stock.Multiselect = true;
            // 
            // dateTime_start
            // 
            this.dateTime_start.Location = new System.Drawing.Point(42, 54);
            this.dateTime_start.Margin = new System.Windows.Forms.Padding(2);
            this.dateTime_start.Name = "dateTime_start";
            this.dateTime_start.Size = new System.Drawing.Size(182, 20);
            this.dateTime_start.TabIndex = 1;
            this.dateTime_start.Value = new System.DateTime(2022, 1, 1, 0, 0, 0, 0);
            this.dateTime_start.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // dateTime_end
            // 
            this.dateTime_end.Location = new System.Drawing.Point(598, 59);
            this.dateTime_end.Margin = new System.Windows.Forms.Padding(2);
            this.dateTime_end.Name = "dateTime_end";
            this.dateTime_end.Size = new System.Drawing.Size(186, 20);
            this.dateTime_end.TabIndex = 2;
            this.dateTime_end.ValueChanged += new System.EventHandler(this.dateTimePicker2_ValueChanged);
            // 
            // startDate_picker
            // 
            this.startDate_picker.AutoSize = true;
            this.startDate_picker.Location = new System.Drawing.Point(110, 31);
            this.startDate_picker.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.startDate_picker.Name = "startDate_picker";
            this.startDate_picker.Size = new System.Drawing.Size(79, 13);
            this.startDate_picker.TabIndex = 3;
            this.startDate_picker.Text = "Pick Start Date";
            // 
            // endDate_picker
            // 
            this.endDate_picker.AutoSize = true;
            this.endDate_picker.Location = new System.Drawing.Point(649, 31);
            this.endDate_picker.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.endDate_picker.Name = "endDate_picker";
            this.endDate_picker.Size = new System.Drawing.Size(76, 13);
            this.endDate_picker.TabIndex = 4;
            this.endDate_picker.Text = "Pick End Date";
            this.endDate_picker.Click += new System.EventHandler(this.endDate_picker_Click);
            // 
            // candlestickChart
            // 
            this.candlestickChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea7.Name = "ChartAreaCandlestick";
            chartArea8.Name = "ChartAreaVolume";
            this.candlestickChart.ChartAreas.Add(chartArea7);
            this.candlestickChart.ChartAreas.Add(chartArea8);
            this.candlestickChart.Location = new System.Drawing.Point(42, 118);
            this.candlestickChart.Margin = new System.Windows.Forms.Padding(2);
            this.candlestickChart.Name = "candlestickChart";
            series7.ChartArea = "ChartAreaCandlestick";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick;
            series7.CustomProperties = "PriceDownColor=192\\, 0\\, 0, PriceUpColor=Lime";
            series7.Name = "SeriesCandlestick";
            series7.XValueMember = "Date";
            series7.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series7.YValueMembers = "High, Low, Open, Close";
            series7.YValuesPerPoint = 6;
            series8.ChartArea = "ChartAreaVolume";
            series8.IsXValueIndexed = true;
            series8.Name = "SeriesVolume";
            series8.XValueMember = "Date";
            series8.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Date;
            series8.YValueMembers = "Volume";
            series8.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.UInt64;
            this.candlestickChart.Series.Add(series7);
            this.candlestickChart.Series.Add(series8);
            this.candlestickChart.Size = new System.Drawing.Size(775, 639);
            this.candlestickChart.TabIndex = 6;
            this.candlestickChart.Text = "chart1";
            this.candlestickChart.Click += new System.EventHandler(this.candlestickChart_Click);
            this.candlestickChart.MouseClick += new System.Windows.Forms.MouseEventHandler(this.candlestickChart_MouseClick);
            // 
            // button_update
            // 
            this.button_update.Location = new System.Drawing.Point(489, 32);
            this.button_update.Margin = new System.Windows.Forms.Padding(2);
            this.button_update.Name = "button_update";
            this.button_update.Size = new System.Drawing.Size(94, 31);
            this.button_update.TabIndex = 7;
            this.button_update.Text = "Update";
            this.button_update.UseVisualStyleBackColor = true;
            this.button_update.Click += new System.EventHandler(this.update_StockData);
            // 
            // numericUpDown_leeway
            // 
            this.numericUpDown_leeway.Location = new System.Drawing.Point(391, 120);
            this.numericUpDown_leeway.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown_leeway.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown_leeway.Name = "numericUpDown_leeway";
            this.numericUpDown_leeway.Size = new System.Drawing.Size(75, 20);
            this.numericUpDown_leeway.TabIndex = 9;
            this.numericUpDown_leeway.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // button_computeBeauty
            // 
            this.button_computeBeauty.Location = new System.Drawing.Point(370, 31);
            this.button_computeBeauty.Name = "button_computeBeauty";
            this.button_computeBeauty.Size = new System.Drawing.Size(96, 32);
            this.button_computeBeauty.TabIndex = 10;
            this.button_computeBeauty.Text = "Beauty";
            this.button_computeBeauty.UseVisualStyleBackColor = true;
            this.button_computeBeauty.Click += new System.EventHandler(this.button_computeBeauty_Click);
            // 
            // Form_StockViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(921, 783);
            this.Controls.Add(this.button_computeBeauty);
            this.Controls.Add(this.button_update);
            this.Controls.Add(this.candlestickChart);
            this.Controls.Add(this.endDate_picker);
            this.Controls.Add(this.startDate_picker);
            this.Controls.Add(this.dateTime_end);
            this.Controls.Add(this.dateTime_start);
            this.Controls.Add(this.stockLoad_button);
            this.Controls.Add(this.numericUpDown_leeway);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form_StockViewer";
            this.Text = "`";
            ((System.ComponentModel.ISupportInitialize)(this.aCandlestickBindingSource_stock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.candlestickChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_leeway)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button stockLoad_button;
        private System.Windows.Forms.OpenFileDialog openFileDialog_stock;
        private System.Windows.Forms.DateTimePicker dateTime_start;
        private System.Windows.Forms.DateTimePicker dateTime_end;
        private System.Windows.Forms.Label startDate_picker;
        private System.Windows.Forms.Label endDate_picker;
        private System.Windows.Forms.BindingSource aCandlestickBindingSource_stock;
        private System.Windows.Forms.DataVisualization.Charting.Chart candlestickChart;
        private System.Windows.Forms.Button button_update;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.NumericUpDown numericUpDown_leeway;
        private System.Windows.Forms.Button button_computeBeauty;
    }
}

