using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace LYM.Middleware
{
    public class CreateDbProviderFactory
    {

        public static DbProviderFactory GetDbProviderFactory(DbFactoryProviderType t)
        {
            DbProviderFactory factory;
            switch (t)
            {
                case DbFactoryProviderType.SqlServer: factory = SqlClientFactory.Instance; break;
                //  case DbFactoryProviderType.Mysql: factory = MySqlClientFactory.Instance; break;
                //case DbFactoryProviderType.Oracle: factory = OracleClientFactory.Instance; break; //没测试过 可能出现问题
                default: factory = SqlClientFactory.Instance; break;

            }
            return factory;
        }

        public static DbProviderFactory GetDbProviderFactory()
        {
            return GetDbProviderFactory(DbFactoryProviderType.SqlServer);
        }
    }
}
