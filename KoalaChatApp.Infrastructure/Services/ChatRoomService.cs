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
        private readonly IRepository<ChatRoom> _chatRoomRepository;
        private readonly IRepository<ChatUser> _userRepository;
        private readonly ILogger<ChatRoomService> _logger;

        public ChatRoomService(IRepository<ChatRoom> chatRoomRepository, 
                                IRepository<ChatUser> userRepository, 
                                ILogger<ChatRoomService> logger) {
            _chatRoomRepository = chatRoomRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public void AddChatMessage(Guid chatRoomId, ChatMessageText chatMessageText) {
            ChatRoom chatRoom = _chatRoomRepository
                                    .Get(new ChatRoomSpecification(chatRoomId))
                                    .FirstOrDefault();
            if (chatRoom != default(ChatRoom)) {
                chatRoom.AddMessage(chatMessageText);
                _chatRoomRepository.Update(chatRoom);
            }
            _logger.LogInformation($"Chat room with id {chatRoomId.ToString()} not found.", null);
        }

        public void AddChatRoom(ChatRoom chatRoom) {
            _chatRoomRepository
                .Add(chatRoom);
        }

        public void DeleteChatRoom(ChatRoom chatRoom) {
            _chatRoomRepository
                .Delete(chatRoom);
        }

        public bool Exists(string name) {
            return _chatRoomRepository
                        .Get(new ChatRoomSpecification(name))
                        .Any();
        }

        public bool Exists(Guid id) {
            return _chatRoomRepository
                        .Get(new ChatRoomSpecification(id))
                        .Any();
        }

        public ChatRoom GetChatRoom(Guid chatRoomId) {
            return _chatRoomRepository
                        .Get(new ChatRoomSpecification(chatRoomId))
                        .FirstOrDefault();
        }

        public ChatRoom GetChatRoom(string name) {
            return _chatRoomRepository
                        .Get(new ChatRoomSpecification(name))
                        .FirstOrDefault();
        }

        public IEnumerable<ChatMessageTextDTO> GetChatRoomMessages(Guid chatRoomId) {
            ChatRoom chatRoom = _chatRoomRepository
                                    .Get(new ChatRoomSpecification(chatRoomId))
                                    .FirstOrDefault();
            if (chatRoom?.Id != Guid.Empty) {
                return chatRoom.Messages
                                .OrderBy(cm => cm.SentDate)
                                .Take(chatRoom.MaxMessagesCount)
                                .Select(cm => new ChatMessageTextDTO {
                                    Text = cm.Text,
                                    Date = cm.SentDate.ToString("yyyy-MM-dd HH:mm"),
                                    RoomName = chatRoom.Name,
                                    User = _userRepository.Get(new UserSpecification(cm.UserId)).FirstOrDefault().UserName,
                                    RoomId = chatRoom.Id
                                }); 
            }
            _logger.LogInformation($"Chat room with Id {chatRoomId.ToString()} not found.", null);
            return null;
        }

        public IEnumerable<ChatRoom> GetChatRooms() {
            return _chatRoomRepository
                        .Get(new ChatRoomSpecification());
        }

        public void UpdateChatRoom(ChatRoom chatRoom) {
            _chatRoomRepository
                .Update(chatRoom);
        }
    }
}
