using System;
using System.Collections.Generic;
using System.Text;

namespace KoalaChatApp.ApplicationCore.DTOs {
    public class ChatMessageTextDTO {
        public string Text { get; set; }
        public string Date { get; set; }
        public string User { get; set; }
        public string RoomName { get; set; }
        public Guid RoomId { get; set; }
    }
}
