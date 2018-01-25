using Shared.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace LYM.Model.SysUsers
{
    public class SysUserInfo: IEntity
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }


        public string UserPwd { get; set; }
    }
}
