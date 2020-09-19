using KoalaChatApp.ApplicationCore.Entities;
using KoalaChatApp.ApplicationCore.Interfaces;
using KoalaChatApp.ApplicationCore.Specifications;
using KoalaChatApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KoalaChatApp.Infrastructure.Services {
    public class ChatRoomService : IChatRoomService {
        private readonly IRepository<ChatRoom> chatRoomRepository;
        public ChatRoomService(IRepository<ChatRoom> chatRoomRepository) {
            this.chatRoomRepository = chatRoomRepository;
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

        public IEnumerable<ChatMessageText> GetChatRoomMessages(Guid chatRoomId) {
            ChatRoom chatRoom = this.chatRoomRepository.Get(new ChatRoomSpecification(chatRoomId)).FirstOrDefault();
            return chatRoom.Messages;
        }

        public IEnumerable<ChatRoom> GetChatRooms() {
            return this.chatRoomRepository.Get(new ChatRoomSpecification());
        }

        public void UpdateChatRoom(ChatRoom chatRoom) {
            this.chatRoomRepository.Update(chatRoom);
        }
    }
}
