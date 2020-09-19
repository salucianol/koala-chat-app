using KoalaChatApp.ApplicationCore.Entities;
using KoalaChatApp.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KoalaChatApp.Infrastructure.Data {
    public class ChatRoomRepository : IRepository<ChatRoom> {
        private readonly KoalaChatDbContext koalaChatDbContext;
        public ChatRoomRepository(KoalaChatDbContext koalaChatDbContext) {
            this.koalaChatDbContext = koalaChatDbContext;
        }
        public void Add(ChatRoom entity) {
            this.koalaChatDbContext.ChatRooms.Add(entity);
            this.SaveChanges();
        }

        public void Delete(ChatRoom entity) {
            entity.IsDeleted = true;
            this.koalaChatDbContext.ChatRooms.Update(entity);
            this.SaveChanges();
        }

        public IEnumerable<ChatRoom> Get(ISpecification<ChatRoom> specification) {
            var queryableIncluded = specification.Includes
                .Aggregate(this.koalaChatDbContext.ChatRooms.AsQueryable(), (current, include) => current.Include(include));
            var queryableIncludedStrings = specification.IncludeStrings
                .Aggregate(queryableIncluded,
                    (current, include) => current.Include(include));
            return queryableIncludedStrings.Where(specification.Criteria).AsEnumerable();
        }

        public void SaveChanges() {
            koalaChatDbContext.SaveChanges();
        }

        public void Update(ChatRoom entity) {
            this.koalaChatDbContext.ChatRooms.Update(entity);
            this.SaveChanges();
        }
    }
}
