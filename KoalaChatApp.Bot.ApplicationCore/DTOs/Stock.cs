using System;
using System.Collections.Generic;
using System.Text;

namespace KoalaChatApp.Bot.ApplicationCore.DTOs {
    public class Stock {
        public string Symbol { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Time { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public int Volume { get; set; }
    }
}
