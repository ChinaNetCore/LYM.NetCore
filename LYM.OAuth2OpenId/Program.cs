using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared.Infrastructure;

namespace LYM.OAuth2OpenId
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: true);
           var  Configuration = config.Build();
            //var seed = args.Contains("/seed");
            //if (seed)
            //{
            //    args = args.Except(new[] { "/seed" }).ToArray();
            //}
            var host = BuildWebHost(args, Configuration);
            //if (true)
            //{
            //    IdrConfig.ConfigSeed.EnsureSeedData(host.Services);
            //}
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args,IConfiguration configuration) =>
            WebHost.CreateDefaultBuilder(args)
                .UseInfrastructureFactory()
                .UseUrls(configuration["Host"])
                .UseStartup<Startup>()
                .Build();
    }
}
