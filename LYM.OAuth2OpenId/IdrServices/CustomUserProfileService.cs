using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using LYM.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LYM.OAuth2OpenId.IdrServices
{
    /// <summary>
    /// 动态获取用户授权信息接口  实现 IProfileService  并添加注入依赖
    /// </summary>
    public class CustomUserProfileService : IProfileService
    {
        //Add by liyouming 2017-11-28 这里其实与Identityserver3基本一样，那么这里怎么访问用户数据库业务服务几口呐
       
        private IUserService _sysuserServices;
        public CustomUserProfileService(IUserService sysuserServices)
        {
            _sysuserServices = sysuserServices;
        }
        /// <summary>
        /// 每当请求用户请求时，就调用此方法（例如令牌创建或通过用户信息的端点） 预期为用户加载声明的API
        /// </summary>
        /// <param name="context">请求上下文对象</param>
        /// <returns></returns>
        public async  Task GetProfileDataAsync(ProfileDataRequestContext context) {

            
            //这里跟IdentityServer3中获取信息的方法一致
            var claim = context.Subject.FindFirst(item => item.Type == "sub");
            var userinfo= await _sysuserServices.FindByUserID(claim.Value);

            // context.Client :获取设置客户端相关配置
            // context.AddRequestedClaims :添加认证请求索赔




        }
        /// <summary>
        /// 每当身份服务器需要确定用户时，就调用此方法。是有效的或主动的（例如，如果用户的帐户已经停用，因为他们登录）。（例如在令牌发布或验证期间）
        /// 期望指示用户当前是否被允许获取令牌的API
        /// </summary>
        /// <param name="context">请求上下文对象</param>
        /// <returns></returns>
        public async Task IsActiveAsync(IsActiveContext context)
        {


             await Task.FromResult(0);
            //context.Subject : 该ClaimsPrincipal模型的用户。如果请求来自用户的cookie的声明将在ClaimsPrincipal
            //context.IsActive:指示用户是否被允许获取令牌的标志。预计这将由自定义IProfileService实现分配
            //context.Caller : 请求声明的上下文的标识符（例如身份令牌，访问令牌或用户信息端点）。常量IdentityServerConstants.ProfileDataCallers包含不同的常量值。
            //context.Client :Client用于正被请求的权利要求
            //说明：
            //context.Caller :  IdentityServerConstants.ProfileDataCallers.ClaimsProviderAccessToken \ClaimsProviderIdentityToken \UserInfoEndpoint

          
        }
    }
}
