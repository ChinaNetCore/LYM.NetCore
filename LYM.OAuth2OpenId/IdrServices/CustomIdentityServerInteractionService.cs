using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LYM.OAuth2OpenId.IdrServices
{
    /// <summary>
    /// 接口服务可以放在IServiceCollection 中提供注入
    /// 接口旨在提供用户界面用于与IdentityServer通信的服务，主要涉及用户交互。
    /// 它可以从依赖注入系统获得，通常作为构造参数注入到IdentityServer用户界面的MVC控制器中。
    /// </summary>
    /// : IIdentityServerInteractionService
    public class CustomIdentityServerInteractionService 
    {
        /// <summary>
        /// 用于创建一个logoutId如果没有一个目前。
        /// 这将创建一个cookie来捕获注销所需的所有当前状态，并logoutId标识该cookie。
        /// 这通常在没有当前时间的情况下使用logoutId，并且注销页面必须在重定向到用于注销的外部身份提供商之前捕获用于singout的当前用户的状态。
        /// 新创建的应用程序logoutId需要在注销时对外部身份提供程序进行往返，然后在注销回调页面上使用，与正常注销页面上的方式相同。
        /// </summary>
        /// <returns></returns>
        public async Task<string> CreateLogoutContextAsync()
        {

            return await Task.FromResult("这里是退出");
        }
        /// <summary>
        /// 返回Consent用户的集合。
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<IdentityServer4.Models.Consent>> GetAllUserConsentsAsync()
        {

            #region Consent 参数说明
            //SubjectId：授予同意的主体ID。
            //ClientId：同意的客户端标识符。
            //Scopes：收集范围同意。
            //CreationTime：同意的日期和时间。
            //Expiration：同意将到期的日期和时间。 
            #endregion

            return await Task.FromResult(new List<IdentityServer4.Models.Consent>() {
                new IdentityServer4.Models.Consent(){ ClientId=""}

            });
        }
        /// <summary>
        /// 返回AuthorizationRequest基于returnUrl传递给登录或同意页面的内容。
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task<AuthorizationRequest> GetAuthorizationContextAsync(string returnUrl)
        {
            #region AuthorizationRequest 参数说明
            //ClientId: 发起请求的客户端标识符。
            //RedirectUri:成功授权后将用户重定向到的URI。
            //DisplayMode:从授权请求传递的显示模式。
            //UiLocales:从授权请求传递的UI区域设置。
            //IdP:外部身份提供者请求。这用于绕过家境发现（HRD）。这是通过acr_values授权请求上参数的“idp：”前缀提供的。
            //Tenant:租客要求。这是通过acr_values授权请求上参数的“tenant：”前缀提供的。
            //LoginHint:用户将用于登录的预期用户名。这是通过login_hint授权请求上的参数从客户端请求的。
            //PromptMode:从授权请求中请求的提示模式。
            //AcrValues:从授权请求传递的acr值。
            //ScopesRequested:从授权请求中请求的范围。
            //Parameters:整个参数集合传递给授权请求。
            #endregion
            return await Task.FromResult(new AuthorizationRequest
            {
                ClientId = ""

            });
        }
        /// <summary>
        /// 返回ErrorMessage基于errorId传递给错误页面的内容。
        /// </summary>
        /// <param name="errorId"></param>
        /// <returns></returns>
        public async Task<ErrorMessage> GetErrorContextAsync(string errorId)
        {
            #region ErrorMessage 参数说明
            //DisplayMode:从授权请求传递的显示模式。
            //UiLocales:从授权请求传递的UI区域设置。
            //Error:错误代码。
            //RequestId:每个请求标识符。这可以用来显示给最终用户，并可用于诊断。 
            #endregion
            return await Task.FromResult(new ErrorMessage
            {
                Error = "客户端授权失败"
            });
        }
        /// <summary>
        /// 返回LogoutRequest基于logoutId传递给注销页面的内容
        /// </summary>
        /// <param name="logoutId"></param>
        /// <returns></returns>
        public async Task<LogoutRequest> GetLogoutContextAsync(string logoutId)
        {
            #region LogoutRequest 参数说明
            //ClientId：发起请求的客户端标识符。
            //PostLogoutRedirectUri：用户在注销后重定向的URL。
            //SessionId：用户当前的会话ID。
            //SignOutIFrameUrl：要在<iframe> 注销的页面上呈现的URL，以启用单一注销。
            //Parameters：将整个参数集合传递给结束会话端点。
            //ShowSignoutPrompt：指示是否根据传递给结束会话端点的参数提示用户注销。 
            #endregion
            return await Task.FromResult(
                new LogoutRequest("iframeUrl",
                new LogoutMessage
                {
                    ClientId = "ClientId"
                }
                )
                );
        }
        /// <summary>
        /// 接受一个ConsentResponse通知IdentityServer用户同意某个特定的AuthorizationRequest
        /// </summary>
        /// <param name="request"></param>
        /// <param name="consent"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        public async Task GrantConsentAsync(AuthorizationRequest request, ConsentResponse consent, string subject = null)
        {

            #region ConsentResponse 参数说明
            //ScopesConsented：用户同意的范围集合。
            //RememberConsent：标志表示用户的同意是否被持续。 
            #endregion

            await Task.FromResult(0);
        }
        /// <summary>
        /// 指出returnUrl登录或同意后是否为重定向的有效网址
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public bool IsValidReturnUrl(string returnUrl)
        {

            return true;
        }
        /// <summary>
        /// 取消用户在当前会话中登录的所有用户的同意和授予。
        /// </summary>
        /// <returns></returns>
        public async Task RevokeTokensForCurrentSessionAsync()
        {

            await Task.FromResult(0);
        }
        /// <summary>
        /// 撤销所有用户的同意和赠款给客户。
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task RevokeUserConsentAsync(string clientId)
        {

            await Task.FromResult(0);
        }

    }
}
