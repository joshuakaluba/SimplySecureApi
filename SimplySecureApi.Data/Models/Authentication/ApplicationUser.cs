using Microsoft.AspNetCore.Identity;
using System;

namespace SimplySecureApi.Data.Models.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}