using LYM.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LYM.Data.EntityFramework.Mappings
{
    public sealed class UserLoginMapping
    {
        public static void Map(EntityTypeBuilder<UserLogin> builder)
        {
            builder.ToTable("Tb_UserLogin");
            builder.HasKey(t => t.UserId);

            builder.Property(t => t.UserId).UseSqlServerIdentityColumn().ValueGeneratedOnAdd();
            builder.Property(t => t.UserName).IsRequired().HasMaxLength(20);
            builder.Property(t => t.UserPwd).IsRequired().HasMaxLength(100);
          
        }
    }
}
