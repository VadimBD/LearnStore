using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Infrastructure.Identity
{
    public class AppIdentityDbContext : IdentityDbContext<IdentityUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "1",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "2752F10D-2938-4540-96F0-80F06F19F1D9"
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "Customer",
                    NormalizedName = "CUSTOMER",
                    ConcurrencyStamp = "DF2EBED8-6D69-4041-B4CE-63219305ECBC"
                },
                new IdentityRole
                {
                    Id = "3",
                    Name = "Seller",
                    NormalizedName = "SELLER",
                    ConcurrencyStamp= "72AF9E5F-F92C-4BB1-949A-98302820ADB3"
                }
            );
           
        }
    }
}
