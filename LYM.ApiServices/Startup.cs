using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using LYM.Data.EntityFramework.EFContext;
using LYM.Cap;
using Autofac;
using Shared.Infrastructure;
using LYM.Core;
using LYM.Data.EntityFramework;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;

namespace LYM.ApiServices
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
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
                    options.MigrationsAssembly("LYM.ApiServices");
                });
            }, ServiceLifetime.Transient);
            #endregion


            #region Cap配置

            //services.AddDbContext<CapApiContext>(builder =>
            //{
            //    builder.UseSqlServer(this.Configuration["CapConnectionString"]);
            //}, ServiceLifetime.Transient);

            //services.AddCap(x =>
            //{

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

            //    ////	执行失败消息时的回调函数，详情见下文		Action<MessageType,string,string>
            //    //x.FailedCallback = null;
            //    ////处理消息的线程默认轮询等待时间（秒）		15 秒
            //    //x.PollingDelay = 15;
            //    ////启动队列中消息的处理器个数    2
            //    //x.QueueProcessorCount = 2;
            //    ////失败重试次数
            //    //x.FailedRetryCount = 3;
            //    ////失败重试时间间隔
            //    //x.FailedRetryInterval = 15;


            //});

            #endregion



            #region Swagger 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "IdentityServer4 & WebApi"
                });
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "LYM.ApiServices.xml");
                c.IncludeXmlComments(xmlPath);


                //c.AddSecurityDefinition("oauth2", new OAuth2Scheme
                //{
                //    Type = "oauth2",
                //    Flow = "implicit",
                //    AuthorizationUrl = "http://petstore.swagger.io/oauth/dialog",
                //    Scopes = new Dictionary<string, string>
                //    {
                //        { "readAccess", "Access read operations" },
                //        { "writeAccess", "Access write operations" }
                //    }
                //});
          
                //c.OperationFilter<SecurityRequirementsOperationFilter>();


            });



            #endregion
        }


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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
           // app.UseCap(); //使用Swagger不能使用这个
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "IdentityServer4 & WebApi");
            });
           
           

           
         
        }
    }
}
