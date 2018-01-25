using LYM.Domain.Entity;
using LYM.Domain.Model;
using Shared.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LYM.Domain.Repository
{
    public interface IUserLoginRepository : IRepository<UserLogin>
    {
        Task<PagedResult<UserLogin>> QueryAsync_UserLogin(int pageIndex, int pageSize, string keywords);
    }
}
