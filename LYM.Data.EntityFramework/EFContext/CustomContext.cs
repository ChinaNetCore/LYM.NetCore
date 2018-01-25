using LYM.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LYM.Data.EntityFramework.EFContext
{
    public class CustomContext : DbContext
    {
        public CustomContext(DbContextOptions<CustomContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Mappings.UserLoginMapping.Map(modelBuilder.Entity<UserLogin>());
            
        }
    }
}
