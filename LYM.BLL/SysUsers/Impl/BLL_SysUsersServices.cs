using LYM.Model.SysUsers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LYM.BLL.SysUsers.Impl
{
    public class BLL_SysUsersServices : IBLL_SysUsersServices
    {
        /// <summary>
        /// 模拟下BLL获取用户的数据
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task<SysUserInfo> FindByUserID(string userid)
        {

            return await Task.FromResult(new SysUserInfo { UserId = new Guid("4B68EB02-711E-4099-B354-52B29F94DFFD"), UserName = "黎又铭" });
        }


        public bool ValidateCredentials(SysUserInfo userInfo)
        {
            if (userInfo.UserName == "admin" && userInfo.UserPwd == "admin")
            {
                return true;
            }
            return false;
        }


        public SysUserInfo FindByUsername(string username)
        {

            return new SysUserInfo { UserId = new Guid("4B68EB02-711E-4099-B354-52B29F94DFFD"), UserName = "黎又铭" };
        }



    }
}
