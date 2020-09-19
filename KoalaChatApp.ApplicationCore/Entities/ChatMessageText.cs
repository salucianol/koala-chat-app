using KoalaChatApp.ApplicationCore.Enums;
using System;

namespace KoalaChatApp.ApplicationCore.Entities {
    public class ChatMessageText : ChatMessage {
        public string Text { get; set; }
        public ChatMessageText(Guid userId, 
                                string text, 
                                ChatMessageType messageType = ChatMessageType.TEXT) : base(userId, messageType) {
            Text = text;
        }
    }
}
