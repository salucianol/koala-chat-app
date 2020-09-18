using KoalaChatApp.ApplicationCore.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace KoalaChatApp.ApplicationCore.Entities {
    public class ChatMessageText : ChatMessage {
        public string Text { get; set; }
        public ChatMessageText(Guid userId, string text, ChatMessageType messageType = ChatMessageType.TEXT) : base(userId, messageType) {
            this.Text = text;
        }
    }
}
