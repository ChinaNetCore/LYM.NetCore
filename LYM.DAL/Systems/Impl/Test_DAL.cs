using LYM.Model.SysUsers;
using Shared.Infrastructure.UnitOfWork.EntityFramework;
using Microsoft.EntityFrameworkCore;

using System;
namespace LYM.DAL.Systems.Impl
{
    /// <summary>
    /// 之前用的自己仓储 ： AbstractDataRepository<SysUserInfo>  现在用第三方提供的
    /// </summary>
    public class Test_DAL : AbstractDataRepository<SysUserInfo>, ITest_DAL
    {
        public Test_DAL(IServiceProvider serviceProvider) : base(serviceProvider)
        {



        }


        public string SayHello(string name)
        {

            var model = new SysUserInfo { UserName = "张三", UserPwd = "123123" };
            Insert(model);

            return "Hello:" + model.UserId;


        }


    }
}
