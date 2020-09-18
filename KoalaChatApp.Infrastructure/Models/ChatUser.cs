using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace KoalaChatApp.Infrastructure.Models {
    public class ChatUser : IdentityUser<Guid> {
    }
}
