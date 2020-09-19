using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace KoalaChatApp.Bot.ApplicationCore.Models {
    public class StockQuoteRequestModel : IRequest<bool> {
        public string Command { get; set; }
        public string RoomId { get; internal set; }
    }
}
