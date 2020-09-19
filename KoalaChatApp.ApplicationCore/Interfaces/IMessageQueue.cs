namespace KoalaChatApp.ApplicationCore.Interfaces {
    public interface IMessageQueue {
        void EnqueueMessage(string message);
        void Connect();
    }
}
