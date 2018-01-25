using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace LYM.Middleware
{
    public static class DapperMiddlewareExtension
    {

        public static IServiceCollection AddDapperContext<TDapperContext>(this IServiceCollection serviceCollection, Action<DapperOptions> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped) where TDapperContext : DapperContext
        {
            serviceCollection.Configure(optionsAction);
            serviceCollection.AddTransient<IDataProvider, SqlServerDataProvider>();
            serviceCollection.AddSingleton<TDapperContext>();
            return serviceCollection;
        }
        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="optionsAction"></param>
        /// <param name="contextLifetime"></param>
        /// <param name="optionsLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddDapperContext(this IServiceCollection serviceCollection, Action<DapperOptions> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped) 
        {
            serviceCollection.Configure(optionsAction);
            serviceCollection.AddTransient<IDataProvider, SqlServerDataProvider>();
            serviceCollection.AddSingleton<DapperContext>();
            return serviceCollection;
        }
    }
}
