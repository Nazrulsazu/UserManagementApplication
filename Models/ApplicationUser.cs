using Microsoft.AspNetCore.Identity;
using System;
namespace UserManagementApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime LastLoginTime { get; set; }
        public DateTime RegistrationTime { get; set; }
        public string Status { get; set; } = "Active";
        public bool IsDeleted { get; set; } = false;
    }
}
