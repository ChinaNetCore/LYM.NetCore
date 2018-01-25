using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using LYM.Data.EntityFramework.EFContext;
using Autofac;
using Shared.Infrastructure;
using LYM.Data.EntityFramework;
using LYM.Core;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.Cookies;
using LYM.Cap;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;

namespace LYM.WebSite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {

          

            #region 数据库连接
            string customUrl = this.Configuration["Authority"];
            services.AddMvc();
            services.AddOptions();
            services.AddDbContext<CustomContext>(builder =>
            {
                builder.UseSqlServer(this.Configuration["ConnectionString"], options =>
                {
                    options.UseRowNumberForPaging();
                    options.MigrationsAssembly("LYM.WebSite");
                });
            }, ServiceLifetime.Transient);
            #endregion

          


            #region 添加授权验证方式 这里是Cookies & OpenId Connect 
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(
                options =>
                {
                    options.DefaultScheme = "lym.oauth.cookies";// CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = "oidc";
                }
                )
            .AddCookie("lym.oauth.cookies", options =>
            {

                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.Cookie.Name = "lym.website";

            })  //监控浏览器Cookies不难发现有这样一个 .AspNetCore.lym.Cookies 记录了加密的授权信息 
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = customUrl;
                options.ClientId = "lym.clienttest1";
                options.ClientSecret = "lym.clienttest";
                options.RequireHttpsMetadata = false;
                options.SaveTokens = true;
                options.ResponseType = "code id_token";
                //布尔值来设置处理程序是否应该转到用户信息端点检索。额外索赔或不在id_token创建一个身份收到令牌端点。默认为“false”
                options.GetClaimsFromUserInfoEndpoint = true;
                options.CallbackPath = new PathString("/oidc/login-callback");
                options.SignInScheme = "lym.oauth.cookies";
                options.SignOutScheme = "lym.oauth.cookies";
                options.RemoteSignOutPath = new PathString("/oidc/front-channel-logout-callback");
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("cloudservices");
                options.Events = new OpenIdConnectEvents
                {
                    OnRedirectToIdentityProvider = OnRedirectToIdentityProvider,
                    OnRemoteSignOut = OnRemoteSignOut,
                    OnRemoteFailure = OnRemoteFailure,
                    OnAuthenticationFailed = OnAuthenticationFailed,
                    OnRedirectToIdentityProviderForSignOut = OnRedirectToIdentityProviderForSignOut,
                    OnAuthorizationCodeReceived = OnAuthorizationCodeReceived,
                    OnMessageReceived = OnMessageReceived,
                    OnTicketReceived = OnTicketReceived,
                    OnTokenResponseReceived = OnTokenResponseReceived,
                    OnTokenValidated = OnTokenValidated,
                    OnUserInformationReceived = OnUserInformationReceived
                };


            });
            #endregion



            #region Cap配置

            //services.AddDbContext<CapApiContext>(builder =>
            //{
            //    builder.UseSqlServer(this.Configuration["CapConnectionString"]);
            //}, ServiceLifetime.Transient);

            //services.AddCap(x =>
            //{
            //    //EF实体框架
            //    x.UseEntityFramework<CapApiContext>();

            //    // Dapper
            //    // x.UseSqlServer("Your ConnectionStrings");

            //    //  RabbitMQ 消息

            //    //x.UseRabbitMQ(rabbitMQOption =>
            //    //{

            //    //    #region UseRabbitMQ 参数说明
            //    //    //HostName 宿主地址    string localhost
            //    //    //UserName 用户名 string guest
            //    //    //Password 密码  string guest
            //    //    //VirtualHost 虚拟主机    string  /
            //    //    //Port    端口号 int -1
            //    //    //TopicExchangeName CAP默认Exchange名称 string cap.default.topic
            //    //    //RequestedConnectionTimeout RabbitMQ连接超时时间  int 30,000 毫秒
            //    //    //SocketReadTimeout   RabbitMQ消息读取超时时间    int 30,000 毫秒
            //    //    //SocketWriteTimeout  RabbitMQ消息写入超时时间    int 30,000 毫秒
            //    //    //QueueMessageExpires 队列中消息自动删除时间 int(10天) 毫秒 
            //    //    #endregion
            //    //    rabbitMQOption.HostName = "192.168.0.42";
            //    //    //rabbitMQOption.VirtualHost = "/rabbit";
            //    //    //rabbitMQOption.UserName = "lym123";
            //    //    //rabbitMQOption.Password = "lym123";
            //    //    rabbitMQOption.Port = 5672;
            //    //    // rabbitmq options.
            //    //});

            //    x.UseRabbitMQ("localhost");

            //    // Kafka 消息
            //    // x.UseKafka("localhost:5003");

            //    //	执行失败消息时的回调函数，详情见下文		Action<MessageType,string,string>
            //    //x.FailedCallback = null;
            //    //处理消息的线程默认轮询等待时间（秒）		15 秒
            //    x.PollingDelay = 15;
            //    //启动队列中消息的处理器个数    2
            //    x.QueueProcessorCount = 2;
            //    //失败重试次数
            //    x.FailedRetryCount = 3;
            //    //失败重试时间间隔
            //    x.FailedRetryInterval = 15;


            //});

            #endregion

        }
        #region  Events事件
        private static Task OnRedirectToIdentityProvider(RedirectContext context)
        {
            if (context.HttpContext.Items.ContainsKey("idp"))
            {
                var idp = context.HttpContext.Items["idp"];
                context.ProtocolMessage.AcrValues = "idp:" + idp;
            }

            return Task.FromResult(0);
        }

        private static Task OnRemoteSignOut(RemoteSignOutContext context)
        {
            return Task.FromResult(0);
        }

        private static Task OnRemoteFailure(RemoteFailureContext context)
        {
            return Task.FromResult(0);
        }

        private static Task OnAuthenticationFailed(AuthenticationFailedContext context)
        {
            return Task.FromResult(0);
        }

        private static Task OnRedirectToIdentityProviderForSignOut(RedirectContext context)
        {
            context.ProtocolMessage.PostLogoutRedirectUri = context.Request.Scheme + "://" + context.Request.Host;
            return Task.FromResult(0);
        }
        private static Task OnAuthorizationCodeReceived(AuthorizationCodeReceivedContext context)
        {
            return Task.FromResult(0);
        }
        private static Task OnMessageReceived(MessageReceivedContext context)
        {
            return Task.FromResult(0);
        }

        private static Task OnTicketReceived(TicketReceivedContext context)
        {
            return Task.FromResult(0);
        }

        private static Task OnTokenResponseReceived(TokenResponseReceivedContext context)
        {
            return Task.FromResult(0);
        }

        private static Task OnTokenValidated(TokenValidatedContext context)
        {
            return Task.FromResult(0);
        }

        private static Task OnUserInformationReceived(UserInformationReceivedContext context)
        {
            return Task.FromResult(0);
        }
        #endregion
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //Autofac 注入
            builder.RegisterInstance(this.Configuration).AsImplementedInterfaces();

            //builder.RegisterType<RedisProvider>().As<IRedisProvider>().SingleInstance();

            builder.AddUnitOfWork(provider =>
            {
                provider.Register(new LYM.Data.EntityFramework.ClubUnitOfWorkRegisteration());
            });

            builder.RegisterModule<CoreModule>()
                .RegisterModule<EntityFrameworkModule>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Iframes}/{id?}");
            });

          // app.UseCap(); //Cap配置
        }
    }
}
