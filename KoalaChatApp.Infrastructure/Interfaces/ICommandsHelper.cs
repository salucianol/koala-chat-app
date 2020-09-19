namespace KoalaChatApp.Infrastructure.Interfaces {
    public interface ICommandsHelper {
        void AddCommand(string command);
        bool IsCommandValid(string command);
    }
}
