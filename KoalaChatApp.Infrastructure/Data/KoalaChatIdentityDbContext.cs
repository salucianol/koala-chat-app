using KoalaChatApp.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace KoalaChatApp.Infrastructure.Data {
    public class KoalaChatIdentityDbContext : IdentityDbContext<ChatUser, IdentityRole<Guid>, Guid> {
        public KoalaChatIdentityDbContext(DbContextOptions options) : base(options) {

        }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
        }
    }
}
