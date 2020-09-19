using KoalaChatApp.ApplicationCore.Specifications;
using KoalaChatApp.Infrastructure.Models;
using System;

namespace KoalaChatApp.Infrastructure.Data.Specifications {
    public class UserSpecification : BaseSpecification<ChatUser> {
        public UserSpecification(Guid userId) 
                                : base(u => u.Id == userId) {
        }
        public UserSpecification(string userName) 
                                : base(u => u.UserName == userName) {
        }
    }
}
