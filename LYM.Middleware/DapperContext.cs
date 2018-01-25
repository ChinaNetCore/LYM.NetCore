using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LYM.Middleware
{
    public class DapperContext 
    {
        DapperOptions _dapperOptions;
        IDataProvider _dataProvider;
        public DapperContext(IOptions<DapperOptions> options, IDataProvider dataProvider)
        {
            _dapperOptions = options.Value;
            _dataProvider = dataProvider;
        }

      


        #region 创建Dapper相关连接


        private IDbConnection CreateConnection(bool ensureClose = true)
        {

            var conn = _dataProvider.CreateConnection();
            conn.ConnectionString = _dapperOptions.ConnectionString;
            conn.Open();

            return conn;
        }
        private IDbConnection _connection;
        private IDbConnection Connection
        {
            get
            {
                if (_connection == null || _connection.State != ConnectionState.Open)
                {
                    _connection = CreateConnection();
                }

                return _connection;
            }
        }


        public void GetFunc()
        {

            _dapperOptions.OnCustomOptionSet(new Options1 {

                UserName = "这里是执行后产生的结果"
            });
        }

        public void insertTest(string sql)
        {

            GetFunc();
            var conn = Connection;
            try
            {
                conn.Execute(sql);
            }

            finally
            {
                if (_connection != null)
                {
                    _connection.Close();
                    _connection = null;
                }
            }


        }



        #endregion



    }
}
