using KoalaChatApp.ApplicationCore.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace KoalaChatApp.ApplicationCore.Entities {
    public class ChatRoom : BaseEntity {
        public string Name { get; set; }
        private List<ChatMessageText> _messages { get; set; }
        private List<Guid> _users { get; set; }
        public IReadOnlyCollection<ChatMessageText> Messages { get => _messages.AsReadOnly(); }
        public IReadOnlyCollection<Guid> Users { get => _users.AsReadOnly(); }
        public short MaxUsersAllowed { get; set; }
        public short MaxCharactersCount { get; set; }
        public short MaxMessagesCount { get; set; }
        public (bool, string) AddMessage(ChatMessageText message) {
            this._messages.Add(message);
            return (true, string.Empty);
        }
        public bool ClearMessages() {
            this._messages.Clear();
            return true;
        }

        public (bool, string) LetUserIn(Guid userId) {
            this._users.Add(userId);
            return (true, string.Empty);
        }

        public bool GetUsetOut(Guid userId) {
            this._users.RemoveAt(this._users.IndexOf(userId));
            return true;
        }

        public bool ClearUsers() {
            this._users.Clear();
            return true;
        }
    }
}
