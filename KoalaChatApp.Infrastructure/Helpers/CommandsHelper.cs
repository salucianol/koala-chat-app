using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Text;

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
