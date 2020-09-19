using KoalaChatApp.ApplicationCore.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace KoalaChatApp.ApplicationCore.Entities {
    public class ChatRoom : BaseEntity {
        public string Name { get; set; }
        public ICollection<ChatMessageText> Messages { get; set; } = new List<ChatMessageText>();
        [NotMapped]
        public ICollection<Guid> Users { get; set; } = new List<Guid>();
        public short MaxUsersAllowed { get; set; }
        public short MaxCharactersCount { get; set; }
        public short MaxMessagesCount { get; set; }

        public (bool, string) AddMessage(ChatMessageText message) {
            if (this.Messages.Count > this.MaxMessagesCount) {
                return (false, $"Chat messages count ({this.MaxMessagesCount}) excedeed.");
            }
            this.Messages.Add(message);
            return (true, string.Empty);
        }
        public bool ClearMessages() {
            this.Messages.Clear();
            return true;
        }

        public (bool, string) LetUserIn(Guid userId) {
            if (this.Users.Count > this.MaxUsersAllowed) {
                return (false, $"Users count ({this.MaxUsersAllowed}) excedeed.");
            }
            this.Users.Add(userId);
            return (true, string.Empty);
        }

        public bool GetUsetOut(Guid userId) {
            this.Users.Remove(userId);
            return true;
        }

        public bool ClearUsers() {
            this.Users.Clear();
            return true;
        }
    }
}
