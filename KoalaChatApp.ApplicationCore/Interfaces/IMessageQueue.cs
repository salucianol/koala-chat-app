using KoalaChatApp.ApplicationCore.DTOs;

namespace KoalaChatApp.ApplicationCore.Interfaces {
    public interface IMessageQueue {
        void EnqueueMessage(QueueMessageDTO message);
        void Connect();
    }
}
