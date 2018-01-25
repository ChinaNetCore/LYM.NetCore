using Microsoft.EntityFrameworkCore;
using System;

namespace LYM.Cap
{

    /// <summary>
    ///  发布消息 订阅事件、消息 数据库
    /// </summary>
    public class CapApiContext : DbContext
    {

        public CapApiContext(DbContextOptions<CapApiContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


        }

    }
}
