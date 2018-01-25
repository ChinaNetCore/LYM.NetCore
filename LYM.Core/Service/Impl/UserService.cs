using DotNetCore.CAP;
using LYM.Core.Model.User;
using LYM.Domain.Entity;
using LYM.Domain.Model;
using LYM.Domain.Repository;
using Shared.Infrastructure.Extensions;
using Shared.Infrastructure.Model;
using Shared.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LYM.Core.Service.Impl
{
    /// <summary>
    /// 业务服务层 liyouming  add  2017-12-07
    /// </summary>
    public class UserService : ServiceBase, IUserService
    {

        private IUserLoginRepository _userLoginRepository;
        public UserService(IServiceProvider serviceProvider, IUserLoginRepository userLoginRepository)
           : base(serviceProvider)
        {
            _userLoginRepository = userLoginRepository;
        }



        public async Task<PagedResult<UserLoginModel>> GetList()
        {

            using (var uw = this.CreateUnitOfWork())
            {

                var result = await uw.CreateRepository<IUserLoginRepository>().QueryAsync_UserLogin(1, 100, "");
                var modelList = this.Transform(result.Data.ToArray());

                return PagedResult<UserLoginModel>.SuccessResult(modelList, result.PageIndex, result.PageSize, result.Total);
            }
        }
        /// <summary>
        /// 模型转换
        /// </summary>
        /// <param name="entityList"></param>
        /// <returns></returns>
        private List<UserLoginModel> Transform(params UserLogin[] entityList)
        {
            if (entityList.IsEmptyCollection())
            {
                return new List<UserLoginModel>();
            }
            //这里也可以用Mapper
            return entityList.Select(t => new UserLoginModel
            {
                UserId = t.UserId,
                UserName = t.UserName,
                UserPwd = t.UserPwd
            }).ToList();
        }
        /// <summary>
        /// 查找用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<UserLoginModel> GetByUserName(string userName)
        {
            using (var uw = this.CreateUnitOfWork())
            {
                var user = await uw.GetAsync<UserLogin>(t => t.UserName == userName);

                if (user == null)
                {
                    return null;
                }
                return this.Transform(user).First();

            }

        }
        /// <summary>
        /// 查找用户
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task<UserLoginModel> FindByUserID(string userid)
        {
            using (var uw = this.CreateUnitOfWork())
            {
                var user = await uw.GetAsync<UserLogin>(t => t.UserId == long.Parse(userid));

                if (user == null)
                {
                    return null;
                }
                return this.Transform(user).First();

            }
        }
        /// <summary>
        /// 验证用户名密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> ValidateCredentials(UserLoginModel model)
        {


            using (var uw = this.CreateUnitOfWork())
            {
                var user = await uw.GetAsync<UserLogin>(t => t.UserName == model.UserName && t.UserPwd == model.UserPwd);

                if (user == null)
                {
                    return false;
                }

                
                return true;

            }


        }




        /// <summary>
        /// 消息订阅回调
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task MessageInsert(UserLoginModel model)
        {
            using (var uw = this.CreateUnitOfWork())
            {
                await uw.InsertAsync(new UserLogin
                {
                    UserName = model.UserName,
                    UserPwd = model.UserPwd
                });
            }


        }

    }
}
