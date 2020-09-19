using KoalaChatApp.ApplicationCore.Entities;
using System;

namespace KoalaChatApp.ApplicationCore.Specifications {
    public class ChatRoomSpecification : BaseSpecification<ChatRoom> {
        public ChatRoomSpecification() : base(cr => cr.IsDeleted == false) {
        }
        public ChatRoomSpecification(Guid chatRoomId) : base(cr => cr.IsDeleted == false 
                                                                && cr.Id == chatRoomId) {
        }
        public ChatRoomSpecification(string name): base(cr => cr.IsDeleted == false 
                                                            && cr.Name == name) {
        }
    }
}
