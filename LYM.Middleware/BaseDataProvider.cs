using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace LYM.Middleware
{
    public abstract class BaseDataProvider : IDataProvider
    {
        #region 字段
        private readonly DbProviderFactory _dbProvider;


        #endregion

        #region 构造函数
        protected BaseDataProvider(DbProviderFactory dbProvider)
        {
            this._dbProvider = dbProvider;

        }
        #endregion

        #region 公开方法

        public virtual DbCommand CreateCommand()
        {
            return _dbProvider.CreateCommand();
        }

        public virtual DbConnection CreateConnection()
        {
            return _dbProvider.CreateConnection();
        }

        public virtual DbParameter CreateParameter()
        {
            return _dbProvider.CreateParameter();
        }

        public DbDataAdapter CreateDataAdapter()
        {
            return _dbProvider.CreateDataAdapter();
        }

        #endregion

    }
}
