using LYM.Model.SysUsers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LYM.BLL.SysUsers
{
    public interface IBLL_SysUsersServices
    {
         Task<SysUserInfo> FindByUserID(string userid);


        bool ValidateCredentials(SysUserInfo userInfo);


        SysUserInfo FindByUsername(string  username);
         
    }
}
