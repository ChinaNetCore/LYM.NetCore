using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LYM.DAL.EFContext
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

            Map.SysUserInfo_Map.Map(modelBuilder.Entity<LYM.Model.SysUsers.SysUserInfo>());
            
        }
    }
}
