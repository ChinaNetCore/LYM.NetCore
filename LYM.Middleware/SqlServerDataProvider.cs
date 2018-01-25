using System;
using System.Collections.Generic;
using System.Text;

namespace LYM.Middleware
{
    public class SqlServerDataProvider : BaseDataProvider
    {

        #region 构造函数
        public SqlServerDataProvider()
            : base(CreateDbProviderFactory.GetDbProviderFactory(DbFactoryProviderType.SqlServer))
        {
        }
        #endregion
    }
}
