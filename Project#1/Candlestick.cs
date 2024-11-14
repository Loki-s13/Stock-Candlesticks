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


    
}
