using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoalaChatApp.Web.Models {
    public class ChatMessageModelResponse {
        public string Text { get; internal set; }
        public string Date { get; internal set; }
        public string User { get; internal set; }
        public string RoomName { get; internal set; }
        public object RoomId { get; internal set; }
    }
}
