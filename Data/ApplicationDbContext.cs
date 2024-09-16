﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserManagementApp.Models;

namespace UserManagementApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }     protected override void OnModelCreating(ModelBuilder builder)
     {
         base.OnModelCreating(builder);

         // Add a unique index to the Email field
         builder.Entity<ApplicationUser>()
             .HasIndex(u => u.Email)
             .IsUnique();

         // Configure other model customizations here
     }
    }
}
