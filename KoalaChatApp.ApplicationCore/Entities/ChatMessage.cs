using KoalaChatApp.ApplicationCore.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace KoalaChatApp.ApplicationCore.Entities {
    public abstract class ChatMessage : BaseEntity {
        public Guid UserId { get; set; }
        public ChatMessageType MessageType { get; set; }
        public DateTimeOffset SentDate { get; set; }
        public ChatMessage(Guid userId, ChatMessageType messageType = ChatMessageType.TEXT) {
            this.UserId = userId;
            this.MessageType = messageType;
            this.SentDate = DateTimeOffset.Now;
        }
    }
}
