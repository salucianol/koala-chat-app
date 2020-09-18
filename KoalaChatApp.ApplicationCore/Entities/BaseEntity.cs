using System;
using System.Collections.Generic;
using System.Text;

namespace KoalaChatApp.ApplicationCore.Entities {
    public class BaseEntity {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
