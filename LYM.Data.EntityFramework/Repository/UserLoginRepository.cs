using LYM.Domain.Entity;
using LYM.Domain.Model;
using LYM.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.UnitOfWork.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LYM.Data.EntityFramework.Repository
{
    public class UserLoginRepository : RepositoryBase<UserLogin>, IUserLoginRepository
    {
        public UserLoginRepository(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
          
        }

        private IQueryable<UserLogin> CreateDefaultQuery()
        {
            return this.Set.Where(t => !t.IsDelete);
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="category"></param>
        /// <param name="keywords"></param>

        /// <returns></returns>
        public async Task<PagedResult<UserLogin>> QueryAsync_UserLogin(int pageIndex, int pageSize, string keywords = null)
        {
            var query = this.CreateDefaultQuery();
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                query = query.Where(t => t.UserName.Contains(keywords));
            }
            var total = await query.CountAsync();
            query = query.OrderByDescending(t => t.Order).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            var entityList = await query.ToListAsync();
            return PagedResult<UserLogin>.SuccessResult(entityList, pageIndex, pageSize, total);
        }
    }
}
