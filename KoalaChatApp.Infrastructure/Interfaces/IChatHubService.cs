using KoalaChatApp.ApplicationCore.DTOs;
using KoalaChatApp.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace KoalaChatApp.Infrastructure.Interfaces {
    public interface IChatHubService {
        void SendMessage(string chatRoomId, ChatMessageTextDTO message);
    }
}
