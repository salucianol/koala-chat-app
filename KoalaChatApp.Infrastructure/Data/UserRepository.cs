using KoalaChatApp.ApplicationCore.Interfaces;
using KoalaChatApp.Infrastructure.Data;
using KoalaChatApp.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KoalaChatApp.Infrastructure.Data {
    public class UserRepository : IRepository<ChatUser> {
        private readonly KoalaChatIdentityDbContext koalaChatIdentityDbContext;
        public UserRepository(KoalaChatIdentityDbContext koalaChatIdentityDbContext) {
            this.koalaChatIdentityDbContext = koalaChatIdentityDbContext;
        }
        public void Add(ChatUser entity) {
            return;
        }

        public void Delete(ChatUser entity) {
            return;
        }

        public IEnumerable<ChatUser> Get(ISpecification<ChatUser> specification) {
            var queryableIncluded = specification.Includes
                .Aggregate(this.koalaChatIdentityDbContext.Users.AsQueryable(), (current, include) => current.Include(include));
            var queryableIncludedStrings = specification.IncludeStrings
                .Aggregate(queryableIncluded,
                    (current, include) => current.Include(include));
            return queryableIncludedStrings.Where(specification.Criteria).AsEnumerable();
        }

        public void SaveChanges() {
            this.koalaChatIdentityDbContext.SaveChanges();
        }

        public void Update(ChatUser entity) {
            return;
        }
    }
}
