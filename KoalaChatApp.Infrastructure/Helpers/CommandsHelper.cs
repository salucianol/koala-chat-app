using System.Collections.Generic;
using KoalaChatApp.Infrastructure.Interfaces;

namespace KoalaChatApp.Infrastructure.Helpers {
    public class CommandsHelper : ICommandsHelper {
        private HashSet<string> Commands { get; set; }
        public CommandsHelper() {
            Commands = new HashSet<string>();
        }
        public void AddCommand(string command) {
            Commands.Add(command.ToLower());
        }
        public bool IsCommandValid(string command) {
            return Commands.Contains(command.ToLower());
        }
    }
}
