using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
            if (Messages.Count > MaxMessagesCount) {
                return (false, $"Chat messages count ({MaxMessagesCount}) excedeed.");
            }
            Messages.Add(message);
            return (true, string.Empty);
        }

        public bool ClearMessages() {
            Messages.Clear();
            return true;
        }

        public (bool, string) LetUserIn(Guid userId) {
            if (Users.Count > MaxUsersAllowed) {
                return (false, $"Users count ({MaxUsersAllowed}) excedeed.");
            }
            Users.Add(userId);
            return (true, string.Empty);
        }

        public bool GetUsetOut(Guid userId) {
            Users.Remove(userId);
            return true;
        }

        public bool ClearUsers() {
            Users.Clear();
            return true;
        }
    }
}
