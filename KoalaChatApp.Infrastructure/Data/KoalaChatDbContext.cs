using KoalaChatApp.ApplicationCore.Entities;
using KoalaChatApp.ApplicationCore.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace KoalaChatApp.Infrastructure.Data {
    public class KoalaChatDbContext : DbContext {
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public KoalaChatDbContext(DbContextOptions<KoalaChatDbContext> dbContextOptions) : base(dbContextOptions) {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ChatRoom>().ToTable("ChatRooms");
            modelBuilder.Entity<ChatRoom>().OwnsMany<ChatMessageText>(chatRoom => chatRoom.Messages);
        }
    }
}
