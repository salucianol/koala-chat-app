using System;

namespace KoalaChatApp.Infrastructure.Exceptions {
    public class CommandNotFoundException : Exception {
        public CommandNotFoundException(string command) 
                                            : base($"Command '{command}' is not a valid command.") {
        }
    }
}
