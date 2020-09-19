using KoalaChatApp.ApplicationCore.DTOs;
using KoalaChatApp.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace KoalaChatApp.ApplicationCore.Interfaces {
    public interface IChatRoomService {
        void DeleteChatRoom(ChatRoom chatRoom);
        void UpdateChatRoom(ChatRoom chatRoom);
        void AddChatRoom(ChatRoom chatRoom);
        IEnumerable<ChatRoom> GetChatRooms();
        ChatRoom GetChatRoom(Guid chatRoomId);
        IEnumerable<ChatMessageTextDTO> GetChatRoomMessages(Guid chatRoomId);
        void AddChatMessage(Guid chatRoomId, ChatMessageText chatMessageText);
    }
}
