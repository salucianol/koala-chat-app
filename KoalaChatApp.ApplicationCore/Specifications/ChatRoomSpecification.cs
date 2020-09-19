using KoalaChatApp.ApplicationCore.Entities;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace KoalaChatApp.ApplicationCore.Specifications {
    public class ChatRoomSpecification : BaseSpecification<ChatRoom> {
        public ChatRoomSpecification() : base(cr => cr.IsDeleted == false) {
        }
        public ChatRoomSpecification(Guid chatRoomId) : base(cr => cr.IsDeleted == false && cr.Id == chatRoomId) {
        }
    }
}
