using KoalaChatApp.Bot.ApplicationCore.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace KoalaChatApp.Bot.ApplicationCore.Interfaces {
    public interface IMessageQueue {
        void EnqueueMessage(QueueMessage message);
        void Connect();
    }
}
