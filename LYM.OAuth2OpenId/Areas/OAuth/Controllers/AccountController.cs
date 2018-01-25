using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.Services;
using LYM.OAuth2OpenId.IdrServices.Account;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using System.Security.Principal;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Events;
using LYM.Core.Service;
using IdentityServer4.Extensions;

namespace LYM.OAuth2OpenId.Areas.OAuth.Controllers
{
    /// <summary>
    /// 账户相关 Controller处理  By Liyouming  此示例控制器实现了一个典型的登录/注销/提供本地和外部账户流程。
    /// </summary>
    [IdrServices.SecurityHeaders]
    public class AccountController : Controller
    {
        #region DI 用户服务相关接口 以及 IdentityServer4相关服务几口  IOC处理  liyouming  2017-11-29
        //服务设置 这里注入 用户服务交互相关接口 然偶
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IEventService _events;

        //自定义业务数据库用户服务 处理用户名 密码等业务逻辑
        private readonly IUserService _customUserStore;

        private readonly AccountService _account;


        //这里要说明下这几个接口
        //IClientStore clientStore,IHttpContextAccessor httpContextAccessor, IAuthenticationSchemeProvider schemeProvider

        // IClientStore 提供客户端仓储服务接口 在退出获取参数需要
        // IHttpContextAccessor  .NET Core 下获取 HttpContext 上下文对象 如获取    HttpContext.User 
        // IAuthenticationSchemeProvider  授权相关提供接口
        public AccountController(IIdentityServerInteractionService interaction, IEventService events, IUserService customStore, IClientStore clientStore,
            IHttpContextAccessor httpContextAccessor,
            IAuthenticationSchemeProvider schemeProvider)
        {
            _interaction = interaction;
            _events = events;
            _customUserStore = customStore;

            _account = new AccountService(_interaction, httpContextAccessor, schemeProvider, clientStore);

        }
        #endregion

        #region 登录

        /// <summary>
        /// 登录显示页面   其实也是通过授权回调地址查找授权客户端配置信息  如果授权客户端配置信息中是扩展登录的话转到不同的页面
        /// </summary>
        /// <param name="returnUrl">登录回调跳转地址</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            // 构建登录页面模型
            var vm = await _account.BuildLoginViewModelAsync(returnUrl);

            if (vm.IsExternalLoginOnly)
            {
                //提供扩展登录服务模型
                return await ExternalLogin(vm.ExternalLoginScheme, returnUrl);
            }
           

            return View(vm);
        }
        /// <summary>
        /// 用户登录提交
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model, string button)
        {
            if (button != "login")
            {

                var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
                if (context != null)
                {

                    await _interaction.GrantConsentAsync(context, ConsentResponse.Denied);


                    return Redirect(model.ReturnUrl);
                }
                else
                {

                    return Redirect("~/");
                }
            }

            if (ModelState.IsValid)
            {

                if (await _customUserStore.ValidateCredentials(new Core.Model.User.UserLoginModel { UserName = model.Username, UserPwd = model.Password }))
                {
                    var user = await _customUserStore.GetByUserName(model.Username);
                    await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.UserId.ToString(), user.UserName));


                    AuthenticationProperties props = null;
                    if (AccountOptions.AllowRememberLogin && model.RememberLogin)
                    {
                        props = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
                        };
                    };

                    await HttpContext.SignInAsync(user.UserId.ToString(), user.UserName, props);


                     return Redirect(model.ReturnUrl);

                    #region liyouming 屏蔽 不复核实际要求
                    //if (_interaction.IsValidReturnUrl(model.ReturnUrl) || Url.IsLocalUrl(model.ReturnUrl))
                    //{
                    //    return Redirect(model.ReturnUrl);
                    //}

                    //return Redirect("~/");
                    #endregion
                }

                await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "登录失败"));

                ModelState.AddModelError("", AccountOptions.InvalidCredentialsErrorMessage);
            }


            var vm = await _account.BuildLoginViewModelAsync(model);
            return View(vm);
        }

        /// <summary>
        /// 展示扩展登录页面 提供来之其他客户端的扩展登录界面
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ExternalLogin(string provider, string returnUrl)
        {
            var props = new AuthenticationProperties()
            {
                RedirectUri = Url.Action("ExternalLoginCallback"),
                Items =
                {
                    { "returnUrl", returnUrl }
                }
            };

            //windows授权需要特殊处理，原因是windows没有对回调跳转地址的处理，所以当我们调用授权请求的时候需要再次触发URL跳转
            if (AccountOptions.WindowsAuthenticationSchemeName == provider)
            {
                var result = await HttpContext.AuthenticateAsync(AccountOptions.WindowsAuthenticationSchemeName);
                if (result?.Principal is WindowsPrincipal wp)
                {
                    props.Items.Add("scheme", AccountOptions.WindowsAuthenticationSchemeName);
                    var id = new ClaimsIdentity(provider);
                    id.AddClaim(new Claim(JwtClaimTypes.Subject, wp.Identity.Name));
                    id.AddClaim(new Claim(JwtClaimTypes.Name, wp.Identity.Name));

                    //将授权认证的索赔信息添加进去 注意索赔信息的大小
                    if (AccountOptions.IncludeWindowsGroups)
                    {
                        var wi = wp.Identity as WindowsIdentity;
                        var groups = wi.Groups.Translate(typeof(NTAccount));
                        var roles = groups.Select(x => new Claim(JwtClaimTypes.Role, x.Value));
                        id.AddClaims(roles);
                    }

                    await HttpContext.SignInAsync(
                        IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme,
                        new ClaimsPrincipal(id),
                        props);
                    return Redirect(props.RedirectUri);
                }
                else
                {

                    return Challenge(AccountOptions.WindowsAuthenticationSchemeName);
                }
            }
            else
            {

                props.Items.Add("scheme", provider);
                return Challenge(props, provider);
            }
        }


        /// <summary>
        /// 扩展授权
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback()
        {

            var result = await HttpContext.AuthenticateAsync(IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme);
            if (result?.Succeeded != true)
            {
                throw new Exception("外部授权错误");
            }

            // 获取外部登录的Claims信息
            var externalUser = result.Principal;
            var claims = externalUser.Claims.ToList();

            //尝试确定外部用户的唯一ID（由提供者发出）
            //最常见的索赔，索赔类型分，nameidentifier
            //取决于外部提供者，可能使用其他一些索赔类型
            var userIdClaim = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject);
            if (userIdClaim == null)
            {
                userIdClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            }
            if (userIdClaim == null)
            {
                throw new Exception("未知用户");
            }

            //从集合中移除用户ID索赔索赔和移动用户标识属性还设置外部身份验证提供程序的名称。
            claims.Remove(userIdClaim);
            var provider = result.Properties.Items["scheme"];
            var userId = userIdClaim.Value;

            // 这是最有可能需要自定义逻辑来匹配您的用户的外部提供者的身份验证结果，并为用户提供您所认为合适的结果。
            //  检查外部用户已经设置
            var user = "";// _users.FindByExternalProvider(provider, userId);
            if (user == null)
            {
                //此示例只是自动提供新的外部用户，另一种常见的方法是首先启动注册工作流
                //user = _users.AutoProvisionUser(provider, userId, claims);
            }

            var additionalClaims = new List<Claim>();

            // 如果外部系统发送了会话ID请求，请复制它。所以我们可以用它进行单点登录
            var sid = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
            if (sid != null)
            {
                additionalClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
            }

            //如果外部供应商发出id_token，我们会把它signout
            AuthenticationProperties props = null;
            var id_token = result.Properties.GetTokenValue("id_token");
            if (id_token != null)
            {
                props = new AuthenticationProperties();
                props.StoreTokens(new[] { new AuthenticationToken { Name = "id_token", Value = id_token } });
            }

            // 为用户颁发身份验证cookie
           //   await _events.RaiseAsync(new UserLoginSuccessEvent(provider, userId, user.SubjectId, user.Username));
            // await HttpContext.SignInAsync(user.SubjectId, user.Username, provider, props, additionalClaims.ToArray());

            // 删除外部验证期间使用的临时cookie
            await HttpContext.SignOutAsync(IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme);

            // 验证返回URL并重定向回授权端点或本地页面
            var returnUrl = result.Properties.Items["returnUrl"];
            if (_interaction.IsValidReturnUrl(returnUrl) || Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return Redirect("~/");
        }

        #endregion



        #region 退出

        /// <summary>
        /// 退出页面显示
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {


            //var vm = await _account.BuildLogoutViewModelAsync(logoutId);

            //if (vm.ShowLogoutPrompt == false)
            //{
            //    //配置是否需要退出确认提示
            //    return await Logout(vm);
            //}

            //return View(vm);


            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            await HttpContext.SignOutAsync();

            ViewBag.SignOutIframeUrl = logout.SignOutIFrameUrl;
            ViewBag.RedirectUri = new Uri(logout.PostLogoutRedirectUri).GetLeftPart(UriPartial.Authority);

            return View("LoggedOut");


        }
        //public IActionResult LoggedOut()
        //{
        //    return View();
        //}


        /// <summary>
        /// 退出回调用页面
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            var vm = await _account.BuildLoggedOutViewModelAsync(model.LogoutId);
            var user = HttpContext.User;
            if (user?.Identity.IsAuthenticated == true)
            {
                //删除本地授权Cookies
                await HttpContext.SignOutAsync();
                await _events.RaiseAsync(new UserLogoutSuccessEvent(user.GetSubjectId(), user.GetName()));
            }

            // 检查是否需要在上游身份提供程序上触发签名
            if (vm.TriggerExternalSignout)
            {
                // 构建一个返回URL，以便上游提供者将重定向回
                // 在用户注销后给我们。这使我们能够
                // 完成单点签出处理。
                string url = Url.Action("Logout", new { logoutId = vm.LogoutId });
                // 这将触发重定向到外部提供者，以便签出
                return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
            }

            return View("LoggedOut", vm);
        }

        #endregion

     


    }
}