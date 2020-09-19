using KoalaChatApp.ApplicationCore.Entities;
using KoalaChatApp.ApplicationCore.Interfaces;
using KoalaChatApp.ApplicationCore.Specifications;
using KoalaChatApp.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using KoalaChatApp.ApplicationCore.DTOs;
using KoalaChatApp.Infrastructure.Data.Specifications;

namespace KoalaChatApp.Infrastructure.Services {
    public class ChatRoomService : IChatRoomService {
        private readonly IRepository<ChatRoom> chatRoomRepository;
        private readonly IRepository<ChatUser> userRepository;
        public ChatRoomService(IRepository<ChatRoom> chatRoomRepository, IRepository<ChatUser> userRepository) {
            this.chatRoomRepository = chatRoomRepository;
            this.userRepository = userRepository;
        }
        public void AddChatMessage(Guid chatRoomId, ChatMessageText chatMessageText) {
            ChatRoom chatRoom = this.chatRoomRepository.Get(new ChatRoomSpecification(chatRoomId)).FirstOrDefault();
            if (chatRoom != default(ChatRoom)) {
                chatRoom.AddMessage(chatMessageText);
            }
            this.chatRoomRepository.Update(chatRoom);
        }

        public void AddChatRoom(ChatRoom chatRoom) {
            this.chatRoomRepository.Add(chatRoom);
        }

        public void DeleteChatRoom(ChatRoom chatRoom) {
            this.chatRoomRepository.Delete(chatRoom);
        }

        public ChatRoom GetChatRoom(Guid chatRoomId) {
            return this.chatRoomRepository.Get(new ChatRoomSpecification(chatRoomId)).FirstOrDefault();
        }

        public IEnumerable<ChatMessageTextDTO> GetChatRoomMessages(Guid chatRoomId) {
            ChatRoom chatRoom = this.chatRoomRepository.Get(new ChatRoomSpecification(chatRoomId)).FirstOrDefault();
            return chatRoom.Messages.Select(cm => new ChatMessageTextDTO {
                Text = cm.Text,
                Date = cm.SentDate.ToString(),
                RoomName = chatRoom.Name,
                User = this.userRepository.Get(new UserSpecification(cm.UserId)).FirstOrDefault().UserName,
                RoomId = chatRoom.Id
            });
        }

        public IEnumerable<ChatRoom> GetChatRooms() {
            return this.chatRoomRepository.Get(new ChatRoomSpecification());
        }

        public void UpdateChatRoom(ChatRoom chatRoom) {
            this.chatRoomRepository.Update(chatRoom);
        }
    }
}
