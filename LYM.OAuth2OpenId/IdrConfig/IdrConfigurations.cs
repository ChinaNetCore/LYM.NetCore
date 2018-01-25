using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LYM.OAuth2OpenId.IdrConfig
{

    ///liyouming add 2017-11-28
    /// <summary>
    /// 配置Idr4的基础配置  这里我定义了两个服务接口资源访问权限的Scope 
    /// </summary>
    public class IdrConfigurations
    {
        const string home = "http://192.168.0.42:5000";
        private const string OidcLoginCallback = "/oidc/login-callback";
        private const string OidcFrontChannelLogoutCallback = "/oidc/front-channel-logout-callback";
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        /// <summary>
        /// 定义API资源访问权限 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("cloudservices", "clientservices")
             };
        }

        public static IEnumerable<Client> GetClient()
        {
            return new List<Client> {

                new Client{
                    ClientId="lym.clienttest",
                    ClientName="测试Idr4",
                    Enabled=true,
                     //"AuthorizationCode","Implicit","Hybrid","ResourceOwner","ClientCredentials"
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RequireConsent=false,
                    ClientSecrets=new List<Secret>(){
                        new Secret("lym.clienttest".Sha256(),DateTime.Now.AddYears(1))
                    },
                    AccessTokenLifetime=3600,
                    AccessTokenType= AccessTokenType.Reference,
                    AllowOfflineAccess=true,
                    RedirectUris= { home + OidcLoginCallback },
                    PostLogoutRedirectUris= { home },
                    AllowedScopes=new List<string>{
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "cloudservices"
                    },
                     FrontChannelLogoutUri = home + OidcFrontChannelLogoutCallback,
                    FrontChannelLogoutSessionRequired = true

                },
                 new Client{
                    ClientId="lym.clienttest1",
                    ClientName="测试Idr4",
                    Enabled=true,
                     //"AuthorizationCode","Implicit","Hybrid","ResourceOwner","ClientCredentials"
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RequireConsent=false,
                    ClientSecrets=new List<Secret>(){
                        new Secret("lym.clienttest".Sha256(),DateTime.Now.AddYears(1))
                    },
                    AccessTokenLifetime=3600,
                    AccessTokenType= AccessTokenType.Reference,
                    AllowOfflineAccess=true,
                    RedirectUris={ home + OidcLoginCallback },
                    PostLogoutRedirectUris= { home },
                    AllowedScopes=new List<string>{
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "cloudservices"
                    },
                     FrontChannelLogoutUri = home + OidcFrontChannelLogoutCallback,
                    FrontChannelLogoutSessionRequired = true

                }
            };

        }

    }
}
