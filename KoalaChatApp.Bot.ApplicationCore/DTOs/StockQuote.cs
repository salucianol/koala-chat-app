using System;
using System.Collections.Generic;
using System.Text;

namespace KoalaChatApp.Bot.ApplicationCore.DTOs {
    public class StockQuote {
        public Stock Stock { get; set; }
        public string GetQuote() {
            return $"{this.Stock.Symbol} quote is {this.Stock.Open} per share.";
        }
    }
}
