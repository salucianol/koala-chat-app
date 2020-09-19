using KoalaChatApp.ApplicationCore.Entities;
using MediatR;

namespace KoalaChatApp.Infrastructure.Models {
    public class ChatMessageRequestModel : IRequest<bool> {
        public ChatMessage ChatMessage { get; set; }
        public string ChatRoomId { get; set; }
    }
}
