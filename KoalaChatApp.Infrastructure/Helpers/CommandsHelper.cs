using System.Collections.Generic;
using KoalaChatApp.Infrastructure.Interfaces;

namespace KoalaChatApp.Infrastructure.Helpers {
    public class CommandsHelper : ICommandsHelper {
        public CommandsHelper() {

        }
        private HashSet<string> Commands { get; set; }
        public void AddCommand(string command) {
            this.Commands.Add(command);
        }
        public bool IsCommandValid(string command) {
            return this.Commands.Contains(command);
        }
    }
}
