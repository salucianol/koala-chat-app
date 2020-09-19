﻿using System;
using System.Collections.Generic;
using System.Text;

namespace KoalaChatApp.Infrastructure {
    public interface ICommandsHelper {
        void AddCommand(string command);
        bool IsCommandValid(string command);
    }
}
