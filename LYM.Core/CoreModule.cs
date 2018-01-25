using Autofac;
using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace LYM.Core
{
    public class CoreModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();

            var assembly = Assembly.Load(new AssemblyName("LYM.Core"));
            foreach (var typeInfo in assembly.DefinedTypes)
            {
                if (typeInfo.Name.EndsWith("Service"))
                {
                    builder.RegisterType(typeInfo.AsType()).AsImplementedInterfaces();
                }
            }

            AutoMapperConfig.Configure();
        }
    }
}
