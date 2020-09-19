using KoalaChatApp.ApplicationCore.Entities;
using KoalaChatApp.ApplicationCore.Enums;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace KoalaChatApp.Tests {
    [TestClass]
    public class ChatRoomTests {
        private ChatRoom chatRoom;
        public ChatRoomTests() {
            this.chatRoom = new ChatRoom() {
                Id = Guid.NewGuid(),
                CreatedAt = DateTimeOffset.Now,
                ModifiedAt = DateTimeOffset.Now,
                IsDeleted = false,
                Name = "Special Guests Room",
                MaxUsersAllowed = 2,
                MaxCharactersCount = 10,
                MaxMessagesCount = 2
            };
        }

        [TestMethod]
        public void AddChatMessageBelowBoundaries() {
            this.chatRoom.ClearMessages();
            chatRoom.AddMessage(new ChatMessageText(Guid.NewGuid(), "Hello!"));
            (bool result, string message) = chatRoom.AddMessage(new ChatMessageText(Guid.NewGuid(), "Hello!"));
            Assert.AreEqual(true, result);
            Assert.AreEqual(string.Empty, message);
        }

        [TestMethod]
        public void AddChatMessageAboveBoundaries() {
            this.chatRoom.ClearMessages();
            chatRoom.AddMessage(new ChatMessageText(Guid.NewGuid(), "Hello madame!"));
            chatRoom.AddMessage(new ChatMessageText(Guid.NewGuid(), "Hello everyone in this special group. God bless you."));
            chatRoom.AddMessage(new ChatMessageText(Guid.NewGuid(), "Hi there. Nice to meet you."));
            (bool result, string message) = chatRoom.AddMessage(new ChatMessageText(Guid.NewGuid(), "Hi there. Nice to meet you."));
            Assert.AreEqual(false, result);
            Assert.AreEqual($"Chat messages count ({chatRoom.MaxMessagesCount}) excedeed.", message);
        }

        [TestMethod]
        public void GetInNewUserAfterMaxUsersAllowedExcedeed() {
            this.chatRoom.ClearUsers();
            this.chatRoom.LetUserIn(Guid.NewGuid());
            this.chatRoom.LetUserIn(Guid.NewGuid());
            (bool result, string message) = this.chatRoom.LetUserIn(Guid.NewGuid());
            Assert.AreEqual(false, result);
            Assert.AreEqual($"Users count ({chatRoom.MaxUsersAllowed}) excedeed.", message);
        }

        [TestMethod]
        public void GetInNewUserBeforeMaxUsersAllowedExcedeed() {
            this.chatRoom.ClearUsers();
            this.chatRoom.LetUserIn(Guid.NewGuid());
            (bool result, string message) = this.chatRoom.LetUserIn(Guid.NewGuid());
            Assert.AreEqual(true, result);
            Assert.AreEqual(string.Empty, message);
        }
    }
}
