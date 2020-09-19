using KoalaChatApp.ApplicationCore.Entities;
using KoalaChatApp.ApplicationCore.Interfaces;
using KoalaChatApp.ApplicationCore.Specifications;
using KoalaChatApp.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using KoalaChatApp.ApplicationCore.DTOs;
using KoalaChatApp.Infrastructure.Data.Specifications;
using Microsoft.Extensions.Logging;

namespace KoalaChatApp.Infrastructure.Services {
    public class ChatRoomService : IChatRoomService {
        private readonly IRepository<ChatRoom> chatRoomRepository;
        private readonly IRepository<ChatUser> userRepository;
        private readonly ILogger<ChatRoomService> logger;
        public ChatRoomService(IRepository<ChatRoom> chatRoomRepository, IRepository<ChatUser> userRepository, ILogger<ChatRoomService> logger) {
            this.chatRoomRepository = chatRoomRepository;
            this.userRepository = userRepository;
            this.logger = logger;
        }
        public void AddChatMessage(Guid chatRoomId, ChatMessageText chatMessageText) {
            ChatRoom chatRoom = this.chatRoomRepository.Get(new ChatRoomSpecification(chatRoomId)).FirstOrDefault();
            if (chatRoom != default(ChatRoom)) {
                chatRoom.AddMessage(chatMessageText);
                this.chatRoomRepository.Update(chatRoom);
            }
            this.logger.LogInformation($"Chat room with id {chatRoomId.ToString()} not found.", null);
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

        public ChatRoom GetChatRoom(string name) {
            return this.chatRoomRepository.Get(new ChatRoomSpecification(name)).FirstOrDefault();
        }

        public IEnumerable<ChatMessageTextDTO> GetChatRoomMessages(Guid chatRoomId) {
            ChatRoom chatRoom = this.chatRoomRepository.Get(new ChatRoomSpecification(chatRoomId)).FirstOrDefault();
            if (chatRoom?.Id != Guid.Empty) {
                return chatRoom.Messages.OrderByDescending(cm => cm.SentDate).Take(chatRoom.MaxMessagesCount).Select(cm => new ChatMessageTextDTO {
                    Text = cm.Text,
                    Date = cm.SentDate.ToString("yyyy-MM-dd HH:mm"),
                    RoomName = chatRoom.Name,
                    User = this.userRepository.Get(new UserSpecification(cm.UserId)).FirstOrDefault().UserName,
                    RoomId = chatRoom.Id
                }); 
            }
            this.logger.LogInformation($"Chat room with Id {chatRoomId.ToString()} not found.", null);
            return null;
        }

        public IEnumerable<ChatRoom> GetChatRooms() {
            return this.chatRoomRepository.Get(new ChatRoomSpecification());
        }

        public void UpdateChatRoom(ChatRoom chatRoom) {
            this.chatRoomRepository.Update(chatRoom);
        }
    }
}
