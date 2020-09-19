using KoalaChatApp.ApplicationCore.Enums;
using System;

namespace KoalaChatApp.ApplicationCore.Entities {
    public abstract class ChatMessage : BaseEntity {
        public Guid UserId { get; set; }
        public ChatMessageType MessageType { get; set; }
        public DateTimeOffset SentDate { get; set; }
        public ChatMessage(Guid userId, 
                            ChatMessageType messageType = ChatMessageType.TEXT) {
            UserId = userId;
            MessageType = messageType;
            SentDate = DateTimeOffset.Now;
        }
    }
}
