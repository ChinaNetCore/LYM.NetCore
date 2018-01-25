using Shared.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace LYM.Domain.Entity
{
    public class UserLogin : IEntity
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string UserPwd { get; set; }

        public bool IsDelete { get; set; }

        public int Order { get; set; }
    }
}
