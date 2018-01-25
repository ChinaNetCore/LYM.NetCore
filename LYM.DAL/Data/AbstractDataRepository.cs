
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;

namespace LYM.DAL
{
    /// <summary>
    /// liyouming add  数据仓储  自己写的仓储  扩展来之 IRepository 并实现了自己的接口扩展仓储ICumstomRepository 2017-12-05  
    /// 这里是抽象类 需要扩展自己可以重写
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractDataRepository<T> : IRepository<T>, ICumstomRepository where T : class
    {

        protected ILoggerFactory LoggerFactory { get; }
        protected IServiceProvider ServiceProvider { get; }
        protected DbContext Context { get; }
        protected ILogger Logger { get; set; }
        protected DbSet<T> Set { get; }


        protected readonly IServiceProvider _serviceProvider;



        public AbstractDataRepository(IServiceProvider serviceProvider)
        {

            _serviceProvider = serviceProvider;
        }

        public virtual void ChangeTable(string table)
        {


            throw new NotImplementedException();
        }

        public virtual int Count(Expression<Func<T, bool>> predicate = null)
        {
            return this.Set.Count(predicate);
        }

        #region 删除仓储  liyouming
        public virtual void Delete(object id)
        {
            var mol = this.Set.Find(id);
            this.Delete(mol);
        }

        public virtual void Delete(T entity)
        {
            this.Set.Attach(entity);
            this.Set.Remove(entity);
        }

        public virtual void Delete(params T[] entities)
        {
            this.Set.AttachRange(entities);
            this.Set.RemoveRange(entities);
        }

        public virtual void Delete(IEnumerable<T> entities)
        {
            this.Set.AttachRange(entities);
            this.Set.RemoveRange(entities);
        }
        #endregion

        #region 查找仓储 liyouming
        public virtual T Find(params object[] keyValues)
        {
            return this.Set.Find(keyValues);
        }

        public virtual async Task<T> FindAsync(params object[] keyValues)
        {
            return await this.Set.FindAsync(keyValues);
        }

        public virtual async Task<T> FindAsync(object[] keyValues, CancellationToken cancellationToken)
        {
            return await this.Set.FindAsync(keyValues, cancellationToken);
        } 
        #endregion

        public virtual IQueryable<T> FromSql(string sql, params object[] parameters)
        {
            return this.Set.FromSql(sql, parameters);
        }
        /// <summary>
        /// 不知道对不对 需要测试 liyouming
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> GetAll()
        {
            
            return this.Set.AsQueryable();
           
        }

        public virtual T GetFirstOrDefault(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true)
        {

            throw new NotImplementedException();

        

        }

        public virtual TResult GetFirstOrDefault<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TResult> GetFirstOrDefaultAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true)
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true)
        {
            throw new NotImplementedException();
        }

        public virtual IPagedList<T> GetPagedList(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, int pageIndex = 0, int pageSize = 20, bool disableTracking = true)
        {
            throw new NotImplementedException();
        }

        public virtual IPagedList<TResult> GetPagedList<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, int pageIndex = 0, int pageSize = 20, bool disableTracking = true) where TResult : class
        {
            throw new NotImplementedException();
        }

        public virtual Task<IPagedList<T>> GetPagedListAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, int pageIndex = 0, int pageSize = 20, bool disableTracking = true, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public virtual Task<IPagedList<TResult>> GetPagedListAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, int pageIndex = 0, int pageSize = 20, bool disableTracking = true, CancellationToken cancellationToken = default(CancellationToken)) where TResult : class
        {
            throw new NotImplementedException();
        }

        #region 新增 同步方法  liyouming
        public virtual void Insert(T entity)
        {
            this.Set.Add(entity);
        }

        public virtual void Insert(params T[] entities)
        {
            this.Set.AddRange(entities);
        }

        public virtual void Insert(IEnumerable<T> entities)
        {
            this.Set.AddRange(entities);
        }
        #endregion

        #region 新增 异步方法  liyouming
        public virtual async Task InsertAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            await this.Set.AddAsync(entity, cancellationToken);
        }

        public virtual async Task InsertAsync(params T[] entities)
        {
            await this.Set.AddRangeAsync(entities);
        }

        public virtual async Task InsertAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            await this.Set.AddRangeAsync(entities, cancellationToken);
        } 
        #endregion

        public virtual void Update(T entity)
        {
           
        }

        public virtual void Update(params T[] entities) {


        }
        

        public void Update(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }
    }
}
