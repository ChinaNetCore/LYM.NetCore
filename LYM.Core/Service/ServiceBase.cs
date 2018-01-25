using LYM.Domain.Consts;
using Shared.Infrastructure.UnitOfWork;
using System;
using Microsoft.Extensions.DependencyInjection;
namespace LYM.Core.Service
{
    public class ServiceBase
    {
        protected IServiceProvider ServiceProvider { get; private set; }

        protected IUnitOfWorkProvider UnitOfWorkProvider => this.ServiceProvider.GetService<IUnitOfWorkProvider>();

        public ServiceBase(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            this.ServiceProvider = serviceProvider;
        }

        protected IUnitOfWork CreateUnitOfWork()
        {
            return this.UnitOfWorkProvider.CreateUnitOfWork(UnitOfWorkNames.EntityFramework);
        }
    }
}
