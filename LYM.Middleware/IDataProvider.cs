using System;
using System.Data;
using System.Data.Common;
namespace LYM.Middleware
{
    public interface IDataProvider
    {

        DbConnection CreateConnection();
        DbCommand CreateCommand();
        DbDataAdapter CreateDataAdapter();

        DbParameter CreateParameter();
    }

    public static class DataProviderExtensions
    {
        public static DbParameter CreateParameter(
           this IDataProvider dataProvider,
           string name,
           object value = null,
           DbType? dbType = null,
           ParameterDirection? direction = null,
           int? size = null)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider:数据访问对象为null");
            }

            var result = dataProvider.CreateParameter();
            result.ParameterName = name;
            result.Value = value;
            if (dbType.HasValue)
            {
                result.DbType = dbType.Value;
            }
            if (direction.HasValue)
            {
                result.Direction = direction.Value;
            }
            if (size.HasValue)
            {
                result.Size = size.Value;
            }

            return result;
        }
    }
}
