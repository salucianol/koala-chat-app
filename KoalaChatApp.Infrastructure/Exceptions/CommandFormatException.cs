using System;
using System.Collections.Generic;
using System.Text;

namespace KoalaChatApp.Infrastructure.Exceptions {
    public class CommandFormatException : Exception {
        public CommandFormatException(string command) : base($"Command {command} has a wrong format.") {
        }
    }
}
