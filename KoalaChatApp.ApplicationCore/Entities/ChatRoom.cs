using KoalaChatApp.ApplicationCore.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace KoalaChatApp.ApplicationCore.Entities {
    public class ChatRoom : BaseEntity {
        public string Name { get; set; }
        private List<ChatMessage> _messages { get; set; }
        public IReadOnlyCollection<ChatMessage> Messages { get => _messages.AsReadOnly(); }
        public void AddMessage(ChatMessage message) {
            this._messages.Add(message);
        }
        public class ChatMessage : BaseEntity {
            public Guid UserId { get; set; }
            public ChatMessageType MessageType { get; set; }
            public string Text { get; set; }
            public DateTimeOffset SentDate { get; set; }
            public ChatMessage(Guid userId, string text, ChatMessageType messageType = ChatMessageType.TEXT) {
                this.UserId = userId;
                this.Text = text;
                this.MessageType = messageType;
                this.SentDate = DateTimeOffset.Now;
            }
        }
    }
}
