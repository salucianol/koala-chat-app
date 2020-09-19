using KoalaChatApp.ApplicationCore.DTOs;

namespace KoalaChatApp.Infrastructure.Interfaces {
    public interface IChatHubService {
        void SendMessage(string chatRoomId, ChatMessageTextDTO message);
    }
}
