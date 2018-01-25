using LYM.Model.SysUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LYM.DAL.Map
{
    public sealed class SysUserInfo_Map
    {
        public static void Map(EntityTypeBuilder<SysUserInfo> builder)
        {
            builder.ToTable("Tb_SysUserInfo");
            builder.HasKey(t => t.UserId);
            builder.Property(t => t.UserId).UseSqlServerIdentityColumn().IsRequired().IsUnicode(false).ValueGeneratedOnAdd();
            builder.Property(t => t.UserName).HasColumnType("nvarchar").HasMaxLength(500);
            builder.Property(t => t.UserPwd).HasColumnType("nvarchar").HasMaxLength(500);
        }
    }
}
