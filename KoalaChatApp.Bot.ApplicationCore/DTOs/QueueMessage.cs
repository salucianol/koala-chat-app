using System;
using System.Collections.Generic;
using System.Text;

namespace KoalaChatApp.Bot.ApplicationCore.DTOs {
    public class QueueMessage {
        public string Command { get; set; }
        public string RoomId { get; set; }
        public string Quote { get; set; }
    }
}
