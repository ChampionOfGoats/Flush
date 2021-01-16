﻿using Microsoft.AspNetCore.Identity;

namespace Flush.Server
{
    public class ApplicationUser : IdentityUser
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
    }
}
