using System.Collections.Generic;
using KoalaChatApp.Infrastructure.Interfaces;

namespace KoalaChatApp.Infrastructure.Helpers {
    public class CommandsHelper : ICommandsHelper {
        private HashSet<string> Commands { get; set; }
        public CommandsHelper() {
            this.Commands = new HashSet<string>();
        }
        public void AddCommand(string command) {
            this.Commands.Add(command.ToLower());
        }
        public bool IsCommandValid(string command) {
            return this.Commands.Contains(command.ToLower());
        }
    }
}
