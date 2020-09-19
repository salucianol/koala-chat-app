using KoalaChatApp.ApplicationCore.Entities;
using KoalaChatApp.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace KoalaChatApp.Infrastructure.Data {
    public class ChatRoomRepository : IRepository<ChatRoom> {
        private readonly KoalaChatDbContext _koalaChatDbContext;
        public ChatRoomRepository(KoalaChatDbContext koalaChatDbContext) {
            _koalaChatDbContext = koalaChatDbContext;
        }
        public void Add(ChatRoom entity) {
            _koalaChatDbContext.ChatRooms.Add(entity);
            SaveChanges();
        }

        public void Delete(ChatRoom entity) {
            entity.IsDeleted = true;
            _koalaChatDbContext.ChatRooms.Update(entity);
            SaveChanges();
        }

        public IEnumerable<ChatRoom> Get(ISpecification<ChatRoom> specification) {
            var queryableIncluded = specification.Includes
                .Aggregate(_koalaChatDbContext.ChatRooms.AsQueryable(), 
                            (current, include) => current.Include(include));

            var queryableIncludedStrings = specification.IncludeStrings
                .Aggregate(queryableIncluded,
                            (current, include) => current.Include(include));

            return queryableIncludedStrings
                            .Where(specification.Criteria)
                            .AsEnumerable();
        }

        public void SaveChanges() {
            _koalaChatDbContext.SaveChanges();
        }

        public void Update(ChatRoom entity) {
            _koalaChatDbContext.ChatRooms.Update(entity);
            SaveChanges();
        }
    }
}
