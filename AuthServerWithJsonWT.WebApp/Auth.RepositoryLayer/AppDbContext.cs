using Auth.CoreLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.RepositoryLayer
{
    public class AppDbContext :IdentityDbContext<UserApp , IdentityRole , string>
    {


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
                 
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<UserRefreshToken> userRefreshTokens  { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}
