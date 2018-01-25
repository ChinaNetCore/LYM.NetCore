using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LYM.Middleware;
namespace WebTEST
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
            services.AddMvc();

            services.AddDapperContext(dapperoptions =>
            {
                dapperoptions.ConnectionString = "Data Source=192.168.0.42;Initial Catalog=NET.Core;User ID=sa;password=lym123!@#;Integrated Security=false";
                dapperoptions.OnCustomOptionSet = (options) =>
                {
                    return Task.FromResult(options.UserName);
                };
            });
            services.AddTransient<ICustomDapperContext, CustomDapperContext>();

        }
        /// <summary>
        /// 模拟委托回调处理
        /// </summary>
        /// <param name="options1"></param>
        /// <returns></returns>
        private async Task _OnCustomOptionSet(Options1 options1)
        {
            string x = options1.UserName;
            await Task.CompletedTask;
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


        }
    }
}
