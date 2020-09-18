using KoalaChatApp.Bot.ApplicationCore.DTOs;
using KoalaChatApp.Bot.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace KoalaChatApp.Bot.Infrastructure.Services {
    public class CsvTextParser : ITextParser {
        private readonly ILogger<CsvTextParser> logger;
        public CsvTextParser(ILogger<CsvTextParser> logger) {
            this.logger = logger;
        }
        public Stock ParseText(string text) {
            if (text != string.Empty && text.Trim().Length < 1) {
                return null;
            }
            string[] stockInfo = text.Split("\n")[1].Split(",");
            if (stockInfo.Length != 8) {
                return null;
            }
            DateTime date = DateTime.MinValue;
            DateTime.TryParse(stockInfo[1], out date);
            Decimal.TryParse(stockInfo[3], out decimal open);
            Decimal.TryParse(stockInfo[4], out decimal high);
            Decimal.TryParse(stockInfo[5], out decimal low);
            int.TryParse(stockInfo[6], out int volume);
            return new Stock() {
                Symbol = stockInfo[0],
                Date = date,
                Time = stockInfo[2],
                Open = open,
                High = high,
                Low = low,
                Volume = volume
            };
        }
    }
}
