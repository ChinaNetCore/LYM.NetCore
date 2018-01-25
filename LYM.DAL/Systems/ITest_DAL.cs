
using System;
using System.Collections.Generic;
using System.Text;
using LYM.Model.SysUsers;
using Shared.Infrastructure.UnitOfWork;

namespace LYM.DAL.Systems
{
    public interface ITest_DAL // : IRepository<SysUserInfo>
    {

        string SayHello(string name);
    }
}
