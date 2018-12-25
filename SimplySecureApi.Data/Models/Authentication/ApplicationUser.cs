using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SimplySecureApi.Data.Models.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [DataMember]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Account Active")]
        public bool Active { get; set; } = true;
    }
}