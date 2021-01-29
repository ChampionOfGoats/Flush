﻿using Microsoft.AspNetCore.Identity;

namespace Flush.Authentication.AspNetCoreIdentity
{
    internal class ApplicationUser : IdentityUser
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
    }
}