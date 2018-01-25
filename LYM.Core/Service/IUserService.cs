using LYM.Core.Model.User;
using LYM.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LYM.Core.Service
{
    public  interface IUserService
    {
        Task<UserLoginModel> GetByUserName(string userName);


        Task<UserLoginModel> FindByUserID(string userid);


        Task<bool> ValidateCredentials(UserLoginModel model);



        Task<PagedResult<UserLoginModel>> GetList();
        /// <summary>
        /// 异步消息订阅处理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task MessageInsert(UserLoginModel model);
    }
}
