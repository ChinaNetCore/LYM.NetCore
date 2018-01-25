using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LYM.OAuth2OpenId.IdrServices.Account
{
    /// <summary>
    /// 登录账户服务相关参数
    /// </summary>
    public class AccountOptions
    {
        /// <summary>
        /// 允许本地登录
        /// </summary>
        public static bool AllowLocalLogin = true;
        /// <summary>
        /// 允许记住登录状态
        /// </summary>
        public static bool AllowRememberLogin = true;
        /// <summary>
        /// 记住登陆的时间周期
        /// </summary>
        public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);
        /// <summary>
        /// 展示退出Prompt提示确认
        /// </summary>
        public static bool ShowLogoutPrompt = false;
        //退出后自动跳转
        public static bool AutomaticRedirectAfterSignOut = true;

        //启用Windows授权  寄宿 IIS or IIS Express
        public static bool WindowsAuthenticationEnabled = true;
        public static bool IncludeWindowsGroups = false;
        // Windows认证名称
        public static readonly string WindowsAuthenticationSchemeName = "Windows";

        public static string InvalidCredentialsErrorMessage = "用户名或密码错误";
    }
}
