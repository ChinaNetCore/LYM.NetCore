using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LYM.Data.EntityFramework
{
    public class ClubUnitOfWorkRegisteration : Shared.Infrastructure.UnitOfWork.EntityFramework.UnitOfWorkRegisteration<EFContext.CustomContext>
    {
        public override string Name => Domain.Consts.UnitOfWorkNames.EntityFramework;

        public override Assembly[] EntityAssemblies => new Assembly[] { Assembly.Load(new AssemblyName("LYM.Domain")) };

        public override Assembly[] RepositoryAssemblies  => new Assembly[] { Assembly.Load(new AssemblyName("LYM.Data.EntityFramework")) };
    }
}
