using LYM.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTEST
{
    public class CustomDapperContext : ICustomDapperContext
    {
        DapperContext _dapperContext;
        public CustomDapperContext(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }
        public void Insert(string sql)
        {
            _dapperContext.insertTest(sql);
        }
    }
}
